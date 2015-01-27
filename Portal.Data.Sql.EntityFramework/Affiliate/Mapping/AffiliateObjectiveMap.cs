using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AffiliateObjectiveMap : EntityTypeConfiguration<AffiliateObjective>
    {
        public AffiliateObjectiveMap()
        {
            // Primary Key
            HasKey(t => new { t.AffiliateID, t.ObjectiveID });

            // Properties
            Property(t => t.ObjectiveID)
                .IsRequired();

            Property(t => t.AffiliateID)
                .IsRequired();

            Property(t => t.AutoTrackingEnabled)
                .IsRequired();

            // Table & Column Mappings
            ToTable("AffiliateBusinessPlanObjective");
            Property(t => t.ObjectiveID).HasColumnName("BusinessPlanObjectiveID");
            Property(t => t.AffiliateID).HasColumnName("AffiliateID");
            Property(t => t.AutoTrackingEnabled).HasColumnName("AutoTrackingEnabled");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.Objective)
                .WithMany(t => t.AffiliateObjectives);

            HasRequired(t => t.Affiliate)
                .WithMany(t => t.Objectives);
        }
    }
}
