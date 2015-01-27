using Portal.Model;
using System;

namespace Portal.Web.Models
{
    public class SearchFilters : Pager
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupID { get; set; }
        public string AffiliateID { get; set; }
        public bool IncludeTerminated { get; set; }
        public bool ExcludeNoData { get; set; }
        public int Year { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}