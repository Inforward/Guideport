
using System.Collections.Generic;

namespace Portal.Model
{
    public class GroupPresentation
    {
        public Group Group { get; set; }
        public int MemberUserCount { get; set; }
        public int MemberGroupCount { get; set; }
        public int AccessibleUserCount { get; set; }
        public ICollection<Group> MemberGroups { get; set; }
        public ICollection<User> MemberUsers { get; set; }
        public ICollection<GroupUserAccess> AccessibleUsers { get; set; }
    }
}
