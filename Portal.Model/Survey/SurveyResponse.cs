using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SurveyResponse : Auditable
    {
        public SurveyResponse()
        {
            Answers = new List<SurveyResponseAnswer>();
            SurveyResponseHistories = new List<SurveyResponseHistory>();
        }

        public int SurveyResponseID { get; set; }
        public int SurveyID { get; set; }
        public int UserID { get; set; }
        public int SelectedSurveyPageID { get; set; }
        public decimal? CurrentScore { get; set; }
        public decimal? PercentComplete { get; set; }
        public int? CompleteUserID { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? CompleteDateUtc { get; set; }
        public ICollection<SurveyResponseAnswer> Answers { get; set; }

        [IgnoreDataMember]
        public Survey Survey { get; set; }

        [IgnoreDataMember]
        public ICollection<SurveyResponseHistory> SurveyResponseHistories { get; set; }

        [IgnoreDataMember]
        public User User { get; set; }

        [IgnoreDataMember]
        public SurveyPage SurveyPage { get; set; }

        [NotMapped]
        public string SurveyName { get; set; }

        [NotMapped]
        public bool ApplyAsComplete { get; set; }
    }

    public static class SurveyResponseExtensions
    {
        public static string GetAnswer(this SurveyResponse response, string questionName)
        {
            if (response != null && response.Answers != null)
            {
                var answer = response.Answers.FirstOrDefault(a => a.SurveyQuestion != null && a.SurveyQuestion.QuestionName == questionName);

                if (answer != null)
                    return answer.Answer;
            }

            return null;
        }
    }
}
