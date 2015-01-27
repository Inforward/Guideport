using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Portal.Model;
using Portal.Infrastructure.Helpers;
using RefactorThis.GraphDiff;

namespace Portal.Data.Sql.EntityFramework
{
    public class UserRepository : EntityRepository<MasterContext>, IUserRepository
    {
        public void UpdateUser(User user)
        {
            Context.UpdateGraph(user, map => map.AssociatedCollection(u => u.Groups));
            Save();
        }
    }
}
