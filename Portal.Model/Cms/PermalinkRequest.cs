
namespace Portal.Model
{
    public class PermalinkRequest
    {
        public string Title { get; set; }
        public int? SiteContentParentID { get; set; }
        public int SiteID { get; set; }
        public int SiteContentTypeID { get; set; }
    }
}
