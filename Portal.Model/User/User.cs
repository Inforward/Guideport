using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using Portal.Model.Attributes;

namespace Portal.Model
{
    public class User
    {
        public User()
        {
            ObjectCache = new List<ObjectCache>();
            SurveyResponses = new List<SurveyResponse>();
            Groups = new List<Group>();
            AccessibleGroups = new List<GroupUserAccess>();
        }

        public int UserID { get; set; }
        public int ProfileTypeID { get; set; }
        public string ProfileTypeName { get; set; }
        public int UserStatusID { get; set; }
        public string UserStatusName { get; set; }
        public int AffiliateID { get; set; }
        public string AffiliateName { get; set; }
        public string ProfileID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayFirstName { get; set; }
        public string DisplayLastName { get; set; }
        public string DisplayName { get; set; }
        public string DBAName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PrimaryPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Fax { get; set; }
        public DateTime? SecurityProfileStartDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TerminateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public double? GDCPriorYear { get; set; }
        public decimal? AUMSplit { get; set; }
        public int? NoOfAccounts { get; set; }
        public double? RevenueRecurring { get; set; }
        public double? RevenueNonRecurring { get; set; }
        public double? BusinessValuationHigh { get; set; }
        public decimal? AccountValueTotal { get; set; }
        public DateTime? MetricsUpdateDate { get; set; }

        public int? BusinessConsultantUserID { get; set; }
        public string BusinessConsultantDisplayName { get; set; }
        public string BusinessConsultantEmail { get; set; }

        [MappedObjective("Total Assets")]
        public decimal? AUM { get; set; }

        [MappedObjective("Gross Dealer Concession")]
        public double? GDCT12 { get; set; }

        [MappedObjective("Number of Clients")]
        public int? NoOfClients { get; set; }

        [MappedObjective("Return on Total Assets")]
        public double? ReturnOnAUM { get; set; }

        [MappedObjective("Average Client Account Size")]
        public decimal? AccountValueAverage { get; set; }

        [MappedObjective("Business Value")]
        public double? BusinessValuationLow { get; set; }

        public Affiliate Affiliate { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<GroupUserAccess> AccessibleGroups { get; set; }

        public ICollection<ObjectCache> ObjectCache { get; set; }
        public ICollection<Branch> Branches { get; set; }
        public ICollection<ApplicationRoleUser> ApplicationRoles { get; set; }
        public ICollection<License> Licenses { get; set; }

        [IgnoreDataMember]
        public ICollection<SurveyResponse> SurveyResponses { get; set; }

        [NotMapped]
        public string Address
        {
            get
            {
                var address = new Address()
                {
                    AddressLine1 = Address1,
                    AddressLine2 = Address2,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    Country = Country
                };

                return address.ToString();
            }
        }

        [NotMapped]
        public string Location
        {
            get
            {
                var location = string.Empty;

                if (!string.IsNullOrWhiteSpace(City))
                    location = City;

                if (!string.IsNullOrWhiteSpace(State))
                {
                    if (!string.IsNullOrWhiteSpace(location))
                        location += ", ";

                    location += State.ToUpper();
                }

                return location;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public string DBAAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(DBAName))
                {
                    return DBAName + Environment.NewLine + Address;
                }

                return Address;
            }
        }
    }

    public enum UserStatuses
    {
        Active = 1,
        Disabled = 2,
        Terminated = 3,
        Deleted = 4,
        Suspended = 5
    }

    public enum SecurityProfileTypes
    {
        Unknown = 0,
        FinancialAdvisor = 1,
        RegisteredSalesAssistant = 2,
        RegisteredEmployee = 3,
        NonRegisteredBranchAssistant = 4,
        NonRegisteredEmployee = 5,
        ForDataConversion = 6,
        Corporation = 7,
        SubClearBdFinancialAdvisor = 8,
        SubClearBdPersonnel = 9,
        RegisteredRockwoodEmployee = 10,
        RegisteredAdvisorServicesEmployee = 11,
        NonRegisteredRockwoodEmployee = 12,
        RegisteredPendingPassingOfExaminations = 13,
        RiaBusiness = 14,
        WsccRegisteredRepresentative = 15,
        RegisteredSalesAssistantPendinPassingExams = 16,
        None = 17,
        RmisDivisionEmployee = 18,
        InvestmentAdvisorRepresentative = 19,
        TempContractor = 20,
        ProducingRsa = 21,
        Solicitor = 22,
        InsuranceOnlyAdvisor = 23,
        NonRegisteredContractor = 24,
        RegisteredContractor = 25,
        ThirdPartyVendors = 26,
        RegisteredOperationsProfessional = 27,
        FAAM = 28
    }

    public static class UserExtensions
    {
        public static bool HasProfileType(this User user, string profileType)
        {
            return user != null && user.HasProfileType(profileType);
        }

        public static bool HasProfileType(this User user, string[] profileTypes)
        {
            return user != null && !string.IsNullOrEmpty(user.ProfileTypeName) && profileTypes.Any(p => user.ProfileTypeName == p);
        }

        public static bool IsInRole(this User user, int roleId)
        {
            return user != null && user.ApplicationRoles != null && user.ApplicationRoles.Any(r => r.ApplicationRoleID == roleId);
        }

        public static bool IsInRole(this User user, string roles)
        {
            return user != null && user.IsInRole(roles.Split(','));
        }

        public static bool IsInRole(this User user, params string[] requiredRoles)
        {
            if (requiredRoles.Length == 0) return true;

            if (user == null || user.ApplicationRoles == null) return false;

            return requiredRoles.Any(role => user.ApplicationRoles.Any(r => r.RoleName.Equals(role, StringComparison.InvariantCultureIgnoreCase)));
        }

        public static bool IsAdmin(this User user)
        {
            var roles = new[]
            {
                PortalRoleValues.AffiliateAdmin,
                PortalRoleValues.ContentAdmin,
                PortalRoleValues.GroupAdmin,
                PortalRoleValues.Reporting,
                PortalRoleValues.SurveyAdmin,
                PortalRoleValues.UserAdmin
            };

            return user.IsInRole(roles);
        }

        public static bool IsRestricted(this User user, string roleName)
        {
            if (user.ApplicationRoles == null)
                return true;

            var role = user.ApplicationRoles.FirstOrDefault(r => r.RoleName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

            if (role == null)
                return true;

            return role.ApplicationAccessID == (int) ApplicationAccessOptions.Restricted;
        }

        public static T GetCachedObject<T>(this User user, string key, T defaultValue = default(T))
        {
            if (user == null || user.ObjectCache == null) return defaultValue;

            var item = user.ObjectCache.FirstOrDefault(o => o.Key == key);

            return item != null ? item.GetValue(defaultValue) : defaultValue;
        }

        public static string AsString(this ICollection<ApplicationRoleUser> applicationRoles, string preText = "", string separator = ", ")
        {
            if (applicationRoles == null) return string.Empty;

            var sb = new StringBuilder();
            var roles = applicationRoles.OrderBy(r => r.ApplicationRole.Name).ToArray();

            if (!string.IsNullOrEmpty(preText))
                sb.Append(preText + separator);

            for(var i = 0; i < roles.Length; i++)
            {
                if (i > 0)
                    sb.Append(separator);

                sb.AppendFormat("{0}-{1}", roles[i].ApplicationRole.Name, roles[i].ApplicationAccess.Name);
            }

            return sb.ToString();
        }
    }
}
