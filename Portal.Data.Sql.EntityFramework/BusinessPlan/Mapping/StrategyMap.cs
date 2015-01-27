using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class StrategyMap : EntityTypeConfiguration<Strategy>
    {
        public StrategyMap()
        {
            // Primary Key
            HasKey(t => t.StrategyID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Description)
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("BusinessPlanStrategy");
            Property(t => t.StrategyID).HasColumnName("BusinessPlanStrategyID");
            Property(t => t.BusinessPlanID).HasColumnName("BusinessPlanID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasMany(t => t.Tactics)
                .WithMany(t => t.Strategies)
                .Map(m =>
                    {
                        m.ToTable("BusinessPlanStrategyTactic");
                        m.MapLeftKey("BusinessPlanStrategyID");
                        m.MapRightKey("BusinessPlanTacticID");
                    });

            HasOptional(t => t.BusinessPlan)
                .WithMany(t => t.Strategies)
                .HasForeignKey(d => d.BusinessPlanID);

        }
    }
}
