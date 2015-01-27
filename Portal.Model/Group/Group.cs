using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Model
{
    public class Group : Auditable
    {
        public Group()
        {
            MemberGroups = new List<Group>();
            ParentGroups = new List<Group>();
            MemberUsers = new List<User>();
            AccessibleUsers = new List<GroupUserAccess>();
        }

        public int GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsReadOnly { get; set; }
        public ICollection<Group> MemberGroups { get; set; }
        public ICollection<Group> ParentGroups { get; set; }

        public ICollection<User> MemberUsers { get; set; }
        public ICollection<GroupUserAccess> AccessibleUsers { get; set; }

        [NotMapped]
        public int MemberUserCount { get; set; }

        [NotMapped]
        public int MemberGroupCount { get; set; }

        [NotMapped]
        public int AccessibleUserCount { get; set; }
    }


}
