using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Infrastructure.Helpers;
using Portal.Model;

namespace Portal.Web.Admin.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString AdminCssClass(this HtmlHelper helper, User currentUser)
        {
            var cssClass = currentUser.ApplicationRoles.Aggregate(string.Empty, (current, role) => current + " " + role.RoleName.Slugify());

            return new HtmlString(cssClass);
        }
    }
}