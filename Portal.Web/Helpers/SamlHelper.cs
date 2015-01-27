using ComponentSpace.SAML2.Configuration;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model.App;
using Portal.Services.Contracts;
using System.Linq;
using System.Web;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Helpers
{
    public static class SamlHelper
    {
        private const string SamlConfigurationCacheKey = "Saml.Configuration";
        private static readonly object Locker = new object();

        public static void CheckConfiguration()
        {
            var logger = NinjectWebCommon.Resolve<ILogger>();
            var cacheStorage = NinjectWebCommon.Resolve<ICacheStorage>();
            var configService = NinjectWebCommon.Resolve<IConfigurationService>();

            // Determine if it's time to retrieve new settings from the database
            var configXml = cacheStorage.Retrieve<string>(SamlConfigurationCacheKey);
            if (!string.IsNullOrEmpty(configXml))
                return;

            // Initialize with current SAMLConfiguration if it exists
            var samlConfiguration = SAMLConfiguration.Current != null ? new SAMLConfiguration(SAMLConfiguration.Current.ToXml()) : new SAMLConfiguration();

            // Service Provider
            var serviceProviderConfig = configService.GetConfigurations(new ConfigurationRequest{ ConfigurationTypeID = (int)ConfigurationTypes.SamlServiceProvider, UseCache = false }).FirstOrDefault();
            if (serviceProviderConfig != null)
            {
                var serviceProviderConfigObject = serviceProviderConfig.To<ServiceProviderConfiguration>(Settings.CurrentEnvironment);
                if (serviceProviderConfigObject != null && !string.IsNullOrEmpty(serviceProviderConfigObject.Name))
                {
                    samlConfiguration.ServiceProviderConfiguration = serviceProviderConfigObject;
                }
            }

            // Partner Identity Providers
            var partnerIdentityConfigs = configService.GetConfigurations(new ConfigurationRequest{ ConfigurationTypeID = (int)ConfigurationTypes.SamlPartnerIdentityProvider, UseCache = false }).ToList();
            if (partnerIdentityConfigs.Any())
            {
                foreach (var partnerIdpConfigObject in partnerIdentityConfigs.Select(pc => pc.To<PartnerIdentityProviderConfiguration>(Settings.CurrentEnvironment)).Where(pc => !string.IsNullOrEmpty(pc.Name)))
                {
                    if (samlConfiguration.PartnerIdentityProviderConfigurations.ContainsKey(partnerIdpConfigObject.Name))
                        samlConfiguration.PartnerIdentityProviderConfigurations.Remove(partnerIdpConfigObject.Name);

                    samlConfiguration.PartnerIdentityProviderConfigurations.Add(partnerIdpConfigObject.Name, partnerIdpConfigObject);
                }
            }

            // Partner Service Providers
            var partnerServiceConfigs = configService.GetConfigurations(new ConfigurationRequest { ConfigurationTypeID = (int)ConfigurationTypes.SamlPartnerServiceProvider, UseCache = false }).ToList();
            if (partnerServiceConfigs.Any())
            {
                foreach (var partnerSvcConfigObject in partnerServiceConfigs.Select(pc => pc.To<PartnerServiceProviderConfiguration>(Settings.CurrentEnvironment)).Where(pc => !string.IsNullOrEmpty(pc.Name)))
                {
                    if (samlConfiguration.PartnerServiceProviderConfigurations.ContainsKey(partnerSvcConfigObject.Name))
                        samlConfiguration.PartnerServiceProviderConfigurations.Remove(partnerSvcConfigObject.Name);

                    samlConfiguration.PartnerServiceProviderConfigurations.Add(partnerSvcConfigObject.Name, partnerSvcConfigObject);
                } 
            }

            lock (Locker)
            {
                SAMLConfiguration.Current = samlConfiguration;    
            }

            // Reset cache & log the configuration
            var cacheDuration = Settings.Get("Saml.Configuration.CacheDuration.Seconds", 1800);

            configXml = SAMLConfiguration.Current.ToXml().OuterXml.FormatXml();
            cacheStorage.Store(SamlConfigurationCacheKey, configXml, cacheDuration);
            logger.Log(HttpContext.Current, string.Format("Updated Current SamlConfiguration. Cache Duration {0} seconds.", cacheDuration), configXml, EventTypes.Information);
        }
    }
}