using System.Collections.Generic;
using Portal.Model;

namespace Portal.Data
{
    public interface IUserRepository : IEntityRepository
    {
        void UpdateUser(User user);
    }
}
