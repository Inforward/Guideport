using Portal.Data;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Infrastructure.Logging
{
    public class EntityFrameworkLogger : ILogger
    {
        private readonly ILogRepository _logRepository;

        public EntityFrameworkLogger(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void Log(Exception ex)
        {
            Log(null, ex);
        }

        public void Log(string message, EventTypes eventType)
        {
            Log(null, message, string.Empty, eventType);
        }

        public void Log(HttpContext context, Exception ex)
        {
            Log(context, ex.FullMessage(), ex.StackTrace, EventTypes.Error);
        }

        public void Log(HttpContext context, string message, string stackTrace, EventTypes eventType)
        {
            context = context ?? HttpContext.Current;

            if (context == null)
                return;

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var requestInfo = RequestInfo.FromHttpContext(context);

                        _logRepository.Log(new Model.EventLog()
                        {
                            EventTypeID = (byte)eventType,
                            EventDate = DateTime.Now,
                            UserID = requestInfo.UserID,
                            Message = message,
                            ErrorText = eventType == EventTypes.Error ? message : null,
                            ServerName = requestInfo.ServerName,
                            ServerIP = requestInfo.ServerIP,
                            RemoteIP = requestInfo.RemoteIP,
                            BrowserType = requestInfo.Browser,
                            RequestMethod = requestInfo.RequestMethod,
                            ScriptName = requestInfo.ScriptName,
                            QueryString = requestInfo.QueryString,
                            PostData = requestInfo.PostData,
                            Referer = requestInfo.Referrer,
                            StackTrace = stackTrace,
                            Source = requestInfo.Source
                        });
                    }
                    catch (Exception ex)
                    {
                        LogToEventLog(message + Environment.NewLine + stackTrace);
                        LogToEventLog(ex);
                    }
                });
        }

        public void LogRequest(HttpContext context, int userID, int loginUserID, int portalID, int pageID, int hasAccess)
        {
            throw new NotImplementedException();
        }

        private void LogToEventLog(Exception ex)
        {
            LogToEventLog(ex.FullMessage());
        }

        private void LogToEventLog(string message)
        {
            try
            {
                const string source = "Portal.Web";
                const string log = "Application";

                if (!System.Diagnostics.EventLog.SourceExists(source))
                    System.Diagnostics.EventLog.CreateEventSource(source, log);

                System.Diagnostics.EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }
            catch { }
        }
    }
}
