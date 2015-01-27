using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteDocumentTypeMap : EntityTypeConfiguration<SiteDocumentType>
    {
        public SiteDocumentTypeMap()
        {
            // Primary Key
            HasKey(t => t.SiteDocumentTypeID);

            // Properties
            Property(t => t.SiteDocumentTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.DocumentTypeName)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.DocumentTypeExtension)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.MIMEType)
                .IsRequired()
                .HasMaxLength(64);

            // Table & Column Mappings
            ToTable("SiteDocumentType", "cms");
            Property(t => t.SiteDocumentTypeID).HasColumnName("SiteDocumentTypeID");
            Property(t => t.DocumentTypeName).HasColumnName("DocumentTypeName");
            Property(t => t.DocumentTypeExtension).HasColumnName("DocumentTypeExtension");
            Property(t => t.MIMEType).HasColumnName("MIMEType");
        }
    }
}
