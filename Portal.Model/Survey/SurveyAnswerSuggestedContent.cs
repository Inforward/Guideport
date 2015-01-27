using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SurveyAnswerSuggestedContent : Auditable
    {
        public int SurveyAnswerSuggestedContentID { get; set; }

        [ForeignKey("SurveyAnswer")]
        public int SurveyQuestionAnswerID { get; set; }
        public int SiteContentID { get; set; }

        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public string Url { get; set; }

        [IgnoreDataMember]
        public SurveyAnswer SurveyAnswer { get; set; }

        [NotMapped]
        public override int ModifyUserID { get; set; }

        [NotMapped]
        public override DateTime ModifyDate { get; set; }

        [NotMapped]
        public override DateTime ModifyDateUtc { get; set; }
    }
}
