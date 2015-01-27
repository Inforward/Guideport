using System;
using System.Collections.Generic;

namespace Portal.Web.Areas.Pentameter.Models.Planning
{
    public class WizardViewModel
    {
        public int WizardId { get; set; }
        public int? ProgressId { get; set; }
        public decimal PercentComplete { get; set; }
        public string CompleteMessage { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public List<PhaseViewModel> Phases { get; set; }

        public WizardViewModel()
        {
            Phases = new List<PhaseViewModel>();
        }
    }
}