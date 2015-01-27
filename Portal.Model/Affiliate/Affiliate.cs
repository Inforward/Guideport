using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Portal.Model.App;

namespace Portal.Model
{
    public class Affiliate : Auditable
    {
        public Affiliate()
        {
            Logos = new List<AffiliateLogo>();
            Features = new List<AffiliateFeature>();
            ThirdPartyResources = new List<ThirdPartyResource>();
            SiteContentVersions = new List<SiteContentVersion>();
        }

        public int AffiliateID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? ExternalID { get; set; }
        public string Phone { get; set; }
        public string WebsiteUrl { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string SamlSourceDomain { get; set; }
        public int? SamlConfigurationID { get; set; }
        public int? SamlDisplayOrder { get; set; }
        public Configuration SamlConfiguration { get; set; }
        public List<AffiliateLogo> Logos { get; set; }
        public List<AffiliateFeature> Features { get; set; }
        public List<AffiliateObjective> Objectives { get; set; }

        [IgnoreDataMember]
        public ICollection<User> Users { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> SiteContents { get; set; }

        [IgnoreDataMember]
        public ICollection<ThirdPartyResource> ThirdPartyResources { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContentVersion> SiteContentVersions { get; set; }

        [NotMapped]
        public int UserCount { get; set; }

        [NotMapped]
        public string Address
        {
            get
            {
                var address = new Address()
                {
                    AddressLine1 = Address1,
                    AddressLine2 = Address2,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    Country = Country
                };

                return address.ToString();
            }
        }
    }

    public static class AffiliateExtensions
    {
        public static bool HasFeature(this Affiliate affiliate, Features featureType)
        {
            if (affiliate == null || affiliate.Features == null)
                return false;

            var feature = affiliate.Features.FirstOrDefault(f => f.FeatureID == (int)featureType);

            return feature != null && feature.IsEnabled;
        }

        public static string GetFeatureSetting(this Affiliate affiliate, Features featureType, FeatureSettings settingType)
        {
            if (affiliate == null || affiliate.Features == null || !affiliate.Features.Any())
                return null;

            var feature = affiliate.Features.FirstOrDefault(f => f.FeatureID == (int) featureType);

            if (feature == null)
                return null;

            var setting = feature.Settings.FirstOrDefault(s => s.FeatureSettingID == (int) settingType);

            return setting != null ? setting.Value : null;
        }
    }
}
