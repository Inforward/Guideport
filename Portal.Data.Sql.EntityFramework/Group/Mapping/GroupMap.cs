using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            // Primary Key
            HasKey(t => t.GroupID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.Description)
                .HasMaxLength(1024);

            // Table & Column Mappings
            ToTable("Group");
            Property(t => t.GroupID).HasColumnName("GroupID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.IsReadOnly).HasColumnName("IsReadOnly");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasMany(t => t.MemberUsers)
                .WithMany(t => t.Groups)
                .Map(m =>
                {
                    m.ToTable("GroupUser");
                    m.MapLeftKey("GroupID");
                    m.MapRightKey("MemberUserID");
                });

            HasMany(t => t.MemberGroups)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("GroupGroup");
                    m.MapLeftKey("GroupID");
                    m.MapRightKey("MemberGroupID");
                });

            HasMany(t => t.ParentGroups)
                .WithMany(t => t.MemberGroups)
                .Map(m =>
                {
                    m.ToTable("GroupGroup");
                    m.MapLeftKey("MemberGroupID");
                    m.MapRightKey("GroupID");
                });

        }
    }
}
