using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class AffiliateLogoType
    {
        public AffiliateLogoType()
        {
            AffiliateLogos = new List<AffiliateLogo>();
        }

        public int AffiliateLogoTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public ICollection<AffiliateLogo> AffiliateLogos { get; set; }
    }
}
