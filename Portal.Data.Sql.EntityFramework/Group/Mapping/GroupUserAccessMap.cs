using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class GroupUserAccessMap : EntityTypeConfiguration<GroupUserAccess>
    {
        public GroupUserAccessMap()
        {
            // Primary Key
            HasKey(t => new { t.GroupID, t.UserID });

            // Properties
            Property(t => t.GroupID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("GroupUserAccess");
            Property(t => t.GroupID).HasColumnName("GroupID");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.IsReadOnly).HasColumnName("IsReadOnly");

            // Relationships
            HasRequired(t => t.Group)
                .WithMany(t => t.AccessibleUsers)
                .HasForeignKey(d => d.GroupID);

            HasRequired(t => t.User)
                .WithMany(t => t.AccessibleGroups)
                .HasForeignKey(d => d.UserID);

        }
    }
}
