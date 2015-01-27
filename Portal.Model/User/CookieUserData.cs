using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Model
{
    public class CookieUserData
    {
        public CookieUserData()
        {
            Roles = new List<CookieUserRole>();
        }

        public int UserID { get; set; }
        public string ProfileID { get; set; }
        public int AffiliateID { get; set; }
        public string AffiliateName { get; set; }
        public int ProfileTypeID { get; set; }
        public string ProfileTypeName { get; set; }
        public string SourceDomain { get; set; }
        public string IdpPartner { get; set; }
        public string DisplayName { get; set; }
        public string BusinessConsultantDisplayName { get; set; }
        public bool SuccessionPlanningEnabled { get; set; }
        public bool Connect2ClientsEnabled { get; set; }
        public string Connect2ClientsMessage { get; set; }
        public DateTime Issued { get; set; }
        public IEnumerable<CookieUserRole> Roles { get; set; }
    }

    public class CookieUserRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AccessID { get; set; }
    }

    public static class CookieUserDataExtensions
    {
        public static CookieUserData ToCookieUserData(this User user)
        {
            return new CookieUserData()
            {
                AffiliateID = user.AffiliateID,
                AffiliateName = user.AffiliateName,
                ProfileID = user.ProfileID,
                UserID = user.UserID,
                DisplayName = user.DisplayName,
                ProfileTypeID = user.ProfileTypeID,
                ProfileTypeName = user.ProfileTypeName,
                BusinessConsultantDisplayName = user.BusinessConsultantDisplayName,
                Issued = DateTime.Now,
                Roles = user.ApplicationRoles.Select(role => new CookieUserRole
                {
                    ID = role.ApplicationRoleID,
                    Name = role.RoleName,
                    AccessID = role.ApplicationAccessID
                })
            };
        }

        public static User ToUser(this CookieUserData cookieData)
        {
            return new User
            {
                UserID = cookieData.UserID,
                AffiliateID = cookieData.AffiliateID,
                AffiliateName = cookieData.AffiliateName,
                ProfileID = cookieData.ProfileID,
                DisplayName = cookieData.DisplayName,
                ProfileTypeID = cookieData.ProfileTypeID,
                ProfileTypeName = cookieData.ProfileTypeName,
                BusinessConsultantDisplayName = cookieData.BusinessConsultantDisplayName,
                ApplicationRoles = cookieData.Roles.Select(role => new ApplicationRoleUser
                {
                    ApplicationRoleID = role.ID,
                    UserID = cookieData.UserID,
                    ApplicationRole = new ApplicationRole{ ApplicationRoleID = role.ID, Name = role.Name },
                    ApplicationAccessID = role.AccessID
                }).ToList()
            };
        }
    }
}