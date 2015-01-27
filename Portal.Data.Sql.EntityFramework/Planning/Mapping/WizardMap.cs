using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Planning;

namespace Portal.Data.Sql.EntityFramework.Planning.Mapping
{
    public class WizardMap : EntityTypeConfiguration<Wizard>
    {
        public WizardMap()
        {
            // Primary Key
            HasKey(t => t.PlanningWizardID);

            // Properties
            Property(t => t.PlanningWizardID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(32);

            Property(t => t.Description)
                .IsRequired();

            Property(t => t.CompleteMessage)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("PlanningWizard");
            Property(t => t.PlanningWizardID).HasColumnName("PlanningWizardID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.CompleteMessage).HasColumnName("CompleteMessage");
        }
    }
}
