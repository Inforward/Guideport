using System.Web.Optimization;
using Portal.Infrastructure.Configuration;

namespace Portal.Web
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
            // Core js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.Core).Include("~/Assets/Scripts/*.js"));

            // Business Planning Wizard Bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.BusinessPlanningWizard).Include(
                "~/Assets/Scripts/Pentameter/business-planning-wizard.js"));

            // Planning Wizard Bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.PlanningWizard).Include(
                "~/Assets/Scripts/Pentameter/planning-wizard.js"));

            // Pentameter js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.Pentameter).Include(
                "~/Assets/Scripts/Pentameter/business-plan.js",
                "~/Assets/Scripts/Pentameter/dashboard.js",
                "~/Assets/Scripts/Pentameter/knowledge-library.js",
                "~/Assets/Scripts/Pentameter/third-party-resources.js"));
            
            // Vendor js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.Vendor).Include(
                "~/Assets/vendor/jquery-1.9.1.min.js",
                "~/Assets/vendor/jquery-ui-1.10.4.custom.js",
                "~/Assets/vendor/underscore-1.4.4.js",
                "~/Assets/vendor/bootstrap/modal.js",
                "~/Assets/vendor/bootstrap/dropdown.js",
                "~/Assets/vendor/bootstrap/tooltip.js",
                "~/Assets/vendor/bootstrap/transition.js",
                "~/Assets/vendor/mmenu/jquery.mmenu.js",
                "~/Assets/vendor/jquery.numeric.js",
                "~/Assets/vendor/jquery.keyfilter.js",
                "~/Assets/vendor/jquery.flextext.js",
                "~/Assets/vendor/jquery.scrollintoview.js",
                "~/Assets/vendor/jquery.mousewheel.js",
                "~/Assets/vendor/jquery.nanoscroller.js",
                "~/Assets/vendor/placeholder.js"));

            // Shims js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.Shims).Include(
                "~/Assets/vendor/html5shiv.js",
                "~/Assets/vendor/respond.js"));

            // Kendo Web js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.KendoWeb).Include(
                "~/Assets/Kendo/kendo.web.js",
                "~/Assets/Kendo/kendo.data-binders.js",
                "~/Assets/Kendo/kendo.grid.ex.js",
                "~/Assets/Kendo/kendo.orgchart.js",
                "~/Assets/Kendo/kendo.listview.ex.js",
                "~/Assets/Kendo/kendo.datepicker.ex.js"));

            // Kendo DataViz js bundle
            bundles.Add(new ScriptBundle(ScriptBundleNames.KendoDataViz).Include(
                "~/Assets/Kendo/kendo.dataviz.js"));

            // Modernizr
            bundles.Add(new ScriptBundle(ScriptBundleNames.Modernizr).Include(
                "~/Assets/Vendor/modernizr.custom.js"));

            // Go JS
            var goJsScript = Settings.Get<string>("app:GoJsScriptPath");
            bundles.Add(new ScriptBundle(ScriptBundleNames.GoJs).Include(goJsScript));

            // Base64
            bundles.Add(new ScriptBundle(ScriptBundleNames.Base64).Include(
                "~/Assets/Vendor/base64.min.js"));

        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            // Core css
            var coreBundle = new StyleBundle(StyleBundleNames.Core);
            coreBundle.IncludeWithUrlTransform("~/Assets/css/normalize.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/bootstrap.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/font-awesome.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/icomoon.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/flaticon.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/jquery.mmenu.all.css");
            //coreBundle.IncludeWithUrlTransform("~/Assets/css/source-sans-pro.css");
            coreBundle.IncludeWithUrlTransform("~/Assets/css/flextext.css");
            bundles.Add(coreBundle);

            // Portal css
            var portalBundle = new StyleBundle(StyleBundleNames.Portal);
            portalBundle.IncludeWithUrlTransform("~/Assets/css/nanoscroller.css");
            portalBundle.IncludeWithUrlTransform("~/Assets/css/portal.css");
            bundles.Add(portalBundle);

            // Admin css
            var adminBundle = new StyleBundle(StyleBundleNames.Admin);
            adminBundle.IncludeWithUrlTransform("~/Assets/css/admin.css");
            bundles.Add(adminBundle);

            // Kendo css
            var kendoBundle = new StyleBundle(StyleBundleNames.Kendo);
            kendoBundle.IncludeWithUrlTransform("~/Assets/Kendo/kendo.common.css");
            kendoBundle.IncludeWithUrlTransform("~/Assets/Kendo/kendo.default.css");
            kendoBundle.IncludeWithUrlTransform("~/Assets/Css/kendo-overrides.css");
            //kendoBundle.IncludeWithUrlTransform("~/Assets/Kendo/kendo.dataviz.css");
            //kendoBundle.IncludeWithUrlTransform("~/Assets/Kendo/kendo.dataviz.default.css");
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
        public const string Core = "~/bundles/scripts";
        public const string Pentameter = "~/bundles/scripts/pentameter";
        public const string BusinessPlanningWizard = "~/bundles/scripts/wizard";
        public const string PlanningWizard = "~/bundles/scripts/planning-wizard";
        public const string Vendor = "~/bundles/vendor";
        public const string Shims = "~/bundles/shims";
        public const string KendoWeb = "~/bundles/scripts/kendo/web";
        public const string KendoDataViz = "~/bundles/scripts/kendo/dataviz";
        public const string Modernizr = "~/bundles/vendor/modernizr";
        public const string GoJs = "~/bundles/vendor/gojs";
        public const string Base64 = "~/bundles/vendor/base64";
    }

    public static class StyleBundleNames
    {
        public const string Core = "~/bundles/css/core";
        public const string Admin = "~/bundles/css/admin";
        public const string Portal = "~/bundles/css/portal";
        public const string Kendo = "~/bundles/kendo";
    }
}
