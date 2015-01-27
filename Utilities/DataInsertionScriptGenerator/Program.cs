using DataInsertionScriptGenerator.Helpers;
using DataInsertionScriptGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataInsertionScriptGenerator
{
    class Program
    {
        private static readonly string OutputFilePath = ConfigurationManager.AppSettings["OutputFilePath"] ?? ".";
        private static readonly SqlConnectionStringBuilder DestinationConnection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Destination"].ConnectionString);
        private static readonly bool ExecuteScripts = bool.Parse(ConfigurationManager.AppSettings["ExecuteScripts"]);
        private static readonly string CreateTemplate = new FileInfo(@".\SqlTemplates\CreateTableGenerateFromTable.sql").OpenText().ReadToEnd();
        private static readonly string InsertTemplate = new FileInfo(@".\SqlTemplates\DataInsertGenerateFromTable.sql").OpenText().ReadToEnd();
        private static readonly List<Script> Scripts = Util.GetScripts();
        private static readonly List<string> ProgressQueue = new List<string>();
        private static readonly object Locker = new Object();

        static void Main(string[] args)
        {
            Console.WriteLine("Scripts to be generated:");
            Scripts.ForEach(Console.WriteLine);
            Console.WriteLine("Output Directory:\n\t{0}", OutputFilePath);
            Console.WriteLine("\nPress ENTER to start generating scripts.");
            Console.ReadLine();

            var tasks = new List<Task>();

            Scripts.ForEach(script => tasks.Add(Task.Factory.StartNew(() =>
            {
                lock (Locker)
                    ProgressQueue.Add(script.Name);

                var outputScript = Run(script);

                if (ExecuteScripts)
                    ExecuteScript(outputScript);

                lock (Locker)
                    ProgressQueue.Remove(script.Name);

                if (ProgressQueue.Count > 0)
                    Console.WriteLine("Still in progress: {0}", string.Join(", ", ProgressQueue));
            })));

            Task.Factory.StartNew(() =>
            {
                var spinner = new ConsoleSpinner();

                while (ProgressQueue.Count > 0)
                {
                    Thread.Sleep(100);
                    spinner.Turn();
                }
            });

            Task.WaitAll(tasks.ToArray());

            CreateBatchFile();
            CreateBatchFileWithCredentials();

            Console.WriteLine();
            Console.WriteLine("Done! Scripts {0}can be found at: {1}.", ExecuteScripts ? "and execution results " : "", OutputFilePath);
            Console.WriteLine("\nPress ENTER to exit.");
            Console.ReadLine();
        }

        private static string Run(Script script)
        {
            var fileName = string.Format(@"{0}\{1}-{2}.sql", OutputFilePath, DestinationConnection.InitialCatalog, script.Name);

            using (var outputFile = new StreamWriter(fileName))
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Source"].ConnectionString))
                {
                    conn.Open();

                    var databases = new List<string> { script.DefaultDatabase };

                    var filterDatabases = script.IncludeTables.Where(t => !t.Database.Equals(script.DefaultDatabase)).Select(t => t.Database).Distinct().ToList();
                    if (filterDatabases.Count > 0)
                        databases.AddRange(filterDatabases);


                    Console.WriteLine("Generating: " + fileName);

                    foreach(var database in databases)
                    {
                        var schemas = new List<string> { script.DefaultSchema };

                        var filterSchemas = script.IncludeTables.Where(t => !t.Schema.Equals(script.DefaultSchema)).Select(t => t.Schema).Distinct().ToList();
                        if (filterSchemas.Count > 0)
                            schemas.AddRange(filterSchemas);

                        foreach(var schema in schemas)
                        {
                            var tableScript = script.GenerateCreateTable ? script.Configure(CreateTemplate, database, schema, DestinationConnection.InitialCatalog) : null;
                            var insertScript = script.Configure(InsertTemplate, database, schema, DestinationConnection.InitialCatalog);

                            foreach (var cmdText in new List<string> { tableScript, insertScript }.Where(s => !string.IsNullOrEmpty(s)))
                            {
                                try
                                {
                                    using (var cmd = new SqlCommand(cmdText, conn){ CommandTimeout = conn.ConnectionTimeout })
                                    using (var dr = cmd.ExecuteReader())
                                        while (dr.Read())
                                            outputFile.WriteLine(dr[0]);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception at: {0}.{1}", database, schema);
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine(ex.StackTrace);
    #if DEBUG
                                    WriteTempFile(cmdText);
                                    throw;
    #endif
                                }

                            }
                        }
                    }

                    Console.WriteLine("Generated: {0} ({1}).", fileName, outputFile.BaseStream.Length.BytesToString());
                }
            }

            return fileName;
        }

        private static void ExecuteScript(string scriptPath)
        {
            var resultFilePath = scriptPath.Replace(".sql", "-Results.txt");
            var processInfo = new ProcessStartInfo("sqlcmd", string.Format("-S {0} -i {1} -o {2} -x", DestinationConnection.DataSource, scriptPath, resultFilePath))
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                try
                {
                    Console.WriteLine(@"Executing: {0} on: {1}\{2}.", scriptPath, DestinationConnection.DataSource, DestinationConnection.InitialCatalog);

                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception with: {0}.", scriptPath);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
#if DEBUG
                    throw;
#endif
                }
                finally
                {
                    Console.WriteLine(@"Completed: {0} on: {1}\{2}.", scriptPath, DestinationConnection.DataSource, DestinationConnection.InitialCatalog);
                    Console.WriteLine("\tExecution Time: {0:d\\.hh\\:mm\\:ss}.\n\tResults located at: {1}.", DateTime.Now - process.StartTime, resultFilePath);
                }
            }
        }

        private static void WriteTempFile(string text)
        {
            using (var sw = new StreamWriter("C:\\Temp\\debug.sql", false))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        private static void CreateBatchFile()
        {
            var batchFileName = string.Format(@"{0}\load.bat", OutputFilePath);

            using (var outputFile = new StreamWriter(batchFileName, false))
            {
                outputFile.WriteLine("echo off");
                outputFile.WriteLine("echo Please Type the Server Name or IP of the SQL server to load to:");
                outputFile.WriteLine("set /p server=");
                outputFile.WriteLine("echo Starting Data Load...");

                foreach (var script in Scripts)
                {
                    var scriptFileName = string.Format(@"{0}-{1}.sql", DestinationConnection.InitialCatalog, script.Name);
                    var resultsFileName = string.Format(@"{0}-{1}-Results.txt", DestinationConnection.InitialCatalog, script.Name);

                    outputFile.WriteLine("echo Loading {0}-{1}.sql", DestinationConnection.InitialCatalog, script.Name);
                    outputFile.WriteLine("sqlcmd -S %server% -i \"{0}\" -o \"{1}\" -x", scriptFileName, resultsFileName);
                }

                outputFile.WriteLine("pause");
            }
        }

        private static void CreateBatchFileWithCredentials()
        {
            var batchFileName = string.Format(@"{0}\load-with-credentials.bat", OutputFilePath);

            using (var outputFile = new StreamWriter(batchFileName, false))
            {
                outputFile.WriteLine("echo off");
                outputFile.WriteLine("echo Please Type the Server Name or IP of the SQL server to load to:");
                outputFile.WriteLine("set /p server=");
                outputFile.WriteLine("echo Please Type your Username:");
                outputFile.WriteLine("set /p uname=");
                outputFile.WriteLine("echo Please Type your Password:");
                outputFile.WriteLine("set /p pwd=");
                outputFile.WriteLine("echo Starting Data Load...");

                foreach (var script in Scripts)
                {
                    var scriptFileName = string.Format(@"{0}-{1}.sql", DestinationConnection.InitialCatalog, script.Name);
                    var resultsFileName = string.Format(@"{0}-{1}-Results.txt", DestinationConnection.InitialCatalog, script.Name);

                    outputFile.WriteLine("echo Loading {0}-{1}.sql", DestinationConnection.InitialCatalog, script.Name);
                    outputFile.WriteLine("sqlcmd -S %server% -U %uname% -P %pwd% -i \"{0}\" -o \"{1}\" -x", scriptFileName, resultsFileName);
                }

                outputFile.WriteLine("pause");
            }
        }
    }
}
