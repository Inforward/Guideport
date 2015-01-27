USE [Innova]
GO

UPDATE dbo.Affiliate SET SAMLConfigurationID = NULL
DELETE app.[ConfigurationSetting]
DELETE app.[Setting]
DELETE app.[Environment]
DELETE app.[Configuration]
DELETE app.[ConfigurationType]

INSERT INTO app.[ConfigurationType]( ConfigurationTypeID, Name, [Description], AssemblyName, ClassName )
SELECT 1, 'General Configuration', 'General Configuration Settings.', NULL, NULL
UNION SELECT 2, 'SAML.ServiceProvider', 'SAML Service Provider Configuration for ComponentSpace Library.', 'ComponentSpace.SAML2', 'ComponentSpace.SAML2.Configuration.ServiceProviderConfiguration'
UNION SELECT 3, 'SAML.IdentityProvider', 'SAML Identity Provider Configuration for ComponentSpace Library.', 'ComponentSpace.SAML2', 'ComponentSpace.SAML2.Configuration.IdentityProviderConfiguration'
UNION SELECT 4, 'SAML.PartnerServiceProvider', 'SAML Partner Service Provider Configuration for ComponentSpace Library.', 'ComponentSpace.SAML2', 'ComponentSpace.SAML2.Configuration.PartnerServiceProviderConfiguration'
UNION SELECT 5, 'SAML.PartnerIdentityProvider', 'SAML Partner Identity Provider Configuration for ComponentSpace Library.', 'ComponentSpace.SAML2', 'ComponentSpace.SAML2.Configuration.PartnerIdentityProviderConfiguration'

INSERT INTO app.[Configuration]( ConfigurationID, ConfigurationTypeID, Name, [Description] )
SELECT 1, 1, 'Guideport', 'Shared Guideport Configuration.'
UNION SELECT 2, 2, 'Guideport Service Provider', 'SAML Service Provider Setting for Guideport.'
UNION SELECT 3, 3, 'Guideport Identity Provider', 'SAML Identity Provider Setting for Guideport.  For testing purposes only.'
UNION SELECT 4, 4, 'Guideport Partner Service Provider', 'SAML Partner Service Provider Setting for Guideport.'
UNION SELECT 5, 5, 'Guideport Partner Identity Provider', 'Guideport SAML Partner Identity Provider Configuration.  For testing purposes only.'
UNION SELECT 6, 5, 'First Allied Partner Identity Provider', 'First Allied SAML Partner Identity Provider Configuration.'
UNION SELECT 7, 5, 'Cetera Advisors Partner Identity Provider', 'Cetera Advisors SAML Partner Identity Provider Configuration.'
UNION SELECT 8, 5, 'Cetera Advisor Networks Partner Identity Provider', 'Cetera Advisor Networks SAML Partner Identity Provider Configuration.'
UNION SELECT 9, 5, 'Cetera Financial Institutions Partner Identity Provider', 'Cetera Financial Institutions SAML Partner Identity Provider Configuration.'
UNION SELECT 10, 5, 'Cetera Financial Specialists Partner Identity Provider', 'Cetera Financial Specialists SAML Partner Identity Provider Configuration.'
UNION SELECT 11, 5, 'Investors Capital Partner Identity Provider', 'Investors Capital SAML Partner Identity Provider Configuration.'
UNION SELECT 12, 5, 'JP Turner Partner Identity Provider', 'JP Turner SAML Partner Identity Provider Configuration.'
UNION SELECT 13, 5, 'Legend Group Partner Identity Provider', 'Legend Group SAML Partner Identity Provider Configuration.'
UNION SELECT 14, 5, 'Summit Brokerage Partner Identity Provider', 'Summit Brokerage SAML Partner Identity Provider Configuration.'
UNION SELECT 15, 5, 'Girard Securities Partner Identity Provider', 'Girard Securities SAML Partner Identity Provider Configuration.'
UNION SELECT 16, 5, 'VSR Financial Partner Identity Provider', 'VSR Financial SAML Partner Identity Provider Configuration.'

