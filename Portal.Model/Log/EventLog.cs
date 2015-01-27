using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Model
{
    public class EventLog
    {
        public int EventLogID { get; set; }
        public byte EventTypeID { get; set; }
        public DateTime EventDate { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string RemoteIP { get; set; }
        public string BrowserType { get; set; }
        public string RequestMethod { get; set; }
        public string ScriptName { get; set; }
        public string QueryString { get; set; }
        public string PostData { get; set; }
        public string Referer { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
    }
}
