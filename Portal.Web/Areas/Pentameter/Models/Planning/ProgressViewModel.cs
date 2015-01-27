using System.Collections.Generic;

namespace Portal.Web.Areas.Pentameter.Models.Planning
{
    public class ProgressViewModel
    {
        public ProgressViewModel()
        {
            Phases = new List<PhaseViewModel>();
        }

        public int ProgressId { get; set; }
        public int CurrentPhaseId { get; set; }
        public decimal PercentComplete { get; set; }
        public List<PhaseViewModel> Phases { get; set; }
    }
}