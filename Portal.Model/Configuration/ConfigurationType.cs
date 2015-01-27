
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model.App
{
    public class ConfigurationType
    {
        public int ConfigurationTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Configuration> Configurations { get; set; }

        public virtual ICollection<Setting> Settings { get; set; }
    }

    public enum ConfigurationTypes
    {
        General = 1,
        SamlServiceProvider = 2,
        SamlIdentityProvider = 3,
        SamlPartnerServiceProvider = 4,
        SamlPartnerIdentityProvider = 5
    }
}
