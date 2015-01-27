using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace Portal.Model.Planning
{
    [Serializable]
    public partial class Step
    {
        public Step()
        {
            ActionItems = new List<ActionItem>();
        }

        public int PlanningWizardStepID { get; set; }
        public int PlanningWizardPhaseID { get; set; }
        public int StepNo { get; set; }
        public decimal StepWeight { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<ActionItem> ActionItems { get; set; }
        [XmlIgnore]
        public virtual Phase Phase { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public string Notes { get; set; }
        [NotMapped]
        public decimal PercentComplete 
        {
            get
            {
                return ActionItems.Count(a => a.IsComplete) == ActionItems.Count ? Math.Round(StepWeight * 100) : 0M;
            }
            set { } // Required for serialization
        }
    }
}
