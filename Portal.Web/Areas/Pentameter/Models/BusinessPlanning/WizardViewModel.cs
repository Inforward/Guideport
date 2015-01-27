using System.Collections.Generic;
using System.Web.Mvc;

namespace Portal.Web.Areas.Pentameter.Models
{
    public class WizardViewModel
    {
        public List<SelectListItem> ExistingPlanYears { get; set; }
        public List<SelectListItem> AvailablePlanYears { get; set; }

        public WizardViewModel()
        {
            ExistingPlanYears = new List<SelectListItem>();
            AvailablePlanYears = new List<SelectListItem>();
        }
    }
}