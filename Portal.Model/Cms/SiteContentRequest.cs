using System.Collections.Generic;


namespace Portal.Model
{
    public class SiteContentRequest
    {
        public SiteContentRequest()
        {
            SearchTerms = new List<string>();
            ContentStatuses = new List<ContentStatus>();
            ContentDocumentTypes = new List<ContentDocumentType>();
            ContentTypes = new List<ContentType>();
        }

        public int? SiteID { get; set; }
        public int? SiteGroupID { get; set; }
        public int? SiteContentID { get; set; }
        public int ProfileTypeID { get; set; }
        public int AffiliateID { get; set; }
        public int? MaxContentCharacters { get; set; }
        public string SiteName { get; set; }
        public string Permalink { get; set; }
        public string ContentFilePath { get; set; }

        public bool IncludeSite { get; set; }
        public bool IncludeSiteContents { get; set; }
        public bool IncludeSiteTemplates { get; set; }
        public bool IncludeProfileTypes { get; set; }
        public bool IncludeAffiliates { get; set; }
        public bool IncludeSiteDocumentType { get; set; }
        public bool IncludeKnowledgeLibraries { get; set; }
        public bool IncludeSiteContentHistories { get; set; }
        public bool IncludeSiteContentStatus { get; set; }
        public bool IncludeSiteContentType { get; set; }
        public bool IncludeParents { get; set; }
        public bool IncludeVersions { get; set; }
        public bool IncludeFileInfo { get; set; }

        public List<string> SearchTerms { get; set; }
        public List<ContentStatus> ContentStatuses { get; set; }
        public List<ContentDocumentType> ContentDocumentTypes { get; set; }
        public List<ContentType> ContentTypes { get; set; }
    }
}
