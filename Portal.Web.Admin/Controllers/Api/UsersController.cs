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
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        public UsersController(IUserService userService, ILogger logger)
            : base(userService, logger)
        {
            
        }

        [HttpGet]
        [Route("")]
        public dynamic Get([FromUri]UserRequest request)
        {
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
                    u.UserStatusName,
                    PrimaryPhone = u.PrimaryPhone.FormatAsPhoneNo(),
                    u.Email,
                    u.ModifyDate,
                    u.BusinessConsultantDisplayName
                })
            };
        }

        [HttpGet]
        [Route("{userId:int}")]
        public User GetUser(int userId)
        {
            var request = new UserRequest()
            {
                UserID = userId,
                IncludeApplicationRoles = true,
                IncludeBranchInfo = true,
                IncludeLicenseInfo = true,
                IncludeGroups = true,
                IncludeAccessibleGroups = true,
                IncludeAffiliates = true,
                IncludeAffiliateLogos = true
            };

            var user = _userService.GetUser(request);

            if(user == null)
                throw new Exception("Invalid User ID");

            return user;
        }

        [HttpGet]
        [Route("{userId:int}/roles")]
        [PortalAuthorize(PortalRoleValues.UserAdmin)]
        public IEnumerable<ApplicationRole> GetUserRoles(int userId)
        {
            var user = _userService.GetUser(new UserRequest() { UserID = userId, IncludeApplicationRoles = true });

            if (user == null)
                throw new Exception("Invalid User ID");

            var roles = _userService.GetApplicationRoles().OrderBy(r => r.DisplayName).ToList();

            foreach (var role in roles)
            {
                var userRole = user.ApplicationRoles.FirstOrDefault(r => r.ApplicationRoleID == role.ApplicationRoleID);

                if (userRole != null)
                {
                    role.ApplicationAccessID = userRole.ApplicationAccessID;
                    role.Enabled = true;
                }
                else
                {
                    role.ApplicationAccessID = (int)ApplicationAccessOptions.Unrestricted;
                    role.Enabled = false;
                }
            }

            return roles;
        }

        [HttpPut]
        [Route("{userId:int}/roles")]
        [PortalAuthorize(PortalRoleValues.UserAdmin)]
        public void UpdateUserRoles(int userId, [FromBody]IEnumerable<ApplicationRole> roles)
        {
            _userService.UpdateUserRoles(userId, roles, CurrentUser.UserID);
        }

        [HttpGet]
        [Route("profile-types")]
        public IEnumerable<ProfileType> GetProfileTypes()
        {
            return _userService.GetProfileTypes().OrderBy(p => p.Name);
        }

        [HttpGet]
        [Route("statuses")]
        public IEnumerable<UserStatus> GetUserStatuses()
        {
            return _userService.GetUserStatuses().OrderBy(p => p.Name);
        }
    }
}