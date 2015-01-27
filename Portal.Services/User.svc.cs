using Portal.Data;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;

namespace Portal.Services
{
    public class User : UserService
    {
        public User(IUserRepository userRepository, ICacheStorage cacheStorage)
            : base(userRepository, cacheStorage)
        { }
    }
}