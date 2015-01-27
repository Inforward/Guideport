using System.ComponentModel.DataAnnotations.Schema;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Report;

namespace Portal.Data.Sql.EntityFramework.Mapping.Report
{
    public class ViewColumnMap : EntityTypeConfiguration<ViewColumn>
    {
        public ViewColumnMap()
        {
            // Primary Key
            HasKey(t => new { t.ViewID, t.ColumnID });

            // Properties
            Property(t => t.ViewID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ColumnID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Template)
                .HasMaxLength(1000);

            Property(t => t.DataFormat)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ViewColumn", "rpt");
            Property(t => t.ViewID).HasColumnName("ViewID");
            Property(t => t.ColumnID).HasColumnName("ColumnID");
            Property(t => t.Ordinal).HasColumnName("Ordinal");
            Property(t => t.Template).HasColumnName("Template");
            Property(t => t.DataFormat).HasColumnName("DataFormat");
            Property(t => t.Width).HasColumnName("Width");
            Property(t => t.IsSortable).HasColumnName("IsSortable");
            Property(t => t.IsEnabled).HasColumnName("IsEnabled");
            Property(t => t.IsLocked).HasColumnName("IsLocked");

            // Relationships
            HasRequired(t => t.Column)
                .WithMany(t => t.ViewColumns)
                .HasForeignKey(d => d.ColumnID);
            HasRequired(t => t.View)
                .WithMany(t => t.ViewColumns)
                .HasForeignKey(d => d.ViewID);
        }
    }
}
