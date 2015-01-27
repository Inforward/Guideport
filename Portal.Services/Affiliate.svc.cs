using Portal.Data;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;

namespace Portal.Services
{
    public class Affiliate : AffiliateService
    {
        public Affiliate(IAffiliateRepository affiliateRepository, ICacheStorage cacheStorage)
            : base(affiliateRepository, cacheStorage)
        { }
    }
}