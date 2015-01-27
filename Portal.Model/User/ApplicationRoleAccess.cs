using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class ApplicationRoleAccess
    {
        public int ApplicationRoleID { get; set; }
        public int ApplicationAccessID { get; set; }
        public string Description { get; set; }
        public virtual ApplicationAccess ApplicationAccess { get; set; }

        [IgnoreDataMember]
        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
