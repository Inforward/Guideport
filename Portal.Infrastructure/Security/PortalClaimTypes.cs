namespace Portal.Infrastructure.Security
{
    public class PortalClaimTypes
    {
        public const string ProfileId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/profileid";
        public const string SourceDomain = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sourcedomain";
        public const string ProfileRoles = "http://schemas.xmlsoap.org/ws/2008/06/identity/claims/role";
        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string RepNumber = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/repnumber";
        public const string UPN = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        public const string LoggedInUser = "http://firstallied.com/loggedinuser";
        public const string RepNumberOptOut = "http://firstallied.com/repnumberoptout";
        public const string PortalRoles = "http://firstallied.com/claims/portal.applicationroles";
        public const string IStationRoles = "http://firstallied.com/ws/2013/01/identity/claims/istationroles";
    }
}