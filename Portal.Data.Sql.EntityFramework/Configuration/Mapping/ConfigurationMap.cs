using Portal.Model.App;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping.App
{
    public class ConfigurationMap : EntityTypeConfiguration<Configuration>
    {
        public ConfigurationMap()
        {
            // Primary Key
            HasKey(t => t.ConfigurationID);

            // Properties
            // Table & Column Mappings
            ToTable("Configuration", "app");
            Property(t => t.ConfigurationID).HasColumnName("ConfigurationID");
            Property(t => t.ConfigurationTypeID).HasColumnName("ConfigurationTypeID");
            Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(1000);

            // Relationships
            HasRequired(t => t.ConfigurationType)
                .WithMany(t => t.Configurations)
                .HasForeignKey(t => t.ConfigurationTypeID);
        }
    }
}
