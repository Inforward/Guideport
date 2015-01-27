using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class SiteKnowledgeLibraryTopicMap : EntityTypeConfiguration<KnowledgeLibraryTopic>
    {
        public SiteKnowledgeLibraryTopicMap()
        {
            // Primary Key
            HasKey(t => t.KnowledgeLibraryTopicID);

            // Properties
            Property(t => t.Topic)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Subtopic)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("SiteKnowledgeLibraryTopic", "cms");
            Property(t => t.KnowledgeLibraryTopicID).HasColumnName("SiteKnowledgeLibraryTopicID");
            Property(t => t.SiteID).HasColumnName("SiteID");
            Property(t => t.Topic).HasColumnName("Topic");
            Property(t => t.Subtopic).HasColumnName("Subtopic");

            // Relationships
            HasRequired(t => t.Site)
                .WithMany(t => t.KnowledgeLibraryTopics)
                .HasForeignKey(d => d.SiteID);

        }
    }
}
