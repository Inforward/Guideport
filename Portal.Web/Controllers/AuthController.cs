using System.Text.RegularExpressions;
using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Configuration;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;
using ComponentSpace.SAML2.Schemas;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Infrastructure.Security;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Common.Helpers;
using Portal.Web.Filters;
using Portal.Web.Helpers;
using Portal.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace Portal.Web.Controllers
{
    [AllowAnonymous]
    [HandleExceptions]
    public class AuthController : Controller
    {
        #region Private Members

        private readonly IUserService _userService;
        private readonly IAffiliateService _affiliateService;
        private readonly ICmsService _cmsService;
        private readonly ILogger _logger;
        private const string DefaultTargetUrl = "~/";
        private const string TestTargetUrl = "~/Saml/Test";
        private const string SamlAttributesSessionKey = "Portal.Saml.Attributes";
        private const string IdpPartnerSessionKey = "Portal.Saml.IdpPartner";
        private const string SamlErrorMessagesSessionKey = "Portal.Saml.ErrorMessages";

        #endregion

        #region Constructor

        public AuthController(IUserService userService, IAffiliateService affiliateService, ICmsService cmsService, ILogger logger)
        {
            _cmsService = cmsService;
            _userService = userService;
            _affiliateService = affiliateService;
            _logger = logger;

            SamlHelper.CheckConfiguration();
        }

        #endregion

        #region Views

        public ActionResult Login()
        {
            return View(GetAffiliates());
        }

        public ActionResult Test()
        {
            var viewModel = null as SamlAttributesViewModel;
            var claims = SessionHelper.Get<SAMLAttribute[]>(SamlAttributesSessionKey);
            var errorMessages = SessionHelper.Get<List<string>>(SamlErrorMessagesSessionKey);

            if (claims != null || errorMessages != null)
            {
                viewModel = new SamlAttributesViewModel() { Attributes = claims, ErrorMessages = errorMessages };
                SessionHelper.Set(SamlErrorMessagesSessionKey, null);
            }

            return View(viewModel);
        }

        #endregion

        #region Sso Methods

        public ActionResult SingleSignOn(string idp, string sourceDomain, string returnUrl)
        {
            // HACK: for idp affiliate testing, use the returnUrl to auto-select affiliate on idp login
            if (SAMLConfiguration.Current.IdentityProviderConfiguration != null
                && idp == SAMLConfiguration.Current.IdentityProviderConfiguration.Name)
            {
                returnUrl = sourceDomain;
            }

            // idp maps to the name attribute of the PartnerIdentityProvider in saml.config
            SAMLServiceProvider.InitiateSSO(Response, returnUrl, idp);

            return new EmptyResult();
        }

        public ActionResult Receive()
        {
            string idpPartner = string.Empty, targetUrl = string.Empty, 
                   sourceDomain = string.Empty, nameId = string.Empty, profileId = string.Empty;
            var affiliateId = 0;
            var loginFailurePage = Settings.Get("Saml.LoginFailurePage", string.Empty);
            var attributeList = new List<SAMLAttribute>();
            var errorMessages = new List<string>();

            try
            {
                SAMLAttribute[] attributes;
                bool isInResponseTo;

                // Receive SAML Assertion
                SAMLServiceProvider.ReceiveSSO(Request, out isInResponseTo, out idpPartner, out nameId, out attributes, out targetUrl);

                attributeList = attributes.Parse(nameId, idpPartner, GetAffiliates(), out profileId, out affiliateId, out sourceDomain);

                if (string.IsNullOrEmpty(profileId))
                    throw new Exception("Unable to retrieve profileId from SAML Response.");

                if (affiliateId <= 0)
                    throw new Exception("Unable to retrieve affiliateId from SAML Response.");
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(loginFailurePage))
                {
                    _logger.Log(ex);
                    errorMessages.Add(ex.Message);
                    targetUrl = loginFailurePage;
                }
                else
                {
                    throw;
                }                
            }

            // Default to query string if there was no value in the relaystate form parameter
            if (string.IsNullOrEmpty(targetUrl))
                targetUrl = Request["TargetUrl"];

            // Log In the User
            if (!string.IsNullOrEmpty(profileId) && affiliateId > 0)
            {
                var user = _userService.GetUser(new UserRequest()
                            {
                                ProfileID = profileId, 
                                AffiliateID = affiliateId, 
                                IncludeApplicationRoles = true,
                                IncludeAffiliates = true,
                                IncludeAffiliateFeatures = true
                            });

                if (user != null)
                {
                    var userData = user.ToCookieUserData();
                    var siteMap = _cmsService.GetSiteMap((int)Sites.Pentameter, user.ProfileTypeID, user.AffiliateID);

                    userData.SourceDomain = sourceDomain;
                    userData.IdpPartner = idpPartner;
                    userData.Connect2ClientsEnabled = user.Affiliate.HasFeature(Features.Connect2Clients);
                    userData.Connect2ClientsMessage = user.Affiliate.GetFeatureSetting(Features.Connect2Clients, FeatureSettings.DisabledMessage);
                    userData.SuccessionPlanningEnabled = siteMap != null && siteMap.Items.AnyRecursive(p => p.Title == "Succession Planning");

                    Response.SetAuthCookie(user.ProfileID, false, userData);
                }
                else
                {
                    var message = string.Format("Could not find user with ProfileId: {0}, AffiliateId: {1}, NameId: {2}, SourceDomain: {3}, IdpPartner: {4}.", profileId, affiliateId, nameId, sourceDomain, idpPartner);

                    if (!string.IsNullOrEmpty(loginFailurePage))
                    {
                        _logger.Log(message, EventTypes.Warning);
                        errorMessages.Add(message);
                        targetUrl = loginFailurePage;
                    }
                    else
                    {
                        throw new Exception(message);
                    }
                }
            }

            SessionHelper.Set(SamlAttributesSessionKey, attributeList.ToArray());
            SessionHelper.Set(SamlErrorMessagesSessionKey, errorMessages);
            SessionHelper.Set(IdpPartnerSessionKey, idpPartner);

            return RedirectToLocal(targetUrl);
        }

        public ActionResult Logout()
        {
            bool isRequest;
            string logoutReason, partnerSp;

            SAMLServiceProvider.ReceiveSLO(Request, out isRequest, out logoutReason, out partnerSp);

            if (isRequest)
            {
                FormsAuthentication.SignOut();

                SAMLServiceProvider.SendSLO(Response, null);
            }
            else
            {
                return RedirectToLocal(FormsAuthentication.LoginUrl);
            }

            return new EmptyResult();
        }

        public ActionResult UploadSaml(FormCollection formCollection)
        {
            var attributes = new List<SAMLAttribute>();
            var errorMessages = new List<string>();
            var xmlDoc = null as XmlDocument;

            SessionHelper.Set(SamlAttributesSessionKey, null);
            SessionHelper.Set(SamlErrorMessagesSessionKey, null);

            if (Request != null)
            {
                var file = Request.Files["SamlFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    try
                    {
                        xmlDoc = new XmlDocument();

                        xmlDoc.Load(file.InputStream);

                        var samlValidator = new SAMLValidator();
                        var isValid = samlValidator.Validate(xmlDoc);

                        if (!isValid)
                        {
                            errorMessages.AddRange(samlValidator.Warnings.Select(warning => string.Format("Warning:, {0}", warning.Message)));

                            errorMessages.AddRange(samlValidator.Errors.Select(error => error.Message));
                        }

                        if (SAMLMessageSignature.IsSigned(xmlDoc.DocumentElement))
                        {
                            if (!SAMLMessageSignature.Verify(xmlDoc.DocumentElement))
                            {
                                errorMessages.Add(string.Format("Failed to verify SAML response signature. [{0}]", xmlDoc.OuterXml));
                            }
                        }

                        var samlResponse = new SAMLResponse(xmlDoc.DocumentElement);
                        var assertions = samlResponse.GetAssertions();
                        var encryptedAssertions = samlResponse.GetEncryptedAssertions();
                        var signedAssertions = samlResponse.GetSignedAssertions();
                        var x509CertificateFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, SAMLConfiguration.Current.ServiceProviderConfiguration.CertificateFile);
                        var x509Certificate = new X509Certificate2(x509CertificateFilePath, SAMLConfiguration.Current.ServiceProviderConfiguration.CertificatePassword);

                        encryptedAssertions.ForEach(a =>
                        {
                            var decryptedAssertion = a.DecryptToXml(x509Certificate);

                            assertions.Add(new SAMLAssertion(decryptedAssertion));
                        });

                        signedAssertions.ForEach(a =>
                        {
                            if (SAMLAssertionSignature.IsSigned(a))
                            {
                                if (!SAMLAssertionSignature.Verify(a))
                                    errorMessages.Add(string.Format("Failed to verify SAML assertion signature. [{0}]", a.OuterXml));
                            }
                            
                            assertions.Add(new SAMLAssertion(a));
                        });

                        assertions.ForEach(a =>
                        {
                            var nameId = a.GetNameID();
                            if (!string.IsNullOrEmpty(nameId))
                                attributes.Add("NameId", nameId);

                            a.GetAttributeStatements().ForEach(
                                s => attributes.AddRange(from object attribute in s.Attributes
                                                         select attribute as SAMLAttribute));
                        });
                    }
                    catch (Exception ex)
                    {
                        var saml = xmlDoc != null ? xmlDoc.OuterXml : string.Empty;

                        _logger.Log(ex);
                        errorMessages.Add(string.Format("{0} [{1}]", ex.Message, saml));
                    }
                }
            }

            SessionHelper.Set(SamlAttributesSessionKey, attributes.ToArray());
            SessionHelper.Set(SamlErrorMessagesSessionKey, errorMessages);

            return Redirect(TestTargetUrl);
        }

        #endregion

        #region Local Methods

        [Route("signout", Name = "Signout")]
        public ActionResult Signout()
        {
            var userData = Request.GetUserData<CookieUserData>();

            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.RemoveCookie("ASP.NET_SessionId");
            Response.RemoveCookie(CookieHelper.UserDataCookieName);
            Response.RemoveCookie(CookieHelper.AssistedUserCookieName);

            if (!string.IsNullOrEmpty(userData.IdpPartner))
            {
                var idpPartnerConfig = SAMLConfiguration.Current.GetPartnerIdentityProvider(userData.IdpPartner);
                if (idpPartnerConfig != null && !string.IsNullOrEmpty(idpPartnerConfig.SingleLogoutServiceUrl))
                {
                    try
                    {
                        SAMLServiceProvider.InitiateSLO(Response, null, userData.IdpPartner);
                        return new EmptyResult();
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(ex);
                    }

                }
            }

            return new RedirectResult(FormsAuthentication.LoginUrl);
        }

        #endregion

        #region Idp Methods

        private const string PartnerSpSessionKey = "partnerSp";
        private const string SourceDomainSessionKey = "idp.SourceDomain";
        private const string UserSessionKey = "idp.User";
        private const string AuthnRequestIdSessionKey = "idp.AuthnRequestId";

        public ActionResult IdpLogin(string returnUrl, string sourceDomain)
        {
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "~/Auth/IdpLogin" : returnUrl;
            
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.SourceDomain = sourceDomain;
            ViewBag.Affiliates = GetAffiliates(true);
            ViewBag.User = SessionHelper.Get<User>(UserSessionKey);

            ViewBag.Users = _userService.GetUsers(new UserRequest{ AffiliateID = 1, IncludeApplicationRoles = true })
                                        .Users.Where(u => u.ApplicationRoles.Any()).ToList();

            return View();
        }

        public ActionResult IdpSingleSignOn(string partnerSp)
        {
            var user = SessionHelper.Get<User>(UserSessionKey);
            var sourceDomain = SessionHelper.Get<string>(SourceDomainSessionKey);

            SendSso(user, sourceDomain);

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IdpLogin(IdpLoginModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var affiliateId = (from a in GetAffiliates(true)
                               where a.SamlSourceDomain.Equals(model.SourceDomain, StringComparison.InvariantCultureIgnoreCase)
                               select a.AffiliateID).FirstOrDefault();

            var user = _userService.GetUser(new UserRequest()
            {
                ProfileID = model.ProfileID,
                AffiliateID = affiliateId,
                IncludeApplicationRoles = true
            });

            if (ModelState.IsValid && user != null)
            {
                SessionHelper.Set(UserSessionKey, user);
                SessionHelper.Set(SourceDomainSessionKey, model.SourceDomain);

                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Invalid credentials.");
            ViewBag.Affiliates = GetAffiliates();

            return View(model);
        }

        public ActionResult IdpLogout()
        {
            SessionHelper.Set(UserSessionKey, null);
            SessionHelper.Set(SourceDomainSessionKey, null);

            return RedirectToAction("IdpLogin");
        }

        public ActionResult IdpSsoService()
        {
            var user = SessionHelper.Get<User>(UserSessionKey);
            var sourceDomain = SessionHelper.Get(SourceDomainSessionKey, string.Empty);

            if (user == null)
            {
                XmlElement authnRequestXml;
                string relayState;

                IdentityProvider.ReceiveAuthnRequestByHTTPPost(Request, out authnRequestXml, out relayState);
                // IdentityProvider.ReceiveAuthnRequestByHTTPRedirect(Request, out authnRequestXml, out relayState, out signed, null);
                // SAMLIdentityProvider.ReceiveSSO(Request, out partnerSp);

                var authnRequest = new AuthnRequest(authnRequestXml);

                SessionHelper.Set(AuthnRequestIdSessionKey, authnRequest.ID);
                SessionHelper.Set(PartnerSpSessionKey, authnRequest.Issuer.NameIdentifier);

                return RedirectToAction("IdpLogin", new { ReturnUrl = "~/Auth/IdpSsoService", SourceDomain = relayState });
            }

            SendSso(user, sourceDomain);

            return new EmptyResult();
        }

        public ActionResult IdpSloService()
        {
            bool isRequest, hasCompleted;
            string logoutReason, partnerSp;

            SAMLIdentityProvider.ReceiveSLO(Request, Response, out isRequest, out hasCompleted, out logoutReason, out partnerSp);

            if (isRequest)
            {
                SessionHelper.Set(SourceDomainSessionKey, null);
                SessionHelper.Set(UserSessionKey, null);

                SAMLIdentityProvider.SendSLO(Response, null);
            }
            else
            {
                if (hasCompleted)
                {
                    Response.Redirect("~/");
                }
            }

            return new EmptyResult();
        }

        #region Idp Models
        public class IdpLoginModel
        {
            public IdpLoginModel()
            {
            }

            [Required]
            [Display(Name = "Available User")]
            public string ProfileID { get; set; }

            [Display(Name = "Affiliate")]
            public string SourceDomain { get; set; }
        }
        #endregion

        #endregion

        #region Private Methods

        private ActionResult RedirectToLocal(string targetUrl)
        {
            return Redirect(Url.IsLocalUrl(targetUrl) ? targetUrl : DefaultTargetUrl);
        }

        private List<Affiliate> GetAffiliates(bool includeAll = false)
        {
            var affiliates = _affiliateService.GetAffiliates(new AffiliateRequest{ IncludeSamlConfiguration = true, UseCache = false, IncludeLogos = true });

            if (!includeAll)
                affiliates = affiliates.Where(a => a.HasFeature(Features.LoginPage));

            return affiliates.ToList();
        }

        private static XmlElement CreateSamlResponse(string assertionConsumerServiceUrl, List<SAMLAttribute> attributes, string requestId = null, bool signAssertion = false, bool signResponse = false, bool encryptAssertion = false)
        {
            var samlResponse = new SAMLResponse { Destination = assertionConsumerServiceUrl };
            var issuer = new Issuer(SAMLConfiguration.Current.IdentityProviderConfiguration.Name);
            var issuerX509CertificateFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, SAMLConfiguration.Current.IdentityProviderConfiguration.CertificateFile);
            var issuerX509Certificate = new X509Certificate2(issuerX509CertificateFilePath, SAMLConfiguration.Current.IdentityProviderConfiguration.CertificatePassword);
            var partner = SessionHelper.Get<string>(PartnerSpSessionKey) ?? SAMLConfiguration.Current.ServiceProviderConfiguration.Name;
            var partnerConfig = SAMLConfiguration.Current.PartnerServiceProviderConfigurations[partner];
            var partnerX509CertificateFilePath = string.Empty;
            var partnerX509Certificate = null as X509Certificate2;

            if (partnerConfig != null)
            {
                partnerX509CertificateFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, partnerConfig.CertificateFile);
                partnerX509Certificate = new X509Certificate2(partnerX509CertificateFilePath);
                signAssertion = partnerConfig.SignAssertion;
                signResponse = partnerConfig.SignSAMLResponse;
                encryptAssertion = partnerConfig.EncryptAssertion;
            }

            samlResponse.Issuer = issuer;
            samlResponse.Status = new Status(SAMLIdentifiers.PrimaryStatusCodes.Success, null);
            samlResponse.IssueInstant = DateTime.Now;
            samlResponse.InResponseTo = requestId;

            var samlAssertion = new SAMLAssertion { Issuer = issuer, IssueInstant = samlResponse.IssueInstant };

            var profileId = attributes.Where(a => a.Name == PortalClaimTypes.ProfileId).Select(a => a.Values[0].ToString()).FirstOrDefault();
            var subject = new Subject(new NameID(profileId));
            var subjectConfirmation = new SubjectConfirmation(SAMLIdentifiers.SubjectConfirmationMethods.Bearer);
            var subjectConfirmationData = new SubjectConfirmationData { Recipient = assertionConsumerServiceUrl };

            subjectConfirmation.SubjectConfirmationData = subjectConfirmationData;
            subject.SubjectConfirmations.Add(subjectConfirmation);
            samlAssertion.Subject = subject;

            var conditions = new Conditions(DateTime.Now, DateTime.Now.AddDays(1));
            var audienceRestriction = new AudienceRestriction();
            audienceRestriction.Audiences.Add(new Audience(partner));
            conditions.ConditionsList.Add(audienceRestriction);
            samlAssertion.Conditions = conditions;

            var authnStatement = new AuthnStatement { AuthnContext = new AuthnContext(), AuthnInstant = samlResponse.IssueInstant };
            authnStatement.AuthnContext.AuthnContextClassRef = new AuthnContextClassRef(SAMLIdentifiers.AuthnContextClasses.X509);
            samlAssertion.Statements.Add(authnStatement);

            attributes.ForEach(a =>
            {
                var attributeStatement = new AttributeStatement();

                attributeStatement.Attributes.Add(a);
                samlAssertion.Statements.Add(attributeStatement);
            });

            var samlAssertionXml = samlAssertion.ToXml();

            if (signAssertion)
                SAMLAssertionSignature.Generate(samlAssertionXml, issuerX509Certificate.PrivateKey, issuerX509Certificate);

            if (encryptAssertion)
            {
                var encryptedAssertion = new EncryptedAssertion(samlAssertionXml, partnerX509Certificate);

                samlResponse.Assertions.Add(encryptedAssertion.ToXml());
            }
            else
            {
                samlResponse.Assertions.Add(samlAssertionXml);                
            }

            var samlResponseXml = samlResponse.ToXml();

            if (signResponse)
                SAMLMessageSignature.Generate(samlResponseXml, issuerX509Certificate.PrivateKey, issuerX509Certificate);

            return samlResponseXml;
        }

        private void SendSso(User user, string sourceDomain)
        {
            var attributes = new List<SAMLAttribute>();
            var requestId = SessionHelper.Get<string>(AuthnRequestIdSessionKey);

            if (user != null)
            {
                attributes.Add(PortalClaimTypes.ProfileId, user.ProfileID);
                attributes.Add(PortalClaimTypes.SourceDomain, sourceDomain);

                SessionHelper.Set(UserSessionKey, null);
            }

            SessionHelper.Set(SourceDomainSessionKey, null);

            var partnerSpName = SessionHelper.Get<string>(PartnerSpSessionKey);
            var assertionConsumerServiceUrl = SAMLConfiguration.Current.GetPartnerServiceProvider(partnerSpName).AssertionConsumerServiceUrl;
            var samlResponseXml = CreateSamlResponse(assertionConsumerServiceUrl, attributes, requestId);

            IdentityProvider.SendSAMLResponseByHTTPPost(Response, assertionConsumerServiceUrl, samlResponseXml, string.Empty);
            //SAMLIdentityProvider.SendSSO(Response, user.ProfileID, attributes.ToArray());
        }

        #endregion
    }

    #region Extensions

    public static class AuthExtensions
    {
        private static readonly Regex SubdomainRegex = new Regex(@"(.*?)\.(?=[^\/]*\..{2,5})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static List<SAMLAttribute> Parse(this SAMLAttribute[] attributes, string nameId, string idpPartner, List<Affiliate> affiliates, out string profileId, out int affiliateId, out string sourceDomain)
        {
            var attributeList = new List<SAMLAttribute>();
            profileId = sourceDomain = string.Empty;
            affiliateId = 0;

            attributeList.Add("NameID", nameId);
            if (attributes != null)
                attributeList.AddRange(attributes.ToList());

            // Retrieve SourceDomain & ProfileID from the Attributes
            foreach (var attribute in attributeList)
            {
                if (attribute.Name.Equals(PortalClaimTypes.SourceDomain, StringComparison.InvariantCultureIgnoreCase))
                    sourceDomain = attribute.Values.Count > 0 ? attribute.Values[0].Data.ToString() : string.Empty;

                if (attribute.Name.Equals(PortalClaimTypes.ProfileId, StringComparison.InvariantCultureIgnoreCase))
                    profileId = attribute.Values.Count > 0 ? attribute.Values[0].Data.ToString() : string.Empty;
            }

            // Set profileId to the UserName/NameID subject value if it wasn't included in the attributes
            if (string.IsNullOrEmpty(profileId))
                profileId = nameId;

            // Set sourceDomain to the idpPartner value if it wasn't included in the attributes
            if (string.IsNullOrEmpty(sourceDomain))
            {
                Uri uri;
                if (Uri.TryCreate(idpPartner, UriKind.Absolute, out uri))
                    sourceDomain = SubdomainRegex.Replace(uri.Host, string.Empty);
            }

            if (affiliates != null)
            {
                var samlSourceDomain = sourceDomain;
                var affiliate = affiliates.FirstOrDefault(a => a.SamlSourceDomain.Equals(samlSourceDomain, StringComparison.InvariantCultureIgnoreCase));
                affiliateId = affiliate != null ? affiliate.AffiliateID : 0;
            }

            return attributeList;
        }

        public static void Add(this ICollection<SAMLAttribute> attributes, string name, params string[] values)
        {
            if (attributes == null || string.IsNullOrEmpty(name) || values == null)
                return;

            attributes.Add(new SAMLAttribute() { Name = name, Values = values.Select(v => new AttributeValue(v)).ToList() });
        }
    }
    #endregion
}