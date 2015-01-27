using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ApplicationRoleMap : EntityTypeConfiguration<ApplicationRole>
    {
        public ApplicationRoleMap()
        {
            // Primary Key
            HasKey(t => t.ApplicationRoleID);

            // Properties
            Property(t => t.ApplicationRoleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .HasMaxLength(50);

            Property(t => t.Description)
                .HasMaxLength(100);

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("ApplicationRole", "usr");
            Property(t => t.ApplicationRoleID).HasColumnName("ApplicationRoleID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.DisplayName).HasColumnName("DisplayName");
            Property(t => t.Description).HasColumnName("Description");
        }
    }
}
