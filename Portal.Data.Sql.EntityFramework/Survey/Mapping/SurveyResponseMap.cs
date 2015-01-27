using Portal.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyResponseMap : EntityTypeConfiguration<SurveyResponse>
    {
        public SurveyResponseMap()
        {
            // Primary Key
            HasKey(t => t.SurveyResponseID);

            // Table & Column Mappings
            ToTable("SurveyResponse");
            Property(t => t.SurveyResponseID).HasColumnName("SurveyResponseID");
            Property(t => t.SurveyID).HasColumnName("SurveyID");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.SelectedSurveyPageID).HasColumnName("SelectedSurveyPageID");
            Property(t => t.CurrentScore).HasColumnName("CurrentScore");
            Property(t => t.PercentComplete).HasColumnName("PercentComplete");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");
            Property(t => t.CompleteUserID).HasColumnName("CompleteUserID");
            Property(t => t.CompleteDate).HasColumnName("CompleteDate");
            Property(t => t.CompleteDateUtc).HasColumnName("CompleteDateUTC");

            // Relationships
            HasRequired(t => t.Survey)
                .WithMany(t => t.SurveyResponses)
                .HasForeignKey(d => d.SurveyID);

            HasRequired(t => t.SurveyPage)
                .WithMany(t => t.SurveyResponses)
                .HasForeignKey(d => d.SelectedSurveyPageID);

            HasRequired(t => t.User)
                .WithMany(t => t.SurveyResponses)
                .HasForeignKey(d => d.UserID);

            //HasRequired(t => t.CreateUser)
            //    .WithMany(t => t.SurveyResponses)
            //    .HasForeignKey(d => d.CreateUserID);

            //HasRequired(t => t.ModifyUser)
            //    .WithMany(t => t.SurveyResponses)
            //    .HasForeignKey(d => d.ModifyUserID);

        }
    }
}
