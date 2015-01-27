using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteMenuIconMap : EntityTypeConfiguration<MenuIcon>
    {
        public SiteMenuIconMap()
        {
            // Primary Key
            HasKey(t => t.MenuIconID);

            // Properties
            Property(t => t.MenuIconID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.IconName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.IconCssClass)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("SiteMenuIcon", "cms");
            Property(t => t.MenuIconID).HasColumnName("SiteMenuIconID");
            Property(t => t.IconName).HasColumnName("IconName");
            Property(t => t.IconCssClass).HasColumnName("IconCssClass");
        }
    }
}
