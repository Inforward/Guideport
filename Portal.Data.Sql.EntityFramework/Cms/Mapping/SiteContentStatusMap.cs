using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteContentStatusMap : EntityTypeConfiguration<SiteContentStatus>
    {
        public SiteContentStatusMap()
        {
            // Primary Key
            HasKey(t => t.SiteContentStatusID);

            // Properties
            Property(t => t.SiteContentStatusID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(64);

            // Table & Column Mappings
            ToTable("SiteContentStatus", "cms");
            Property(t => t.SiteContentStatusID).HasColumnName("SiteContentStatusID");
            Property(t => t.StatusDescription).HasColumnName("StatusDescription");
        }
    }
}
