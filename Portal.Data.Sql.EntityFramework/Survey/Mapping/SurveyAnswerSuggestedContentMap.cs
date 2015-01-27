using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyAnswerSuggestedContentMap : EntityTypeConfiguration<SurveyAnswerSuggestedContent>
    {
        public SurveyAnswerSuggestedContentMap()
        {
            // Primary Key
            HasKey(t => t.SurveyAnswerSuggestedContentID);

            // Properties

            // Table & Column Mappings
            ToTable("SurveyQuestionAnswerSuggestedContent");
            Property(t => t.SurveyAnswerSuggestedContentID).HasColumnName("SurveyQuestionAnswerSuggestedContentID");
            Property(t => t.SurveyQuestionAnswerID).HasColumnName("SurveyQuestionAnswerID");
            Property(t => t.SiteContentID).HasColumnName("SiteContentID");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");

            // Relationships
            HasRequired(t => t.SurveyAnswer)
                .WithMany(t => t.SuggestedContents)
                .HasForeignKey(d => d.SurveyQuestionAnswerID);

        }
    }
}
