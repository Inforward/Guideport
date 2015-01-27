using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Portal.Model
{
    public partial class Survey : Auditable
    {
        public Survey()
        {
            Pages = new List<SurveyPage>();
            SurveyResponses = new List<SurveyResponse>();
        }

        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }
        public string RulesetValidationName { get; set; }
        public string RulesetCoreName { get; set; }
        public string CompleteMessage { get; set; }
        public string CompleteRedirectUrl { get; set; }
        public string StatusCalculator { get; set; }
        public string NotificationType { get; set; }
        public string StatusLabel { get; set; }
        public int? SuggestedContentSiteID { get; set; }
        public bool IsAutoCompleteEnabled { get; set; }
        public bool IsReviewVisible { get; set; }
        public bool IsStatusVisible { get; set; }
        public bool IsActive { get; set; }
        public string ReviewTabText { get; set; }
        public ICollection<SurveyPage> Pages { get; set; }

        [IgnoreDataMember]
        public ICollection<SurveyResponse> SurveyResponses { get; set; }

        [NotMapped]
        public SurveyStatus Status { get; set; }

        [NotMapped]
        public SurveyResponse CurrentResponse { get; set; }

        [NotMapped]
        public int CurrentPage { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public List<SurveyQuestion> Questions
        {
            get
            {
                var questions = new List<SurveyQuestion>();

                foreach (var page in Pages)
                {
                    questions.AddRange(page.Questions);
                }

                return questions;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public SurveyNotificationType SurveyNotificationType
        {
            get
            {
                var type = SurveyNotificationType.None;

                if (!string.IsNullOrEmpty(NotificationType))
                {
                    Enum.TryParse(NotificationType, true, out type);
                }

                return type;
            }
        }
    }

    public enum SurveyState
    {
        NotStarted,
        InProgress,
        Complete
    }

    public enum SurveyProgressType
    {
        /// <summary>
        /// Survey's Progress is indicated by it's state (Not Started, In Progress, etc.)
        /// </summary>
        State,
        /// <summary>
        /// Survey's Progress is indicated by an overall score
        /// </summary>
        Score
    }

    public enum SurveyNotificationType
    {
        None,
        BusinessConsultantEmail
    }

    public static class SurveyExtensions
    {
        public static SurveyPage GetPage(this Survey survey, string name)
        {
            return survey.Pages == null ? null : survey.Pages.FirstOrDefault(p => p.PageName == name);
        }

        public static SurveyQuestion GetQuestion(this Survey survey, string pageName, string questionName)
        {
            return survey.GetPage(pageName).GetQuestion(questionName);
        }
    }
}
