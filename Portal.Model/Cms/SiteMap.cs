using System.Collections.Generic;

namespace Portal.Model
{
    public class SiteMap
    {
        public SiteMap()
        {
            Items = new List<SiteMapItem>();
        }

        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteRootUrl { get; set; }
        public string SiteContactUrl { get; set; }
        public string SiteTermsUrl { get; set; }
        public List<SiteMapItem> Items { get; set; }
    }
}
