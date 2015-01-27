using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace CmsFileMigration.DownloadFilesFromFileServer
{
    public static class Utilities
    {
        private static readonly string ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
        public const int DefaultChunkSize = 1000000;

        public static string GetDataSource(string connectionStringName)
        {
            var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);

            return string.Format("{0}:{1}", sqlConnection.DataSource, sqlConnection.Database);
        }

        public static void LogInfo(this TextWriter writer, string messageFormat, params object[] args)
        {
            writer.Log(EventTypes.Information, null, messageFormat, args);
        }

        public static void LogError(this TextWriter writer, Exception ex, string messageFormat, params object[] args)
        {
            writer.Log(EventTypes.Error, ex, messageFormat, args);
        }

        public static void Log(this TextWriter writer, EventTypes eventType, Exception ex, string messageFormat, params object[] args)
        {
            Console.ForegroundColor = eventType == EventTypes.Error ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.WriteLine(messageFormat, args);

            if (writer != null)
                writer.WriteLine(messageFormat, args);
       
        }

        public static string ParseFileName(string contentFilePath)
        {
            return Path.GetFileName(contentFilePath);
        }

        public static string ParseFileSubdirectory(string contentFilePath)
        {
            var subDirectory = string.Empty;

            if (!string.IsNullOrEmpty(contentFilePath))
            {
                var startIndex = contentFilePath.IndexOf(':') + 1;
                var length = contentFilePath.IndexOf('/') - startIndex;

                subDirectory = contentFilePath.Substring(startIndex, length);
            }

            return subDirectory;
        }

        public static string ParseFileContentLocation(string contentFilePath)
        {
            var contentLocation = string.Empty;

            if (!string.IsNullOrEmpty(contentFilePath))
                contentLocation = contentFilePath.Substring(0, contentFilePath.IndexOf(':'));

            return contentLocation;
        }

        public static int CalculateChunks(long fileSize, int chunkSize = DefaultChunkSize)
        {
            if (fileSize < chunkSize)
                return 1;

            var chunks = fileSize / chunkSize;

            if (fileSize % chunkSize != 0)
                chunks++;

            return (int)chunks;
        }

        public static string BytesToString(this long byteCount)
        {
            var suffixes = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (byteCount == 0)
                return "0" + suffixes[0];

            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + " " + suffixes[place];
        }
    }

    public enum EventTypes : byte
    {
        Information = 1,
        Warning = 2,
        Error = 3
    }
}
