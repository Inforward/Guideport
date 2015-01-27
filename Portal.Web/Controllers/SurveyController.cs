using Portal.Domain.Survey.Notifications;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Helpers;
using Portal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using InputType = Portal.Model.InputType;

namespace Portal.Web.Controllers
{
    [ProfileTypeAuthorize(ProfileTypeValues.FinancialAdvisor, ProfileTypeValues.Employee)]
    public class SurveyController : BaseController
    {
        #region Private Members
        private readonly ISurveyService _surveyService;
        #endregion

        #region Constructor

        public SurveyController(IUserService userService, ISurveyService surveyService, ILogger logger)
            : base(userService, logger)
        {
            _surveyService = surveyService;
        }

        #endregion

        #region Actions

        [HttpGet]
        [Route("survey")]
        [Route("pentameter/survey", Name="Pentameter.Survey")]
        [Route("pentameter/succession-planning/enrollment-survey", Name = "Pentameter.Succession.Enrollment")]
        [Route("pentameter/succession-planning/qualified-buyer-survey", Name = "Pentameter.Succession.QualifiedBuyer")]
        public ActionResult Index(string surveyName)
        {
            var survey = new Survey() {SurveyName = surveyName};

            return View(survey);
        }

        [HttpGet]
        [Route("survey/print/{surveyName}/{userId?}")]
        public ActionResult Print(string surveyName, int? userId)
        {
            ViewBag.SurveyUser = GetSurveyUser(userId);

            var survey = _surveyService.GetSurvey(new SurveyRequest
            {
                SurveyName = surveyName,
                IncludeResponse = true,
                UserID = ViewBag.SurveyUser.UserID
            });

            return View(ToSurveyViewModel(survey));
        }

        [HttpPost]
        [Route("survey/get-survey/{surveyName}")]
        public JsonResult JsonGetSurvey(string surveyName)
        {
            var survey = GetSurveyForUser(surveyName);

            return Json(ToSurveyViewModel(survey));
        }

