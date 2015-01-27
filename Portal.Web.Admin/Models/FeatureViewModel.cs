using System.Collections.Generic;

namespace Portal.Web.Admin.Models
{
    public class FeatureViewModel
    {
        public FeatureViewModel()
        {
            Settings = new List<FeatureSettingViewModel>();
        }

        public int AffiliateFeatureID { get; set; }
        public int AffiliateID { get; set; }
        public int FeatureID { get; set; }
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public bool IsEnabled { get; set; }
        public List<FeatureSettingViewModel> Settings { get; set; }
    }

    public class FeatureSettingViewModel
    {
        public int AffiliateFeatureID { get; set; }
        public int FeatureSettingID { get; set; }
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public string VisibleState { get; set; }
        public bool IsRequired { get; set; }
        public string ValidationRegEx { get; set; }
    }
}