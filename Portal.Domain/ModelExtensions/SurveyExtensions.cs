using Portal.Domain.Survey.StatusCalculators;
using Portal.Model;

namespace Portal.Domain.Extensions
{
    public static class SurveyExtensions
    {
        public static Model.Survey CalculateStatus(this Model.Survey survey)
        {
            var calculator = StatusCalculatorFactory.CreateCalculator(survey.StatusCalculator);

            survey.Status = calculator.Calculate(survey);
            survey.Status.ProgressType = calculator.ProgressType;

            return survey;
        }

        public static bool IsComplete(this Model.Survey survey)
        {
            survey.CalculateStatus();

            return survey.Status.State == SurveyState.Complete;
        }
    }
}