-- Update dbo.Affiliate SAMLConfigurationID
UPDATE dbo.Affiliate SET SAMLConfigurationID = 6 WHERE AffiliateID = 1 -- FirstAllied
UPDATE dbo.Affiliate SET SAMLConfigurationID = 7 WHERE AffiliateID = 2 -- Cetera Advisors
UPDATE dbo.Affiliate SET SAMLConfigurationID = 8 WHERE AffiliateID = 3 -- Cetera Advisor Networks
UPDATE dbo.Affiliate SET SAMLConfigurationID = 9 WHERE AffiliateID = 4 -- Cetera Financial Institutions
UPDATE dbo.Affiliate SET SAMLConfigurationID = 10 WHERE AffiliateID = 5 -- Cetera Financial Specialists
UPDATE dbo.Affiliate SET SAMLConfigurationID = 11 WHERE AffiliateID = 6 -- Investors Capital
UPDATE dbo.Affiliate SET SAMLConfigurationID = 12 WHERE AffiliateID = 7 -- JP Turner
UPDATE dbo.Affiliate SET SAMLConfigurationID = 13 WHERE AffiliateID = 8 -- Legend Group
UPDATE dbo.Affiliate SET SAMLConfigurationID = 14 WHERE AffiliateID = 9 -- Summit Brokerage
UPDATE dbo.Affiliate SET SAMLConfigurationID = 15 WHERE AffiliateID = 10 -- Girard Securities
UPDATE dbo.Affiliate SET SAMLConfigurationID = 16 WHERE AffiliateID = 11 -- VSR Financial Services

INSERT INTO app.[Environment]( EnvironmentID, Name, [Description] )
SELECT 0, 'Default', 'Default Environment.  May be overridden by other explicitly named Environments.'
UNION SELECT 1, 'Development', 'Development Environment'
UNION SELECT 2, 'CeteraQA', 'Cetera Testing Environment'
UNION SELECT 3, 'CeteraProduction', 'Cetera Production Environment'
UNION SELECT 4, 'FirstAlliedQA', 'First Allied Testing Environment'
UNION SELECT 5, 'FirstAlliedProduction', 'First Allied Production Environment'

DBCC CHECKIDENT ('app.Setting', RESEED, 0);

INSERT INTO app.[Setting]( ConfigurationTypeID, Name, [Description], DataTypeName, IsRequired )
-- SAML.ServiceProvider
SELECT 2, 'Name', 'Gets or sets the service provider name.', 'System.String', 1
UNION SELECT 2, 'AssertionConsumerServiceUrl', 'Gets or sets the service provider''s assertion consumer service (ACS) URL.  The assertion consumer service URL is the endpoint at which the SAML response is received.', 'System.String', 1
UNION SELECT 2, 'CertificateFile', 'Gets or sets the certificate file path.  The file path is either absolute or relative to the application folder.', 'System.String', 0
UNION SELECT 2, 'CertificatePassword', 'Gets or sets the certificate file password.', 'System.String', 0
-- SAML.IdentityProvider
UNION SELECT 3, 'Name', 'Gets or sets the identity provider name.', 'System.String', 1
UNION SELECT 3, 'CertificateFile', 'Gets or sets the certificate file path.  The file path is either absolute or relative to the application folder.', 'System.String', 0
UNION SELECT 3, 'CertificatePassword', 'Gets or sets the certificate file password.', 'System.String', 0
-- SAML.PartnerServiceProvider
UNION SELECT 4, 'Name', 'Unique identifier for the Partner Service Provider.', 'System.String', 1
UNION SELECT 4, 'AssertionConsumerServiceUrl', 'Gets or sets the partner service provider''s assertion consumer service URL.', 'System.String', 1
UNION SELECT 4, 'WantAuthnRequestSigned', 'Gets or sets the flag indicating whether authn requests should be signed.', 'System.Boolean', 0
UNION SELECT 4, 'EncryptAssertion', 'Gets or sets the flag indicating whether to encrypt SAML assertions', 'System.Boolean', 0
UNION SELECT 4, 'SignAssertion', 'Gets or sets the flag indicating whether to sign SAML assertions.', 'System.Boolean', 0
UNION SELECT 4, 'SignSAMLResponse', 'Gets or sets the flag indicating whether to sign SAML responses.', 'System.Boolean', 0
UNION SELECT 4, 'SingleLogoutServiceUrl', 'Gets or sets the partner provider''s single sign-on service URL.', 'System.String', 0
UNION SELECT 4, 'CertificateFile', 'The certificate file path.  The file path is either absolute or relative to the Configuration folder.', 'System.String', 0
-- SAML.PartnerIdentityProvider
UNION SELECT 5, 'Name', 'Unique identifier for the Partner Identity Provider.', 'System.String', 1
UNION SELECT 5, 'SignAuthnRequest', 'The flag indicating whether to sign authn requests.', 'System.Boolean', 0
UNION SELECT 5, 'SignatureMethod', 'The XML signature algorithm. The default algorithm is http://www.w3.org/2000/09/xmldsig#rsa-sha1.', 'System.String', 0
UNION SELECT 5, 'WantSAMLResponseSigned', 'The flag indicating whether SAML responses should be signed.', 'System.Boolean', 0
UNION SELECT 5, 'WantAssertionSigned', 'The flag indicating whether SAML assertions should be signed.', 'System.Boolean', 0
UNION SELECT 5, 'WantAssertionEncrypted', 'The flag indicating whether SAML assertions should be encrypted.', 'System.Boolean', 0
UNION SELECT 5, 'WantLogoutRequestSigned', 'The flag indicating whether received logout requests should be signed.', 'System.Boolean', 0
UNION SELECT 5, 'UseEmbeddedCertificate', 'The flag to indicate whether to use embedded certificates.', 'System.Boolean', 0
UNION SELECT 5, 'ClockSkew', 'Gets or sets the clock skew. The clock skew allows for differences between local and partner computer clocks when checking time intervals.  The default time span is 3 minutes.', 'System.TimeSpan', 0
UNION SELECT 5, 'SingleSignOnServiceUrl', 'The Partner Identity Provider''s single sign-on service URL.', 'System.String', 0
UNION SELECT 5, 'SingleSignOnServiceBinding', 'The Partner Identity Provider''s single sign-on service binding. The default binding is urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect.', 'System.String', 0
UNION SELECT 5, 'SingleLogoutServiceUrl', 'The partner provider''s single sign-on service URL', 'System.String', 0
UNION SELECT 5, 'SingleLogoutServiceBinding', 'The Partner Provider''s single sign-on service binding. The default binding is urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect.', 'System.String', 0
UNION SELECT 5, 'CertificateFile', 'The certificate file path.  The file path is either absolute or relative to the Configuration folder.', 'System.String', 0
UNION SELECT 5, 'DisableInResponseToCheck', 'Gets or sets the flag indicating whether the InResponseTo is checked.', 'System.Boolean', 0

