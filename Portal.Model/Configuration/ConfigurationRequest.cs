

namespace Portal.Model.App
{
    public class ConfigurationRequest
    {
        public int ConfigurationID { get; set; }
        public int ConfigurationTypeID { get; set; }
        public string ConfigurationName { get; set; }
        public string ConfigurationTypeName { get; set; }
        public bool UseCache { get; set; }
    }
}
