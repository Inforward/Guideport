using System.Web;
using System.Web.Optimization;

namespace Portal.Web.Admin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);
        }

        private static void RegisterScriptBundles(BundleCollection bundles)
        {
            // App js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.App).Include(
                "~/App/app.js",
                "~/App/Services/dataService.js",
                "~/App/Services/affiliateService.js",
                "~/App/Services/cacheService.js",
                "~/App/Services/contentService.js",
                "~/App/Services/groupService.js",
                "~/App/Services/userService.js",
                "~/App/Services/reportService.js",
                "~/App/Services/surveyService.js",
                "~/App/Services/geoService.js",
                "~/App/Services/configurationService.js",
                "~/App/Filters/*.js",
                "~/App/Controllers/*.js",
                "~/App/Directives/*.js"));

            // Vendor js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.Vendor).Include(
                "~/Assets/vendor/jquery-1.9.1.min.js",
                "~/Assets/vendor/underscore-1.4.4.js",
                "~/Assets/vendor/zeroclipboard.js",
                "~/Assets/vendor/jszip.min.js",
                "~/Assets/vendor/bootstrap/tooltip.js",
                "~/Assets/vendor/bootstrap/transition.js",
                "~/Assets/vendor/bootstrap-switch.js"));

            // Angular bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.Angular).Include(
                "~/Assets/Vendor/angular/angular.js",
                "~/Assets/Vendor/angular/angular-touch.js",
                "~/Assets/Vendor/angular/angular-aria.js",
                "~/Assets/Vendor/angular/angular-messages.js",
                "~/Assets/Vendor/angular/angular-scenario.js",
                "~/Assets/Vendor/angular/angular-sanitize.js",
                "~/Assets/Vendor/angular/angular-route.js",
                "~/Assets/Vendor/angular/angular-resource.js",
                "~/Assets/Vendor/angular/angular-mocks.js",
                "~/Assets/Vendor/angular/angular-loader.js",
                "~/Assets/Vendor/angular/angular-cookies.js",
                "~/Assets/Vendor/angular/angular-animate.js",
                "~/Assets/Vendor/angular/angular-bootstrap.js",
                "~/Assets/Vendor/angular/directives/*.js"));

            // Kendo Web js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.KendoWeb).Include(
                "~/Assets/Kendo/kendo.web.js",
                "~/Assets/Kendo/kendo.angular.js"));
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            // App css
            var coreBundle = new StyleBundle(StyleBundleNames.App);
            coreBundle.IncludeWithUrlTransform("~/Assets/css/bootstrap.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/bootstrap-switch.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/font-awesome.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/flaticon.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/admin.css");
            bundles.Add(coreBundle);

            // Kendo css
            var kendoBundle = new StyleBundle(StyleBundleNames.Kendo);
            kendoBundle.IncludeWithUrlTransform("~/Assets/Kendo/kendo.common.css");
            kendoBundle.IncludeWithUrlTransform("~/Assets/Kendo/kendo.default.css");
            bundles.Add(kendoBundle);

        }
    }

    public static class BundleExtensions
    {
        public static Bundle IncludeWithUrlTransform(this StyleBundle styleBundle, string path)
        {
            return styleBundle.Include(path, new CssRewriteUrlTransform());
        }
    }

    public static class ScriptBundleNames
    {
        public const string App = "~/bundles/scripts/app";
        public const string Angular = "~/bundles/scripts/angular";
        public const string KendoWeb = "~/bundles/scripts/kendo/web";
        public const string Vendor = "~/bundles/scripts/vendor";
    }

    public static class StyleBundleNames
    {
        public const string App = "~/bundles/css/core";
        public const string Kendo = "~/bundles/kendo";
    }
}
