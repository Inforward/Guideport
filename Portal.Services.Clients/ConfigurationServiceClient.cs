using Portal.Model.App;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using System.Collections.Generic;

namespace Portal.Services.Clients
{
    public class ConfigurationServiceClient : IConfigurationService
    {
        private readonly ServiceClient<IConfigurationServiceChannel> _configurationService = new ServiceClient<IConfigurationServiceChannel>();

        public IEnumerable<Configuration> GetConfigurations(ConfigurationRequest request)
        {
            var proxy = _configurationService.CreateProxy();
            return proxy.GetConfigurations(request);
        }

        public IEnumerable<ConfigurationType> GetConfigurationTypes()
        {
            var proxy = _configurationService.CreateProxy();
            return proxy.GetConfigurationTypes();
        }

        public IEnumerable<Environment> GetEnvironments()
        {
            var proxy = _configurationService.CreateProxy();
            return proxy.GetEnvironments();
        }

        public IEnumerable<Setting> GetSettings(string configurationType)
        {
            var proxy = _configurationService.CreateProxy();
            return proxy.GetSettings(configurationType);
        }

        public void SaveConfiguration(ref Configuration configuration)
        {
            var proxy = _configurationService.CreateProxy();
            proxy.SaveConfiguration(ref configuration);
        }
    }
}
