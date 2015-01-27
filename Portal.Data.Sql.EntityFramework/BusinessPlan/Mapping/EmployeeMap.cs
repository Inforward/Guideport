using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            // Primary Key
            HasKey(t => t.EmployeeID);

            // Properties
            Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.MiddleInitial)
                .HasMaxLength(1);

            Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("BusinessPlanEmployee");
            Property(t => t.EmployeeID).HasColumnName("BusinessPlanEmployeeID");
            Property(t => t.EmployeeParentID).HasColumnName("BusinessPlanEmployeeParentID");
            Property(t => t.BusinessPlanID).HasColumnName("BusinessPlanID");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.MiddleInitial).HasColumnName("MiddleInitial");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasMany(t => t.EmployeeRoles)
                .WithMany(t => t.Employees)
                .Map(m =>
                    {
                        m.ToTable("BusinessPlanEmployeeEmployeeRole");
                        m.MapLeftKey("BusinessPlanEmployeeID");
                        m.MapRightKey("BusinessPlanEmployeeRoleID");
                    });

            HasRequired(t => t.BusinessPlan)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.BusinessPlanID);

            HasOptional(t => t.ParentEmployee)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.EmployeeParentID);
        }
    }
}
