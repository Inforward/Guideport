using System.Data.Entity.ModelConfiguration;
using Portal.Model.Planning;

namespace Portal.Data.Sql.EntityFramework.Planning.Mapping
{
    public class ProgressMap : EntityTypeConfiguration<Progress>
    {
        public ProgressMap()
        {
            // Primary Key
            HasKey(t => t.PlanningWizardProgressID);

            // Properties
            Property(t => t.ProgressXml)
                .IsRequired();

            // Table & Column Mappings
            ToTable("PlanningWizardProgress");
            Property(t => t.PlanningWizardProgressID).HasColumnName("PlanningWizardProgressID");
            Property(t => t.PlanningWizardID).HasColumnName("PlanningWizardID");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.CurrentPlanningWizardPhaseID).HasColumnName("CurrentPlanningWizardPhaseID");
            Property(t => t.PercentComplete).HasColumnName("PercentComplete");
            Property(t => t.ProgressXml).HasColumnName("ProgressXml");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.Wizard)
                .WithMany(t => t.Progresses)
                .HasForeignKey(d => d.PlanningWizardID);

            HasRequired(t => t.CurrentPhase)
                .WithMany(t => t.Progresses)
                .HasForeignKey(d => d.CurrentPlanningWizardPhaseID);

        }
    }
}
