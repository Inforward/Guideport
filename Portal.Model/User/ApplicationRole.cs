using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class ApplicationRole
    {
        public ApplicationRole()
        {
            ApplicationRoleUsers = new List<ApplicationRoleUser>();
            ApplicationRoleAccesses = new List<ApplicationRoleAccess>();
        }

        public int ApplicationRoleID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ICollection<ApplicationRoleAccess> ApplicationRoleAccesses { get; set; }

        [IgnoreDataMember]
        public ICollection<ApplicationRoleUser> ApplicationRoleUsers { get; set; }

        [NotMapped]
        public bool Enabled { get; set; }

        [NotMapped]
        public int ApplicationAccessID { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, ApplicationRoleID);
        }
    }

    public class PortalRoleValues
    {
        public const string SurveyAdmin = "Portal.SurveyAdmin";
        public const string ContentAdmin = "Portal.ContentAdmin";
        public const string AdvisorView = "Portal.AdvisorView";
        public const string Reporting = "Portal.Reporting";
        public const string GroupAdmin = "Portal.GroupAdmin";
        public const string UserAdmin = "Portal.UserAdmin";
        public const string AffiliateAdmin = "Portal.AffiliateAdmin";
    }
}
