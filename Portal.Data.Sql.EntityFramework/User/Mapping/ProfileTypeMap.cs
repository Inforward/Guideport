using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ProfileTypeMap : EntityTypeConfiguration<ProfileType>
    {
        public ProfileTypeMap()
        {
            // Primary Key
            HasKey(t => t.ProfileTypeID);

            // Properties
            // Table & Column Mappings
            ToTable("ProfileType", "usr");

            Property(t => t.ProfileTypeID).HasColumnName("ProfileTypeID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");

            // Relationships
            HasMany(t => t.SiteContents)
                .WithMany(t => t.ProfileTypes)
                .Map(m =>
                {
                    m.ToTable("SiteContentProfileType", "cms");
                    m.MapLeftKey("ProfileTypeID");
                    m.MapRightKey("SiteContentID");
                });
        }
    }
}
