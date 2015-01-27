using Portal.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Objective : Auditable, IBusinessPlanEntity
    {
        public Objective()
        {
            Strategies = new List<Strategy>();
        }

        public int ObjectiveID { get; set; }

        [ForeignKey("BusinessPlan")]
        public int? BusinessPlanID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string BaselineValue { get; set; }
        public DateTime? BaselineValueDate { get; set; }
        public string DataType { get; set; }
        public int PercentComplete { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public int? SortOrder { get; set; }
        public bool AutoTrackingEnabled { get; set; }
        public virtual List<Strategy> Strategies { get; set; }

        [NotMapped]
        public string CurrentValue { get; set; }

        [NotMapped]
        public DateTime? CurrentValueDate { get; set; }

        [IgnoreDataMember]
        public ICollection<AffiliateObjective> AffiliateObjectives { get; set; }

        [IgnoreDataMember]
        public BusinessPlan BusinessPlan { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Objective)
            {
                return (obj as Objective).ObjectiveID == ObjectiveID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ObjectiveID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
