
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model.App
{
    public class Setting
    {
        public int SettingID { get; set; }
        public int ConfigurationTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DataTypeName { get; set; }
        public bool IsRequired { get; set; }

        [IgnoreDataMember]
        public virtual ConfigurationType ConfigurationType { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ConfigurationSetting> ConfigurationSettings { get; set; }
    }
}
