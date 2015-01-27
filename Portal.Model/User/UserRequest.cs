using System.Collections.Generic;

namespace Portal.Model
{
    public class UserRequest : Pager
    {
        public UserRequest()
        {
            MemberGroupIDs = new List<int>();
            AccessibleGroupIDs = new List<int>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public int? AffiliateID { get; set; }
        public string ProfileID { get; set; }
        public List<int> MemberGroupIDs { get; set; }
        public List<int> AccessibleGroupIDs { get; set; }
        public int? ExcludeMemberGroupID { get; set; }
        public int? ExcludeAccessibleGroupID { get; set; }
        public int? ProfileTypeID { get; set; }
        public int? UserID { get; set; }
        public int? UserStatusID { get; set; }
        public IEnumerable<int> UserIDs { get; set; }
        public bool IncludeTerminated { get; set; }
        public bool IncludeApplicationRoles { get; set; }
        public bool IncludeBranchInfo { get; set; }
        public bool IncludeLicenseInfo { get; set; }
        public bool IncludeGroups { get; set; }
        public bool IncludeAccessibleGroups { get; set; }
        public bool IncludeAffiliates { get; set; }
        public bool IncludeAffiliateLogos { get; set; }
        public bool IncludeAffiliateFeatures { get; set; }
        public bool IncludeObjectCache { get; set; }
    }
}
