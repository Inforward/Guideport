using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Model.Planning;
using Portal.Services.Contracts;
using Portal.Web.Areas.Pentameter.Models.Succession;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Areas.Pentameter.Controllers
{
    public class SuccessionController : BaseController
    {
        #region Private Members

        private readonly Lazy<EnrollmentStatus> EnrollmentStatus;
        private readonly IPlanningService _planningService;
        private readonly ISurveyService _surveyService;
        private const string BusinessValuationTermsKey = "Pentameter.BusinessValuation.TermsAcceptance";
        private const string SuccessionDashboardTermsKey = "Pentameter.SuccessionDashboard.TermsAcceptance";

        #endregion

        #region Constructor

        public SuccessionController(IUserService userService, IPlanningService planningService, ISurveyService surveyService, ILogger logger)
            : base(userService, logger)
        {
            _planningService = planningService;
            _surveyService = surveyService;
            EnrollmentStatus = new Lazy<EnrollmentStatus>(() => _planningService.GetEnrollmentStatus(AssistedUser.UserID));
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            return Dashboard();
        }

        [HttpGet]
        [ProfileTypeAuthorize(ProfileTypeValues.FinancialAdvisor)]
        public ActionResult Dashboard()
        {
            var viewModel = new DashboardViewModel
            {
                EnrollmentStatus = EnrollmentStatus.Value,
                QualifiedBuyerStatus = GetQualifiedBuyerStatus(),
                ContinuityPlanningStatus = GetPlanningStatus(PlanningType.Continuity),
                SuccessionPlanningStatus = GetPlanningStatus(PlanningType.Succession),
                BusinessAcquisitionStatus = GetPlanningStatus(PlanningType.Acquisition)
            };

            ViewBag.TermsKey = SuccessionDashboardTermsKey;

            return View(viewModel);
        }

        [HttpGet]
        [ProfileTypeAuthorize(ProfileTypeValues.FinancialAdvisor)]
        public ActionResult BusinessValuation()
        {
            ViewBag.TermsKey = BusinessValuationTermsKey;

            return View();
        }

        #endregion

        #region Private Methods

        private Status GetPlanningStatus(PlanningType planningType)
        {
            var summary = _planningService.GetPlanningSummary((int) planningType, AssistedUser.UserID);
            var enrollmentInterest = EnrollmentStatus.Value.EnrollmentInterests.First(i => i.Name == summary.PlanningName);

            var status = new Status()
            {
                Title = summary.PlanningName,
                Message = GetPlanningMessage(summary),
                Unlocked = enrollmentInterest.Interested
            };

            if (status.Unlocked)
            {
                status.PercentComplete = decimal.ToInt32(summary.PercentComplete);
                status.ButtonText = summary.PercentComplete > 0 ? "Continue Planning" : "Start Planning";
                status.ButtonUrl = Url.Action("Index", "Planning", new { wizardId = summary.PlanningWizardID, area = "Pentameter" });
            }

            return status;
        }

        private string GetPlanningMessage(PlanningSummary summary)
        {
            var enrollmentInterest = EnrollmentStatus.Value.EnrollmentInterests.First(i => i.Name == summary.PlanningName);
            var planningInterest = GetPlanningInterest(summary, enrollmentInterest);
            var text = string.Empty;

            if (planningInterest == PlanningInterest.NotInterested)
            {
                text = "<p>Your enrollment profile states that " + enrollmentInterest.Message.ToLower() + "  To unlock this tool, update your enrollment interests.</p>";
            }
            else if (planningInterest == PlanningInterest.Unknown)
            {
                switch (summary.PlanningType)
                {
                    case PlanningType.Acquisition:
                        text = "<p>After you have completed enrollment, use this tool to help guide you through the steps to successfully complete a business acquisition.</p>";
                        break;
                    case PlanningType.Continuity:
                        text = "<p>After you have completed enrollment, use this tool to help guide you through the steps to manage your continuity plan.</p>";
                        break;
                    case PlanningType.Succession:
                        text = "<p>After you have completed enrollment, use this tool to help guide you through the steps to manage your succession plan.</p>";
                        break;
                }
            }
            else if (summary.PercentComplete <= 0)
            {
                switch (summary.PlanningType)
                {
                    case PlanningType.Acquisition:
                        text = "<p><strong>You have unlocked the Business Acquisition Planning Tool.</strong></p><p>Use this tool to help guide you through the steps to successfully complete a business acquisition.</p>";
                        break;
                    case PlanningType.Continuity:
                        text = "<p><strong>You have unlocked the Continuity Planning Tool.</strong></p><p>Use this tool to help guide you through the steps to manage your continuity plan.</p>";
                        break;
                    case PlanningType.Succession:
                        text = "<p><strong>You have unlocked the Succession Planning Tool.</strong></p><p>Use this tool to help guide you through the steps to manage your succession plan.</p>";
                        break;
                }
            }

            return text;
        }

        private PlanningInterest GetPlanningInterest(PlanningSummary summary, EnrollmentInterest enrollmentInterest)
        {
            var planningInterest = PlanningInterest.Unknown;

            if (enrollmentInterest.HasAnswer)
            {
                if (enrollmentInterest.Interested)
                {
                    planningInterest = summary.IsComplete ? PlanningInterest.InterestedComplete : PlanningInterest.InterestedIncomplete;
                }
                else
                {
                    planningInterest = PlanningInterest.NotInterested;
                }
            }

            return planningInterest;
        }

        private Status GetQualifiedBuyerStatus()
        {
            var survey = GetSurveyForUser(Settings.SurveyNames.QualifiedBuyer);
            var enrollmentStatus = EnrollmentStatus.Value;

            var status = new Status
            {
                Title = survey.SurveyName,
                Unlocked = EnrollmentStatus.Value.BusinessAcquisitionFunding.Interested
            };

            if (!enrollmentStatus.BusinessAcquisition.HasAnswer || (enrollmentStatus.BusinessAcquisition.Interested && !enrollmentStatus.BusinessAcquisitionFunding.HasAnswer))
            {
                status.Message = "<p><strong>Need funding for an acquisition?</strong></p><p>Cetera Financial Group may be able to assist. After you have completed enrollment, complete this short assessment to see if you meet the requisites for Cetera Financial Group's Qualified Buyer program.</p>";
            }
            else if (!enrollmentStatus.BusinessAcquisition.Interested || (enrollmentStatus.BusinessAcquisition.Interested && !enrollmentStatus.BusinessAcquisitionFunding.Interested))
            {
                status.Message = "<p>Your enrollment profile states that you currently do not need funding for an acquisition.  To unlock this assessment, update your enrollment interests.</p>";
            }

            if (status.Unlocked)
            {
                status.PercentComplete = decimal.ToInt32(Math.Round(survey.Status.Score * 100));
                status.ButtonText = survey.Status.State == SurveyState.NotStarted ? "Start Assessment" : "Update Assessment";
                status.ButtonUrl = Url.RouteUrl("Pentameter.Succession.QualifiedBuyer", new {surveyName = Settings.SurveyNames.QualifiedBuyer});
                status.Message = "<p><strong>You have unlocked the Qualified Buyer Program Assessment.</strong></p><p>Complete this short assessment to see if you meet the requisites for Cetera Financial Group's Qualified Buyer program.</p>";
            }

            return status;
        }

        private Survey GetSurveyForUser(string surveyName)
        {
            return _surveyService.GetSurvey(new SurveyRequest
            {
                SurveyName = surveyName,
                UserID = AssistedUser.UserID,
                IncludeResponse = true
            });
        }

        #endregion
    }
}
