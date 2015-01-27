using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class ApplicationRoleUser : Auditable
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        public int ApplicationRoleID { get; set; }
        public int ApplicationAccessID { get; set; }
        public virtual ApplicationAccess ApplicationAccess { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }

        [NotMapped]
        public override DateTime CreateDateUtc { get; set; }
        [NotMapped]
        public override int ModifyUserID { get; set; }
        [NotMapped]
        public override DateTime ModifyDate { get; set; }
        [NotMapped]
        public override DateTime ModifyDateUtc { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }

        [NotMapped]
        public string RoleName
        {
            get { return ApplicationRole != null ? ApplicationRole.Name : null; }
        }

        [NotMapped]
        public string RoleDescription
        {
            get { return ApplicationRole != null ? ApplicationRole.Description : null; }
        }
    }
}
