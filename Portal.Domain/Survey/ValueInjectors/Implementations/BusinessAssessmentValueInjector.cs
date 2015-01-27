using System.Linq;
using Portal.Domain.Extensions;
using Portal.Infrastructure.Configuration;
using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Survey.ValueInjectors.Implementations
{
    public class BusinessAssessmentValueInjector : IValueInjector
    {
        public void Inject(Model.Survey survey, int userId, IUserService userService, ISurveyService surveyService)
        {
            
        }

        public void PreSelect(Model.Survey survey, int userId, IUserService userService, ISurveyService surveyService)
        {
            var request = new SurveyRequest()
            {
                SurveyName = Settings.SurveyNames.Enrollment,
                UserID = userId,
                IncludeResponse = true
            };

            var isComplete = false;

            // Get Enrollment status
            var enrollmentSurvey = surveyService.GetSurvey(request);

            if (enrollmentSurvey != null)
            {
                isComplete = enrollmentSurvey.CalculateStatus().IsComplete();
            }

            // Find succession question
            var question = survey.Questions.FirstOrDefault(q => q.QuestionName == "EnrollmentComplete");

            if (question == null)
                return;

            // Pre-select answers based on enrollment status
            question.FindPossibleAnswer("Yes").IsSelected = isComplete;
            question.FindPossibleAnswer("No").IsSelected = !isComplete;

            // Disable question
            question.IsDisabled = true;
        }
    }
}
