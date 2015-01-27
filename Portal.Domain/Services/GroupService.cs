using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Portal.Data;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Services
{
    public class GroupService : BaseService, IGroupService
    {
        #region Private Members

        private readonly IGroupRepository _groupRepository;

        #endregion

        #region Constructor

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        #endregion

        #region Public Methods

        public Group GetGroup(GroupRequest request)
        {
            if(!request.GroupID.HasValue)
                throw new Exception("Group ID not specified");

            return GetGroups(request).Groups.FirstOrDefault();
        }

        public GroupResponse GetGroups(GroupRequest request)
        {
            var response = new GroupResponse();

            var query = _groupRepository.GetAll<Group>().AsNoTracking();

            if (request.GroupID.HasValue)
                query = query.Where(g => g.GroupID == request.GroupID);

            if (request.MemberOfGroupID > 0)
                query = query.Where(g => g.ParentGroups.Any(gm => gm.GroupID == request.MemberOfGroupID));

            if (request.ExcludeGroupID > 0)
            {
                query = query.Where(g => g.GroupID != request.ExcludeGroupID);
                query = query.Where(g => g.ParentGroups.All(mg => mg.GroupID != request.ExcludeGroupID));    
            }

            if (request.IncludeMemberUsers)
                query = query.Include(g => g.MemberUsers);

            if (request.IncludeMemberGroups)
                query = query.Include(g => g.MemberGroups);

            if (request.IncludeAccessibleUsers)
                query = query
                            .Include("AccessibleUsers")
                            .Include("AccessibleUsers.User");

            // TODO: Apply Dynamic Sorting
            query = query.OrderBy(g => g.Name);

            if (request.Skip > 0 || request.Take > 0)
            {
                response.TotalRecordCount = query.Count();

                if (request.Skip > 0)
                    query = query.Skip(request.Skip);

                if (request.Take > 0)
                    query = query.Take(request.Take);

                response.Groups = request.IncludeMemberCounts ? GetGroupMemberCounts(query).ToList() : query.ToList();
            }
            else
            {
                response.Groups = request.IncludeMemberCounts ? GetGroupMemberCounts(query).ToList() : query.ToList();
                response.TotalRecordCount = response.Groups.Count();
            }

            if(!request.IncludeAccessibleUsers)
                response.Groups.ForEach(g => g.AccessibleUsers.Clear());

            if (!request.IncludeMemberGroups)
                response.Groups.ForEach(g => g.MemberGroups.Clear());

            if (!request.IncludeMemberUsers)
                response.Groups.ForEach(g => g.MemberUsers.Clear());

            return response;
        }

        public IEnumerable<Group> GetAccessibleGroups(int userId)
        {
            return _groupRepository.GetAll<Group>()
                                   .Where(g => g.AccessibleUsers.Any(u => u.UserID == userId))
                                   .OrderBy(g => g.Name)
                                   .ToList();
        }

        public IEnumerable<Group> GetGroupsFromHierarchy(IEnumerable<int> groupIds)
        {
            return _groupRepository.GetGroupsFromHierarchy(groupIds);
        }

        public void CreateGroup(ref Group group, int auditUserId)
        {
            var incomingGroup = group;

            var existingGroup = _groupRepository.FindBy<Group>(g =>
                                    g.Name.Equals(incomingGroup.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (existingGroup != null)
                throw new Exception("A group with this name already exists");

            AddAuditData(group, auditUserId);

            _groupRepository.Add(group);
            _groupRepository.Save();
        }

        public void UpdateGroup(ref Group group, int auditUserId)
        {
            var incomingGroup = group;

            var existingGroup = _groupRepository.FindBy<Group>(g => g.GroupID == incomingGroup.GroupID).FirstOrDefault();

            if (existingGroup == null)
                throw new Exception("Invalid Group ID");

            var exists = _groupRepository.FindBy<Group>(g => g.Name.Equals(incomingGroup.Name, StringComparison.InvariantCultureIgnoreCase) && g.GroupID != incomingGroup.GroupID).Any();

            if (exists)
                throw new Exception("A group with this name exists");

            UpdateAuditData(group, auditUserId);

            group = _groupRepository.SaveGraph(incomingGroup);
        }

        public void DeleteGroup(int groupId, int auditUserId)
        {
            var group = _groupRepository.FindBy<Group>(g => g.GroupID == groupId).FirstOrDefault();

            if (group == null)
                throw new Exception("Invalid Group ID");

            if(group.IsReadOnly)
                throw new Exception("This group is marked as read-only, cannot delete.");

            _groupRepository.DeleteGroup(groupId, auditUserId);
            _groupRepository.Save();
        }

        public void AddMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            _groupRepository.AddMemberUsers(groupId, userIds, auditUserId);
        }

        public void RemoveMemberUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            _groupRepository.RemoveMemberUsers(groupId, userIds, auditUserId);
        }

        public void AddAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            _groupRepository.AddAccessibleUsers(groupId, userIds, auditUserId);
        }

        public void RemoveAccessibleUsers(int groupId, IEnumerable<int> userIds, int auditUserId)
        {
            _groupRepository.RemoveAccessibleUsers(groupId, userIds, auditUserId);
        }

        public void AddMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId)
        {
            _groupRepository.AddMemberGroups(groupId, groupIds, auditUserId);
        }

        public void RemoveMemberGroups(int groupId, IEnumerable<int> groupIds, int auditUserId)
        {
            _groupRepository.RemoveMemberGroups(groupId, groupIds, auditUserId);
        }

        #endregion

        #region Private Methods

        private static IEnumerable<Group> GetGroupMemberCounts(IQueryable<Group> query)
        {
            var groups = query.Select(g => new GroupPresentation()
                                {
                                    Group = g,
                                    MemberUsers = g.MemberUsers,
                                    MemberGroups = g.MemberGroups,
                                    AccessibleUsers = g.AccessibleUsers,
                                    MemberUserCount = g.MemberUsers.Count(),
                                    MemberGroupCount = g.MemberGroups.Count(),
                                    AccessibleUserCount = g.AccessibleUsers.Count()
                                }).ToList();

            foreach (var group in groups)
            {
                group.Group.MemberUserCount = group.MemberUserCount;
                group.Group.MemberGroupCount = group.MemberGroupCount;
                group.Group.AccessibleUserCount = group.AccessibleUserCount;

                group.Group.MemberUsers = group.MemberUsers;
                group.Group.MemberGroups = group.MemberGroups;
                group.Group.AccessibleUsers = group.AccessibleUsers;
            }

            return groups.Select(g => g.Group);
        }

        #endregion
    }
}
