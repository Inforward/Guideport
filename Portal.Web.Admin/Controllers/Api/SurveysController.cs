using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Common.Filters.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Portal.Web.Admin.Controllers
{
    [RoutePrefix("api/surveys")]
    [PortalAuthorize(PortalRoleValues.SurveyAdmin)]
    public class SurveysController : BaseApiController
    {
        private readonly ISurveyService _surveyService;
        private readonly ICmsService _cmsService;

        public SurveysController(IUserService userService, ISurveyService surveyService, ICmsService cmsService, ILogger logger)
            : base(userService, logger)
        {
            _surveyService = surveyService;
            _cmsService = cmsService;
        }

        [HttpGet]
        [Route("")]
        public dynamic Get([FromUri]SurveyRequest request)
        {
            var surveys = _surveyService.GetSurveys();

            return new
            {
                Surveys = surveys
            };
        }

        [HttpGet]
        [Route("{surveyId:int}")]
        public dynamic GetSurvey(int surveyId)
        {
            var survey = _surveyService.GetSurvey(new SurveyRequest { SurveyID = surveyId });

            if (survey == null)
                throw new ArgumentException("Invalid Survey ID");

            var contents = new List<SiteContent>();

            if (survey.SuggestedContentSiteID.HasValue)
            {
                var request = new SiteRequest() {SiteID = survey.SuggestedContentSiteID.Value, IncludeAll = true};
                var response = _cmsService.GetSite(request);

                contents = response.SiteContents
                                .Where(s => s.SiteContentStatusID == (int)ContentStatus.Published)
                                .Where(s => Settings.SearchableDocumentTypes.Contains((ContentDocumentType)s.SiteDocumentTypeID))
                                .OrderBy(s => s.SiteContentTypeID).ThenBy(s => s.TitlePath)
                                .ToList();
            }

            return new
            {
                Survey = survey,
                SuggestedContents = contents.Select(c => new {c.SiteContentID, c.TitlePath})
            };
        }

        [HttpPut]
        [Route("{surveyId:int}")]
        public void UpdateSurvey(int surveyId, [FromBody]Survey survey)
        {
            _surveyService.UpdateSurvey(survey, CurrentUser.UserID);
        }
    }
}