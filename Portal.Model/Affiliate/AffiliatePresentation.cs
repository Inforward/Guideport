
using System.Collections.Generic;

namespace Portal.Model
{
    public class AffiliatePresentation
    {
        public Affiliate Affiliate { get; set; }
        public int UserCount { get; set; }
        public List<User> Users { get; set; }
        public List<AffiliateLogo> Logos { get; set; }
        public List<AffiliateFeature> Features { get; set; }
    }
}
