using Portal.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Tactic : Auditable, IBusinessPlanEntity
    {
        public Tactic()
        {
            Strategies = new List<Strategy>();
        }

        public int TacticID { get; set; }

        [ForeignKey("BusinessPlan")]
        public int? BusinessPlanID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool Editable { get; set; }

        [IgnoreDataMember]
        public BusinessPlan BusinessPlan { get; set; }
        [IgnoreDataMember]
        public List<Strategy> Strategies { get; set; }

        [NotMapped]
        public bool IsCompleted
        {
            get { return CompletedDate.HasValue; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Tactic)
            {
                return (obj as Tactic).TacticID == TacticID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return TacticID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
