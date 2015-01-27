using Portal.Model;

namespace Portal.Domain.Survey.StatusCalculators
{
    internal interface IStatusCalculator
    {
        SurveyProgressType ProgressType { get; }

        SurveyStatus Calculate(Model.Survey survey);
    }
}
