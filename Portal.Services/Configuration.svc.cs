using Portal.Data;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;

namespace Portal.Services
{
    public class Configuration : ConfigurationService
    {
        public Configuration(IConfigurationRepository configurationRepository, ICacheStorage cacheStorage)
            : base(configurationRepository, cacheStorage)
        { }
    }
}