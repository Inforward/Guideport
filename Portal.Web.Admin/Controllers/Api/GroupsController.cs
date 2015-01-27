using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Admin.Models;
using Portal.Web.Common.Filters.Api;

namespace Portal.Web.Admin.Controllers
{
    [RoutePrefix("api/groups")]
    public class GroupsController : BaseApiController
    {
        #region Private Members

        private readonly IGroupService _groupService;

        #endregion

        #region Constructor

        public GroupsController(IUserService userService, ILogger logger, IGroupService groupService)
            : base(userService, logger)
        {
            _groupService = groupService;
        }

        #endregion

        #region Actions

        [HttpGet]
        [Route("")]
        public IEnumerable<Group> GetAll([FromUri]GroupRequest request)
        {
            var response = _groupService.GetGroups(request);

            return response.Groups;
        }

        [HttpGet]
        [Route("{id:int}")]
        public Group GetGroup(int id)
        {
            var request = new GroupRequest() {GroupID = id};
            return _groupService.GetGroup(request);
        }

        [HttpPost]
        [Route("")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public Group CreateGroup([FromBody]Group group)
        {
            _groupService.CreateGroup(ref group, CurrentUser.UserID);

            return GetGroup(group.GroupID);
        }

        [HttpPut]
        [Route("{id:int}")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public Group UpdateGroup(int id, [FromBody]Group group)
        {
            _groupService.UpdateGroup(ref group, CurrentUser.UserID);

            return GetGroup(group.GroupID);
        }

        [HttpDelete]
        [Route("{groupId:int}")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void DeleteGroup(int groupId)
        {
            _groupService.DeleteGroup(groupId, CurrentUser.UserID);
        }

        [HttpGet]
        [Route("{groupId:int}/users")]
        public dynamic GetGroupUserMembers(int groupId, [FromUri]Pager pager)
        {
            var request = new UserRequest()
            {
                MemberGroupIDs = new List<int>{ groupId },
                Take = pager.Take,
                Skip = pager.Skip,
                IncludeTerminated = true
            };

            var response = _userService.GetUsers(request);

            return new
            {
                response.TotalRecordCount,
                Results = response.Users.Select(u => new
                {
                    u.UserID,
                    u.DisplayName,
                    u.ProfileTypeName,
                    u.AffiliateName,
                    u.Location,
                    u.UserStatusName
                })
            };
        }

        [HttpPut]
        [Route("{groupId:int}/users")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void RemoveUsersFromGroup(int groupId, [FromBody]IEnumerable<int> userIds)
        {
            _groupService.RemoveMemberUsers(groupId, userIds, CurrentUser.UserID);
        }

        [HttpPost]
        [Route("{groupId:int}/users")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void AddUsersToGroup(int groupId, [FromBody]IEnumerable<int> userIds)
        {
            _groupService.AddMemberUsers(groupId, userIds, CurrentUser.UserID);
        }

        [HttpGet]
        [Route("{groupId:int}/accessible-users")]
        public dynamic GetGroupAccessibleUsers(int groupId, [FromUri]Pager pager)
        {
            var request = new GroupRequest()
            {
                GroupID = groupId,
                IncludeAccessibleUsers = true
            };

            var response = _groupService.GetGroup(request);

            return response.AccessibleUsers.OrderByDescending(a => a.IsReadOnly).Select(a => new
            {
                a.IsReadOnly,
                a.UserID,
                a.User.DisplayName,
                a.User.ProfileTypeName,
                a.User.AffiliateName,
                a.User.Location
            });
        }

        [HttpPut]
        [Route("{groupId:int}/accessible-users")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void RemoveAccessibleUsersFromGroup(int groupId, [FromBody]IEnumerable<int> userIds)
        {
            _groupService.RemoveAccessibleUsers(groupId, userIds, CurrentUser.UserID);
        }

        [HttpPost]
        [Route("{groupId:int}/accessible-users")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void AddAccessibleUsersToGroup(int groupId, [FromBody]IEnumerable<int> userIds)
        {
            _groupService.AddAccessibleUsers(groupId, userIds, CurrentUser.UserID);
        }

        [HttpGet]
        [Route("{groupId:int}/groups")]
        public IEnumerable<Group> GetGroupGroupMembers(int groupId)
        {
            var request = new GroupRequest()
            {
                IncludeMemberCounts = true,
                MemberOfGroupID = groupId
            };

            var response = _groupService.GetGroups(request);

            return response.Groups;
        }

        [HttpDelete]
        [Route("{groupId:int}/groups/{memberGroupId:int}")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void RemoveGroupFromGroup(int groupId, int memberGroupId)
        {
            _groupService.RemoveMemberGroups(groupId, new[] { memberGroupId }, CurrentUser.UserID);
        }

        [HttpPost]
        [Route("{groupId:int}/groups")]
        [PortalAuthorize(PortalRoleValues.GroupAdmin)]
        public void AddGroupToGroup(int groupId, [FromBody]IEnumerable<int> memberGroupIds)
        {
            _groupService.AddMemberGroups(groupId, memberGroupIds, CurrentUser.UserID);
        }

        #endregion
    }
}