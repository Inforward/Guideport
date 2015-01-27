using System;

namespace Portal.Model
{
    public class SiteContentViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public int SiteContentID { get; set; }
        public int? SiteContentParentID { get; set; }
        public string Title { get; set; }
        public string TemplatePath { get; set; }
        public string ContentStyles { get; set; }
        public string ContentScripts { get; set; }
        public string ContentHtml { get; set; }
        public SiteMap SiteMap { get; set; }
        public string Url { get; set; }
        public int FileID { get; set; }
        public int SearchRank { get; set; }
        public DateTime ModifyDate { get; set; }
        public ContentType ContentType { get; set; }
        public ContentDocumentType DocumentType { get; set; }
    }
}
