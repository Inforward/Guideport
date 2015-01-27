using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class AffiliateFeature : Auditable
    {
        public AffiliateFeature()
        {
            Settings = new List<AffiliateFeatureSetting>();
        }

        public int AffiliateFeatureID { get; set; }
        [ForeignKey("Affiliate")]
        public int AffiliateID { get; set; }
        public int FeatureID { get; set; }
        public bool IsEnabled { get; set; }
        public Feature Feature { get; set; }
        public List<AffiliateFeatureSetting> Settings { get; set; }

        [IgnoreDataMember]
        public Affiliate Affiliate { get; set; }
    }
}
