using Portal.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyAnswerMap : EntityTypeConfiguration<SurveyAnswer>
    {
        public SurveyAnswerMap()
        {
            // Primary Key
            HasKey(t => t.SurveyQuestionAnswerID);

            // Properties
            Property(t => t.AnswerText)
                .IsRequired();

            // Table & Column Mappings
            ToTable("SurveyQuestionAnswer");
            Property(t => t.SurveyQuestionAnswerID).HasColumnName("SurveyQuestionAnswerID");
            Property(t => t.SurveyQuestionID).HasColumnName("SurveyQuestionID");
            Property(t => t.AnswerText).HasColumnName("AnswerText");
            Property(t => t.ReviewAnswerText).HasColumnName("ReviewAnswerText");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.AnswerWeight).HasColumnName("AnswerWeight");

            // Relationships
            HasRequired(t => t.SurveyQuestion)
                .WithMany(t => t.PossibleAnswers)
                .HasForeignKey(d => d.SurveyQuestionID);

        }
    }
}
