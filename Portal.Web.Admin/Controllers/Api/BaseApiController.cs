using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Admin.Filters;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Admin.Controllers
{
    [HandleExceptions]
    public abstract class BaseApiController : ApiController
    {
        #region Private Members

        protected readonly IUserService _userService;
        protected readonly ILogger _logger;
        private User _currentUser;

        #endregion

        #region Properties

        public User CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                if (_currentUser == null)
                {
                    var cookieData = Request.GetUserData<CookieUserData>();

                    if (cookieData == null)
                        throw new Exception("Could not retrieve user data from cookie.  Cannot continue.");

                    _currentUser = cookieData.ToUser();
                }

                return _currentUser;
            }
        }

        #endregion

        #region Constructor

        protected BaseApiController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        #endregion

        #region Overrides

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("You are not logged in.");
            }
        }

        #endregion

        #region Protected Methods

        protected void AddAuditData(Auditable entity)
        {
            entity.CreateUserID = CurrentUser.UserID;
            entity.CreateDate = DateTime.Now;
            entity.CreateDateUtc = DateTime.UtcNow;

            UpdateAuditData(entity);
        }

        protected void UpdateAuditData(Auditable entity)
        {
            entity.ModifyUserID = CurrentUser.UserID;
            entity.ModifyDate = DateTime.Now;
            entity.ModifyDateUtc = DateTime.UtcNow;

            if (entity.CreateUserID <= 0)
            {
                AddAuditData(entity);
            }
        }

        #endregion
    }
}