using System.Globalization;
using System.Text;
using System.Workflow.Activities.Rules;
using Portal.Data;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Helpers;
using Portal.Model.Rules;
using Portal.RuleSet;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Domain.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRulesRepository _rulesRepository;
        private readonly ICacheStorage _cacheStorage;

        public RuleService(IRulesRepository rulesRepository, ICacheStorage cacheStorage)
        {
            _rulesRepository = rulesRepository;
            _cacheStorage = cacheStorage;
        }

        public Ruleset GetRuleSet(RulesetRequest request)
        {
            if (request == null) return null;

            var query = _rulesRepository.FindBy<Ruleset>(r => r.Name == request.Name);

            if (!(request.MajorVersion == 0 && request.MinorVersion == 0))
                query = query.Where(r => r.MajorVersion == request.MajorVersion && r.MinorVersion == request.MinorVersion);

            return query.FirstOrDefault();
        }

        public IEnumerable<Ruleset> GetRuleSets()
        {
            return _rulesRepository.GetAll<Ruleset>()
                                .OrderBy(r => r.Name)
                                .ThenBy(r => r.MajorVersion)
                                .ThenBy(r => r.MinorVersion)
                                .ToList();
        }

        public void SaveRuleSet(ref Ruleset ruleSet)
        {
            if (ruleSet == null) return;

            var ruleName = ruleSet.Name;
            var ruleMajorVersion = ruleSet.MajorVersion;
            var ruleMinorVersion = ruleSet.MinorVersion;

            var existingRuleSet = _rulesRepository.FindBy<Ruleset>(r => r.Name == ruleName && r.MajorVersion == ruleMajorVersion && r.MinorVersion == ruleMinorVersion).FirstOrDefault();

            if (existingRuleSet != null)
            {
                var history = new RulesetHistory()
                {
                    Name = existingRuleSet.Name,
                    MajorVersion = existingRuleSet.MajorVersion,
                    MinorVersion = existingRuleSet.MinorVersion,
                    RuleSet = existingRuleSet.RuleSetDefinition,
                    Status = existingRuleSet.Status,
                    AssemblyPath = existingRuleSet.AssemblyPath,
                    ActivityName = existingRuleSet.ActivityName,
                    ModifiedBy = existingRuleSet.ModifiedBy,
                    ModifiedDate = DateTime.Now
                };

                _rulesRepository.Add(history);
                _rulesRepository.Update(ruleSet);
                _rulesRepository.Save();
            }
            else
            {
                _rulesRepository.Add(ruleSet);
                _rulesRepository.Save();
            }
        }

        public void DeleteRuleSet(RulesetRequest request)
        {
            if (request == null) return;

            var ruleSet = _rulesRepository.FindBy<Ruleset>(r => r.Name == request.Name && r.MajorVersion == request.MajorVersion && r.MinorVersion == request.MinorVersion).FirstOrDefault();

            if (ruleSet != null)
            {
                var history = new RulesetHistory()
                {
                    Name = ruleSet.Name,
                    MajorVersion = ruleSet.MajorVersion,
                    MinorVersion = ruleSet.MinorVersion,
                    RuleSet = ruleSet.RuleSetDefinition,
                    Status = ruleSet.Status,
                    AssemblyPath = ruleSet.AssemblyPath,
                    ActivityName = ruleSet.ActivityName,
                    ModifiedBy = ruleSet.ModifiedBy,
                    ModifiedDate = DateTime.Now
                };

                _rulesRepository.Add(history);
                _rulesRepository.Delete(ruleSet);
                _rulesRepository.Save();
            }
        }

        public void ExecuteRuleSet(RulesetRequest request)
        {
            //if (request == null || string.IsNullOrEmpty(request.Name) || request.Entity == null) return;

            var cacheKey = string.Format("Rules_{0}", request.Name);

            var ruleSet = _cacheStorage.Retrieve(cacheKey, () =>
            {
                var extRuleService = new ExternalRuleSetService();

                return extRuleService.GetRuleSet(new RuleSetInfo{ Name = request.Name });
            });

            if (ruleSet == null)
                throw new ApplicationException("Could not retrieve rule set for rule name: " + request.Name);

            var entity = request.GetEntity();
            var ruleValidation = new RuleValidation(entity.GetType(), null);

            if (ruleSet.Validate(ruleValidation))
            {
                var ruleExecution = new RuleExecution(ruleValidation, entity);

                ruleSet.Execute(ruleExecution);
            }
            else
            {
                var sb = new StringBuilder();

                foreach (var error in ruleValidation.Errors)
                {
                    sb.AppendLine(string.Format("{0}:{1}", error.ErrorNumber, error.ErrorText));
                }

                throw new ApplicationException(sb.ToString());
            }
        }
    }
}
