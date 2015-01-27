using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class ApplicationAccess
    {
        public ApplicationAccess()
        {
            ApplicationRoleAccesses = new List<ApplicationRoleAccess>();
            ApplicationRoleUsers = new List<ApplicationRoleUser>();
        }

        public int ApplicationAccessID { get; set; }
        public string Name { get; set; }

        [IgnoreDataMember]
        public ICollection<ApplicationRoleAccess> ApplicationRoleAccesses { get; set; }

        [IgnoreDataMember]
        public ICollection<ApplicationRoleUser> ApplicationRoleUsers { get; set; }
    }

    public enum ApplicationAccessOptions
    {
        Unrestricted = 1,
        Restricted = 2
    }
}
