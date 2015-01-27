using System.Collections.Generic;
using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class GroupServiceClient : IGroupService
    {
        private readonly ServiceClient<IGroupServiceChannel> _groupService = new ServiceClient<IGroupServiceChannel>();

        public Group GetGroup(GroupRequest request)
        {
            var proxy = _groupService.CreateProxy();
            return proxy.GetGroup(request);
        }

        public void CreateGroup(ref Group group, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.CreateGroup(ref group, auditUserId);
        }

        public void UpdateGroup(ref Group group, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.UpdateGroup(ref group, auditUserId);
        }

        public void DeleteGroup(int groupId, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.DeleteGroup(groupId, auditUserId);
        }

        public GroupResponse GetGroups(GroupRequest request)
        {
            var proxy = _groupService.CreateProxy();
            return proxy.GetGroups(request);
        }

        public IEnumerable<Group> GetAccessibleGroups(int userId)
        {
            var proxy = _groupService.CreateProxy();
            return proxy.GetAccessibleGroups(userId);
        }

        public IEnumerable<Group> GetGroupsFromHierarchy(IEnumerable<int> groupIds)
        {
            var proxy = _groupService.CreateProxy();
            return proxy.GetGroupsFromHierarchy(groupIds);
        }

        public void AddMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.AddMemberUsers(groupId, userIds, auditUserId);
        }

        public void RemoveMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.RemoveMemberUsers(groupId, userIds, auditUserId);
        }

        public void AddAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.AddAccessibleUsers(groupId, userIds, auditUserId);
        }

        public void RemoveAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.RemoveAccessibleUsers(groupId, userIds, auditUserId);
        }

        public void AddMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.AddMemberGroups(groupId, groupIds, auditUserId);
        }

        public void RemoveMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId)
        {
            var proxy = _groupService.CreateProxy();
            proxy.RemoveMemberGroups(groupId, groupIds, auditUserId);
        }
    }
}