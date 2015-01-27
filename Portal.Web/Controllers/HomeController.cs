using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.ActionResults;
using Portal.Web.Models;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Private Members

        private readonly IAffiliateService _affiliateService;

        #endregion

        #region Constructor

        public HomeController(IUserService userService, ILogger logger, IAffiliateService affiliateService)
            : base(userService, logger)
        {
            _affiliateService = affiliateService;
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Throw()
        {
            throw new Exception("Hello");
        }

        [HttpPost]
        public JsonResult KeepAlive()
        {
            return new JsonNetResult(new JsonResponse() { Success = true });
        }

        [HttpPost]
        [Route("terms/{termsKey}/accept")]
        public JsonResult JsonSaveTermsAcceptance(string termsKey)
        {
            return JsonResponse(() =>
            {
                var item = new TermsAcceptance() {Accepted = true};

                var cacheItem = new ObjectCache()
                {
                    Key = termsKey,
                    ValueSerialized = ObjectCache.SerializeItem(item),
                    UserID = CurrentUser.UserID
                };

                SaveUserObjectCache(cacheItem);
            });
        }

        [HttpPost]
        [Route("terms/{termsKey}/validate")]
        public JsonResult JsonValidateTermsAcceptance(string termsKey)
        {
            return JsonResponse(() =>
            {
                if (IsAssisting)
                {
                    return new TermsAcceptance() { Accepted = true };
                }

                return GetCurrentUser().GetCachedObject(termsKey, new TermsAcceptance() { Accepted = false });

            });
        }

        #endregion
    }
}
