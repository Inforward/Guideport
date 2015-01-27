using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SurveyResponseHistory
    {
        public int SurveyResponseHistoryID { get; set; }
        public int SurveyResponseID { get; set; }
        public int SurveyPageID { get; set; }
        public DateTime ResponseDate { get; set; }
        public decimal? Score { get; set; }
        public bool IsLatestScore { get; set; }
        public decimal? PercentComplete { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime ModifyDateUtc { get; set; }

        [IgnoreDataMember]
        public SurveyPage SurveyPage { get; set; }

        [IgnoreDataMember]
        public SurveyResponse SurveyResponse { get; set; }

        [NotMapped]
        public decimal? DisplayScore
        {
            get
            {
                if (Score == null)
                    return null;

                return Math.Floor(Score.Value * 100) / 100;
            }
        }
    }
}
