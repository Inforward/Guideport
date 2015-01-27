using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ApplicationAccessMap : EntityTypeConfiguration<ApplicationAccess>
    {
        public ApplicationAccessMap()
        {
            // Primary Key
            HasKey(t => t.ApplicationAccessID);

            // Properties
            Property(t => t.ApplicationAccessID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ApplicationAccess", "usr");
            Property(t => t.ApplicationAccessID).HasColumnName("ApplicationAccessID");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
