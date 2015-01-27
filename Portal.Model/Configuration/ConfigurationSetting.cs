

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model.App
{
    public class ConfigurationSetting : Auditable
    {
        public int ConfigurationSettingID { get; set; }
        [ForeignKey("Configuration")]
        public int ConfigurationID { get; set; }
        public int EnvironmentID { get; set; }
        public int SettingID { get; set; }
        public string Value { get; set; }

        public virtual Environment Environment { get; set; }
        public virtual Setting Setting { get; set; }

        [IgnoreDataMember]
        public virtual Configuration Configuration { get; set; }

    }
}
