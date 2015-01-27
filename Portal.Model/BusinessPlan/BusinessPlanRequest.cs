using System;

namespace Portal.Model
{
    public class BusinessPlanRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessConsultant { get; set; }
        public bool IncludeTerminated { get; set; }
        public bool ExcludeNoData { get; set; }
        public int Year { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}