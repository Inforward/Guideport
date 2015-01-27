using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class AffiliateObjective : Auditable
    {
        [ForeignKey("Affiliate")]
        public int AffiliateID { get; set; }
        public int ObjectiveID { get; set; }
        public bool AutoTrackingEnabled { get; set; }        
        [IgnoreDataMember]
        public Objective Objective { get; set; }
        [IgnoreDataMember]
        public Affiliate Affiliate { get; set; }
        [NotMapped]
        public string ObjectiveName { get; set; }
    }
}
