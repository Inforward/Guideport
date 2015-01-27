using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class SurveyPageScoreRange : Auditable
    {
        public int ScoreRangeID { get; set; }

        [ForeignKey("SurveyPage")]
        public int SurveyPageID { get; set; }
        public decimal MinScore { get; set; }
        public decimal MaxScore { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public SurveyPage SurveyPage { get; set; }

        [NotMapped]
        public int MinDisplayScore { get { return decimal.ToInt32(Math.Floor(MinScore * 100)); } }

        [NotMapped]
        public int MaxDisplayScore { get { return decimal.ToInt32(Math.Floor(MaxScore * 100)); } }

        [NotMapped]
        public string RangeText
        {
            get { return string.Format("{0} - {1}", MinDisplayScore, MaxDisplayScore); }
        }
    }
}
