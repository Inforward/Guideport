using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteContentMap : EntityTypeConfiguration<SiteContent>
    {
        public SiteContentMap()
        {
            // Primary Key
            HasKey(t => t.SiteContentID);

            // Properties
            Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.Description)
                .HasMaxLength(1000);

            Property(t => t.Permalink)
                .HasMaxLength(1000);

            Property(t => t.MenuIconCssClass)
                .HasMaxLength(64);

            Property(t => t.MenuTarget)
                .HasMaxLength(64);

            // Table & Column Mappings
            ToTable("SiteContent", "cms");
            Property(t => t.SiteContentID).HasColumnName("SiteContentID");
            Property(t => t.SiteContentParentID).HasColumnName("SiteContentParentID");
            Property(t => t.SiteID).HasColumnName("SiteID");
            Property(t => t.SiteContentStatusID).HasColumnName("SiteContentStatusID");
            Property(t => t.SiteContentTypeID).HasColumnName("SiteContentTypeID");
            Property(t => t.SiteDocumentTypeID).HasColumnName("SiteDocumentTypeID");
            Property(t => t.FileID).HasColumnName("FileID");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.Permalink).HasColumnName("Permalink");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.MenuVisible).HasColumnName("MenuVisible");
            Property(t => t.MenuIconCssClass).HasColumnName("MenuIconCssClass");
            Property(t => t.MenuTarget).HasColumnName("MenuTarget");
            Property(t => t.PublishDateUtc).HasColumnName("PublishDateUTC");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.Site)
                .WithMany(t => t.SiteContents)
                .HasForeignKey(d => d.SiteID);

            //HasOptional(t => t.FileInfo)
            //    .WithMany(t => t.SiteContents)
            //    .HasForeignKey(d => d.FileID);

            HasOptional(t => t.Parent)
                .WithMany(t => t.Children)
                .HasForeignKey(d => d.SiteContentParentID);

            HasRequired(t => t.SiteContentStatus)
                .WithMany(t => t.SiteContents)
                .HasForeignKey(d => d.SiteContentStatusID);

            HasRequired(t => t.SiteContentType)
                .WithMany(t => t.SiteContents)
                .HasForeignKey(d => d.SiteContentTypeID);

            HasRequired(t => t.SiteDocumentType)
                .WithMany(t => t.SiteContents)
                .HasForeignKey(d => d.SiteDocumentTypeID);
        }
    }
}
