using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Portal.Model.Planning
{
    [Serializable]
    public partial class ActionItem
    {
        public int PlanningWizardActionItemID { get; set; }
        public int PlanningWizardStepID { get; set; }
        public string ActionItemText { get; set; }
        public int SortOrder { get; set; }
        public string ResourcesContent { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        [XmlIgnore]
        public virtual Step Step { get; set; }

        [NotMapped]
        public bool IsComplete { get; set; }
        [NotMapped]
        public DateTime? CompleteDate { get; set; }
        [NotMapped]
        public DateTime? CompleteDateUtc { get; set; }
    }
}
