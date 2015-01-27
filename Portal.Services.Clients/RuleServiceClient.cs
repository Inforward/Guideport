using Portal.Model.Rules;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using System.Collections.Generic;

namespace Portal.Services.Clients
{
    public class RuleServiceClient : IRuleService
    {
        private readonly ServiceClient<IRuleServiceChannel> _ruleSetService = new ServiceClient<IRuleServiceChannel>();

        public Ruleset GetRuleSet(RulesetRequest request)
        {
            var proxy = _ruleSetService.CreateProxy();
            return proxy.GetRuleSet(request);
        }

        public IEnumerable<Ruleset> GetRuleSets()
        {
            var proxy = _ruleSetService.CreateProxy();
            return proxy.GetRuleSets();
        }

        public void SaveRuleSet(ref Ruleset ruleSet)
        {
            var proxy = _ruleSetService.CreateProxy();
            proxy.SaveRuleSet(ref ruleSet);
        }

        public void DeleteRuleSet(RulesetRequest request)
        {
            var proxy = _ruleSetService.CreateProxy();
            proxy.DeleteRuleSet(request);
        }

        public void ExecuteRuleSet(RulesetRequest request)
        {
            var proxy = _ruleSetService.CreateProxy();
            proxy.ExecuteRuleSet(request);            
        }
    }
}