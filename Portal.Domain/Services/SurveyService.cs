using Portal.Data;
using Portal.Domain.Extensions;
using Portal.Domain.Survey.ValueInjectors;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Model.Rules;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Portal.Domain.Services
{
    public class SurveyService : BaseService, ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICacheStorage _cacheStorage;
        private readonly ILogger _logger;
        private readonly ICmsService _cmsService;
        private readonly IRuleService _ruleService;
        private readonly IUserService _userService;

        public SurveyService(ISurveyRepository surveyRepository, ICacheStorage cacheStorage, ILogger logger, ICmsService cmsService, IRuleService ruleService, IUserService userService)
        {
            _surveyRepository = surveyRepository;
            _cacheStorage = cacheStorage;
            _logger = logger;
            _cmsService = cmsService;
            _ruleService = ruleService;
            _userService = userService;
        }

        public Model.Survey GetSurvey(SurveyRequest request)
        {
            if (!request.CacheForUser || request.UserID <= 0 || string.IsNullOrEmpty(request.SurveyName) || !request.IncludeResponse)
                return GetSurveyInternal(request);

            var cacheKey = string.Format("Survey.{0}.{1}", request.SurveyName, request.UserID);

            return _cacheStorage.Retrieve(cacheKey, () => GetSurveyInternal(request));
        }

        public IEnumerable<Model.Survey> GetSurveys()
        {
            return _surveyRepository.GetAll<Model.Survey>().OrderBy(s => s.SurveyName).ToList();
        }

        public void UpdateSurvey(Model.Survey survey, int auditUserId)
        {
            if (survey == null) return;

            // Ensure audit-related fields have values
            UpdateAuditFields(auditUserId, survey);

            _surveyRepository.UpdateSurvey(survey, auditUserId);

            _cacheStorage.ClearNamespace("Survey");
        }

        public SurveyResponse GetSurveyResponse(SurveyResponseRequest request)
        {
            if (request == null || (request.UserID <= 0 && string.IsNullOrEmpty(request.SurveyName)))
                return null;

            var query = _surveyRepository.GetAll<SurveyResponse>();

            if (request.UserID > 0)
                query = query.Where(s => s.UserID == request.UserID);

            if (!string.IsNullOrEmpty(request.SurveyName))
                query = query.Where(s => s.Survey.SurveyName == request.SurveyName);

            if (request.IncludeAnswers)
                query = query.Include("Answers");

            return query.FirstOrDefault();
        }

        public void CreateSurveyResponse(ref SurveyResponse surveyResponse)
        {
            if (surveyResponse == null) return;

            _surveyRepository.CreateSurveyResponse(surveyResponse);
        }

        public SaveSurveyResponseResponse SaveSurveyResponse(ref SurveyResponse surveyResponse)
        {
            Required.NotNull(surveyResponse, "surveyResponse");

            var response = new SaveSurveyResponseResponse();

            try
            {
                ClearResponseCache(surveyResponse);

                // Get user's survey and apply response
                var survey = GetSurvey(new SurveyRequest
                {
                    SurveyName = surveyResponse.SurveyName,
                    UserID = surveyResponse.UserID,
                    ApplyResponse = true,
                    ResponseToApply = surveyResponse
                });

                // Grab any questions that have errors (rule engine adds items to Question's Error collection)
                var questionsWithErrors = survey.Questions.Where(q => q.Errors.Any()).ToList();

                if (questionsWithErrors.Any())
                {
                    response.Success = false;
                    response.Message = "Validation Failed";
                    response.Result = questionsWithErrors.Select(q => new SurveyQuestionError()
                    {
                        SurveyQuestionID = q.SurveyQuestionID,
                        ErrorMessage = q.Errors[0].ErrorMessage
                    }).ToList();
                }
                else
                {
                    var existingResponse = GetSurveyResponse(new SurveyResponseRequest()
                    {
                        SurveyName = surveyResponse.SurveyName,
                        UserID = surveyResponse.UserID,
                        IncludeAnswers = true
                    });

                    surveyResponse.CurrentScore = survey.Status.Score;
                    surveyResponse.PercentComplete = survey.Status.PercentComplete;

                    // If survey is complete, ensure corresponding fields have values
                    if (survey.Status.State == SurveyState.Complete && (survey.IsAutoCompleteEnabled || surveyResponse.ApplyAsComplete))
                    {
                        if (existingResponse != null)
                        {
                            surveyResponse.CompleteDate = existingResponse.CompleteDate ?? DateTime.Now;
                            surveyResponse.CompleteDateUtc = existingResponse.CompleteDateUtc ?? DateTime.UtcNow;
                            surveyResponse.CompleteUserID = existingResponse.CompleteUserID ?? surveyResponse.ModifyUserID;
                        }
                        else
                        {
                            surveyResponse.CompleteDate = DateTime.Now;
                            surveyResponse.CompleteDateUtc = DateTime.UtcNow;
                            surveyResponse.CompleteUserID = surveyResponse.ModifyUserID;
                        }
                    }
                    else
                    {
                        surveyResponse.CompleteDate = null;
                        surveyResponse.CompleteDateUtc = null;
                        surveyResponse.CompleteUserID = null;
                    }

                    // Ensure answers have audit info
                    foreach (var answer in surveyResponse.Answers)
                    {
                        answer.CreateUserID = surveyResponse.ModifyUserID;
                        answer.CreateDate = surveyResponse.ModifyDate;
                        answer.CreateDateUtc = surveyResponse.ModifyDateUtc;
                    }

                    // Create or Update
                    if (existingResponse == null)
                    {
                        surveyResponse.CreateUserID = surveyResponse.ModifyUserID;
                        surveyResponse.CreateDate = surveyResponse.ModifyDate;
                        surveyResponse.CreateDateUtc = surveyResponse.ModifyDateUtc;

                        CreateSurveyResponse(ref surveyResponse);
                    }
                    else
                    {
                        surveyResponse.SurveyResponseID = existingResponse.SurveyResponseID;
                        surveyResponse.CreateUserID = existingResponse.ModifyUserID;
                        surveyResponse.CreateDate = existingResponse.ModifyDate;
                        surveyResponse.CreateDateUtc = existingResponse.ModifyDateUtc;

                        _surveyRepository.UpdateSurveyResponse(surveyResponse);
                    }

                    SaveSurveyResponseHistory(surveyResponse.SurveyResponseID, surveyResponse.ModifyUserID, survey.Status.Pages);

                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.FullMessage();

                _logger.Log(ex);
            }

            return response;
        }

        public SurveySummary GetSurveySummary(string surveyName, int userId)
        {
            var request = new SurveyRequest()
            {
                SurveyName = surveyName,
                IncludeResponse = true,
                UserID = userId,
                CacheForUser = true
            };

            var survey = GetSurvey(request);

            if(survey == null)
                throw new Exception("Invalid Survey Name: " + surveyName);

            var summary = new SurveySummary()
            {
                SurveyID = survey.SurveyID,
                SurveyName = survey.SurveyName,
                Score = survey.Status.Score,
                State = survey.Status.State,
                DisplayScore = survey.Status.DisplayScore
            };

            foreach (var page in survey.Pages.Where(p => !p.IsSummary))
            {
                var pageStatus = survey.Status.Pages.First(p => p.Name == page.PageName);
                var pageScoreDescription = page.ScoreRanges.FirstOrDefault(d => pageStatus.Score >= d.MinScore && pageStatus.Score <= d.MaxScore);

                var pageSummary = new SurveyPageSummary()
                {
                    SurveyPageID = page.SurveyPageID,
                    PageName = page.PageName,
                    Score = pageStatus.Score,
                    DisplayScore = pageStatus.DisplayScore,
                    State = pageStatus.State,
                    Tooltip = page.Tooltip,
                    ScoreSummary = pageScoreDescription != null ? pageScoreDescription.Description : string.Empty
                };

                if (!page.SuggestedContents.IsNullOrEmpty())
                {
                    pageSummary.SuggestedContent = page.SuggestedContents.DistinctBy(a => a.Url).Select(s => new SurveySummaryContent() { Title = s.Title, Url = s.Url }).ToList();
                }

                summary.Pages.Add(pageSummary);
            }

            return summary;
        }

        #region Private Methods

        private Model.Survey GetSurveyInternal(SurveyRequest request)
        {
            if (request == null || (request.SurveyID <= 0 && string.IsNullOrEmpty(request.SurveyName)))
                return null;

            var survey = null as Model.Survey;

            if (request.SurveyID > 0)
                survey = _surveyRepository.GetSurveyByID(request.SurveyID);
            else if (!string.IsNullOrEmpty(request.SurveyName))
                survey = _surveyRepository.GetSurveyByName(request.SurveyName);

            if (survey == null)
                throw new Exception(string.Format("Invalid survey id: {0}, name: {1}: ", request.SurveyID, request.SurveyName));

            if (request.UserID <= 0)
                return survey;

            var injector = ValueInjectorFactory.CreateValueInjector(survey.SurveyName);

            // Inject Possible Answers to questions based on the user's profile
            if (injector != null)
                injector.Inject(survey, request.UserID, _userService, this);

            if (survey.IsReviewVisible)
            {
                survey.Pages = new List<SurveyPage>(survey.Pages);

                survey.Pages.Add(new SurveyPage()
                {
                    PageName = survey.ReviewTabText,
                    SurveyID = survey.SurveyID,
                    IsVisible = true,
                    IsSummary = true,
                    SortOrder = survey.Pages.Last().SortOrder + 1,
                    SurveyPageID = 0
                });
            }

            if (request.IncludeResponse)
            {
                var surveyResponse = GetSurveyResponse(new SurveyResponseRequest()
                                            {
                                                SurveyName = request.SurveyName,
                                                UserID = request.UserID,
                                                IncludeAnswers = true
                                            });

                if (surveyResponse == null)
                {
                    surveyResponse = new SurveyResponse()
                    {
                        SurveyID = survey.SurveyID,
                        SelectedSurveyPageID = survey.Pages.OrderBy(p => p.SortOrder).First().SurveyPageID,
                        CreateUserID = request.UserID,
                        CreateDate = DateTime.Now,
                        CreateDateUtc = DateTime.UtcNow,
                        ModifyUserID = request.UserID,
                        ModifyDate = DateTime.Now,
                        ModifyDateUtc = DateTime.UtcNow
                    };
                }

                ApplySurveyResponse(survey, surveyResponse);
            }

            if (request.ApplyResponse && request.ResponseToApply != null)
            {
                ApplySurveyResponse(survey, request.ResponseToApply);
            }

            // Ensure Status is calculated
            survey.CalculateStatus();

            // Pre-Select answers based on the user's profile (if not already answered)
            if (injector != null)
                injector.PreSelect(survey, request.UserID, _userService, this);

            // Populate suggested content
            PopulateSuggestedContent(survey);

            return survey;            
        }

        private void UpdateAuditFields(int auditUserId, Model.Survey survey)
        {
            UpdateAuditData(survey, auditUserId);

            var contentList = (from page in survey.Pages
                               from question in page.Questions
                               from answer in question.PossibleAnswers.Where(a => a.SuggestedContents.Any())
                               from content in answer.SuggestedContents
                               select content);
            
            foreach (var content in contentList)
            {
                if (content.CreateUserID <= 0)
                    content.CreateUserID = auditUserId;

                if (content.CreateDate == DateTime.MinValue)
                    content.CreateDate = DateTime.Now;

                if (content.CreateDateUtc == DateTime.MinValue)
                    content.CreateDateUtc = DateTime.UtcNow;

            }
        }

        private void ApplySurveyResponse(Model.Survey survey, SurveyResponse surveyResponse)
        {
            // Select/Populate the answers on the survey object based on the response
            foreach (var answer in surveyResponse.Answers)
            {
                var question = survey.Questions.Find(q => q.SurveyQuestionID == answer.SurveyQuestionID);

                if (question != null)
                {
                    switch (question.AnswerType)
                    {
                        case AnswerType.MultiSelect:
                        case AnswerType.Select:
                            {
                                var option = question.PossibleAnswers.FirstOrDefault(a => a.AnswerText == answer.Answer);

                                if (option != null)
                                {
                                    option.IsSelected = true;
                                }
                            }
                            break;
                        case AnswerType.Text:
                            {
                                if (question.InputType == InputType.MultiText)
                                {
                                    question.PossibleAnswers.Add(new SurveyAnswer()
                                    {
                                        SurveyQuestionID = answer.SurveyQuestionID,
                                        AnswerText = answer.Answer
                                    });
                                    question.PossibleAnswers.Last().IsSelected = true;
                                }
                                else
                                {
                                    question.Answer = answer.Answer;
                                }
                            }
                            break;
                    }
                }
            }

            // Set the raw Current Response
            survey.CurrentResponse = surveyResponse;

            // Set the audit info
            survey.CreateUserID = surveyResponse.CreateUserID;
            survey.CreateDate = surveyResponse.CreateDate;
            survey.CreateDateUtc = surveyResponse.CreateDateUtc;
            survey.ModifyUserID = surveyResponse.ModifyUserID;
            survey.ModifyDate = surveyResponse.ModifyDate;
            survey.ModifyDateUtc = surveyResponse.ModifyDateUtc;

            // Set the active page
            survey.Pages.First(p => p.SurveyPageID == surveyResponse.SelectedSurveyPageID).IsSelected = true;

            // Now, run the rulesets
            if (!string.IsNullOrEmpty(survey.RulesetCoreName))
            {
                _ruleService.ExecuteRuleSet(new RulesetRequest() { Name = survey.RulesetCoreName, Survey = survey, EntityType = RulesetRequestEntityType.Survey});
            }

            if (!string.IsNullOrEmpty(survey.RulesetValidationName))
            {
                _ruleService.ExecuteRuleSet(new RulesetRequest() { Name = survey.RulesetCoreName, Survey = survey, EntityType = RulesetRequestEntityType.Survey });
            }
        }

        private void PopulateSuggestedContent(Model.Survey survey)
        {
            if (!survey.SuggestedContentSiteID.HasValue)
                return;

            if (!survey.Pages.Any(p => p.SuggestedContents.Any()))
                return;

            var site = _cmsService.GetSite(new SiteRequest { SiteID = survey.SuggestedContentSiteID.Value, IncludeAll = true });

            var contents = site.SiteContents
                                .Where(s => s.SiteContentStatusID == (int)ContentStatus.Published && s.PublishDateUtc <= DateTime.UtcNow)
                                .OrderBy(s => s.Title)
                                .ToList();

            foreach (var page in survey.Pages)
            {
                foreach (var suggestedContent in page.SuggestedContents)
                {
                    var content = contents.FirstOrDefault(s => s.SiteContentID == suggestedContent.SiteContentID);

                    if (content != null)
                    {
                        suggestedContent.Title = content.Title;
                        suggestedContent.Url = string.Format("https://{0}{1}", site.DomainName, content.Permalink);
                    }
                }
            }
        }

        private void ClearResponseCache(SurveyResponse surveyResponse)
        {
            var cacheKey = string.Format("Survey.{0}.{1}", surveyResponse.SurveyName, surveyResponse.UserID);
            _cacheStorage.Remove(cacheKey);
        }

        private void SaveSurveyResponseHistory(int surveyResponseId, int modifyUserId, IEnumerable<SurveyPageStatus> pageStatuses)
        {
            var responseHistories = pageStatuses.Select(page => new SurveyResponseHistory
            {
                SurveyResponseID = surveyResponseId,
                SurveyPageID = page.SurveyPageID,
                ResponseDate = DateTime.Today,
                Score = page.Score,
                PercentComplete = page.PercentComplete,
                CreateUserID = modifyUserId,
                CreateDate = DateTime.Now,
                CreateDateUtc = DateTime.UtcNow,
                ModifyUserID = modifyUserId,
                ModifyDate = DateTime.Now,
                ModifyDateUtc = DateTime.UtcNow,
                IsLatestScore = true
            }).ToList();

            _surveyRepository.UpdateSurveyResponseHistory(responseHistories);
        }

        #endregion
    }
}
