using System.Data.Entity.ModelConfiguration;
using Portal.Model.Geo;

namespace Portal.Data.Sql.EntityFramework.Geo.Mapping
{
    public class StateProvinceMap : EntityTypeConfiguration<StateProvince>
    {
        public StateProvinceMap()
        {
            // Primary Key
            HasKey(t => t.StateProvinceID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            Property(t => t.StateCode)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            ToTable("StateProvince", "geo");
            Property(t => t.StateProvinceID).HasColumnName("StateProvinceID");
            Property(t => t.CountryID).HasColumnName("CountryID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.StateCode).HasColumnName("StateCode");

            // Relationships
            HasRequired(t => t.Country)
                .WithMany(t => t.StateProvinces)
                .HasForeignKey(d => d.CountryID);

        }
    }
}
