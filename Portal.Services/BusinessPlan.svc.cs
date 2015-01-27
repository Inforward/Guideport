using Portal.Data;
using Portal.Domain.Services;
using Portal.Services.Contracts;

namespace Portal.Services
{
    public class BusinessPlan : BusinessPlanService
    {
        public BusinessPlan(IBusinessPlanRepository businessPlanRepository, IUserService userService) 
            : base(businessPlanRepository, userService)
        { }
    }
}
