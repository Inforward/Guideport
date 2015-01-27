using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace Portal.Model.Planning
{
    [Serializable]
    public partial class Phase
    {
        public Phase()
        {
            Progresses = new List<Progress>();
            Steps = new List<Step>();
        }

        public int PlanningWizardPhaseID { get; set; }
        public int PlanningWizardID { get; set; }
        public string Name { get; set; }
        public string NameHtml { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        [XmlIgnore]
        public virtual Wizard Wizard { get; set; }
        [XmlIgnore]
        public virtual List<Progress> Progresses { get; set; }
        public virtual List<Step> Steps { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public decimal PercentComplete
        {
            get { return Steps.Sum(s => s.PercentComplete); }
            set { } // Required for serialization
        }
    }
}
