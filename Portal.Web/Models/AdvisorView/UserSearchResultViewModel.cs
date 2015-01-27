namespace Portal.Web.Models.AdvisorView
{
    public class UserSearchResultViewModel
    {
        public string DisplayName { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PrimaryPhone { get; set; }
        public string BusinessConsultantDisplayName { get; set; }
        public decimal? BusinessAssessmentScore { get; set; }
        public string AffiliateName { get; set; }
    }
}