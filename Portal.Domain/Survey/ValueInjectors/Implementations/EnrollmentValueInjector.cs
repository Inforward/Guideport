using Portal.Domain.Helpers;
using Portal.Model;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Domain.Survey.ValueInjectors
{
    internal class EnrollmentValueInjector : IValueInjector
    {
        private const string SellerCriteriaPageName = "My Criteria as a Seller";
        private const string BuyerCriteriaPageName = "My Criteria as a Buyer";
        private const string BusinessProfilePageName = "My Business Profile";

        public void Inject(Model.Survey survey, int userId, IUserService userService, ISurveyService surveyService)
        {
            var request = new UserRequest()
            {
                UserID = userId,
                IncludeBranchInfo = true

            };

            var surveyUser = userService.GetUsers(request).Users.FirstOrDefault();

            if (surveyUser != null)
            {
                InjectBranchLocations(survey, surveyUser);
            }
        }

        public void PreSelect(Model.Survey survey, int userId, IUserService userService, ISurveyService surveyService)
        {
            var request = new UserRequest()
            {
                UserID = userId,
                IncludeLicenseInfo = true
            };

            var surveyUser = userService.GetUsers(request).Users.FirstOrDefault();

            if (surveyUser != null)
            {
                PreSelectYearsInOperation(survey, surveyUser);

                PreSelectBrokerDealerAffiliation(survey, surveyUser);

                PreSelectLicenseDesignations(survey, surveyUser);

                PreSelectProductionOptions(survey, surveyUser);
            }
        }

        private static void InjectBranchLocations(Model.Survey survey, User surveyUser)
        {
            var locationQuestions = new List<SurveyQuestion>()
                {
                    survey.GetQuestion(SellerCriteriaPageName, "LocationC"),
                    survey.GetQuestion(BuyerCriteriaPageName, "LocationC"),
                    survey.GetQuestion(SellerCriteriaPageName, "LocationS"),
                    survey.GetQuestion(BuyerCriteriaPageName, "LocationS"),
                    survey.GetQuestion(BuyerCriteriaPageName, "LocationB"),
                    survey.GetQuestion(BusinessProfilePageName, "Location")
                };

            // 
            // Add City/State options based on the branches the user is currently associated with
            //
            if (surveyUser.Branches != null && surveyUser.Branches.Any())
            {
                var cityStates = surveyUser.Branches.Where(b => !string.IsNullOrWhiteSpace(b.City) && !string.IsNullOrWhiteSpace(b.State))
                        .Select(b => string.Format("{0}, {1}", b.City, b.State)).ToList();

                cityStates.Sort();

                for (var i = 0; i < cityStates.Count; i++)
                {
                    foreach (var question in locationQuestions)
                    {
                        question.PossibleAnswers.Add(new SurveyAnswer()
                        {
                            AnswerText = cityStates[i],
                            SortOrder = i,
                            SurveyQuestionID = question.SurveyQuestionID,
                            SurveyQuestionAnswerID = (i + 1)
                        });
                    }
                }
            }

            //
            // Add an "Other" selection to all but the Business Profile Question
            //
            foreach (var question in locationQuestions.Where(question => question.PageName != BusinessProfilePageName))
            {
                var order = question.PossibleAnswers.Count;

                question.PossibleAnswers.Add(new SurveyAnswer()
                    {
                        AnswerText = "Other",
                        SortOrder = order,
                        IsTrigger = true,
                        SurveyQuestionID = question.SurveyQuestionID,
                        SurveyQuestionAnswerID = question.PossibleAnswers.Count + 1
                    });
            }
        }

        private static void PreSelectYearsInOperation(Model.Survey survey, User surveyUser)
        {
            if (!surveyUser.SecurityProfileStartDate.HasValue)
                return;

            var question = survey.GetQuestion(BusinessProfilePageName, "YearsOperations");

            if (question == null)
                return;

            var selectedAnswer = question.PossibleAnswers.FirstOrDefault(a => a.IsSelected);

            if (selectedAnswer != null && selectedAnswer.AnswerText != "None Selected")
                return;

            var span = DateTime.Today - surveyUser.SecurityProfileStartDate.Value;
            var years = (new DateTime(1, 1, 1) + span).Year - 1;

            if (years <= 5)
            {
                question.PreSelectPossibleAnswer("0 - 5 years");
            }
            else if (years > 5 && years <= 10)
            {
                question.PreSelectPossibleAnswer("6 - 10 years");
            }
            else if (years > 10 && years <= 20)
            {
                question.PreSelectPossibleAnswer("11 - 20 years");
            }
            else
            {
                question.PreSelectPossibleAnswer("More than 20 years");
            }
        }

        private static void PreSelectBrokerDealerAffiliation(Model.Survey survey, User surveyUser)
        {
            if (!surveyUser.SecurityProfileStartDate.HasValue)
                return;

            var question = survey.GetQuestion(BusinessProfilePageName, "BD");

            if (question == null)
                return;

            if (string.IsNullOrWhiteSpace(question.Answer))
            {
                question.Answer = surveyUser.AffiliateName;
            }

            question.IsDisabled = true;
            question.IsPreSelected = true;
        }

        private static void PreSelectLicenseDesignations(Model.Survey survey, User surveyUser)
        {
            if (surveyUser.Licenses == null || surveyUser.Licenses.Count == 0)
                return;

            var designations = surveyUser.Licenses.Where(l => l.LicenseExamTypeID == (int)LicenseExamTypes.ProfessionalDesignations).ToList();

            if (designations.Any())
            {
                var question = survey.GetQuestion(BusinessProfilePageName, "Designations");

                if (question != null && !question.PossibleAnswers.Any(a => a.IsSelected))
                {
                    designations.ForEach(d =>
                        {
                            var answers = question.PossibleAnswers.Where(a => a.AnswerText.IndexOf(d.RegistrationCategory, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();

                            foreach (var answer in answers)
                            {
                                answer.IsSelected = true;
                            }

                            question.IsPreSelected = answers.Any();
                        });
                }
            }

            var nonDesignations = surveyUser.Licenses.Where(l => l.LicenseExamTypeID != (int)LicenseExamTypes.ProfessionalDesignations).ToList();

            if (nonDesignations.Any())
            {
                var question = survey.GetQuestion(BusinessProfilePageName, "Licenses");

                if (question != null)
                {
                    if (string.IsNullOrWhiteSpace(question.Answer))
                    {
                        nonDesignations.ForEach(nd =>
                            {
                                if (!string.IsNullOrEmpty(question.Answer))
                                    question.Answer += ", ";

                                question.Answer += nd.Description;
                            });

                        question.IsPreSelected = true;
                    }

                    question.IsDisabled = true;
                }
            }
        }

        private static void PreSelectProductionOptions(Model.Survey survey, User surveyUser)
        {
            var aum = surveyUser.AUM ?? 0;
            var gdc = surveyUser.GDCT12 ?? 0;
            var aumQuestion = survey.GetQuestion(BusinessProfilePageName, "AUM");
            var gdcQuestion = survey.GetQuestion(BusinessProfilePageName, "GDC");

            if (aumQuestion != null)
            {
                var answer = aumQuestion.PossibleAnswers.FirstOrDefault(a => a.IsSelected);

                if (answer == null || answer.AnswerText == "None Selected")
                {
                    if (aum.GreaterThanAndLessOrEqualTo(-1, 10000000))
                        aumQuestion.PreSelectPossibleAnswer("Less than $10,000,000");

                    if (aum.GreaterThanAndLessOrEqualTo(10000000, 25000000))
                        aumQuestion.PreSelectPossibleAnswer("$10,000,001 - $25,000,000");

                    if (aum.GreaterThanAndLessOrEqualTo(25000000, 50000000))
                        aumQuestion.PreSelectPossibleAnswer("$25,000,001 - $50,000,000");

                    if (aum.GreaterThanAndLessOrEqualTo(50000000, 75000000))
                        aumQuestion.PreSelectPossibleAnswer("$50,000,001 - $75,000,000");

                    if (aum.GreaterThanAndLessOrEqualTo(75000000, 100000000))
                        aumQuestion.PreSelectPossibleAnswer("$75,000,001 - $100,000,000");

                    if (aum > 100000000)
                        aumQuestion.PreSelectPossibleAnswer("More than $100,000,000");
                }
            }

            if (gdcQuestion != null)
            {
                var answer = gdcQuestion.PossibleAnswers.FirstOrDefault(a => a.IsSelected);

                if (answer == null || answer.AnswerText == "None Selected")
                {
                    if (gdc.GreaterThanAndLessOrEqualTo(-1, 100000))
                        gdcQuestion.PreSelectPossibleAnswer("Less than $100,000");

                    if (gdc.GreaterThanAndLessOrEqualTo(100000, 250000))
                        gdcQuestion.PreSelectPossibleAnswer("$100,001 - $250,000");

                    if (gdc.GreaterThanAndLessOrEqualTo(250000, 500000))
                        gdcQuestion.PreSelectPossibleAnswer("$250,001 - $500,000");

                    if (gdc.GreaterThanAndLessOrEqualTo(500000, 1000000))
                        gdcQuestion.PreSelectPossibleAnswer("$500,001 - $1,000,000");

                    if (gdc.GreaterThanAndLessOrEqualTo(1000000, 2500000))
                        gdcQuestion.PreSelectPossibleAnswer("$1,000,001 - $2,500,000");

                    if (gdc.GreaterThanAndLessOrEqualTo(2500000, 5000000))
                        gdcQuestion.PreSelectPossibleAnswer("$2,500,001 - $5,000,000");

                    if (gdc.GreaterThanAndLessOrEqualTo(5000000, 10000000))
                        gdcQuestion.PreSelectPossibleAnswer("$5,000,001 - $10,000,000");

                    if (gdc > 10000000)
                        gdcQuestion.PreSelectPossibleAnswer("More than $10,000,000");
                }
            }
        }
    }
}
