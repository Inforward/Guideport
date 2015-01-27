using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AuditLogMap : EntityTypeConfiguration<AuditLog>
    {
        public AuditLogMap()
        {
            // Primary Key
            HasKey(t => t.AuditLogID);

            // Properties
            // Table & Column Mappings
            ToTable("AuditLog", "app");

            Property(t => t.AuditLogID).HasColumnName("AuditLogID");
            Property(t => t.AuditTypeID).HasColumnName("AuditTypeID");
            Property(t => t.AuditDate).HasColumnName("AuditDate");
            Property(t => t.TableName).HasColumnName("TableName");
            Property(t => t.TableIDValue).HasColumnName("TableIDValue");
            Property(t => t.RelatedKeyName).HasColumnName("RelatedKeyName");
            Property(t => t.RelatedKeyValue).HasColumnName("RelatedKeyValue");
            Property(t => t.OldData).HasColumnName("OldData");
            Property(t => t.NewData).HasColumnName("NewData");
            Property(t => t.UserID).HasColumnName("UserID");
        }
    }
}
