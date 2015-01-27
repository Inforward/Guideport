using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyResponseAnswerMap : EntityTypeConfiguration<SurveyResponseAnswer>
    {
        public SurveyResponseAnswerMap()
        {
            // Primary Key
            HasKey(t => t.SurveyResponseAnswerID);

            // Properties
            Property(t => t.Answer)
                .HasMaxLength(int.MaxValue);

            // Table & Column Mappings
            ToTable("SurveyResponseAnswer");
            Property(t => t.SurveyResponseAnswerID).HasColumnName("SurveyResponseAnswerID");
            Property(t => t.SurveyResponseID).HasColumnName("SurveyResponseID");
            Property(t => t.SurveyQuestionID).HasColumnName("SurveyQuestionID");
            Property(t => t.Answer).HasColumnName("Answer");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");

            // Relationships
            HasRequired(t => t.SurveyQuestion)
                .WithMany(t => t.ResponseAnswers)
                .HasForeignKey(d => d.SurveyQuestionID);

            HasRequired(t => t.SurveyResponse)
                .WithMany(t => t.Answers)
                .HasForeignKey(d => d.SurveyResponseID);

        }
    }
}
