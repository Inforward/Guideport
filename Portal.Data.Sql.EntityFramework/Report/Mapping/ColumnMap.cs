using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Report;

namespace Portal.Data.Sql.EntityFramework.Mapping.Report
{
    public class ColumnMap : EntityTypeConfiguration<Column>
    {
        public ColumnMap()
        {
            // Primary Key
            HasKey(t => t.ColumnID);

            // Properties
            Property(t => t.ColumnID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.DataField)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.DataTypeName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.DataFormat)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Column", "rpt");
            Property(t => t.ColumnID).HasColumnName("ColumnID");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.DataField).HasColumnName("DataField");
            Property(t => t.DataFormat).HasColumnName("DataFormat");
            Property(t => t.DataTypeName).HasColumnName("DataTypeName");
            Property(t => t.Width).HasColumnName("Width");
        }
    }
}
