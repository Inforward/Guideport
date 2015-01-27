using System.Collections.Generic;

namespace Portal.Web.Areas.Pentameter.Models.Planning
{
    public class PhaseViewModel
    {
        public int PhaseId { get; set; }
        public int SortOrder { get; set; }
        public int Number { get; set; }
        public List<StepViewModel> Steps { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public bool Selected { get; set; }

        public PhaseViewModel()
        {
            Steps = new List<StepViewModel>();
        }
    }
}