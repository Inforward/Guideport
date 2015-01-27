using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class SwotMap : EntityTypeConfiguration<Swot>
    {
        public SwotMap()
        {
            // Primary Key
            HasKey(t => t.SwotID);

            // Properties
            Property(t => t.Description)
                .IsRequired();

            // Table & Column Mappings
            ToTable("BusinessPlanSwot");
            Property(t => t.SwotID).HasColumnName("BusinessPlanSwotID");
            Property(t => t.BusinessPlanID).HasColumnName("BusinessPlanID");
            Property(t => t.Type).HasColumnName("SwotType");
            Property(t => t.Description).HasColumnName("SwotDescription");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");
            

            // Relationships
            HasRequired(t => t.BusinessPlan)
                .WithMany(t => t.Swots)
                .HasForeignKey(d => d.BusinessPlanID);

        }
    }
}
