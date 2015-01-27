using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Portal.Model;

namespace Portal.Infrastructure.Logging
{
    public interface ILogger
    {
        void Log(Exception ex);
        void Log(string message, EventTypes eventType);
        void Log(HttpContext context, Exception ex);
        void Log(HttpContext context, string message, string stackTrace, EventTypes eventType);
        void LogRequest(HttpContext context, int userID, int loginUserID, int portalID, int pageID, int hasAccess);
    }

    public enum EventTypes : byte
    {
        Information = 1,
        Warning = 2,
        Error = 3
    }
}
