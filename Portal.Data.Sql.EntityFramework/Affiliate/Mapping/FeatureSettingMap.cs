using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class FeatureSettingMap : EntityTypeConfiguration<FeatureSetting>
    {
        public FeatureSettingMap()
        {
            // Primary Key
            HasKey(t => t.FeatureSettingID);

            // Properties
            Property(t => t.FeatureSettingID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(512);

            Property(t => t.PlaceholderValue)
                .IsRequired()
                .HasMaxLength(512);

            Property(t => t.ValidationRegEx)
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("FeatureSetting");
            Property(t => t.FeatureSettingID).HasColumnName("FeatureSettingID");
            Property(t => t.FeatureID).HasColumnName("FeatureID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.PlaceholderValue).HasColumnName("PlaceholderValue");
            Property(t => t.VisibleState).HasColumnName("VisibleState");
            Property(t => t.IsRequired).HasColumnName("IsRequired");
            Property(t => t.ValidationRegEx).HasColumnName("ValidationRegEx");

            // Relationships
            HasRequired(t => t.Feature)
                .WithMany(t => t.Settings)
                .HasForeignKey(d => d.FeatureID);

        }
    }
}
