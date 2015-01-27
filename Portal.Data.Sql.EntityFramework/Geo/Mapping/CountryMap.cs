using System.Data.Entity.ModelConfiguration;
using Portal.Model.Geo;

namespace Portal.Data.Sql.EntityFramework.Geo.Mapping
{
    public class CountryMap : EntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            // Primary Key
            HasKey(t => t.CountryID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.CountryCode)
                .IsRequired()
                .HasMaxLength(2);

            Property(t => t.PostalCodeRegEx)
                .HasMaxLength(256);

            // Table & Column Mappings
            ToTable("Country", "geo");
            Property(t => t.CountryID).HasColumnName("CountryID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.CountryCode).HasColumnName("CountryCode");
            Property(t => t.PostalCodeRegEx).HasColumnName("PostalCodeRegEx");
        }
    }
}
