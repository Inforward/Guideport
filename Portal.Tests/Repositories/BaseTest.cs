using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Data.Sql;

namespace Portal.Tests.Repositories
{
    public abstract class BaseTest
    {
        protected BaseTest()
        {
            var _ = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
