using System.Collections.Generic;

namespace Portal.Web.Areas.Pentameter.Models.Planning
{
    public class StatusViewModel
    {
        public string Name { get; set; }
        public string ProgressName { get; set; }
        public bool Enabled { get; set; }
        public decimal PercentComplete { get; set; }
        public string PercentCompleteText { get; set; }
        public string InstructionalText { get; set; }
        public List<StatusViewModel> Phases { get; set; }

        public StatusViewModel()
        {
            Phases = new List<StatusViewModel>();
        }
    }
}