using System.Collections;
using System.Collections.Generic;
using Portal.Model;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface IGroupServiceChannel : IGroupService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IGroupService
    {
        [OperationContract]
        Group GetGroup(GroupRequest request);

        [OperationContract]
        void CreateGroup(ref Group group, int auditUserId);

        [OperationContract]
        void UpdateGroup(ref Group group, int auditUserId);

        [OperationContract]
        void DeleteGroup(int groupId, int auditUserId);

        [OperationContract]
        GroupResponse GetGroups(GroupRequest request);

        [OperationContract]
        IEnumerable<Group> GetAccessibleGroups(int userId);

        [OperationContract]
        IEnumerable<Group> GetGroupsFromHierarchy(IEnumerable<int> groupIds);

        [OperationContract]
        void AddMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId);

        [OperationContract]
        void RemoveMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId);

        [OperationContract]
        void AddAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId);

        [OperationContract]
        void RemoveAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId);

        [OperationContract]
        void AddMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId);

        [OperationContract]
        void RemoveMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId);
    }
}
