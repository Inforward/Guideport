using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SurveyPage
    {
        public SurveyPage()
        {
            Questions = new List<SurveyQuestion>();
            SurveyResponses = new List<SurveyResponse>();
            SurveyResponseHistories = new List<SurveyResponseHistory>();
            ScoreRanges = new List<SurveyPageScoreRange>();
        }

        public int SurveyPageID { get; set; }
        public int SurveyID { get; set; }
        public string PageName { get; set; }
        public int SortOrder { get; set; }
        public bool IsVisible { get; set; }
        public string Tooltip { get; set; }
        public ICollection<SurveyQuestion> Questions { get; set; }
        public ICollection<SurveyPageScoreRange> ScoreRanges { get; set; }

        [IgnoreDataMember]
        public Survey Survey { get; set; }

        [IgnoreDataMember]
        public ICollection<SurveyResponse> SurveyResponses { get; set; }

        [IgnoreDataMember]
        public ICollection<SurveyResponseHistory> SurveyResponseHistories { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

        [NotMapped]
        public bool IsSummary { get; set; }

        [NotMapped]
        public List<SurveyAnswerSuggestedContent> SuggestedContents
        {
            get
            {
                var list = new List<SurveyAnswerSuggestedContent>();

                foreach (var question in Questions.Where(q => q.IsVisible))
                {
                    var answer = question.PossibleAnswers.FirstOrDefault(a => a.IsSelected);

                    if(answer != null && answer.SuggestedContents.Any())
                        list.AddRange(answer.SuggestedContents);
                }

                return list;
            }
            set { }
        }
        
        [NotMapped]
        [IgnoreDataMember]
        public List<SurveyQuestion> RequiredQuestions
        {
            get
            {
                return Questions.Where(q => q.IsVisible && q.IsRequired && q.AnswerType != AnswerType.None && !q.IsDisabled).ToList();
            }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", PageName);
        }
    }

    public static class SurveyPageExtensions
    {
        public static SurveyQuestion GetQuestionByID(this SurveyPage page, int id)
        {
            if (page.Questions == null) return null;

            return page.Questions.FirstOrDefault(q => q.SurveyQuestionID == id);
        }

        public static SurveyQuestion GetQuestion(this SurveyPage page, string name)
        {
            if (page.Questions == null) return null;

            return page.Questions.FirstOrDefault(q => q.QuestionName == name);
        }

        public static SurveyQuestion GetQuestionByText(this SurveyPage page, string question)
        {
            if (page.Questions == null) return null;

            return page.Questions.FirstOrDefault(q => q.QuestionText == question);
        }
    }
}
