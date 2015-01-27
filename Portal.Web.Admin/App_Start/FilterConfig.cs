using System.Web;
using System.Web.Mvc;

namespace Portal.Web.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
