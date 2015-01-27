using System.Collections.Generic;

namespace Portal.Model.Planning
{
    public class EnrollmentStatus : SurveyStatus
    {
        internal const string ContinuityPlanningName = "Continuity Planning";
        internal const string SuccessionPlanningName = "Succession Planning";
        internal const string BusinessAcquisitionName = "Business Acquisition";
        internal const string BusinessAcquisitionFundingName = "Qualified Buyer Program";

        public EnrollmentInterest ContinuityPlanning
        {
            get { return EnrollmentInterests.Find(i => i.Name == ContinuityPlanningName); }
        }
        public EnrollmentInterest SuccessionPlanning
        {
            get { return EnrollmentInterests.Find(i => i.Name == SuccessionPlanningName); }
        }
        public EnrollmentInterest BusinessAcquisition
        {
            get { return EnrollmentInterests.Find(i => i.Name == BusinessAcquisitionName); }
        }
        public EnrollmentInterest BusinessAcquisitionFunding
        {
            get { return EnrollmentInterests.Find(i => i.Name == BusinessAcquisitionFundingName); }
        }

        public List<EnrollmentInterest> EnrollmentInterests { get; set; }

        public EnrollmentStatus()
        {
            EnrollmentInterests = new List<EnrollmentInterest>()
            {
                new EnrollmentInterest() { Name = ContinuityPlanningName },
                new EnrollmentInterest() { Name = SuccessionPlanningName },
                new EnrollmentInterest() { Name = BusinessAcquisitionName },
                new EnrollmentInterest() { Name = BusinessAcquisitionFundingName }
            };
        }
    }

    public class EnrollmentInterest
    {
        public string Name { get; set; }
        public bool HasAnswer { get; set; }
        public bool Interested { get; set; }
        public string Message { get; set; }
    }
}
