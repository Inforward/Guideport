using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteContentTypeMap : EntityTypeConfiguration<SiteContentType>
    {
        public SiteContentTypeMap()
        {
            // Primary Key
            HasKey(t => t.SiteContentTypeID);

            // Properties
            Property(t => t.SiteContentTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ContentTypeName)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.ContentTypeDescription)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            ToTable("SiteContentType", "cms");
            Property(t => t.SiteContentTypeID).HasColumnName("SiteContentTypeID");
            Property(t => t.ContentTypeName).HasColumnName("ContentTypeName");
            Property(t => t.ContentTypeDescription).HasColumnName("ContentTypeDescription");
        }
    }
}
