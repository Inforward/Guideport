
using Portal.Model;

namespace Portal.Data
{
    public interface IAffiliateRepository : IEntityRepository
    {
        void SaveAffiliateFeature(AffiliateFeature feature);
    }
}
