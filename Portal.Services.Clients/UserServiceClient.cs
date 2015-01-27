using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using System.Collections.Generic;

namespace Portal.Services.Clients
{
    public class UserServiceClient : IUserService
    {
        private readonly ServiceClient<IUserServiceChannel> _userService = new ServiceClient<IUserServiceChannel>();

        public User GetUserByUserId(int userId)
        {
            var proxy = _userService.CreateProxy();
            return proxy.GetUserByUserId(userId);
        }

        public User GetUser(UserRequest criteria)
        {
            var proxy = _userService.CreateProxy();
            return proxy.GetUser(criteria);
        }

        public UserResponse GetUsers(UserRequest criteria)
        {
            var proxy = _userService.CreateProxy();
            return proxy.GetUsers(criteria);
        }

        public IEnumerable<ProfileType> GetProfileTypes()
        {
            var proxy = _userService.CreateProxy();
            return proxy.GetProfileTypes();
        }

        public IEnumerable<UserStatus> GetUserStatuses()
        {
            var proxy = _userService.CreateProxy();
            return proxy.GetUserStatuses();
        }

        public IEnumerable<ApplicationRole> GetApplicationRoles()
        {
            var proxy = _userService.CreateProxy();
            return proxy.GetApplicationRoles();
        }

        public void UpdateUserRoles(int userId, IEnumerable<ApplicationRole> roles, int auditUserId)
        {
            var proxy = _userService.CreateProxy();
            proxy.UpdateUserRoles(userId, roles, auditUserId);
        }

        public void SaveUserObjectCache(ObjectCache objectCache)
        {
            var proxy = _userService.CreateProxy();
            proxy.SaveUserObjectCache(objectCache);
        }
    }
}
