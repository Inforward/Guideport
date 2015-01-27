using Portal.Model.App;
using System.Collections.Generic;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface IConfigurationServiceChannel : IConfigurationService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IConfigurationService
    {
        [OperationContract]
        IEnumerable<Configuration> GetConfigurations(ConfigurationRequest request);

        [OperationContract]
        IEnumerable<ConfigurationType> GetConfigurationTypes();

        [OperationContract]
        IEnumerable<Environment> GetEnvironments();

        [OperationContract]
        IEnumerable<Setting> GetSettings(string configurationType);

        [OperationContract]
        void SaveConfiguration(ref Configuration configuration);
    }
}
