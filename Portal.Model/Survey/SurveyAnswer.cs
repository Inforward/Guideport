using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SurveyAnswer
    {
        public SurveyAnswer()
        {
            SuggestedContents = new List<SurveyAnswerSuggestedContent>();
        }

        public int SurveyQuestionAnswerID { get; set; }
        public int SurveyQuestionID { get; set; }
        public string AnswerText { get; set; }
        public string ReviewAnswerText { get; set; }
        public int SortOrder { get; set; }
        public decimal? AnswerWeight { get; set; }
        public ICollection<SurveyAnswerSuggestedContent> SuggestedContents { get; set; }

        [IgnoreDataMember]
        public SurveyQuestion SurveyQuestion { get; set; }

        [NotMapped]
        public int SurveyPageID { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

        [NotMapped]
        public bool IsTrigger { get; set; }

        public override string ToString()
        {
            return string.Format("QuestionID: {0} - {1}", SurveyQuestionID, AnswerText);
        }
    }
}
