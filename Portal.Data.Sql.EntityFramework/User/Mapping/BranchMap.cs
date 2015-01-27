using Portal.Infrastructure.Configuration;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class BranchMap : EntityTypeConfiguration<Branch>
    {
        public BranchMap()
        {
            // Primary Key
            HasKey(t => t.BranchID);

            // Properties
            // Table & Column Mappings
            ToTable("vwBranch", Settings.UserProfileSchema);

            Property(t => t.BranchID).HasColumnName("BranchID");
            Property(t => t.AffiliateID).HasColumnName("AffiliateID");
            Property(t => t.BranchNo).HasColumnName("BranchNo");
            Property(t => t.Address1).HasColumnName("Address1");
            Property(t => t.Address2).HasColumnName("Address2");
            Property(t => t.City).HasColumnName("City");
            Property(t => t.State).HasColumnName("State");
            Property(t => t.Country).HasColumnName("Country");
            Property(t => t.ZipCode).HasColumnName("ZipCode");
            Property(t => t.MailingAddress1).HasColumnName("MailingAddress1");
            Property(t => t.MailingAddress2).HasColumnName("MailingAddress2");
            Property(t => t.MailingCity).HasColumnName("MailingCity");
            Property(t => t.MailingState).HasColumnName("MailingState");
            Property(t => t.MailingCountry).HasColumnName("MailingCountry");
            Property(t => t.MailingZipCode).HasColumnName("MailingZipCode");
            Property(t => t.Phone).HasColumnName("Phone");
            Property(t => t.Fax).HasColumnName("Fax");
        }
    }
}
