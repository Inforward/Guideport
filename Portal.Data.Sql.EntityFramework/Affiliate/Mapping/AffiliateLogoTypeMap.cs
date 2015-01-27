using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AffiliateLogoTypeMap : EntityTypeConfiguration<AffiliateLogoType>
    {
        public AffiliateLogoTypeMap()
        {
            // Primary Key
            HasKey(t => t.AffiliateLogoTypeID);

            // Properties
            Property(t => t.AffiliateLogoTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("AffiliateLogoType");
            Property(t => t.AffiliateLogoTypeID).HasColumnName("AffiliateLogoTypeID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
        }
    }
}
