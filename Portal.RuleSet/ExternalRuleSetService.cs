using System;
using System.Globalization;
using System.Linq;
using System.Workflow.Runtime.Hosting;
using Portal.Model.Rules;
using Portal.Data.Sql.EntityFramework.Rules;

namespace Portal.RuleSet
{
    public class ExternalRuleSetService : WorkflowRuntimeService
    {
        private readonly RulesRepository _rulesRepository = new RulesRepository();

        public System.Workflow.Activities.Rules.RuleSet GetRuleSet(RuleSetInfo ruleSetInfo)
        {
            if (ruleSetInfo == null) return null;

            Ruleset ruleSet;

            var query = _rulesRepository.FindBy<Ruleset>(r => r.Name == ruleSetInfo.Name);

            if (!(ruleSetInfo.MajorVersion == 0 && ruleSetInfo.MinorVersion == 0))
            {
                ruleSet = query.FirstOrDefault(r => r.MajorVersion == ruleSetInfo.MajorVersion && r.MinorVersion == ruleSetInfo.MinorVersion);
            }
            else
            {
                ruleSet = query.FirstOrDefault();
            }

            if (ruleSet != null)
            {
                var data = new RuleSetData()
                {
                    Name = ruleSet.Name,
                    OriginalName = ruleSet.Name,
                    MajorVersion = ruleSet.MajorVersion,
                    OriginalMajorVersion = ruleSet.MajorVersion,
                    MinorVersion = ruleSet.MinorVersion,
                    OriginalMinorVersion = ruleSet.MinorVersion,
                    RuleSetDefinition = ruleSet.RuleSetDefinition,
                    Status = ruleSet.Status ?? 1,
                    AssemblyPath = ruleSet.AssemblyPath,
                    ActivityName = ruleSet.ActivityName,
                    ModifiedDate = ruleSet.ModifiedDate ?? DateTime.Now,
                    Dirty = false
                };

                return data.RuleSet;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No RuleSets exist with this name: '{0}'", ruleSetInfo.Name));
        }
    }
}
