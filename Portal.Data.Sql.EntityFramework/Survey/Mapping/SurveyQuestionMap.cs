using Portal.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyQuestionMap : EntityTypeConfiguration<SurveyQuestion>
    {
        public SurveyQuestionMap()
        {
            // Primary Key
            HasKey(t => t.SurveyQuestionID);

            // Properties
            Property(t => t.SurveyQuestionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.QuestionName)
                .HasMaxLength(50);

            Property(t => t.QuestionText)
                .IsRequired();

            Property(t => t.QuestionType)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.LayoutType)
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("SurveyQuestion");
            Property(t => t.SurveyQuestionID).HasColumnName("SurveyQuestionID");
            Property(t => t.SurveyPageID).HasColumnName("SurveyPageID");
            Property(t => t.QuestionName).HasColumnName("QuestionName");
            Property(t => t.QuestionText).HasColumnName("QuestionText");
            Property(t => t.QuestionType).HasColumnName("QuestionType");
            Property(t => t.LayoutType).HasColumnName("LayoutType");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.MaxLength).HasColumnName("MaxLength");
            Property(t => t.IsRequired).HasColumnName("IsRequired");
            Property(t => t.IsVisible).HasColumnName("IsVisible");
            Property(t => t.IsDisabled).HasColumnName("IsDisabled");

            // Relationships
            HasRequired(t => t.SurveyPage)
                .WithMany(t => t.Questions)
                .HasForeignKey(d => d.SurveyPageID);

        }
    }
}
