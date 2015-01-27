using Portal.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SurveyPageScoreRangeMap : EntityTypeConfiguration<SurveyPageScoreRange>
    {
        public SurveyPageScoreRangeMap()
        {
            // Primary Key
            HasKey(t => t.ScoreRangeID)
                .Property(t => t.ScoreRangeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("SurveyPageScoreRange");
            Property(t => t.ScoreRangeID).HasColumnName("SurveyPageScoreRangeID");
            Property(t => t.SurveyPageID).HasColumnName("SurveyPageID");
            Property(t => t.MinScore).HasColumnName("MinScore");
            Property(t => t.MaxScore).HasColumnName("MaxScore");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.SurveyPage)
                .WithMany(t => t.ScoreRanges)
                .HasForeignKey(d => d.SurveyPageID);

        }
    }
}
