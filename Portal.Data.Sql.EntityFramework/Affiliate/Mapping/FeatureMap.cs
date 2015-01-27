using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class FeatureMap : EntityTypeConfiguration<Feature>
    {
        public FeatureMap()
        {
            // Primary Key
            HasKey(t => t.FeatureID);

            // Properties
            Property(t => t.FeatureID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("Feature");
            Property(t => t.FeatureID).HasColumnName("FeatureID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
        }
    }
}
