using System.Linq;
using Portal.Model;

namespace Portal.Domain.Survey.StatusCalculators
{
    internal class DefaultCalculator : IStatusCalculator
    {
        public SurveyProgressType ProgressType
        {
            get { return SurveyProgressType.State; }
        }

        public SurveyStatus Calculate(Model.Survey survey)
        {
            var status = new SurveyStatus()
                {
                    PercentComplete = GetPercentComplete(survey)
                };

            status.Pages.AddRange(survey.Pages.Where(page => !page.IsSummary).Select(page => new SurveyPageStatus()
                {
                    SurveyPageID = page.SurveyPageID,
                    Name = page.PageName,
                    IsVisible = page.IsVisible,
                    PercentComplete = GetPercentComplete(page),
                    State = GetState(page)
                }));

            var visiblePages = status.Pages.Where(p => p.IsVisible).ToList();

            if (visiblePages.Count == visiblePages.Count(p => p.State == SurveyState.Complete))
            {
                status.State = SurveyState.Complete;
            }
            else if (visiblePages.Any(p => p.State == SurveyState.Complete || p.State == SurveyState.InProgress))
            {
                status.State = SurveyState.InProgress;
            }
            else
            {
                status.State = SurveyState.NotStarted;
            }

            return status;
        }

        private static decimal GetPercentComplete(Model.Survey survey)
        {
            var totalQuestions = survey.Pages.Where(p => p.IsVisible).Sum(page => page.RequiredQuestions.Count);
            var completedQuestions = survey.Pages.Where(p => p.IsVisible).Sum(page => page.RequiredQuestions.Count(q => q.HasAnswer));

            return totalQuestions > 0 ? (completedQuestions / (decimal)totalQuestions) : 0M;
        }

        private static SurveyState GetState(SurveyPage page)
        {
            var percentComplete = GetPercentComplete(page);

            if (percentComplete == 0)
                return SurveyState.NotStarted;

            return percentComplete == 1M ? SurveyState.Complete : SurveyState.InProgress;
        }

        private static decimal GetPercentComplete(SurveyPage page)
        {
            var totalQuestionCount = page.RequiredQuestions.Count;
            var completedQuestionCount = page.RequiredQuestions.Count(q => q.HasAnswer);

            return totalQuestionCount > 0 ? (completedQuestionCount / (decimal)totalQuestionCount) : 0M;
        }
    }
}
