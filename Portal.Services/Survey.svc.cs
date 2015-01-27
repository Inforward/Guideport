using Portal.Data;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Logging;
using Portal.Services.Contracts;

namespace Portal.Services
{
    public class Survey : SurveyService
    {
        public Survey(ISurveyRepository surveyRepository, ICacheStorage cacheStorage, ILogger logger, ICmsService cmsService, IRuleService ruleService, IUserService userService)
            : base(surveyRepository, cacheStorage, logger, cmsService, ruleService, userService)
        { }
    }
}