using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Model.Planning
{
    [Serializable]
    public partial class Wizard
    {
        public Wizard()
        {
            Phases = new List<Phase>();
            Progresses = new List<Progress>();
        }

        public int PlanningWizardID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompleteMessage { get; set; }
        public virtual List<Phase> Phases { get; set; }
        public virtual List<Progress> Progresses { get; set; }

        [NotMapped]
        public PlanningType PlanningType
        {
            get { return (PlanningType) PlanningWizardID; }
        }

        [NotMapped]
        public Progress Progress { get; set; }
    }

    public enum PlanningType
    {
        Acquisition = 1,
        Continuity = 2,
        Succession = 3
    }

    public enum PlanningInterest
    {
        Unknown = 1,
        InterestedIncomplete = 2,
        InterestedComplete = 3,
        NotInterested = 4
    }
}
