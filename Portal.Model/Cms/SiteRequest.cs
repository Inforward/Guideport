using System.Collections.Generic;

namespace Portal.Model
{
    public class SiteRequest
    {
        public int? SiteID { get; set; }
        public string SiteName { get; set; }
        public bool IncludeSiteContents { get; set; }
        public bool IncludeSiteTemplates { get; set; }
        public bool IncludeAll { get; set; }
        public bool UseCache { get; set; }
    }
}
