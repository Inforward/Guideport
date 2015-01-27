using Portal.Infrastructure.Configuration;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.UserID);

            // Properties
            // Table & Column Mappings
            ToTable("vwUser", Settings.UserProfileSchema);

            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.ProfileTypeID).HasColumnName("ProfileTypeID");
            Property(t => t.ProfileTypeName).HasColumnName("ProfileTypeName");
            Property(t => t.AffiliateID).HasColumnName("AffiliateID");
            Property(t => t.AffiliateName).HasColumnName("AffiliateName");
            Property(t => t.ProfileID).HasColumnName("ProfileID");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.MiddleName).HasColumnName("MiddleName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.DisplayFirstName).HasColumnName("DisplayFirstName");
            Property(t => t.DisplayLastName).HasColumnName("DisplayLastName");
            Property(t => t.DisplayName).HasColumnName("DisplayName");
            Property(t => t.DBAName).HasColumnName("DBAName");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.Address1).HasColumnName("Address1");
            Property(t => t.Address2).HasColumnName("Address2");
            Property(t => t.City).HasColumnName("City");
            Property(t => t.State).HasColumnName("State");
            Property(t => t.ZipCode).HasColumnName("ZipCode");
            Property(t => t.Country).HasColumnName("Country");
            Property(t => t.PrimaryPhone).HasColumnName("PrimaryPhone");
            Property(t => t.HomePhone).HasColumnName("HomePhone");
            Property(t => t.WorkPhone).HasColumnName("WorkPhone");
            Property(t => t.Fax).HasColumnName("Fax");
            Property(t => t.SecurityProfileStartDate).HasColumnName("SecurityProfileStartDate");
            Property(t => t.StartDate).HasColumnName("StartDate");
            Property(t => t.TerminateDate).HasColumnName("TerminateDate");
            Property(t => t.UserStatusID).HasColumnName("UserStatusID");
            Property(t => t.UserStatusName).HasColumnName("UserStatusName");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.DeleteDate).HasColumnName("DeleteDate");

            // Business Metrics
            Property(t => t.GDCT12).HasColumnName("GDC_T12");
            Property(t => t.GDCPriorYear).HasColumnName("GDC_PriorYear");
            Property(t => t.AUM).HasColumnName("AUM");
            Property(t => t.AUMSplit).HasColumnName("AUM_Split");
            Property(t => t.ReturnOnAUM).HasColumnName("ReturnOnAUM");
            Property(t => t.NoOfClients).HasColumnName("NoOfClients");
            Property(t => t.NoOfAccounts).HasColumnName("NoOfAccounts");
            Property(t => t.RevenueRecurring).HasColumnName("RevenueRecurring");
            Property(t => t.RevenueNonRecurring).HasColumnName("RevenueNonRecurring");
            Property(t => t.BusinessValuationLow).HasColumnName("BusinessValuationLow");
            Property(t => t.BusinessValuationHigh).HasColumnName("BusinessValuationHigh");
            Property(t => t.AccountValueTotal).HasColumnName("AccountValueTotal");
            Property(t => t.AccountValueAverage).HasColumnName("AccountValueAverage");
            Property(t => t.MetricsUpdateDate).HasColumnName("MetricsUpdateDate");

            // Business Consultant
            Property(t => t.BusinessConsultantUserID).HasColumnName("BusinessConsultantUserID");
            Property(t => t.BusinessConsultantDisplayName).HasColumnName("BusinessConsultantDisplayName");
            Property(t => t.BusinessConsultantEmail).HasColumnName("BusinessConsultantEmail");

            // Relationships                
            HasRequired(t => t.Affiliate)
                .WithMany(t => t.Users)
                .HasForeignKey(t => t.AffiliateID);

            // Group Memberships
            HasMany(t => t.Groups)
                .WithMany(t => t.MemberUsers)
                .Map(m =>
                {
                    m.ToTable("GroupUser");
                    m.MapLeftKey("MemberUserID");
                    m.MapRightKey("GroupID");
                });

            // Group access
            HasMany(t => t.AccessibleGroups)
                .WithRequired(t => t.User)
                .HasForeignKey(t => t.UserID);

            HasMany(t => t.Branches)
                .WithMany(t => t.Users)
                .Map(m =>
                {
                    m.ToTable("vwBranchUser", Settings.UserProfileSchema);
                    m.MapLeftKey("UserID");
                    m.MapRightKey("BranchID");
                });

            //HasMany(t => t.ApplicationRoles)
            //    .WithMany(t => t.Users)
            //    .Map(m =>
            //    {
            //        m.ToTable("vwApplicationRoleUser");
            //        m.MapLeftKey("UserID");
            //        m.MapRightKey("ApplicationRoleID");
            //    });

            HasMany(t => t.Licenses)
                .WithMany(t => t.Users)
                .Map(m =>
                {
                    m.ToTable("vwLicenseUser", Settings.UserProfileSchema);
                    m.MapLeftKey("UserID");
                    m.MapRightKey("LicenseID");
                });
        }
    }
}
