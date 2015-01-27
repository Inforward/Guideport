using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class FileMap : EntityTypeConfiguration<File>
    {
        public FileMap()
        {
            // Primary Key
            HasKey(t => t.FileID);

            // Properties
            // Table & Column Mappings
            ToTable("File");

            Property(t => t.FileID).HasColumnName("FileID");
            Property(t => t.Data).HasColumnName("Data");

            // Relationships
            HasOptional(o => o.Info)
                .WithRequired(o => o.File);

        }
    }
}
