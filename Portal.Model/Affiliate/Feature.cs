using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Feature
    {
        public Feature()
        {
            Settings = new List<FeatureSetting>();
            AffiliateFeatures = new List<AffiliateFeature>();
        }

        public int FeatureID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<FeatureSetting> Settings { get; set; }

        [IgnoreDataMember]
        public ICollection<AffiliateFeature> AffiliateFeatures { get; set; }
    }

    public enum Features
    {
        Connect2Clients = 1,
        LoginPage = 2,
        QlikView = 3
    }
}
