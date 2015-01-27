using System.Collections.Generic;

namespace Portal.Model
{
    public class GroupRequest : Pager
    {
        public int? GroupID { get; set; }
        public bool IncludeMemberUsers { get; set; }
        public bool IncludeMemberGroups { get; set; }
        public bool IncludeMemberCounts { get; set; }
        public bool IncludeAccessibleUsers { get; set; }
        public int MemberOfGroupID { get; set; }
        public int ExcludeGroupID { get; set; }
    }
}
