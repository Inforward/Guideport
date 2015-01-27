using Portal.Data.Sql.EntityFramework;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CmsFileMigration.UploadFilesToDb
{
    class Program
    {
        private static readonly string ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string CmsFilePath = ConfigurationManager.AppSettings["CmsFilePath"] ?? ".";
        private static readonly string InnovaServer = Utilities.GetDataSource("default");
        private static readonly string SettingsMessage = string.Format("{0} Settings:\r\n\t- CmsFilePath:\t{1}\r\n\t- Destination:\t{2}",
                                                                            ApplicationName, System.IO.Path.GetFullPath(CmsFilePath), InnovaServer);

        private static bool _isRunning;

        static void Main(string[] args)
        {
            Console.WriteLine(SettingsMessage);
            Console.WriteLine("\nPress ENTER to begin to saving files.");
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
            var cmsRepository = new CmsRepository();
            var fileRepository = new FileRepository();
            var errorFiles = new List<SiteContent>();
            var logFilePath = ConfigurationManager.AppSettings["LogFilePath"] ?? ".";
            var logFileName = string.Format(@"{0}\{1}-{2:yyyyMMdd-hhmmsstt}.txt", logFilePath, ApplicationName, DateTime.Now);
            var logFile = new StreamWriter(logFileName);

            try
            {
                logFile.WriteLine(SettingsMessage);
                logFile.LogInfo("Retrieving files to copy.");

                var directories = Directory.GetDirectories(CmsFilePath);

                foreach (var directory in directories)
                {
                    var file = Directory.GetFiles(directory).FirstOrDefault();

                    if (string.IsNullOrEmpty(file))
                    {
                        logFile.LogError(null, string.Format("Could not find file in directory: {0}", directory));
                    }

                    var fileInfo = new System.IO.FileInfo(file);
                    var contentId = int.Parse(fileInfo.Directory.Name);

                    var content = cmsRepository.FindBy<SiteContent>(s => s.SiteContentID == contentId).FirstOrDefault();

                    logFile.LogInfo("Uploading File: {0} ({1:#,00.00} KB)", fileInfo.Name, fileInfo.Length / 1024);

                    if (content == null)
                    {
                        logFile.LogError(null, string.Format("SiteContent Record not found: {0}", contentId));
                        continue;                        
                    }

                    var fileBytes = System.IO.File.ReadAllBytes(file);

                    var newFile = new Portal.Model.File
                    {
                        Data = fileBytes,
                        Info = new Portal.Model.FileInfo
                        {
                            Name = fileInfo.Name,
                            Extension = Path.GetExtension(fileInfo.Name),
                            SizeBytes = fileBytes.Length,
                            CreateUserID = 0,
                            CreateDate = DateTime.Now
                        }
                    };

                    // Save File
                    fileRepository.Add(newFile);
                    fileRepository.Save();

                    if (newFile.FileID > 0)
                    {
                        content.FileID = newFile.FileID;
                        content.ModifyUserID = 0;
                        content.ModifyDate = DateTime.Now;

                        // Update SiteContent with FileID
                        cmsRepository.Update(content);
                        cmsRepository.Save();
                    }

                    logFile.LogInfo("\tSaved file: {0}, FileID: {1}.", fileInfo.Name, newFile.FileID);
                }

                logFile.LogInfo("Import Completed.\r\n\r\nSummary:\r\nTotal Files: {0}\r\nErrored Files: {1}", directories.Length, errorFiles.Count);

                //if (errorFiles.Count > 0)
                //{
                //    logFile.Log(EventTypes.Warning, null, "Files that could not be imported:");
                //    errorFiles.ForEach(f => logFile.Log(EventTypes.Warning, null, "\tSiteContentID: {0}, File: {1}:{2}/{3}", f.SiteContentID, f.FileContentLocation, f.FileSubdirectory, f.FileName));
                //}

                logFile.LogInfo("\r\n\r\nDone!  Log file saved to: {0}.", Path.GetFullPath(logFileName));
            }
            catch (Exception ex)
            {
                logFile.LogError(ex, "\t{0}", ex.FullMessage());
            }
            finally
            {
                logFile.Dispose();
                cmsRepository.Dispose();
                fileRepository.Dispose();
            }
        }
    }
}