using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Admin.Models
{
    public class SearchContentRequest
    {
        public int SiteID { get; set; }
        public int SiteContentStatusID { get; set; }
        public string SearchText { get; set; }
        public bool OrderByHierarchy { get; set; }
    }
}