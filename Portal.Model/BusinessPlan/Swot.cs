using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Swot : Auditable
    {
        public int SwotID { get; set; }

        [ForeignKey("BusinessPlan")]
        public int BusinessPlanID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public BusinessPlan BusinessPlan { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Swot)
            {
                return (obj as Swot).SwotID == SwotID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return SwotID;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Type, Description);
        }
    }
}
