using System.Collections.Generic;

namespace Portal.Web.Models
{
    public class SurveyPageViewModel
    {
        public SurveyPageViewModel()
        {
            Questions = new List<SurveyQuestionViewModel>();
            SuggestedContents = new List<SurveyPageSuggestedContentViewModel>();
        }

        public int Id { get; set; }
        public int PageOrder { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public bool IsSelected { get; set; }
        public bool IsSummary { get; set; }
        public int Score { get; set; }
        public string ScoreDescription { get; set; }
        public List<SurveyQuestionViewModel> Questions { get; set; }
        public List<SurveyPageSuggestedContentViewModel> SuggestedContents { get; set; }
    }

    public class SurveyPageSuggestedContentViewModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class SurveyPageStatusViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal PercentComplete { get; set; }
        public decimal Score { get; set; }
    }
}