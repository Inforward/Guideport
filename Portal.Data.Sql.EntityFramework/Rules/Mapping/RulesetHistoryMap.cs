using System.Data.Entity.ModelConfiguration;
using Portal.Model.Rules;

namespace Portal.Data.Sql.EntityFramework.Rules.Mapping
{
    public class RulesetHistoryMap : EntityTypeConfiguration<RulesetHistory>
    {
        public RulesetHistoryMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            Property(t => t.RuleSet)
                .IsRequired();

            Property(t => t.ModifiedBy)
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("RulesetHistory", "app");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.MajorVersion).HasColumnName("MajorVersion");
            Property(t => t.MinorVersion).HasColumnName("MinorVersion");
            Property(t => t.RuleSet).HasColumnName("RuleSet");
            Property(t => t.Status).HasColumnName("Status");
            Property(t => t.AssemblyPath).HasColumnName("AssemblyPath");
            Property(t => t.ActivityName).HasColumnName("ActivityName");
            Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}
