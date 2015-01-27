using System;
using System.Collections.Generic;

namespace Portal.Web.Models
{
    public class SurveyViewModel
    {
        public int Id { get; set; }
        public int ActivePageId { get; set; }
        public string Name { get; set; }
        public string LastModifiedDate { get; set; }
        public string Introduction { get; set; }
        public string CompleteMessage { get; set; }
        public string Status { get; set; }
        public string StatusTypeLabel { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsStatusVisible { get; set; }
        public string CompleteRedirectUrl { get; set; }
        public string ReviewTabText { get; set; }

        public List<SurveyPageViewModel> Pages { get; set; }

        public SurveyViewModel()
        {
            Pages = new List<SurveyPageViewModel>();
        }
    }

    public class SurveyStatusViewModel
    {
        public string SurveyName { get; set; }
        public string Status { get; set; }
        public bool Enabled { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string InstructionalText { get; set; }
        public decimal PercentComplete { get; set; }
        public decimal Score { get; set; }
        public List<SurveyPageStatusViewModel> Pages { get; set; }

        public SurveyStatusViewModel()
        {
            Pages = new List<SurveyPageStatusViewModel>();
        }
    }
}