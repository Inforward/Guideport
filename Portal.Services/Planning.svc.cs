using Portal.Data;
using Portal.Domain.Services;
using Portal.Services.Contracts;

namespace Portal.Services
{
    public class Planning : PlanningService
    {
        public Planning(IPlanningRepository planningRepository, ISurveyService surveyService)
            : base(planningRepository, surveyService)
        { }
    }
}
