using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AffiliateFeatureMap : EntityTypeConfiguration<AffiliateFeature>
    {
        public AffiliateFeatureMap()
        {
            // Primary Key
            HasKey(t => t.AffiliateFeatureID);

            // Properties
            // Table & Column Mappings
            ToTable("AffiliateFeature");
            Property(t => t.AffiliateFeatureID).HasColumnName("AffiliateFeatureID");
            Property(t => t.AffiliateID).HasColumnName("AffiliateID");
            Property(t => t.FeatureID).HasColumnName("FeatureID");
            Property(t => t.IsEnabled).HasColumnName("IsEnabled");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.Affiliate)
                .WithMany(t => t.Features)
                .HasForeignKey(d => d.AffiliateID);

            HasRequired(t => t.Feature)
                .WithMany(t => t.AffiliateFeatures)
                .HasForeignKey(d => d.FeatureID);

        }
    }
}
