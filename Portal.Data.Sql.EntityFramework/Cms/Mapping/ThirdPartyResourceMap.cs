using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework
{
    public class ThirdPartyResourceMap : EntityTypeConfiguration<ThirdPartyResource>
    {
        public ThirdPartyResourceMap()
        {
            // Primary Key
            HasKey(t => t.ThirdPartyResourceID);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.Description)
                .HasMaxLength(1000);

            Property(t => t.AddressLine1)
                .HasMaxLength(100);

            Property(t => t.AddressLine2)
                .HasMaxLength(100);

            Property(t => t.City)
                .HasMaxLength(50);

            Property(t => t.State)
                .HasMaxLength(50);

            Property(t => t.PostalCode)
                .HasMaxLength(12);

            Property(t => t.Country)
                .HasMaxLength(50);

            Property(t => t.PhoneNo)
                .HasMaxLength(50);

            Property(t => t.PhoneNoExt)
                .HasMaxLength(25);

            Property(t => t.FaxNo)
                .HasMaxLength(50);

            Property(t => t.Email)
                .HasMaxLength(100);

            Property(t => t.WebsiteUrl)
                .HasMaxLength(256);

            Property(t => t.Services)
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("SiteThirdPartyResource", "cms");

            Property(t => t.ThirdPartyResourceID).HasColumnName("SiteThirdPartyResourceID");
            Property(t => t.AddressLine1).HasColumnName("AddressLine1");
            Property(t => t.AddressLine2).HasColumnName("AddressLine2");
            Property(t => t.City).HasColumnName("City");
            Property(t => t.State).HasColumnName("State");
            Property(t => t.PostalCode).HasColumnName("PostalCode");
            Property(t => t.Country).HasColumnName("Country");
            Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            Property(t => t.PhoneNoExt).HasColumnName("PhoneNoExt");
            Property(t => t.FaxNo).HasColumnName("FaxNo");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.WebsiteUrl).HasColumnName("WebsiteUrl");
            Property(t => t.Services).HasColumnName("Services");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

        }
    }
}
