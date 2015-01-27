using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Portal.Infrastructure.Helpers;

namespace Portal.Web.Common.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString ToHtmlJson(this object obj)
        {
            return new HtmlString(obj.ToJson(Formatting.None));
        }
    }
}
