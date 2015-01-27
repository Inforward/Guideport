
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model.App
{
    public class Environment
    {
        public int EnvironmentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public ICollection<ConfigurationSetting> ConfigurationSettings { get; set; }
    }

    public enum Environments
    {
        Default = 0,
        Development = 1,
        CeteraQA = 2,
        CeteraProduction = 3,
        FirstAlliedQA = 4,
        FirstAlliedProduction = 5
    }
}
