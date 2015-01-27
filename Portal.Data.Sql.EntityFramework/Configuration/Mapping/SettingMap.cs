using Portal.Model.App;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping.App
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            // Primary Key
            HasKey(t => t.SettingID);

            // Properties
            // Table & Column Mappings
            ToTable("Setting", "app");
            Property(t => t.SettingID).HasColumnName("SettingID");
            Property(t => t.ConfigurationTypeID).HasColumnName("ConfigurationTypeID");
            Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(1000);
            Property(t => t.DataTypeName).HasColumnName("DataTypeName").IsRequired().HasMaxLength(50);
            Property(t => t.IsRequired).HasColumnName("IsRequired");

            // Relationships
            HasRequired(t => t.ConfigurationType)
                .WithMany(t => t.Settings);
        }
    }
}
