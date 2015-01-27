using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ObjectiveMap : EntityTypeConfiguration<Objective>
    {
        public ObjectiveMap()
        {
            // Primary Key
            HasKey(t => t.ObjectiveID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Value)
                .HasMaxLength(1000);

            Property(t => t.BaselineValue)
                .HasMaxLength(1000);

            Property(t => t.DataType)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.PercentComplete)
                .IsRequired();

            Property(t => t.AutoTrackingEnabled)
                .IsRequired();

            // Table & Column Mappings
            ToTable("BusinessPlanObjective");
            Property(t => t.ObjectiveID).HasColumnName("BusinessPlanObjectiveID");
            Property(t => t.BusinessPlanID).HasColumnName("BusinessPlanID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Value).HasColumnName("Value");
            Property(t => t.BaselineValue).HasColumnName("BaselineValue");
            Property(t => t.BaselineValueDate).HasColumnName("BaselineDate");
            Property(t => t.DataType).HasColumnName("DataType");
            Property(t => t.PercentComplete).HasColumnName("PercentComplete");
            Property(t => t.EstimatedCompletionDate).HasColumnName("EstimatedCompletionDate");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.AutoTrackingEnabled).HasColumnName("AutoTrackingEnabled");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasMany(t => t.Strategies)
                .WithMany(t => t.Objectives)
                .Map(m =>
                {
                    m.ToTable("BusinessPlanObjectiveStrategy");
                    m.MapLeftKey("BusinessPlanObjectiveID");
                    m.MapRightKey("BusinessPlanStrategyID");
                });

            HasOptional(t => t.BusinessPlan)
                .WithMany(t => t.Objectives)
                .HasForeignKey(d => d.BusinessPlanID);

        }
    }
}
