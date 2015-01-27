using System.Web.Mvc;
using Portal.Infrastructure.Logging;
using Portal.Services.Contracts;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.Retirement.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUserService userService, ILogger logger)
            : base(userService, logger)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}