using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    [RoutePrefix("error")]
    [Route("{action=index}")]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Sorry, an error occurred while processing your request.";

            return View();
        }

        public ActionResult NotFound()
        {
            ViewBag.Message = "The content you are looking for does not exist.";

            return View("Index");
        }

        public ActionResult Unauthorized()
        {
            ViewBag.Message = "This is intended for financial advisors only.";

            return View("Index");
        }

    }
}
