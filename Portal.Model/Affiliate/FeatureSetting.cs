using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class FeatureSetting
    {
        public int FeatureSettingID { get; set; }
        public int FeatureID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PlaceholderValue { get; set; }
        public string VisibleState { get; set; }
        public bool IsRequired { get; set; }
        public string ValidationRegEx { get; set; }

        [IgnoreDataMember]
        public Feature Feature { get; set; }
    }

    public enum FeatureSettings
    {
        DisabledMessage = 1,
        BaseUrl = 2,
        PentameterDashboardDocument = 3,
        UserIdentifierField = 4,
    }
}
