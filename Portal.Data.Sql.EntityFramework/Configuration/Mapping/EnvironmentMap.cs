using Portal.Model.App;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping.App
{
    public class EnvironmentMap : EntityTypeConfiguration<Environment>
    {
        public EnvironmentMap()
        {
            // Primary Key
            HasKey(t => t.EnvironmentID);

            // Properties
            // Table & Column Mappings
            ToTable("Environment", "app");
            Property(t => t.EnvironmentID).HasColumnName("EnvironmentID");
            Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(100);            
        }
    }
}
