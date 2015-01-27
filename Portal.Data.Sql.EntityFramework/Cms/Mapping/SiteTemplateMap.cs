using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteTemplateMap : EntityTypeConfiguration<SiteTemplate>
    {
        public SiteTemplateMap()
        {
            // Primary Key
            HasKey(t => t.SiteTemplateID);

            // Properties
            Property(t => t.SiteTemplateID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.TemplateName)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.TemplateDescription)
                .IsRequired()
                .HasMaxLength(512);

            Property(t => t.LayoutPath)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("SiteTemplate", "cms");
            Property(t => t.SiteTemplateID).HasColumnName("SiteTemplateID");
            Property(t => t.SiteID).HasColumnName("SiteID");
            Property(t => t.TemplateName).HasColumnName("TemplateName");
            Property(t => t.TemplateDescription).HasColumnName("TemplateDescription");
            Property(t => t.DefaultContent).HasColumnName("DefaultContent");
            Property(t => t.LayoutPath).HasColumnName("LayoutPath");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            HasRequired(t => t.Site)
                .WithMany(t => t.SiteTemplates)
                .HasForeignKey(d => d.SiteID);

        }
    }
}
