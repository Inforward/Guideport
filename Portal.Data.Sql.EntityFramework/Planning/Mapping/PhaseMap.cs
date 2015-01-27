using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Planning;

namespace Portal.Data.Sql.EntityFramework.Planning.Mapping
{
    public class PhaseMap : EntityTypeConfiguration<Phase>
    {
        public PhaseMap()
        {
            // Primary Key
            HasKey(t => t.PlanningWizardPhaseID);

            // Properties
            Property(t => t.PlanningWizardPhaseID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            ToTable("PlanningWizardPhase");
            Property(t => t.PlanningWizardPhaseID).HasColumnName("PlanningWizardPhaseID");
            Property(t => t.PlanningWizardID).HasColumnName("PlanningWizardID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameHtml).HasColumnName("NameHtml");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.SortOrder).HasColumnName("SortOrder");

            // Relationships
            HasRequired(t => t.Wizard)
                .WithMany(t => t.Phases)
                .HasForeignKey(d => d.PlanningWizardID);

        }
    }
}
