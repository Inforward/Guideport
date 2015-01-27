using Portal.Infrastructure.Configuration;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class LicenseMap : EntityTypeConfiguration<License>
    {
        public LicenseMap()
        {
            // Primary Key
            HasKey(t => t.LicenseID);

            // Properties
            // Table & Column Mappings
            ToTable("vwLicense", Settings.UserProfileSchema);

            Property(t => t.LicenseID).HasColumnName("LicenseID");
            Property(t => t.LicenseTypeID).HasColumnName("LicenseTypeID");
            Property(t => t.LicenseTypeName).HasColumnName("LicenseTypeName");
            Property(t => t.LicenseExamTypeID).HasColumnName("LicenseExamTypeID");
            Property(t => t.LicenseExamTypeName).HasColumnName("LicenseExamTypeName");
            Property(t => t.RegistrationCategory).HasColumnName("RegistrationCategory");
            Property(t => t.Description).HasColumnName("Description");
        }
    }
}
