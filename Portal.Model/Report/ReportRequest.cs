using System;
using System.Collections.Generic;

namespace Portal.Model.Report
{
    public class ReportRequest
    {
        public int ViewID { get; set; }
        public List<Filter> Filters { get; set; }
        public Pager Pager { get; set; }
        public bool FormatResults { get; set; }
    }
}