DBCC CHECKIDENT ('app.ConfigurationSetting', RESEED, 0);

INSERT INTO app.[ConfigurationSetting]( ConfigurationID, EnvironmentID, SettingID, Value )
-- SAML Service Provider --
-- Default
SELECT 2, 0, SettingID, '~/saml/receive'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'AssertionConsumerServiceUrl'
UNION SELECT 2, 0, SettingID, 'Saml\\Certificates\\qa-guideport.pfx'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'CertificateFile'
UNION SELECT 2, 0, SettingID, '1st_Allied'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'CertificatePassword'
-- Development
UNION SELECT 2, 1, SettingID, 'https://localhost:44303'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'Name'
-- Cetera QA
UNION SELECT 2, 2, SettingID, 'https://qa.guideportcfg.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'Name'
-- Cetera Production
UNION SELECT 2, 3, SettingID, 'https://www.guideportcfg.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'Name'
UNION SELECT 2, 3, SettingID, 'https://www.guideportcfg.com/saml/receive'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'AssertionConsumerServiceUrl'
-- QA
UNION SELECT 2, 4, SettingID, 'https://qa-guideport.firstallied.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'Name'
-- Production
UNION SELECT 2, 5, SettingID, 'https://guideport.firstallied.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 2 AND Name = 'Name'

-- SAML Identity Provider (For Testing) --
-- Default
UNION SELECT 3, 0, SettingID, 'urn:guideport:idp'
FROM app.[Setting] WHERE ConfigurationTypeID = 3 AND Name = 'Name'
UNION SELECT 3, 0, SettingID, 'Saml\\Certificates\\idp.pfx'
FROM app.[Setting] WHERE ConfigurationTypeID = 3 AND Name = 'CertificateFile'
UNION SELECT 3, 0, SettingID, 'password'
FROM app.[Setting] WHERE ConfigurationTypeID = 3 AND Name = 'CertificatePassword'

-- SAML Partner Service Provider --
-- Default
UNION SELECT 4, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'WantAuthnRequestSigned'
UNION SELECT 4, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'SignSAMLResponse'
UNION SELECT 4, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'SignAssertion'
UNION SELECT 4, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'EncryptAssertion'
UNION SELECT 4, 0, SettingID, 'saml\\certificates\\qa-guideport.cer'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'CertificateFile'
-- Development
UNION SELECT 4, 1, SettingID, 'https://qa.guideportcfg.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'Name'
UNION SELECT 4, 1, SettingID, 'https://qa.guideportcfg.com/saml/receive'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'AssertionConsumerServiceUrl'
UNION SELECT 4, 1, SettingID, 'https://qa.guideportcfg.com/saml/logout'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'SingleLogoutServiceUrl'
-- Cetera QA
UNION SELECT 4, 2, SettingID, 'https://localhost:44303'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'Name'
UNION SELECT 4, 2, SettingID, 'https://localhost:44303/saml/receive'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'AssertionConsumerServiceUrl'
UNION SELECT 4, 2, SettingID, 'https://localhost:44303/saml/logout'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'SingleLogoutServiceUrl'
-- First Allied QA
UNION SELECT 4, 4, SettingID, 'https://qa-guideport.firstallied.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'Name'
UNION SELECT 4, 4, SettingID, 'https://qa-guideport.firstallied.com/saml/receive'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'AssertionConsumerServiceUrl'
UNION SELECT 4, 4, SettingID, 'https://qa-guideport.firstallied.com/saml/logout'
FROM app.[Setting] WHERE ConfigurationTypeID = 4 AND Name = 'SingleLogoutServiceUrl'

