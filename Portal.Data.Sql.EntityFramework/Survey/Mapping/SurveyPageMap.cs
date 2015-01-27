using Portal.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyPageMap : EntityTypeConfiguration<SurveyPage>
    {
        public SurveyPageMap()
        {
            // Primary Key
            HasKey(t => t.SurveyPageID);

            // Properties
            Property(t => t.SurveyPageID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.PageName)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.Tooltip)
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("SurveyPage");
            Property(t => t.SurveyPageID).HasColumnName("SurveyPageID");
            Property(t => t.SurveyID).HasColumnName("SurveyID");
            Property(t => t.PageName).HasColumnName("PageName");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.IsVisible).HasColumnName("IsVisible");
            Property(t => t.Tooltip).HasColumnName("Tooltip");

            // Relationships
            HasRequired(t => t.Survey)
                .WithMany(t => t.Pages)
                .HasForeignKey(d => d.SurveyID);

        }
    }
}
