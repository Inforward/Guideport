using Newtonsoft.Json;

namespace Portal.Web.Models
{
    public class JsonSettings
    {
        [JsonProperty(PropertyName = "keepAlive")]
        public KeepAliveSettings KeepAliveSettings { get; set; }

        [JsonProperty(PropertyName = "sessionMonitor")]
        public SessionMonitorSettings SessionMonitorSettings { get; set; }

        [JsonProperty(PropertyName = "autoSave")]
        public AutoSaveSettings AutoSaveSettings { get; set; }

        [JsonProperty(PropertyName = "adminConsoleUrl")]
        public string AdminConsoleUrl { get; set; }
    }

    public class KeepAliveSettings
    {
        [JsonProperty(PropertyName = "interval")]
        public int Interval { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string PingUrl { get; set; }
    }

    public class SessionMonitorSettings
    {
        [JsonProperty(PropertyName = "warningTimeout")]
        public int WarningTimeout { get; set; }

        [JsonProperty(PropertyName = "expirationTimeout")]
        public int ExpirationTimeout { get; set; }
    }

    public class AutoSaveSettings
    {
        [JsonProperty(PropertyName = "interval")]
        public int Interval { get; set; }
    }
}