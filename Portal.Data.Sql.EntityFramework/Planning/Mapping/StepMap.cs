using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Planning;

namespace Portal.Data.Sql.EntityFramework.Planning.Mapping
{
    public class StepMap : EntityTypeConfiguration<Step>
    {
        public StepMap()
        {
            // Primary Key
            HasKey(t => t.PlanningWizardStepID);

            // Properties
            Property(t => t.PlanningWizardStepID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            // Table & Column Mappings
            ToTable("PlanningWizardStep");
            Property(t => t.PlanningWizardStepID).HasColumnName("PlanningWizardStepID");
            Property(t => t.PlanningWizardPhaseID).HasColumnName("PlanningWizardPhaseID");
            Property(t => t.StepNo).HasColumnName("StepNo");
            Property(t => t.StepWeight).HasColumnName("StepWeight");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");

            // Relationships
            HasRequired(t => t.Phase)
                .WithMany(t => t.Steps)
                .HasForeignKey(d => d.PlanningWizardPhaseID);

        }
    }
}
