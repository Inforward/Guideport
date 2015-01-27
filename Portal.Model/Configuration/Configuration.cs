
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Model.App
{
    public class Configuration
    {
        public int ConfigurationID { get; set; }
        public int ConfigurationTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ConfigurationType ConfigurationType { get; set; }

        public virtual ICollection<ConfigurationSetting> ConfigurationSettings { get; set; }

        [NotMapped]
        public ICollection<Affiliate> Affiliates { get; set; }
    }
}
