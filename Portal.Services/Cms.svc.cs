using Portal.Data;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;

namespace Portal.Services
{
    public class Cms : CmsService
    {
        public Cms(ICmsRepository cmsRepository, ICacheStorage cacheStorage)
            : base(cmsRepository, cacheStorage)
        { }
    }
}
