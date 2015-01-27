using System.Collections.Generic;
using Portal.Model;
using Portal.Model.Report;

namespace Portal.Web.Admin.Models
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            Columns = new List<ViewColumn>();
            Filters = new List<Filter>();
            Pager = new Pager();
        }

        public int ReportID { get; set; }
        public string FullName { get; set; }
        public int PageSize { get; set; }
        public bool IsSortable { get; set; }
        public bool IsPageable { get; set; }
        public Pager Pager { get; set; }
        public ICollection<ViewColumn> Columns { get; set; }
        public ICollection<Filter> Filters { get; set; }
    }
}