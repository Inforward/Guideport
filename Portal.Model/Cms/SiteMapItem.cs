using System.Collections.Generic;
using Portal.Model.Interfaces;

namespace Portal.Model
{
    public class SiteMapItem : IHierarchy<SiteMapItem>
    {
        public SiteMapItem()
        {
            Children = new List<SiteMapItem>();
        }

        public int SiteContentID { get; set; }
        public int? SiteContentParentID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string IconCssClass { get; set; }
        public bool IsActive { get; set; }
        public bool IsMenuVisible { get; set; }
        public SiteMapItem Parent { get; set; }
        public List<SiteMapItem> Children { get; set; }
    }

    public static class SiteMapItemExtensions
    {
        
    }
}
