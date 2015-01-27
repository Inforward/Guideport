using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AffiliateMap : EntityTypeConfiguration<Affiliate>
    {
        public AffiliateMap()
        {
            // Primary Key
            HasKey(t => t.AffiliateID);

            // Properties
            // Table & Column Mappings
            ToTable("Affiliate");

            Property(t => t.AffiliateID).HasColumnName("AffiliateID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.ShortName).HasColumnName("ShortName");
            Property(t => t.ExternalID).HasColumnName("ExternalID");
            Property(t => t.Phone).HasColumnName("Phone");
            Property(t => t.WebsiteUrl).HasColumnName("WebsiteUrl");
            Property(t => t.Address1).HasColumnName("Address1");
            Property(t => t.Address2).HasColumnName("Address2");
            Property(t => t.City).HasColumnName("City");
            Property(t => t.State).HasColumnName("State");
            Property(t => t.ZipCode).HasColumnName("ZipCode");
            Property(t => t.Country).HasColumnName("Country");
            Property(t => t.SamlSourceDomain).HasColumnName("SAMLSourceDomain");
            Property(t => t.SamlConfigurationID).HasColumnName("SAMLConfigurationID");
            Property(t => t.SamlDisplayOrder).HasColumnName("SAMLDisplayOrder");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasMany(t => t.SiteContents)
                .WithMany(t => t.Affiliates)
                .Map(m =>
                {
                    m.ToTable("SiteContentAffiliate", "cms");
                    m.MapLeftKey("AffiliateID");
                    m.MapRightKey("SiteContentID");
                });

            HasMany(t => t.SiteContentVersions)
                .WithMany(t => t.Affiliates)
                .Map(m =>
                {
                    m.ToTable("SiteContentVersionAffiliate", "cms");
                    m.MapLeftKey("AffiliateID");
                    m.MapRightKey("SiteContentVersionID");
                });

            HasMany(t => t.ThirdPartyResources)
                .WithMany(t => t.Affiliates)
                .Map(m =>
                {
                    m.ToTable("SiteThirdPartyResourceAffiliate", "cms");
                    m.MapLeftKey("AffiliateID");
                    m.MapRightKey("SiteThirdPartyResourceID");
                });

            HasOptional(t => t.SamlConfiguration)
                .WithMany(t => t.Affiliates)
                .HasForeignKey(t => t.SamlConfigurationID);
        }
    }
}
