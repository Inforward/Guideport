using System.Data.Entity.ModelConfiguration;
using Portal.Model.Geo;

namespace Portal.Data.Sql.EntityFramework.Geo.Mapping
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            // Primary Key
            HasKey(t => t.CityID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            ToTable("City", "geo");
            Property(t => t.CityID).HasColumnName("CityID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.StateProvinceID).HasColumnName("StateProvinceID");
            Property(t => t.CountryID).HasColumnName("CountryID");
            Property(t => t.TimeZoneID).HasColumnName("TimeZoneID");
            Property(t => t.Latitude).HasColumnName("Latitude");
            Property(t => t.Longitude).HasColumnName("Longitude");

            // Relationships
            HasOptional(t => t.Country)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.CountryID);

            HasOptional(t => t.StateProvince)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.StateProvinceID);

            HasOptional(t => t.TimeZone)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.TimeZoneID);

        }
    }
}