-- SAML Partner Identity Providers --
-- Guideport Default (For Testing)
UNION SELECT 5, 0, SettingID, 'urn:guideport:idp'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 
UNION SELECT 5, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'
UNION SELECT 5, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantSAMLResponseSigned' 
UNION SELECT 5, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionSigned' 
UNION SELECT 5, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionEncrypted'
UNION SELECT 5, 0, SettingID, '00:10:00'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'ClockSkew' 
UNION SELECT 5, 0, SettingID, 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceBinding' 
UNION SELECT 5, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'UseEmbeddedCertificate' 
-- Development
UNION SELECT 5, 1, SettingID, 'https://localhost:44303/saml/idpssoservice'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl'
-- QA Cetera
UNION SELECT 5, 2, SettingID, 'https://qa.guideportcfg.com/saml/idpssoservice'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl'
-- First Allied QA
UNION SELECT 5, 4, SettingID, 'https://qa-guideport.firstallied.com/saml/idpssoservice'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl'

-- First Allied Default
UNION SELECT 6, 0, SettingID, 'http://www.w3.org/2001/04/xmldsig-more#rsa-sha256'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignatureMethod' 
UNION SELECT 6, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantSAMLResponseSigned' 
UNION SELECT 6, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionSigned' 
UNION SELECT 6, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionEncrypted'
UNION SELECT 6, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantLogoutRequestSigned' 
UNION SELECT 6, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'UseEmbeddedCertificate' 
UNION SELECT 6, 0, SettingID, '00:10:00'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'ClockSkew' 
UNION SELECT 6, 0, SettingID, 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceBinding' 
-- Development
UNION SELECT 6, 1, SettingID, 'http://sneakersdev.1stallied.com/adfs/services/trust'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 6, 1, SettingID, 'https://sneakersdev.1stallied.com/adfs/ls/'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl'
UNION SELECT 6, 1, SettingID, 'https://sneakersdev.1stallied.com/adfs/ls/?wa=wsignout1.0'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleLogoutServiceUrl' 
UNION SELECT 6, 1, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest' 
-- Cetera QA
UNION SELECT 6, 2, SettingID, 'http://sneakersqa.1stallied.com/adfs/services/trust'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 6, 2, SettingID, 'https://sneakersqa.1stallied.com/adfs/ls/'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 
UNION SELECT 6, 2, SettingID, 'https://sneakersqa.1stallied.com/adfs/ls/?wa=wsignout1.0'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleLogoutServiceUrl'
UNION SELECT 6, 2, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest' 
-- Cetera Production
UNION SELECT 6, 3, SettingID, 'http://sneakers.1stallied.com/adfs/services/trust'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 6, 3, SettingID, 'https://sneakers.1stallied.com/adfs/ls/'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 
UNION SELECT 6, 3, SettingID, 'https://sneakers.1stallied.com/adfs/ls/?wa=wsignout1.0'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleLogoutServiceUrl'
UNION SELECT 6, 3, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'
-- FirstAllied QA
UNION SELECT 6, 4, SettingID, 'http://sneakersqa.1stallied.com/adfs/services/trust'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 6, 4, SettingID, 'https://sneakersqa.1stallied.com/adfs/ls/'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 
UNION SELECT 6, 4, SettingID, 'https://sneakersqa.1stallied.com/adfs/ls/?wa=wsignout1.0'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleLogoutServiceUrl'
UNION SELECT 6, 4, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest' 
-- FirstAllied Production
UNION SELECT 6, 5, SettingID, 'http://sneakers.1stallied.com/adfs/services/trust'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 6, 5, SettingID, 'https://sneakers.1stallied.com/adfs/ls/'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 
UNION SELECT 6, 5, SettingID, 'https://sneakers.1stallied.com/adfs/ls/?wa=wsignout1.0'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleLogoutServiceUrl'
UNION SELECT 6, 5, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'

