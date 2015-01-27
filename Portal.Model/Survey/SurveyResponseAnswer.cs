using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SurveyResponseAnswer : Auditable
    {
        public int SurveyResponseAnswerID { get; set; }

        [ForeignKey("SurveyResponse")]
        public int SurveyResponseID { get; set; }
        public int SurveyQuestionID { get; set; }
        public string Answer { get; set; }

        [NotMapped]
        public override int ModifyUserID { get; set; }

        [NotMapped]
        public override DateTime ModifyDate { get; set; }

        [NotMapped]
        public override DateTime ModifyDateUtc { get; set; }

        [IgnoreDataMember]
        public SurveyQuestion SurveyQuestion { get; set; }

        [IgnoreDataMember]
        public SurveyResponse SurveyResponse { get; set; }

        [NotMapped]
        public int SurveyPageID { get; set; }

        public override string ToString()
        {
            return string.Format("ResponseID: {0}, QuestionID: {1} - {2}", SurveyResponseID, SurveyQuestionID, Answer);
        }
    }
}
