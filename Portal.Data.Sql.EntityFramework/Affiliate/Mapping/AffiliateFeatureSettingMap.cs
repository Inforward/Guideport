using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AffiliateFeatureSettingMap : EntityTypeConfiguration<AffiliateFeatureSetting>
    {
        public AffiliateFeatureSettingMap()
        {
            // Primary Key
            HasKey(t => new { t.AffiliateFeatureID, t.FeatureSettingID });

            // Properties
            Property(t => t.AffiliateFeatureID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.FeatureSettingID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("AffiliateFeatureSetting");
            Property(t => t.AffiliateFeatureID).HasColumnName("AffiliateFeatureID");
            Property(t => t.FeatureSettingID).HasColumnName("FeatureSettingID");
            Property(t => t.Value).HasColumnName("Value");

            // Relationships
            HasRequired(t => t.AffiliateFeature)
                .WithMany(t => t.Settings)
                .HasForeignKey(d => d.AffiliateFeatureID);

        }
    }
}