        [HttpPost]
        [Route("survey/process-trigger")]
        public JsonResult JsonProcessSurveyTrigger(SurveyResponseViewModel responseViewModel)
        {
            // Convert from view model
            var surveyResponse = ToSurveyResponse(responseViewModel);

            // Get survey with this response applied
            var survey = _surveyService.GetSurvey(new SurveyRequest
            {
                SurveyName = surveyResponse.SurveyName,
                UserID = AssistedUser.UserID,
                IncludeResponse = false,
                ApplyResponse = true,
                ResponseToApply = surveyResponse
            });

            // Convert to our standard View Model
            var viewModel = ToSurveyViewModel(survey);

            // Now, let's trim this down by dynamically generating a new object that contains only the properties we need
            var lightWeight = new
            {
                Pages = viewModel.Pages.Select(p => new
                {
                    p.Id,
                    p.IsVisible,
                    Questions = p.Questions.Select(q => new
                    {
                        q.Id,
                        q.Number,
                        q.IsVisible,
                        PossibleAnswers = q.PossibleAnswers.Select(a => new
                        {
                            a.Id,
                            a.IsSelected
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return Json(lightWeight);
        }

        [HttpPost]
        [Route("survey/update")]
        public JsonResult JsonUpdateSurveyResponse(SurveyResponseViewModel responseViewModel)
        {
            // Convert from view model
            var surveyResponse = ToSurveyResponse(responseViewModel);

            // Add audit info
            surveyResponse.UserID = AssistedUser.UserID;
            surveyResponse.ModifyUserID = CurrentUser.UserID;
            surveyResponse.ModifyDate = DateTime.Now;
            surveyResponse.ModifyDateUtc = DateTime.UtcNow;

            if (surveyResponse.CreateUserID <= 0)
                surveyResponse.CreateUserID = CurrentUser.UserID;

            // Get 'previous' version
            var previousSurvey = GetSurveyForUser(surveyResponse.SurveyName);

            // Save 'new' version
            var response = _surveyService.SaveSurveyResponse(ref surveyResponse);

            // If save was successful, return fresh copy of the survey
            var surveyViewModel = default(SurveyViewModel);

            if (response.Success)
            {
                var currentSurvey = GetSurveyForUser(surveyResponse.SurveyName);
                var notification = Using<NotificationFactory>().CreateNotification(currentSurvey.SurveyNotificationType, previousSurvey, currentSurvey, GetAssistedUser());

                if (notification != null)
                {
                    notification.SummaryUrl = Url.Action("Print", "Survey", new { currentSurvey.SurveyName, AssistedUser.UserID }, Request.Url.Scheme);
                    notification.Send();
                }

                surveyViewModel = ToSurveyViewModel(currentSurvey);
            }

            return Json(new
            {
                success = response.Success,
                message = response.Message,
                validationErrors = response.Result,
                survey = surveyViewModel
            });
        }

        #endregion

        #region Mapping Methods

        public SurveyViewModel ToSurveyViewModel(Survey survey)
        {
            if (survey == null)
                return null;

            var viewModel = new SurveyViewModel()
            {
                Id = survey.SurveyID,
                Name = survey.SurveyName,
                Introduction = survey.SurveyDescription,
                LastModifiedDate = string.Format("{0:MM/dd/yyyy}", survey.ModifyDate),
                CompleteMessage = survey.CompleteMessage,
                Status = survey.Status.State.ToSurveyStatusDisplayText(),
                StatusTypeLabel = survey.StatusLabel + ":",
                ActivePageId = survey.CurrentPage,
                CompleteRedirectUrl = survey.CompleteRedirectUrl,
                IsStatusVisible = survey.IsStatusVisible,
                ReviewTabText = survey.ReviewTabText
            };

            if (survey.Status.ProgressType == SurveyProgressType.Score)
            {
                viewModel.Status = Math.Round(survey.Status.Score * 100, 2).ToString("G29") + " %";
            }

            viewModel.Pages.AddRange(survey.Pages.OrderBy(p => p.SortOrder).Select(p => ToSurveyPageViewModel(p, survey)));

            return viewModel;
        }

        public static SurveyPageViewModel ToSurveyPageViewModel(SurveyPage page, Survey survey)
        {
            var viewModel = new SurveyPageViewModel()
            {
                Id = page.SurveyPageID,
                Name = page.PageName,
                IsVisible = page.IsVisible,
                IsSelected = page.IsSelected,
                IsSummary = page.IsSummary,
                PageOrder = page.SortOrder,
                Questions = new List<SurveyQuestionViewModel>()
            };

            // Add Score & Description
            if (survey.Status != null)
            {
                var status = survey.Status.Pages.FirstOrDefault(p => p.SurveyPageID == viewModel.Id);
                viewModel.Score = status != null ? status.DisplayScore : 0;

                var description = page.ScoreRanges.FirstOrDefault(d => viewModel.Score >= d.MinDisplayScore && viewModel.Score <= d.MaxDisplayScore);
                viewModel.ScoreDescription = description != null ? description.Description : string.Empty;
            }

            // Add suggested contents
            if (page.SuggestedContents.Any())
            {
                viewModel.SuggestedContents = page.SuggestedContents
                    .Where(sc => !string.IsNullOrWhiteSpace(sc.Title))
                    .Select(s => new SurveyPageSuggestedContentViewModel() { Title = s.Title, Url = s.Url })
                    .OrderBy(v => v.Title)
                    .DistinctBy(v => v.Url)
                    .ToList();
            }

            var number = 0;

            foreach (var question in page.Questions.Where(q => q.Layout != LayoutType.EndSection).OrderBy(q => q.SortOrder))
            {
                var questionViewModel = ToSurveyQuestionViewModel(question);

                if (question.InputType != InputType.None && question.Layout == LayoutType.None && question.IsVisible)
                    questionViewModel.Number = string.Format("{0}.", (++number));

                viewModel.Questions.Add(questionViewModel);
            }

            return viewModel;
        }

        public static SurveyQuestionViewModel ToSurveyQuestionViewModel(SurveyQuestion question)
        {
            var viewModel = new SurveyQuestionViewModel()
            {
                Id = question.SurveyQuestionID,
                Text = question.QuestionText.StripLineBreaks(),
                InputType = Enum.GetName(typeof(InputType), question.InputType),
                AnswerType = Enum.GetName(typeof(AnswerType), question.AnswerType),
                Layout = Enum.GetName(typeof(LayoutType), question.Layout),
                IsRequired = question.IsRequired,
                IsDisabled = question.IsDisabled,
                IsVisible = question.IsVisible,
                HasTrigger = question.HasTrigger,
                Answer = question.Answer ?? string.Empty,
                PossibleAnswers = new List<SurveyAnswerViewModel>()
            };

            if (question.InputType == InputType.MultiText)
            {
                viewModel.Answers = question.Answers.Select(s => new JsonMultiTextAnswer() { Text = s, Value = s }).ToList();
            }

            foreach (var answer in question.PossibleAnswers.OrderBy(a => a.SortOrder))
            {
                viewModel.PossibleAnswers.Add(new SurveyAnswerViewModel()
                {
                    Id = answer.SurveyQuestionAnswerID,
                    QuestionId = answer.SurveyQuestionID,
                    Text = answer.AnswerText,
                    ReviewText = !string.IsNullOrEmpty(answer.ReviewAnswerText) ? answer.ReviewAnswerText : answer.AnswerText,
                    IsSelected = answer.IsSelected,
                    IsTrigger = answer.IsTrigger
                });
            }

            return viewModel;
        }

        public static SurveyResponse ToSurveyResponse(SurveyResponseViewModel viewModel)
        {
            var surveyResponse = new SurveyResponse()
            {
                SurveyID = viewModel.SurveyId,
                SelectedSurveyPageID = viewModel.SelectedPageId,
                SurveyName = viewModel.SurveyName,
                ApplyAsComplete = viewModel.ApplyAsComplete
            };

            if (!viewModel.Answers.IsNullOrEmpty())
            {
                surveyResponse.Answers = viewModel.Answers.Select(a => new SurveyResponseAnswer()
                                            {
                                                SurveyQuestionID = a.SurveyQuestionId,
                                                SurveyPageID = a.SurveyPageId,
                                                Answer = a.Answer
                                            }).ToList();
            }

            return surveyResponse;
        }

        #endregion

        #region Private Methods

        private Survey GetSurveyForUser(string surveyName)
        {
            return _surveyService.GetSurvey(new SurveyRequest
                    {
                        SurveyName = surveyName,
                        UserID = AssistedUser.UserID,
                        IncludeResponse = true
                    });
        }

        private User GetSurveyUser(int? userId)
        {
            return userId != null ? _userService.GetUserByUserId(userId.Value) : GetAssistedUser();
        }

        #endregion
    }
}
