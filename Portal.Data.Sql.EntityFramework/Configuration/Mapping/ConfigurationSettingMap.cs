using Portal.Model.App;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping.App
{
    public class ConfigurationSettingMap : EntityTypeConfiguration<ConfigurationSetting>
    {
        public ConfigurationSettingMap()
        {
            // Primary Key
            HasKey(t => t.ConfigurationSettingID);

            // Properties
            // Table & Column Mappings
            ToTable("ConfigurationSetting", "app");
            Property(t => t.ConfigurationSettingID).HasColumnName("ConfigurationSettingID");
            Property(t => t.ConfigurationID).HasColumnName("ConfigurationID");
            Property(t => t.EnvironmentID).HasColumnName("EnvironmentID");
            Property(t => t.SettingID).HasColumnName("SettingID");
            Property(t => t.Value).HasColumnName("Value");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.Environment)
                .WithMany(t => t.ConfigurationSettings);

            HasRequired(t => t.Configuration)
                .WithMany(t => t.ConfigurationSettings);

            HasRequired(t => t.Setting)
                .WithMany(t => t.ConfigurationSettings);
        }
    }
}
