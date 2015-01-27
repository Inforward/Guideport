using System.Web.Mvc;
using Portal.Model.Planning;

namespace Portal.Web.Areas.Pentameter
{
    public class PentameterAreaRegistration : BaseAreaRegistration
    {
        public override string AreaSlug
        {
            get { return "pentameter"; }
        }

        public override string AreaName
        {
            get
            {
                return "Pentameter";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + ".Home",
                AreaSlug,
                new { controller = "Home", action = "Index", area = AreaName }
            );

            RegisterSuccessionRoutes(context);
            RegisterBusinessPlanRoutes(context);

            context.MapRoute(
                "Resources.ThirdParty",
                AreaSlug + "/resources/third-party-resources",
                new { controller = "Resources", action = "ThirdParty", area = AreaName }
            );

            context.MapRoute(
                "Resources.KnowledgeLibrary",
                AreaSlug + "/resources/knowledge-library",
                new { controller = "Resources", action = "KnowledgeLibrary", area = AreaName }
            );

            base.RegisterArea(context);
        }

        private void RegisterBusinessPlanRoutes(AreaRegistrationContext context)
        {
            var path = string.Concat(AreaSlug, "/business-management/strategic-planning/business-planning");

            context.MapRoute(
                "BusinessPlanning.MyBusinessPlans",
                path + "/my-business-plans/{year}",
                new { controller = "BusinessPlanning", action = "MyBusinessPlans", year = UrlParameter.Optional, area = AreaName }
            );

            context.MapRoute(
                "BusinessPlanning.Wizard",
                string.Concat(path, "/business-planning-wizard/{action}/{year}/{copyFromYear}"),
                new { controller = "BusinessPlanning", action = "Wizard", year = UrlParameter.Optional, copyFromYear = UrlParameter.Optional }
            );

            context.MapRoute(
                "BusinessPlanning",
                path + "/{action}/{year}/{copyFromYear}",
                new { controller = "BusinessPlanning", action = "Wizard", year = UrlParameter.Optional, copyFromYear = UrlParameter.Optional }
            );
        }

        private void RegisterSuccessionRoutes(AreaRegistrationContext context)
        {
            var path = string.Concat(AreaSlug, "/succession-planning");

            context.MapRoute(
                "Succession.Dashboard",
                path + "/dashboard",
                new { controller = "Succession", action = "Dashboard", area = AreaName }
            );

            context.MapRoute(
                "Succession.Planning",
                path + "/planning-tool",
                new { controller = "Planning", action = "Index", area = AreaName }
            );

            context.MapRoute(
                "Succession.Planning.Actions",
                path + "/planning-tool/{wizardId}/{action}",
                new { controller = "Planning", action = "Index", area = AreaName }
            );

            context.MapRoute(
                "Succession.BusinessValuation",
                path + "/business-valuation",
                new { controller = "Succession", action = "BusinessValuation", area = AreaName }
            );
        }
    }
}
