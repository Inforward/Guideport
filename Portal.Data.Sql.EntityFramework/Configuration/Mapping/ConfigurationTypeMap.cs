using Portal.Model.App;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping.App
{
    public class ConfigurationTypeMap : EntityTypeConfiguration<ConfigurationType>
    {
        public ConfigurationTypeMap()
        {
            // Primary Key
            HasKey(t => t.ConfigurationTypeID);

            // Properties
            // Table & Column Mappings
            ToTable("ConfigurationType", "app");
            Property(t => t.ConfigurationTypeID).HasColumnName("ConfigurationTypeID");
            Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(1000);
            Property(t => t.AssemblyName).HasColumnName("AssemblyName").HasMaxLength(250);
            Property(t => t.ClassName).HasColumnName("ClassName").HasMaxLength(250);
        }
    }
}