-- Cetera Advisors Default
UNION SELECT 7, 0, SettingID, 'http://www.cetera.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 7, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'
UNION SELECT 7, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantSAMLResponseSigned' 
UNION SELECT 7, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionSigned' 
UNION SELECT 7, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionEncrypted'
UNION SELECT 7, 0, SettingID, '00:10:00'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'ClockSkew' 
UNION SELECT 7, 0, SettingID, 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceBinding' 
UNION SELECT 7, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'UseEmbeddedCertificate' 
UNION SELECT 7, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'DisableInResponseToCheck' 
UNION SELECT 7, 0, SettingID, 'https://qa.myceterasmartworks.com/sso/saml20/default.aspx?Id=26&Code=aHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc291cmNlZG9tYWluPWNldGVyYWFkdmlzb3JzLmNvbQ=='
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 
-- Cetera PROD
UNION SELECT 7, 3, SettingID, 'https://www.myceterasmartworks.com/sso/saml20/default.aspx?Id=26&Code=aHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc291cmNlZG9tYWluPWNldGVyYWFkdmlzb3JzLmNvbQ=='
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 

-- Cetera Advisor Networks Default
UNION SELECT 8, 0, SettingID, 'http://www.ceteraadvisornetworks.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name'
UNION SELECT 8, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'
UNION SELECT 8, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantSAMLResponseSigned' 
UNION SELECT 8, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionSigned' 
UNION SELECT 8, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionEncrypted'
UNION SELECT 8, 0, SettingID, '00:10:00'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'ClockSkew' 
UNION SELECT 8, 0, SettingID, 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceBinding' 
UNION SELECT 8, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'UseEmbeddedCertificate' 
UNION SELECT 8, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'DisableInResponseToCheck' 
UNION SELECT 8, 0, SettingID, 'https://qa.myceterasmartworks.com/sso/saml20/default.aspx?Id=22&Code=aHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc291cmNlZG9tYWluPWNldGVyYWFkdmlzb3JuZXR3b3Jrcy5jb20='
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 
-- Cetera PROD
UNION SELECT 8, 3, SettingID, 'https://www.myceterasmartworks.com/sso/saml20/default.aspx?Id=22&Code=aHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc291cmNlZG9tYWluPWNldGVyYWFkdmlzb3JuZXR3b3Jrcy5jb20='
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 

-- Placeholder for other BDs, Name Setting is required

-- Cetera Financial Institutions
UNION SELECT 9, 0, SettingID, 'http://www.ceterafinancialinstitutions.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 

-- Cetera Financial Specialists
UNION SELECT 10, 0, SettingID, 'http://ceterafinancialspecialists.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 

-- Investors Capital
UNION SELECT 11, 0, SettingID, 'http://www.investorscapital.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 
UNION SELECT 11, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'
UNION SELECT 11, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantSAMLResponseSigned' 
UNION SELECT 11, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionSigned' 
UNION SELECT 11, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionEncrypted'
UNION SELECT 11, 0, SettingID, '00:10:00'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'ClockSkew' 
UNION SELECT 11, 0, SettingID, 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceBinding' 
UNION SELECT 11, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'UseEmbeddedCertificate' 

-- JP Turner
UNION SELECT 12, 0, SettingID, 'http://www.jpturner.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 

-- Legend Group
UNION SELECT 13, 0, SettingID, 'http://www.legendgroup.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 
UNION SELECT 13, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SignAuthnRequest'
UNION SELECT 13, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantSAMLResponseSigned' 
UNION SELECT 13, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionSigned' 
UNION SELECT 13, 0, SettingID, 'false'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'WantAssertionEncrypted'
UNION SELECT 13, 0, SettingID, '00:10:00'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'ClockSkew' 
UNION SELECT 13, 0, SettingID, 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceBinding' 
UNION SELECT 13, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'UseEmbeddedCertificate' 
UNION SELECT 13, 0, SettingID, 'true'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'DisableInResponseToCheck'
UNION SELECT 13, 0, SettingID, 'https://login.legendgroup.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'SingleSignOnServiceUrl' 

-- Summit Brokerage
UNION SELECT 14, 0, SettingID, 'http://www.summitbrokerage.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 

-- Girard Securities
UNION SELECT 15, 0, SettingID, 'http://www.girardsecurities.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 

-- VSR Financial
UNION SELECT 16, 0, SettingID, 'http://www.vsrfinancial.com'
FROM app.[Setting] WHERE ConfigurationTypeID = 5 AND Name = 'Name' 

GO
