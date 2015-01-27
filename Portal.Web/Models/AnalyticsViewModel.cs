using Newtonsoft.Json;

namespace Portal.Web.Models
{
    public class AnalyticsViewModel
    {
        [JsonProperty(PropertyName = "dimension1")]
        public string ProfileID { get; set; }
        [JsonProperty(PropertyName = "dimension2")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "dimension3")]
        public string ProfileType { get; set; }
        [JsonProperty(PropertyName = "dimension4")]
        public string SiteName { get; set; }
        [JsonProperty(PropertyName = "dimension5")]
        public string AdvisorName { get; set; }
        [JsonProperty(PropertyName = "dimension6")]
        public string BusinessConsultantName { get; set; }
        [JsonProperty(PropertyName = "dimension7")]
        public string AdvisorAffiliateName { get; set; }
    }
}