using Portal.Data;
using Portal.Infrastructure.Caching;
using Portal.Model.App;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Environment = Portal.Model.App.Environment;

namespace Portal.Domain.Services
{
    public class ConfigurationService : BaseService, IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ICacheStorage _cacheStorage;

        public ConfigurationService(IConfigurationRepository appRepository, ICacheStorage cacheStorage)
        {
            _configurationRepository = appRepository;
            _cacheStorage = cacheStorage;
        }

        public IEnumerable<Configuration> GetConfigurations(ConfigurationRequest request)
        {
            return request.UseCache ? _cacheStorage.Retrieve("AppService.Configurations", () => GetConfigurationsInternal(request))
                                    : GetConfigurationsInternal(request);
        }

        public IEnumerable<ConfigurationType> GetConfigurationTypes()
        {
            return _cacheStorage.Retrieve("AppService.ConfigurationTypes", () =>
                        _configurationRepository.GetAll<ConfigurationType>().ToList());
        }

        public IEnumerable<Environment> GetEnvironments()
        {
            return _cacheStorage.Retrieve("AppService.Environments", () => 
                        _configurationRepository.GetAll<Environment>().ToList());
        }

        public IEnumerable<Setting> GetSettings(string configurationTypeName)
        {
            return _configurationRepository
                        .FindBy<Setting>(s => s.ConfigurationType.Name.Equals(configurationTypeName, StringComparison.InvariantCultureIgnoreCase))
                        .Include("ConfigurationType")
                        .ToList();
        }

        public void SaveConfiguration(ref Configuration configuration)
        {
            _configurationRepository.UpdateConfiguration(configuration);
        }

        private IEnumerable<Configuration> GetConfigurationsInternal(ConfigurationRequest request)
        {
            var query = _configurationRepository.GetAll<Configuration>()
                                                .Include("ConfigurationType")
                                                .Include("ConfigurationType.Settings")
                                                .Include("ConfigurationSettings")
                                                .Include("ConfigurationSettings.Environment")
                                                .Include("ConfigurationSettings.Setting");

            if (request.ConfigurationID > 0)
                query = query.Where(c => c.ConfigurationID == request.ConfigurationID);

            if (request.ConfigurationTypeID > 0)
                query = query.Where(c => c.ConfigurationTypeID == request.ConfigurationTypeID);

            if (!string.IsNullOrEmpty(request.ConfigurationName))
                query = query.Where(c => c.Name.Equals(request.ConfigurationName, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(request.ConfigurationTypeName))
                query = query.Where(c => c.ConfigurationType.Name.Equals(request.ConfigurationTypeName, StringComparison.InvariantCultureIgnoreCase));

            return query.ToList();
        }
    }
}
