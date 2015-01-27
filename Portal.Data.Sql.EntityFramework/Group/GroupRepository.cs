using Portal.Infrastructure.Helpers;
using Portal.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Portal.Data.Sql.EntityFramework
{
    public class GroupRepository : EntityRepository<MasterContext>, IGroupRepository
    {
        public void AddMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var oldData = FindBy<Group>(g => g.GroupID == groupId).Include(g => g.MemberUsers).First().MemberUsers.Select(u => u.UserID).ToList();
            var newUserIds = userIds as List<int> ?? userIds.ToList();
            var newData = new List<int>(oldData).Concat(newUserIds).ToList();

            Audit<Group>("MemberUsers", AuditTypes.Insert, null, groupId, auditUserId, new { MemberUserIDs = oldData }, new { MemberUserIDs = newData},
                () => Context.Database.ExecuteSqlCommand("grp.AddMemberUsers @groupId, @userIds",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@userIds", string.Join(",", newUserIds))));
        }

        public void RemoveMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var oldData = FindBy<Group>(g => g.GroupID == groupId).Include(g => g.MemberUsers).First().MemberUsers.Select(u => u.UserID).ToList();
            var removeUserIds = userIds as List<int> ?? userIds.ToList();
            var newData = oldData.Where(o => removeUserIds.All(r => o != r)).ToList();

            Audit<Group>("MemberUsers", AuditTypes.Delete, null, groupId, auditUserId, new { MemberUserIDs = oldData }, new { MemberUserIDs = newData }, 
                () => Context.Database.ExecuteSqlCommand("grp.RemoveMemberUsers @groupId, @userIds",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@userIds", string.Join(",", removeUserIds))));
        }

        public void AddAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var oldData = FindBy<Group>(g => g.GroupID == groupId).Include(g => g.AccessibleUsers).First().AccessibleUsers.Select(u => u.UserID).ToList();
            var newUserIds = userIds as List<int> ?? userIds.ToList();
            var newData = new List<int>(oldData).Concat(newUserIds).ToList();

            Audit<Group>("AccessibleUsers", AuditTypes.Insert, null, groupId, auditUserId, new { UserIDs = oldData }, new { UserIDs = newData },
                () => Context.Database.ExecuteSqlCommand("grp.AddAccessibleUsers @groupId, @userIds",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@userIds", string.Join(",", newUserIds))));
        }

        public void RemoveAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var oldData = FindBy<Group>(g => g.GroupID == groupId).Include(g => g.AccessibleUsers).First().AccessibleUsers.Select(u => u.UserID).ToList();
            var removeUserIds = userIds as List<int> ?? userIds.ToList();
            var newData = oldData.Where(o => removeUserIds.All(r => o != r)).ToList();

            Audit<Group>("AccessibleUsers", AuditTypes.Delete, null, groupId, auditUserId, new { UserIDs = oldData }, new { UserIDs = newData },
                () => Context.Database.ExecuteSqlCommand("grp.RemoveAccessibleUsers @groupId, @userIds",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@userIds", string.Join(",", removeUserIds))));
        }

        public void AddMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId)
        {
            var oldData = FindBy<Group>(g => g.GroupID == groupId).Include(g => g.MemberGroups).First().MemberGroups.Select(g => g.GroupID).ToList();
            var newGroupIds = groupIds as List<int> ?? groupIds.ToList();
            var newData = new List<int>(oldData).Concat(newGroupIds).ToList();

            Audit<Group>("MemberGroups", AuditTypes.Insert,  null, groupId, auditUserId, new { MemberGroupIDs = oldData }, new { MemberGroupIDs = newData },
                () => Context.Database.ExecuteSqlCommand("grp.AddMemberGroups @groupId, @groupIds",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@groupIds", string.Join(",", newGroupIds))));
        }

        public void RemoveMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId)
        {
            var oldData = FindBy<Group>(g => g.GroupID == groupId).Include(g => g.MemberGroups).First().MemberGroups.Select(g => g.GroupID).ToList();
            var removeGroupIds = groupIds as List<int> ?? groupIds.ToList();
            var newData = oldData.Where(o => removeGroupIds.All(r => o != r)).ToList();

            Audit<Group>("MemberGroups", AuditTypes.Delete, null, groupId, auditUserId, new { MemberGroupIDs = oldData }, new { MemberGroupIDs = newData },
                () => Context.Database.ExecuteSqlCommand("grp.RemoveMemberGroups @groupId, @groupIds",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@groupIds", string.Join(",", removeGroupIds))));
        }

        public void DeleteGroup(int groupId, int auditUserId)
        {
            var removeGroup = FindBy<Group>(g => g.GroupID == groupId).First();

            Audit<Group>(AuditTypes.Delete, groupId, auditUserId, removeGroup, null,
                () => Context.Database.ExecuteSqlCommand("grp.DeleteGroup @groupId", new SqlParameter("@groupId", groupId)));
        }

        public IEnumerable<Group> GetGroupsFromHierarchy(IEnumerable<int> groupIds)
        {
            var groups = Context.Database.SqlQuery<Group>("grp.GetGroupsFromHierarchy @groupIDList",
                                new SqlParameter("@groupIDList", groupIds.ToCsv()));

            return groups;
        }
    }
}
