using Portal.Model;
using Portal.Model.Rules;
using System.Collections.Generic;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface IRuleServiceChannel : IRuleService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IRuleService
    {
        [OperationContract]
        Ruleset GetRuleSet(RulesetRequest request);

        [OperationContract]
        IEnumerable<Ruleset> GetRuleSets();

        [OperationContract]
        void SaveRuleSet(ref Ruleset ruleSet);

        [OperationContract]
        void DeleteRuleSet(RulesetRequest request);

        [OperationContract]
        void ExecuteRuleSet(RulesetRequest request);
    }
}
