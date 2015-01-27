using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteKnowledgeLibraryMap : EntityTypeConfiguration<KnowledgeLibrary>
    {
        public SiteKnowledgeLibraryMap()
        {
            // Primary Key
            HasKey(t => t.KnowledgeLibraryID);

            Property(t => t.Topic)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Subtopic)
                .HasMaxLength(255);

            Property(t => t.CreatedBy)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("SiteKnowledgeLibrary", "cms");

            Property(t => t.KnowledgeLibraryID).HasColumnName("SiteKnowledgeLibraryID");
            Property(t => t.SiteContentID).HasColumnName("SiteContentID");
            Property(t => t.Topic).HasColumnName("Topic");
            Property(t => t.Subtopic).HasColumnName("Subtopic");
            Property(t => t.CreatedBy).HasColumnName("CreatedBy");

            HasRequired(t => t.SiteContent)
                .WithMany(t => t.KnowledgeLibraries)
                .HasForeignKey(t => t.SiteContentID);
        }
    }
}
