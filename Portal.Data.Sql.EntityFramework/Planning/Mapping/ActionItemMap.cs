using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Planning;

namespace Portal.Data.Sql.EntityFramework.Planning.Mapping
{
    public class ActionItemMap : EntityTypeConfiguration<ActionItem>
    {
        public ActionItemMap()
        {
            // Primary Key
            HasKey(t => t.PlanningWizardActionItemID);

            // Properties
            Property(t => t.PlanningWizardActionItemID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ActionItemText)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("PlanningWizardActionItem");
            Property(t => t.PlanningWizardActionItemID).HasColumnName("PlanningWizardActionItemID");
            Property(t => t.PlanningWizardStepID).HasColumnName("PlanningWizardStepID");
            Property(t => t.ActionItemText).HasColumnName("ActionItemText");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.ResourcesContent).HasColumnName("ResourcesContent");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            HasRequired(t => t.Step)
                .WithMany(t => t.ActionItems)
                .HasForeignKey(d => d.PlanningWizardStepID);

        }
    }
}
