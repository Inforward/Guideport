using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Model;

namespace Portal.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static string PageTitle(this HtmlHelper helper, string subtitle = null)
        {
            return helper.PageTitle(new [] {subtitle});
        }

        public static string PageTitle(this HtmlHelper helper, params string[] titleParts)
        {
            var title = GetPageTitleBase();

            if (titleParts != null && titleParts.Length > 0)
            {
                title = titleParts.Where(t => !string.IsNullOrEmpty(t)).Aggregate(title, (current, part) => current + string.Concat(" | ", part));
            }

            return title;
        }

        public static HtmlString BodyCssClass(this HtmlHelper helper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            return
                new HtmlString(
                    string.Format("{0} {1} {2}", routeValues["controller"], routeValues["action"], routeValues["area"])
                        .ToLower());
        }

        public static HtmlString Breadcrumb(this HtmlHelper helper, Model.SiteMap siteMap)
        {
            var home = Settings.Get<string>("Root.SiteName");
            var html = string.Empty;

            if (siteMap != null)
            {
                var currentPage = siteMap.Items.FindRecursive(s => s.IsActive);

                if (currentPage != null)
                {
                    while (currentPage != null)
                    {
                        if (currentPage.IsActive)
                        {
                            html = string.Format("<li class='active'>{0}</li>", currentPage.Title);
                        }
                        else
                        {
                            html = string.Format("<li><a href='{0}'>{1}</a></li>", currentPage.Url, currentPage.Title) + html;
                        }

                        var parentId = currentPage.SiteContentParentID;

                        currentPage = parentId.HasValue ? siteMap.Items.FindRecursive(s => s.SiteContentID == parentId) : null;
                    }

                    // Now, prepend the current site's root link
                    html = string.Format("<li><a href='{0}'>{1}</a></li>", siteMap.SiteRootUrl, siteMap.SiteName) + html;

                    // And finally, add the root most site (unless we've already hit it)
                    if (!siteMap.SiteName.Equals(home))
                    {
                        html = "<li><a href='/'>" + home + "</a></li>" + html;
                    }
                }
            }

            return new HtmlString(string.Format("<ol class='breadcrumb'>{0}</ol>", html));
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string url, string altText = null, object htmlAttributes = null)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            var tag = new TagBuilder("img");
            tag.Attributes.Add("src", urlHelper.StaticContent(url));

            if (!string.IsNullOrEmpty(altText))
                tag.Attributes.Add("alt", altText);

            if(htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString ContentLink(this HtmlHelper helper, SiteContent content, object htmlAttributes = null)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) as IDictionary<string, object>;

            var tag = new TagBuilder("a");
            tag.SetInnerText(content.Title);
            tag.MergeAttributes(attributes);
            tag.Attributes["href"] = content.Permalink;

            if (content.ContentType == ContentType.File)
            {
                tag.Attributes["target"] = "_blank";
            }

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString Menu(this HtmlHelper helper, Model.SiteMap siteMap, object htmlAttributes = null)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) as IDictionary<string, object>;

            var tag = new TagBuilder("ul");
            tag.MergeAttributes(attributes);

            foreach (var item in siteMap.Items)
            {
                tag.InnerHtml += RenderMenu(item);
            }

            return MvcHtmlString.Create(tag.ToString());
        }

        public static string AdminConsoleUrl(this UrlHelper helper, string relativeUrl)
        {
            var adminUrl = Settings.Get("app:AdminConsole.Url", string.Empty);

            if (adminUrl.EndsWith("/"))
                adminUrl = adminUrl.TrimEnd('/');

            if (!relativeUrl.StartsWith("/#"))
                relativeUrl = "/#" + relativeUrl;

            return string.Concat(adminUrl, relativeUrl);
        }

        public static string StaticContent(this UrlHelper helper, string path)
        {
            var assetsPath = Settings.Get("app:StaticAssets.RootUrl", string.Empty);
            var url = helper.Content(path);

            if (!string.IsNullOrEmpty(assetsPath))
            {
                if (!url.StartsWith("/"))
                    url = "/" + url;

                if (assetsPath.EndsWith("/"))
                    assetsPath = assetsPath.TrimEnd('/');

                url = string.Concat(assetsPath, url);
            }

            return url;
        }

        public static string PentameterSurveyUrl(this UrlHelper helper)
        {
            var surveyName = Settings.Get<string>("Pentameter.SurveyName");

            return helper.RouteUrl("Pentameter.Survey", new {surveyName});
        }

        public static string RetirementSurveyUrl(this UrlHelper helper)
        {
            var surveyName = Settings.Get<string>("Retirement.SurveyName");

            return helper.RouteUrl("Retirement.Survey", new {surveyName});
        }

        public static string AdminConsoleUrl(this UrlHelper helper)
        {
            return Settings.Get<string>("app:AdminConsole.Url");
        }

        public static HelperResult SeriesSplitter<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, int itemsBeforeSplit, Func<T, HelperResult> template, IHtmlString seriesSplitter)
        {
            if (items == null)
            {
                return null;
            }

            return new HelperResult(writer =>
            {
                var i = 0;

                foreach (var item in items)
                {
                    if (i != 0 && i%itemsBeforeSplit == 0)
                    {
                        writer.Write(seriesSplitter.ToString());
                    }

                    template(item).WriteTo(writer);

                    i++;
                }
            });
        }

        public static ScriptTag BeginHtmlTemplate(this HtmlHelper helper, string id, string type = "text/template")
        {
            return new ScriptTag(helper, type, id);
        }

        private static string RenderMenu(SiteMapItem item)
        {
            if (!item.IsMenuVisible)
                return string.Empty;

            var html = string.Empty;
            var hasChildren = item.Children.Any(c => c.IsMenuVisible);
            var cssClasses = new List<string>();

            if(hasChildren)
                cssClasses.Add("toggle");

            if (item.IsActive || item.Children.AnyRecursive(m => m.IsActive))
                cssClasses.Add("expanded");

            if (item.IsActive)
                cssClasses.Add("active");

            html += string.Format("<li class=\"{0}\">", string.Join(" ", cssClasses));

            html += "<div class='menu-item-container'>";

            if (!string.IsNullOrEmpty(item.IconCssClass))
                html += string.Format("<i class=\"menu-icon {0}\"></i>", item.IconCssClass);

            html += string.Format("<a class='menu-link' href=\"{0}\">{1}</a>", item.Url, item.Title);

            html += "<i class='menu-toggle-icon'></i>";

            html += "</div>";

            if (hasChildren)
            {
                html += "<ul>";

                html = item.Children.Aggregate(html, (current, childMenu) => current + RenderMenu(childMenu));

                html += "</ul>";
            }

            html += "</li>";

            return html;
        }

        private static string GetPageTitleBase()
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var area = routeValues["area"];

            var title = Settings.Get<string>("Root.SiteName");

            if (area != null && !string.IsNullOrWhiteSpace(area.ToString()))
            {
                title = Settings.Get<string>(string.Format("Area.{0}.SiteName", area));
            }

            return title;
        }
    }

    public class ScriptTag : IDisposable
    {
        private readonly TextWriter writer;
        private readonly TagBuilder builder;

        public ScriptTag(HtmlHelper helper, string type, string id)
        {
            writer = helper.ViewContext.Writer;
            builder = new TagBuilder("script");
            builder.MergeAttribute("type", type);
            builder.MergeAttribute("id", id);
            writer.WriteLine(builder.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            writer.WriteLine(builder.ToString(TagRenderMode.EndTag));
        }
    }
}