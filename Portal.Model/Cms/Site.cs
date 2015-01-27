using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Site
    {
        public Site()
        {
            KnowledgeLibraryTopics = new List<KnowledgeLibraryTopic>();
            SiteContents = new List<SiteContent>();
            SiteTemplates = new List<SiteTemplate>();
        }

        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string DomainName { get; set; }
        public string ApplicationRootPath { get; set; }
        public int? DefaultSiteTemplateID { get; set; }
        public int? DefaultSiteContentID { get; set; }
        public ICollection<SiteContent> SiteContents { get; set; }
        public ICollection<SiteTemplate> SiteTemplates { get; set; }

        [IgnoreDataMember]
        public ICollection<KnowledgeLibraryTopic> KnowledgeLibraryTopics { get; set; }
    }

    public enum Sites
    {
        Guideport = 1,
        Pentameter = 2,
        GuidedRetirementSolutions = 3,
        Unknown = 99
    }
}
