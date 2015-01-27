using System.Collections.Generic;
using System.ServiceModel;
using Portal.Model;

namespace Portal.Services.Contracts
{
    public interface IUserServiceChannel : IUserService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IUserService
    {
        [OperationContract]
        User GetUserByUserId(int userId);

        [OperationContract]
        User GetUser(UserRequest criteria);

        [OperationContract]
        UserResponse GetUsers(UserRequest criteria);

        [OperationContract]
        IEnumerable<ProfileType> GetProfileTypes();

        [OperationContract]
        IEnumerable<UserStatus> GetUserStatuses();

        [OperationContract]
        IEnumerable<ApplicationRole> GetApplicationRoles();

        [OperationContract]
        void UpdateUserRoles(int userId, IEnumerable<ApplicationRole> roles, int auditUserId);

        [OperationContract]
        void SaveUserObjectCache(ObjectCache objectCache);
    }
}
