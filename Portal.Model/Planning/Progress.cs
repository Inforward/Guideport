using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Portal.Model.Planning
{
    [Serializable]
    public partial class Progress
    {
        public Progress()
        {
            Phases = new List<Phase>();
        }

        public int PlanningWizardProgressID { get; set; }
        public int PlanningWizardID { get; set; }
        public int UserID { get; set; }
        public int CurrentPlanningWizardPhaseID { get; set; }
        public decimal PercentComplete { get; set; }
        public string ProgressXml { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime ModifyDateUtc { get; set; }
        public virtual Wizard Wizard { get; set; }
        public virtual Phase CurrentPhase { get; set; }

        [NotMapped]
        public List<Phase> Phases { get; set; }
    }

    public static class ProgressExtensions
    {
        public static IEnumerable<ActionItem> GetActionItems(this Progress progress)
        {
            var list = new List<ActionItem>();

            foreach (var step in progress.Phases.SelectMany(phase => phase.Steps))
            {
                list.AddRange(step.ActionItems);
            }

            return list;
        }

        public static bool IsComplete(this Progress progress)
        {
            return progress.Phases.Sum(p => p.PercentComplete) >= 100;
        }
    }
}
