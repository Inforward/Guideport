using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class FileInfoMap : EntityTypeConfiguration<FileInfo>
    {
        public FileInfoMap()
        {
            // Primary Key
            HasKey(t => t.FileID);

            // Properties
            // Table & Column Mappings
            ToTable("FileInfo");

            Property(t => t.FileID).HasColumnName("FileID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.SizeBytes).HasColumnName("SizeBytes");
            Property(t => t.Extension).HasColumnName("Extension");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
