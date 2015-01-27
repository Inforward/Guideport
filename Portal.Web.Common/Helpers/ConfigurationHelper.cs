using System.Collections.Generic;
using System.ComponentModel;
using Portal.Model.App;
using System;
using System.Linq;
using System.Reflection;

namespace Portal.Web.Common.Helpers
{
    public static class ConfigurationHelper
    {
        public static List<string> Validate<T>(this Configuration configuration, Environments environment) where T : new()
        {
            List<string> errorMessages;

            var config = configuration.To<T>(environment, out errorMessages, false);

            return errorMessages;
        }

        public static T To<T>(this Configuration configuration, Environments environment, bool useDefaultValueIfEmpty = true) where T: new()
        {
            List<string> errorMessages;
            return configuration.To<T>(environment, out errorMessages, useDefaultValueIfEmpty);
        }

        public static T To<T>(this Configuration configuration, Environments environment, out List<string> errorMessages, bool useDefaultValueIfEmpty = true) where T: new()
        {
            var config = new T();
            var configProperties = config.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            var settings = configuration.ConfigurationType.Settings;
            var envSettings = configuration.ConfigurationSettings.Where(cs => cs.EnvironmentID == (int) environment).ToList();
            var defaultSettings = configuration.ConfigurationSettings.Where(cs => cs.EnvironmentID == (int)Environments.Default).ToList();
            errorMessages = new List<string>();

            try
            {
                foreach (var setting in settings)
                {
                    try
                    {
                        var configProperty = configProperties.FirstOrDefault(p => p.Name.Equals(setting.Name, StringComparison.InvariantCultureIgnoreCase));

                        if (configProperty != null)
                        {
                            var envSetting = envSettings.FirstOrDefault(ds => ds.Setting.Name.Equals(setting.Name, StringComparison.InvariantCultureIgnoreCase));
                            var defaultSetting = defaultSettings.FirstOrDefault(ds => ds.Setting.Name.Equals(setting.Name, StringComparison.InvariantCultureIgnoreCase));
                            var value = envSetting != null && !string.IsNullOrEmpty(envSetting.Value) ? envSetting.Value : useDefaultValueIfEmpty && defaultSetting != null ? defaultSetting.Value : null;

                            if (string.IsNullOrEmpty(value) && setting.IsRequired)
                            {
                                errorMessages.Add(string.Format("{0} - is required.", setting.Name));
                            }
                            else if (!string.IsNullOrEmpty(value))
                            {
                                var type = Type.GetType(setting.DataTypeName) ?? typeof(string);
                                var converter = TypeDescriptor.GetConverter(type);

                                configProperty.SetValue(config, converter.ConvertFrom(value));
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        errorMessages.Add(string.Format("{0} - {1}", setting.Name, ex.Message));
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessages.Add(ex.Message);
            }

            return config;
        }

        public static dynamic GetSetting(this Configuration configuration, string settingName, Environments environment, bool useDefaultValueIfEmpty = true)
        {
            var setting = configuration.ConfigurationType.Settings.FirstOrDefault(s => s.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase));
            var envSetting = configuration.ConfigurationSettings.FirstOrDefault(cs => cs.EnvironmentID == (int)environment && cs.Setting.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase));
            var defaultSetting = configuration.ConfigurationSettings.FirstOrDefault(cs => cs.EnvironmentID == (int)Environments.Default && cs.Setting.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase));
            var value = envSetting != null && !string.IsNullOrEmpty(envSetting.Value) ? envSetting.Value : useDefaultValueIfEmpty && defaultSetting != null ? defaultSetting.Value : null;

            if (setting == null) return null;

            var type = Type.GetType(setting.DataTypeName) ?? typeof(string);
            var converter = TypeDescriptor.GetConverter(type);

            return converter.ConvertFrom(value);
        }
    }
}
