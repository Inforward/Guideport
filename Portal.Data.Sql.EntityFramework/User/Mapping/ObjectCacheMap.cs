using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ObjectCacheMap : EntityTypeConfiguration<ObjectCache>
    {
        public ObjectCacheMap()
        {
            // Primary Key
            HasKey(t => new { t.UserID, t.Key });

            // Properties
            Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.ValueSerialized)
                .IsRequired();

            // Table & Column Mappings
            ToTable("ObjectCache", "usr");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.Key).HasColumnName("Key");
            Property(t => t.ValueSerialized).HasColumnName("Value");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");

            // Relationships
            HasRequired(t => t.User)
                .WithMany(t => t.ObjectCache)
                .HasForeignKey(d => d.UserID);

        }
    }
}
