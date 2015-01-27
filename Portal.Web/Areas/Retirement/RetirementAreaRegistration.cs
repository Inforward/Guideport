using System.Web.Mvc;

namespace Portal.Web.Areas.Retirement
{
    public class RetirementAreaRegistration : BaseAreaRegistration
    {
        public override string AreaSlug
        {
            get { return "guided-retirement-solutions"; }
        }

        public override string AreaName
        {
            get
            {
                return "Retirement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + ".Home",
                AreaSlug,
                new { controller = "Home", action = "Index", area = AreaName }
            );

            base.RegisterArea(context);
        }
    }
}
