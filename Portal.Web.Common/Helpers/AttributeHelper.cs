using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Common.Helpers
{
    public static class AttributeHelper
    {
        public static bool AllowAnonymous(this AuthorizationContext context)
        {
            return context.ActionDescriptor.GetCustomAttributes(typeof (AllowAnonymousAttribute), false).Any();
        }
    }
}
