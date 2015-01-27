using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class BusinessPlanMap : EntityTypeConfiguration<BusinessPlan>
    {
        public BusinessPlanMap()
        {
            // Primary Key
            HasKey(t => t.BusinessPlanID);

            // Properties
            // Table & Column Mappings
            ToTable("BusinessPlan");
            Property(t => t.BusinessPlanID).HasColumnName("BusinessPlanID");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.Year).HasColumnName("Year");
            Property(t => t.MissionWhat).HasColumnName("MissionWhat");
            Property(t => t.MissionHow).HasColumnName("MissionHow");
            Property(t => t.MissionWhy).HasColumnName("MissionWhy");
            Property(t => t.VisionOneYear).HasColumnName("VisionOneYear");
            Property(t => t.VisionFiveYear).HasColumnName("VisionFiveYear");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");
            Property(t => t.DeleteUserID).HasColumnName("DeleteUserID");
            Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            Property(t => t.DeleteDateUtc).HasColumnName("DeleteDateUTC");
        }
    }
}
