using Portal.Model;

namespace Portal.Web.Areas.Retirement.Models
{
    public class ResourcesViewModel
    {
        public int SiteID { get; set; }
        public string CssClass { get; set; }
        public SiteMap SiteMap { get; set; }
        public int Columns { get; set; }

        public ResourcesViewModel()
        {
            Columns = 1;
        }
    }
}