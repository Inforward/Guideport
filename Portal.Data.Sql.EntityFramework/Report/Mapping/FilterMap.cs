using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Report;

namespace Portal.Data.Sql.EntityFramework.Mapping.Report
{
    public class FilterMap : EntityTypeConfiguration<Filter>
    {
        public FilterMap()
        {
            // Primary Key
            HasKey(t => t.FilterID);

            // Properties
            Property(t => t.FilterID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.DataTypeName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.ParameterName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Filter", "rpt");
            Property(t => t.FilterID).HasColumnName("FilterID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Label).HasColumnName("Label");
            Property(t => t.IsRequired).HasColumnName("IsRequired");
            Property(t => t.DataTypeName).HasColumnName("DataTypeName");
            Property(t => t.ParameterName).HasColumnName("ParameterName");
            Property(t => t.InputType).HasColumnName("InputType");

            // Relationships
            HasMany(t => t.Views)
                .WithMany(t => t.Filters)
                .Map(m =>
                {
                    m.ToTable("ViewFilter", "rpt");
                    m.MapLeftKey("FilterID");
                    m.MapRightKey("ViewID");
                });
        }
    }
}
