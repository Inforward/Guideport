using System.Data.Entity.ModelConfiguration;
using Portal.Model.Geo;

namespace Portal.Data.Sql.EntityFramework.Geo.Mapping
{
    public class TimeZoneMap : EntityTypeConfiguration<TimeZone>
    {
        public TimeZoneMap()
        {
            // Primary Key
            HasKey(t => t.TimeZoneID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            // Table & Column Mappings
            ToTable("TimeZone", "geo");
            Property(t => t.TimeZoneID).HasColumnName("TimeZoneID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.OffsetGMT).HasColumnName("OffsetGMT");
            Property(t => t.OffsetDST).HasColumnName("OffsetDST");
        }
    }
}
