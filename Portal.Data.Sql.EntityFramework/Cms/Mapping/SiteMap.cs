using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteMap : EntityTypeConfiguration<Site>
    {
        public SiteMap()
        {
            // Primary Key
            HasKey(t => t.SiteID);

            // Properties
            Property(t => t.SiteID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.SiteName)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.SiteDescription)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.DomainName)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.ApplicationRootPath)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("Site", "cms");
            Property(t => t.SiteID).HasColumnName("SiteID");
            Property(t => t.SiteName).HasColumnName("SiteName");
            Property(t => t.SiteDescription).HasColumnName("SiteDescription");
            Property(t => t.DomainName).HasColumnName("DomainName");
            Property(t => t.ApplicationRootPath).HasColumnName("ApplicationRootPath");
            Property(t => t.DefaultSiteTemplateID).HasColumnName("DefaultSiteTemplateID");
            Property(t => t.DefaultSiteContentID).HasColumnName("DefaultSiteContentID");

        }
    }
}
