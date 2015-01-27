using Portal.Data.Sql.EntityFramework;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.ServiceModel.Configuration;

namespace CmsFileMigration.UploadFilesToDb
{
    public static class Utilities
    {
        private static readonly string ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;

        public static string GetDataSource(string connectionStringName)
        {
            var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);

            return string.Format("{0}:{1}", sqlConnection.DataSource, sqlConnection.Database);
        }

        public static string GetEndpointAddress(string endpointName)
        {
            var address = string.Empty;

            var clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

            if (clientSection != null)
                foreach (ChannelEndpointElement endpoint in clientSection.Endpoints)
                {
                    if (endpoint.Name.Equals(endpointName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        address = endpoint.Address.AbsoluteUri;
                        break;
                    }
                }

            return address;
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
            var logRepository = new LogRepository();

            logRepository.Log(new EventLog()
            {
                EventTypeID = (byte)eventType,
                EventDate = DateTime.Now,
                ServerName = Environment.MachineName,
                Message = string.Format(messageFormat, args),
                ErrorText = ex != null ? ex.FullMessage() : null,
                StackTrace = ex != null ? ex.StackTrace : null,
                ScriptName = ApplicationName
            });

            Console.ForegroundColor = eventType == EventTypes.Error ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.WriteLine(messageFormat, args);

            if (writer != null)
                writer.WriteLine(messageFormat, args);
        }
    }
}
