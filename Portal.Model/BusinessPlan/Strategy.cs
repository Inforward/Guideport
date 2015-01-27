using Portal.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Strategy : Auditable, IBusinessPlanEntity
    {
        public Strategy()
        {
            Tactics = new List<Tactic>();
            Objectives = new List<Objective>();
        }

        public int StrategyID { get; set; }

        [ForeignKey("BusinessPlan")]
        public int? BusinessPlanID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SortOrder { get; set; }
        public bool Editable { get; set; }
        public virtual List<Tactic> Tactics { get; set; }
        
        [IgnoreDataMember]
        public List<Objective> Objectives { get; set; }

        [IgnoreDataMember]
        public BusinessPlan BusinessPlan { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Strategy)
            {
                return (obj as Strategy).StrategyID == StrategyID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return StrategyID;
        }

        public override string ToString()
        {
            return Name;
        }
        
    }
}
