using Portal.Model;
using System.Linq;

namespace Portal.Domain.Survey.StatusCalculators
{
    internal class WeightedAnswerCalculator : IStatusCalculator
    {
        public SurveyProgressType ProgressType
        {
            get { return SurveyProgressType.Score; }
        }

        public SurveyStatus Calculate(Model.Survey survey)
        {
            var status = new SurveyStatus()
            {
                Score = GetScore(survey),
                PercentComplete = GetPercentComplete(survey),
                State = GetState(survey)
            };

            status.Pages.AddRange(survey.Pages.Where(page => !page.IsSummary).Select(page => new SurveyPageStatus()
            {
                SurveyPageID = page.SurveyPageID,
                Name = page.PageName,
                IsVisible = page.IsVisible,
                Score = GetScore(page),
                PercentComplete = GetPercentComplete(page),
                State = GetState(page)
            }));

            return status;
        }

        private static SurveyState GetState(Model.Survey survey)
        {
            var state = SurveyState.NotStarted;
            var percentComplete = GetPercentComplete(survey);

            if (percentComplete > 0 && percentComplete < 1M)
                state = SurveyState.InProgress;

            if (percentComplete == 1M)
                state = SurveyState.Complete;

            return state;
        }

        private static SurveyState GetState(SurveyPage page)
        {
            var state = SurveyState.NotStarted;
            var percentComplete = GetPercentComplete(page);

            if (percentComplete > 0 && percentComplete < 1M)
                state = SurveyState.InProgress;

            if (percentComplete == 1M)
                state = SurveyState.Complete;

            return state;
        }

        private static decimal GetPercentComplete(Model.Survey survey)
        {
            var allQuestions = (from p in survey.Pages
                                from q in p.Questions
                                where q.IsVisible
                                select q).ToList();
            decimal answeredQuestionCount = allQuestions.Count(q => q.HasAnswer);

            return answeredQuestionCount / allQuestions.Count;
        }

        private static decimal GetPercentComplete(SurveyPage page)
        {
            var allQuestions = (from q in page.Questions
                                where q.IsVisible
                                select q).ToList();
            decimal answeredQuestionCount = allQuestions.Count(q => q.HasAnswer);

            return answeredQuestionCount / allQuestions.Count;
        }

        private static decimal GetScore(Model.Survey survey)
        {
            decimal totalMaxScore = 0;
            decimal totalScore = 0;

            foreach (var page in survey.Pages.Where(p => p.IsVisible && !p.IsSummary))
            {
                foreach (var question in page.RequiredQuestions)
                {
                    var maxPoints = question.PossibleAnswers.Max(a => a.AnswerWeight.HasValue ? a.AnswerWeight.Value : 0);
                    var selectedAnswer = question.PossibleAnswers.FirstOrDefault(a => a.IsSelected);
                    var points = 0M;

                    if (selectedAnswer != null && selectedAnswer.AnswerWeight.HasValue)
                    {
                        points = selectedAnswer.AnswerWeight.Value;
                    }

                    totalMaxScore += maxPoints;
                    totalScore += points;
                }
            }

            if (totalMaxScore > 0)
                return totalScore / totalMaxScore;

            return 0;
        }

        private static decimal GetScore(SurveyPage page)
        {
            decimal totalMaxScore = 0;
            decimal totalScore = 0;

            foreach (var question in page.RequiredQuestions)
            {
                var maxPoints = question.PossibleAnswers.Max(a => a.AnswerWeight.HasValue ? a.AnswerWeight.Value : 0);
                var selectedAnswer = question.PossibleAnswers.FirstOrDefault(a => a.IsSelected);
                var points = 0M;

                if (selectedAnswer != null && selectedAnswer.AnswerWeight.HasValue)
                {
                    points = selectedAnswer.AnswerWeight.Value;
                }

                totalMaxScore += maxPoints;
                totalScore += points;
            }

            if (totalMaxScore > 0)
                return totalScore / totalMaxScore;

            return 0;
        }
    }
}
