using System;
using System.Linq;
using ComponentSpace.SAML2.Configuration;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Model.App;
using Portal.Services.Contracts;
using Portal.Web.Common.Filters.Api;
using System.Collections.Generic;
using System.Web.Http;
using Portal.Web.Common.Helpers;
using WebGrease.Css.Extensions;
using Environment = Portal.Model.App.Environment;

namespace Portal.Web.Admin.Controllers
{
    [RoutePrefix("api/app")]
    public class ConfigurationsController : BaseApiController
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationsController(IConfigurationService configurationService, IUserService userService, ILogger logger) 
            : base(userService, logger)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        [Route("configurations")]
        [AllowAnonymous]
        public IEnumerable<Configuration> GetConfigurations([FromUri]ConfigurationRequest request)
        {
            var configurations = _configurationService.GetConfigurations(request).ToList();
            var environments = GetEnvironments().ToList();

            foreach (var config in configurations)
            {
                var configSettings = config.ConfigurationSettings.ToList();

                foreach(var env in environments)
                {
                    var emptySettings = config.ConfigurationType.Settings.Where(s => 
                            !configSettings.Any(cs => cs.EnvironmentID == env.EnvironmentID && cs.SettingID == s.SettingID));

                    emptySettings.ForEach(es =>
                    {
                        configSettings.Add(new ConfigurationSetting
                        {
                            ConfigurationID = config.ConfigurationID,
                            EnvironmentID = env.EnvironmentID,
                            Environment = env,
                            SettingID = es.SettingID,
                            Setting = es
                        });
                    });
                }

                config.ConfigurationSettings = configSettings.OrderBy(cs => cs.SettingID).ToList();
            }

            return configurations;
        }

        [HttpGet]
        [Route("settings")]
        public IEnumerable<Setting> GetSettings([FromUri]string configurationType)
        {
            return _configurationService.GetSettings(configurationType);
        }

        [HttpGet]
        [Route("environments")]
        public IEnumerable<Environment> GetEnvironments()
        {
            return _configurationService.GetEnvironments();
        }

        [HttpPut]
        [Route("configurations/{id:int}/validate")]
        [PortalAuthorize(PortalRoleValues.AffiliateAdmin)]
        public IDictionary<string, List<string>> ValidateConfiguration(int id, [FromBody] Configuration configuration)
        {
            var configTypeObject = Activator.CreateInstance(configuration.ConfigurationType.AssemblyName, configuration.ConfigurationType.ClassName).Unwrap();

            return Validate(configuration, (dynamic)configTypeObject);
        }

        [HttpPut]
        [Route("configurations/{id:int}")]
        [PortalAuthorize(PortalRoleValues.AffiliateAdmin)]
        public void SaveConfiguration(int id, [FromBody] Configuration configuration)
        {
            configuration.ConfigurationSettings.ForEach(cs =>
            {
                if (cs.ConfigurationSettingID == 0)
                    AddAuditData(cs);

                UpdateAuditData(cs);
            });

            _configurationService.SaveConfiguration(ref configuration);
        }

        private static IDictionary<string, List<string>> Validate<T>(Configuration configuration, T configTypeObject) where T : new()
        {
            return configuration.ConfigurationSettings.Select(cs => new { cs.EnvironmentID, cs.Environment.Name }).Distinct()
                                        .ToDictionary(environment => environment.Name,
                                            environment => configuration.Validate<T>((Environments)environment.EnvironmentID));
        }
    }
}