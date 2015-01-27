using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Report;

namespace Portal.Data.Sql.EntityFramework.Mapping.Report
{
    public class ViewMap : EntityTypeConfiguration<View>
    {
        public ViewMap()
        {
            // Primary Key
            HasKey(t => t.ViewID);

            // Properties
            Property(t => t.ViewID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.StoredProcedureName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("View", "rpt");
            Property(t => t.ViewID).HasColumnName("ViewID");
            Property(t => t.CategoryID).HasColumnName("CategoryID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.FullName).HasColumnName("FullName");
            Property(t => t.StoredProcedureName).HasColumnName("StoredProcedureName");
            Property(t => t.PageSize).HasColumnName("PageSize");
            Property(t => t.IsPageable).HasColumnName("IsPageable");
            Property(t => t.IsSortable).HasColumnName("IsSortable");
            Property(t => t.IsEnabled).HasColumnName("IsEnabled");

            // Relationships
            HasRequired(t => t.Category)
                .WithMany(t => t.Views)
                .HasForeignKey(d => d.CategoryID);
        }
    }
}
