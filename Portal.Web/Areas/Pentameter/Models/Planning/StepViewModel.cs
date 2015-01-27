using System.Collections.Generic;

namespace Portal.Web.Areas.Pentameter.Models.Planning
{
    public class StepViewModel
    {
        public int StepId { get; set; }
        public int PhaseId { get; set; }
        public int StepNo { get; set; }
        public decimal StepWeight { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string LearnWhyHtml { get; set; }
        public bool Selected { get; set; }
        public List<ActionItemViewModel> ActionItems { get; set; }

        public StepViewModel()
        {
            ActionItems = new List<ActionItemViewModel>();
        }
    }
}