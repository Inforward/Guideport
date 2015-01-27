using System;
using System.Collections.Generic;

namespace Portal.Model
{
    public class SurveyStatus
    {
        public decimal PercentComplete { get; set; }
        public decimal Score { get; set; }
        public SurveyState State { get; set; }
        public SurveyProgressType ProgressType { get; set; }
        public List<SurveyPageStatus> Pages { get; set; }

        public int DisplayScore { get { return decimal.ToInt32(Math.Floor(Score * 100)); } }

        public SurveyStatus()
        {
            Pages = new List<SurveyPageStatus>();
            ProgressType = SurveyProgressType.State;
            State = SurveyState.NotStarted;
        }
    }

    public class SurveyPageStatus
    {
        public int SurveyPageID { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public decimal PercentComplete { get; set; }
        public decimal Score { get; set; }
        public SurveyState State { get; set; }
        public int DisplayScore { get { return decimal.ToInt32(Math.Floor(Score * 100)); } }
    }
}
