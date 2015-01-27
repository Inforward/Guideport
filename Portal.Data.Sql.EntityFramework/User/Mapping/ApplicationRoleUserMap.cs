using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ApplicationRoleUserMap : EntityTypeConfiguration<ApplicationRoleUser>
    {
        public ApplicationRoleUserMap()
        {
            // Primary Key
            HasKey(t => new { t.UserID, t.ApplicationRoleID });

            // Properties
            Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ApplicationRoleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("ApplicationRoleUser", "usr");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.ApplicationRoleID).HasColumnName("ApplicationRoleID");
            Property(t => t.ApplicationAccessID).HasColumnName("ApplicationAccessID");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships
            HasRequired(t => t.ApplicationAccess)
                .WithMany(t => t.ApplicationRoleUsers)
                .HasForeignKey(d => d.ApplicationAccessID);

            HasRequired(t => t.ApplicationRole)
                .WithMany(t => t.ApplicationRoleUsers)
                .HasForeignKey(d => d.ApplicationRoleID);

            HasRequired(t => t.User)
                .WithMany(t => t.ApplicationRoles)
                .HasForeignKey(d => d.UserID);

        }
    }
}
