using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyMap : EntityTypeConfiguration<Survey>
    {
        public SurveyMap()
        {
            // Primary Key
            HasKey(t => t.SurveyID);

            // Properties
            Property(t => t.SurveyName)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.RulesetCoreName)
                .HasMaxLength(1000);

            Property(t => t.RulesetValidationName)
                .HasMaxLength(1000);

            Property(t => t.StatusCalculator)
                .HasMaxLength(32);

            Property(t => t.CompleteRedirectUrl)
                .HasMaxLength(64);

            Property(t => t.NotificationType)
                .HasMaxLength(64);

            // Table & Column Mappings
            ToTable("Survey");
            Property(t => t.SurveyID).HasColumnName("SurveyID");
            Property(t => t.SurveyName).HasColumnName("SurveyName");
            Property(t => t.SurveyDescription).HasColumnName("SurveyDescription");
            Property(t => t.RulesetCoreName).HasColumnName("RulesetCoreName");
            Property(t => t.RulesetValidationName).HasColumnName("RulesetValidationName");
            Property(t => t.CompleteMessage).HasColumnName("CompleteMessage");
            Property(t => t.CompleteRedirectUrl).HasColumnName("CompleteRedirectUrl");
            Property(t => t.StatusCalculator).HasColumnName("StatusCalculator");
            Property(t => t.NotificationType).HasColumnName("NotificationType");
            Property(t => t.SuggestedContentSiteID).HasColumnName("SuggestedContentSiteID");
            Property(t => t.StatusLabel).HasColumnName("StatusLabel");
            Property(t => t.IsAutoCompleteEnabled).HasColumnName("IsAutoCompleteEnabled");
            Property(t => t.IsReviewVisible).HasColumnName("IsReviewVisible");
            Property(t => t.IsStatusVisible).HasColumnName("IsStatusVisible");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.ReviewTabText).HasColumnName("ReviewTabText");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");
        }
    }
}
