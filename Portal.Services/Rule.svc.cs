using Portal.Data;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;

namespace Portal.Services
{
    public class Rule : RuleService
    {
        public Rule(IRulesRepository rulesRepository, ICacheStorage cacheStorage)
            : base(rulesRepository, cacheStorage)
        { }
    }
}