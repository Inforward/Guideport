using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class AffiliateFeatureSetting
    {
        public int AffiliateFeatureID { get; set; }
        public int FeatureSettingID { get; set; }
        public string Value { get; set; }

        [IgnoreDataMember]
        public virtual AffiliateFeature AffiliateFeature { get; set; }
    }
}
