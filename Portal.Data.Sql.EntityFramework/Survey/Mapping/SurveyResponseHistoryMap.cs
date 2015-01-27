using Portal.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyResponseHistoryMap : EntityTypeConfiguration<SurveyResponseHistory>
    {
        public SurveyResponseHistoryMap()
        {
            // Primary Key
            HasKey(t => t.SurveyResponseHistoryID);

            // Properties
            // Table & Column Mappings
            ToTable("SurveyResponseHistory");
            Property(t => t.SurveyResponseHistoryID).HasColumnName("SurveyResponseHistoryID");
            Property(t => t.SurveyResponseID).HasColumnName("SurveyResponseID");
            Property(t => t.SurveyPageID).HasColumnName("SurveyPageID");
            Property(t => t.ResponseDate).HasColumnName("ResponseDate");
            Property(t => t.Score).HasColumnName("Score");
            Property(t => t.IsLatestScore).HasColumnName("IsLatestScore");
            Property(t => t.PercentComplete).HasColumnName("PercentComplete");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.SurveyPage)
                .WithMany(t => t.SurveyResponseHistories)
                .HasForeignKey(d => d.SurveyPageID);

            HasRequired(t => t.SurveyResponse)
                .WithMany(t => t.SurveyResponseHistories)
                .HasForeignKey(d => d.SurveyResponseID);

        }
    }
}
