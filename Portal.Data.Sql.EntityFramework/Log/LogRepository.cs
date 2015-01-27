using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Portal.Model;
using Portal.Infrastructure.Helpers;

namespace Portal.Data.Sql.EntityFramework
{
    public class LogRepository : EntityRepository<MasterContext>, ILogRepository
    {
        public void Log(EventLog eventLog)
        {
            // Trim to prevent truncation errors
            eventLog.ServerName.TrimToLength(100, true);
            eventLog.ServerIP.TrimToLength(25, true);
            eventLog.RemoteIP.TrimToLength(25, true);
            eventLog.Message.TrimToLength(2000, true);
            eventLog.ErrorText.TrimToLength(1000, true);
            eventLog.RequestMethod.TrimToLength(50, true);
            eventLog.ScriptName.TrimToLength(300, true);
            eventLog.QueryString.TrimToLength(1000, true);
            eventLog.Referer.TrimToLength(300, true);
            eventLog.BrowserType.TrimToLength(300, true);
            eventLog.Source.TrimToLength(2000, true);

            Add(eventLog);
            Save();
        }
    }
}
