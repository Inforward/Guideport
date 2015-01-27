using System.Collections.Generic;
using System.Web.Mvc;
using Portal.Model;

namespace Portal.Web.Areas.Pentameter.Models
{
    public class MyBusinessPlansViewModel
    {
        public List<SelectListItem> PlanYears { get; set; }
        public BusinessPlan BusinessPlan { get; set; }
        public string ExportUrl { get; set; }
        public string EditUrl { get; set; }

        public MyBusinessPlansViewModel()
        {
            PlanYears = new List<SelectListItem>();
        }
    }
}