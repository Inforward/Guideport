using System;
using System.Web.Mvc;
using System.Web.Security;
using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Configuration;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Admin.Controllers.Mvc
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(string profileId)
        {
            var user = _userService.GetUser(new UserRequest() { ProfileID = profileId, IncludeApplicationRoles = true });

            if(user == null)
                throw new Exception("Invalid Profile ID");

            Response.SetAuthCookie(user.ProfileID, false, user.ToCookieUserData());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("signout", Name = "Signout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.RemoveCookie("ASP.NET_SessionId");
            Response.RemoveCookie(CookieHelper.UserDataCookieName);
            Response.RemoveCookie(CookieHelper.AssistedUserCookieName);

            return new RedirectResult(FormsAuthentication.LoginUrl);
        }
    }
}