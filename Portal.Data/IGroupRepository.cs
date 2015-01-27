using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Portal.Model;

namespace Portal.Data
{
    public interface IGroupRepository : IEntityRepository
    {
        void AddMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId);
        void RemoveMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId);

        void AddAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId);
        void RemoveAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId);

        void AddMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId);
        void RemoveMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId);

        void DeleteGroup(int groupId, int auditUserId);

        IEnumerable<Group> GetGroupsFromHierarchy(IEnumerable<int> groupIds);
    }
}