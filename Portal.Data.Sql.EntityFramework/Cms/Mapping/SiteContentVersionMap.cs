using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SiteContentVersionMap : EntityTypeConfiguration<SiteContentVersion>
    {
        public SiteContentVersionMap()
        {
            // Primary Key
            HasKey(t => t.SiteContentVersionID);

            // Properties
            Property(t => t.VersionName)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            ToTable("SiteContentVersion", "cms");
            Property(t => t.SiteContentVersionID).HasColumnName("SiteContentVersionID");
            Property(t => t.SiteContentID).HasColumnName("SiteContentID");
            Property(t => t.SiteTemplateID).HasColumnName("SiteTemplateID");
            Property(t => t.VersionName).HasColumnName("VersionName");
            Property(t => t.ContentText).HasColumnName("ContentText");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.SiteContent)
                .WithMany(t => t.Versions)
                .HasForeignKey(d => d.SiteContentID);

            HasRequired(t => t.SiteTemplate)
                .WithMany(t => t.SiteContentVersions)
                .HasForeignKey(d => d.SiteTemplateID);

        }
    }
}
