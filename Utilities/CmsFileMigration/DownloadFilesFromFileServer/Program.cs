using CmsFileMigration.DownloadFilesFromFileServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CmsFileMigration.DownloadFilesFromFileServer.FirstAllied.FileServer;

namespace CmsFileServerToDb.DownloadCmsFiles
{
    class Program
    {
        private static readonly string ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string DbServer = Utilities.GetDataSource("default");
        private static readonly string DownloadFilePath = (ConfigurationManager.AppSettings["DownloadFilePath"] ?? ".") + @"\CmsFiles";
        private static readonly string SettingsMessage = string.Format("{0} Settings:\r\n\t- Source Database:\t{1}\r\n\t- Download Path:\t{2}",
                                                                            ApplicationName, DbServer, DownloadFilePath);
                                                                            
        private static bool _isRunning;
        static void Main(string[] args)
        {
            Console.WriteLine(SettingsMessage);
            Console.WriteLine("\nPress ENTER to begin to downloading files.");
            Console.ReadLine();

            _isRunning = true;

            Task.Factory.StartNew(() =>
            {
                var spinner = new ConsoleSpinner();

                while (_isRunning)
                {
                    Thread.Sleep(100);
                    spinner.Turn();
                }
            });

            Run();

            _isRunning = false;

            Console.WriteLine();
            Console.WriteLine("\nPress ENTER to exit.");
            Console.ReadLine();
        }

        private static void Run()
        {
            var fileServer = new FileIOSoapClient();
            var files = new Dictionary<int, string>();
            var errorFiles = new List<string>();
            var logFilePath = ConfigurationManager.AppSettings["LogFilePath"] ?? ".";
            var logFileName = string.Format(@"{0}\{1}-{2:yyyyMMdd-hhmmsstt}.txt", logFilePath, ApplicationName, DateTime.Now);

            if (!Directory.Exists(logFilePath))
                Directory.CreateDirectory(logFilePath);

            var logFile = new StreamWriter(logFileName);

            if (!Directory.Exists(DownloadFilePath))
                Directory.CreateDirectory(DownloadFilePath);

            logFile.WriteLine(SettingsMessage);
            logFile.LogInfo("Retrieving files to copy.");

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            using (var cmd = new SqlCommand("SELECT SiteContentID, ContentFilePath FROM AE_Portal.cms.SiteContent WHERE ContentFilePath IS NOT NULL AND SiteID IN (1,2,3) AND SiteContentStatusID <> 3", conn))
            {
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        files.Add((int)dr["SiteContentID"], dr["ContentFilePath"].ToString());
                    }

                    dr.Close();
                }

                conn.Close();
            }

            foreach (var siteContentId in files.Keys)
            {
                var contentFilePath = files[siteContentId];
                var fileContentLocation = Utilities.ParseFileContentLocation(contentFilePath);
                var fileSubdirectory = Utilities.ParseFileSubdirectory(contentFilePath);
                var fileName = Utilities.ParseFileName(contentFilePath);

                logFile.LogInfo("Downloading File: SiteContentID: {0}, File: {1}", siteContentId, fileName);

                var length = fileServer.FileSize(fileContentLocation, fileSubdirectory, fileName);

                if (length <= 0)
                {
                    errorFiles.Add(contentFilePath);
                    logFile.LogError(null, string.Format("\tFile not found: {0}:{1}/{2}", fileContentLocation, fileSubdirectory, fileName));
                    continue;
                }

                using (var memoryStream = new MemoryStream())
                {
                    var chunks = Utilities.CalculateChunks(length);

                    for (var chunk = 0; chunk < chunks; chunk++)
                    {
                        var buffer = fileServer.DownloadChunk(fileContentLocation, fileSubdirectory,
                            fileName, Utilities.DefaultChunkSize, chunk);

                        if (buffer != null)
                            memoryStream.Write(buffer, 0, buffer.Length);
                    }

                    var saveFilePath = string.Format(@"{0}\{1}", DownloadFilePath, siteContentId);

                    if (!Directory.Exists(saveFilePath))
                        Directory.CreateDirectory(saveFilePath);

                    using (var fileStream = new FileStream(string.Format(@"{0}\{1}", saveFilePath, fileName), FileMode.Create))
                    {
                        memoryStream.Position = 0;
                        memoryStream.CopyTo(fileStream);

                        logFile.LogInfo("Downloaded File: {0} ({1})", fileName, fileStream.Length.BytesToString());
                    }
                }
            }

            logFile.LogInfo("\r\n\r\nDone!  Log file saved to: {0}.", Path.GetFullPath(logFileName));
        }
    }
}
