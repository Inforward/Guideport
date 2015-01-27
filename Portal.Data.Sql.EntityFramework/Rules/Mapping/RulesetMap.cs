using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Rules;

namespace Portal.Data.Sql.EntityFramework.Rules.Mapping
{
    public class RulesetMap : EntityTypeConfiguration<Ruleset>
    {
        public RulesetMap()
        {
            // Primary Key
            HasKey(t => new { t.Name, t.MajorVersion, t.MinorVersion });

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            Property(t => t.MajorVersion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.MinorVersion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.RuleSetDefinition)
                .IsRequired();

            Property(t => t.ModifiedBy)
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("Ruleset", "app");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.MajorVersion).HasColumnName("MajorVersion");
            Property(t => t.MinorVersion).HasColumnName("MinorVersion");
            Property(t => t.RuleSetDefinition).HasColumnName("RuleSet");
            Property(t => t.Status).HasColumnName("Status");
            Property(t => t.AssemblyPath).HasColumnName("AssemblyPath");
            Property(t => t.ActivityName).HasColumnName("ActivityName");
            Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}
