using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Portal.Web.ActionResults
{
    /// <summary>
    /// Represents a class that is used to send JSON data (via JSON.Net) to the client
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                // Ignore circular references (this is key for many-to-many Entity Framework entities)
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Ensure Timezone is specified on dates so they can be correctly parsed in javascript
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        public JsonNetResult(object data) : this()
        {
            Data = data;
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            var response = context.HttpContext.Response;

            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, Data);
                response.Write(sw.ToString());
            }
        }
    }
}