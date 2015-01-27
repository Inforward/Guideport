USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Site_SiteContent_SiteContentID' )
	ALTER TABLE [cms].[Site] DROP  CONSTRAINT [FK_Site_SiteContent_SiteContentID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Site_SiteTemplate_SiteTemplateID' )
	ALTER TABLE [cms].[Site] DROP  CONSTRAINT [FK_Site_SiteTemplate_SiteTemplateID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_Site_SiteID' )
	ALTER TABLE [cms].[SiteContent] DROP CONSTRAINT [FK_SiteContent_Site_SiteID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_FileInfo_FileID' )
	ALTER TABLE [cms].[SiteContent] DROP CONSTRAINT [FK_SiteContent_FileInfo_FileID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteContentType_SiteContentTypeID' )
	ALTER TABLE [cms].[SiteContent] DROP CONSTRAINT [FK_SiteContent_SiteContentType_SiteContentTypeID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteDocumentType_SiteDocumentTypeID' )
	ALTER TABLE [cms].[SiteContent] DROP CONSTRAINT [FK_SiteContent_SiteDocumentType_SiteDocumentTypeID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteTemplate_SiteTemplateID' )
	ALTER TABLE [cms].[SiteContent] DROP CONSTRAINT [FK_SiteContent_SiteTemplate_SiteTemplateID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteContentStatus_SiteContentStatusID' )
	ALTER TABLE [cms].[SiteContent] DROP CONSTRAINT [FK_SiteContent_SiteContentStatus_SiteContentStatusID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteContentParentID' )
	ALTER TABLE [cms].[SiteContent] DROP  CONSTRAINT [FK_SiteContent_SiteContentParentID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteTemplate_Site_SiteID' )
	ALTER TABLE [cms].[SiteTemplate] DROP  CONSTRAINT [FK_SiteTemplate_Site_SiteID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteKnowledgeLibrary_SiteContentID' )
	ALTER TABLE [cms].[SiteKnowledgeLibrary] DROP CONSTRAINT FK_SiteKnowledgeLibrary_SiteContentID

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Site_SiteGroup_SiteGroupID' )
	ALTER TABLE [cms].[Site] DROP CONSTRAINT [FK_Site_SiteGroup_SiteGroupID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentProfileType_SiteContentID' )
	ALTER TABLE [cms].[SiteContentProfileType] DROP CONSTRAINT [FK_SiteContentProfileType_SiteContentID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentProfileType_ProfileTypeID' )
	ALTER TABLE [cms].[SiteContentProfileType] DROP CONSTRAINT [FK_SiteContentProfileType_ProfileTypeID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteKnowledgeLibraryTopic_Site' )
	ALTER TABLE [cms].[SiteKnowledgeLibraryTopic] DROP CONSTRAINT [FK_SiteKnowledgeLibraryTopic_Site]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentAffiliate_AffiliateID' )
	ALTER TABLE [cms].[SiteContentAffiliate] DROP CONSTRAINT [FK_SiteContentAffiliate_AffiliateID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersion_SiteContentID' )
	ALTER TABLE [cms].[SiteContentVersion] DROP CONSTRAINT [FK_SiteContentVersion_SiteContentID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersion_SiteTemplateID' )
	ALTER TABLE [cms].[SiteContentVersion] DROP CONSTRAINT [FK_SiteContentVersion_SiteTemplateID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteThirdPartyResourceAffiliate_ThirdPartyResourceID' )
	ALTER TABLE [cms].[SiteThirdPartyResourceAffiliate] DROP CONSTRAINT [FK_SiteThirdPartyResourceAffiliate_ThirdPartyResourceID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteThirdPartyResourceAffiliate_AffiliateID' )
	ALTER TABLE [cms].[SiteThirdPartyResourceAffiliate] DROP CONSTRAINT [FK_SiteThirdPartyResourceAffiliate_AffiliateID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersionAffiliate_AffiliateID' )
	ALTER TABLE [cms].[SiteContentVersionAffiliate] DROP CONSTRAINT [FK_SiteContentVersionAffiliate_AffiliateID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersionAffiliate_SiteContentVersionID' )
	ALTER TABLE [cms].[SiteContentVersionAffiliate] DROP CONSTRAINT [FK_SiteContentVersionAffiliate_SiteContentVersionID]

GO

USE Innova
GO

DELETE FROM [dbo].[FileInfo]
DELETE FROM [dbo].[File]
DELETE FROM [cms].[SiteKnowledgeLibrary]
DELETE FROM [cms].[SiteKnowledgeLibraryTopic]
DELETE FROM [cms].[SiteMenuIcon]
DELETE FROM [cms].[SiteContentAffiliate]
DELETE FROM [cms].[SiteContentProfileType]
DELETE FROM [cms].[SiteThirdPartyResourceService]
DELETE FROM [cms].[SiteThirdPartyResourceAffiliate]
DELETE FROM [cms].[SiteThirdPartyResource]
DELETE FROM [cms].[SiteContentVersionAffiliate]
DELETE FROM [cms].[SiteContentVersion]
DELETE FROM [cms].[SiteContent]
DELETE FROM [cms].[Site]
DELETE FROM [cms].[SiteContentStatus]
DELETE FROM [cms].[SiteContentType]
DELETE FROM [cms].[SiteDocumentType]
DELETE FROM [cms].[SiteTemplate]


SET IDENTITY_INSERT [cms].[SiteKnowledgeLibrary] OFF
SET IDENTITY_INSERT [cms].[SiteContent] OFF
SET IDENTITY_INSERT [cms].[SiteThirdPartyResource] OFF
GO

-- Menu Icon
INSERT INTO [cms].[SiteMenuIcon] ([SiteMenuIconID],[IconName],[IconCssClass])
     SELECT [MenuIconID],[IconName],[IconCssClass] FROM [Innova_Staging].[cms].[MenuIcon]
GO

-- Site
INSERT INTO [cms].[Site] ([SiteID],[SiteName],[SiteDescription],[DomainName],[ApplicationRootPath],[DefaultSiteTemplateID],[DefaultSiteContentID])
	SELECT [SiteID],[SiteName],[SiteDescription],[DomainName],[ApplicationRootPath],[DefaultSiteTemplateID],[DefaultSiteContentID]
	FROM [Innova_Staging].cms.[Site] S
	WHERE S.SiteID IN ( 1, 2, 3 )
GO

-- Site Content
SET IDENTITY_INSERT [cms].[SiteContent] ON
GO

INSERT INTO [cms].[SiteContent] ([SiteContentID],[SiteContentParentID],[SiteID],[SiteContentStatusID],[SiteContentTypeID],[SiteDocumentTypeID],[Title],[Description],[Permalink],[SortOrder],[MenuVisible],[MenuIconCssClass],[MenuTarget],[PublishDateUTC],[CreateUserID],[CreateDate],[CreateDateUTC],[ModifyUserID],[ModifyDate],[ModifyDateUTC])
	SELECT [SiteContentID],[SiteContentParentID],[SiteID],[SiteContentStatusID],[SiteContentTypeID],[SiteDocumentTypeID],[Title],[Description],[Permalink],[SortOrder],[MenuVisible],[MenuIconCssClass],[MenuTarget],
	DATEADD( hh, 7, [PublishDate] ),
	ISNULL(SC.CreateUser, 0 ),
	SC.[CreateDate],
	DATEADD( hh, 7, SC.[CreateDate] ),
	ISNULL( SC.ModifyUser , 0 ),
	SC.[ModifyDate],
	DATEADD( hh, 7, SC.[ModifyDate] ) 
	FROM 
		[Innova_Staging].cms.SiteContent SC
	WHERE 
		SC.SiteID IN ( 1, 2, 3 )
		AND SC.SiteContentStatusID <> 3
GO

SET IDENTITY_INSERT [cms].[SiteContent] OFF
GO

-- Site Content Versions (default)
INSERT INTO [cms].[SiteContentVersion] 
( 
	[SiteContentID],
	[SiteTemplateID],
	[VersionName],
	[ContentText],
	[CreateUserID],
	[CreateDate],
	[CreateDateUTC],
	[ModifyUserID],
	[ModifyDate],
	[ModifyDateUTC] 
) 
SELECT
	S.SiteContentID,
	S.SiteTemplateID,
	'Default',
	S.ContentText,
	S.CreateUser,
	S.CreateDate,
	DATEADD( hh, 7, S.[CreateDate] ),
	S.ModifyUser,
	S.ModifyDate,
	DATEADD( hh, 7, S.[ModifyDate] )
FROM 
	[Innova_Staging].cms.SiteContent S
WHERE 
	S.SiteID IN ( 1, 2, 3 )
	AND S.SiteContentStatusID <> 3
	AND S.SiteContentTypeID = 1 -- Content Pages
GO	




-- KnowledgeLibrary
SET IDENTITY_INSERT [cms].[SiteKnowledgeLibrary] ON
GO

INSERT INTO [cms].[SiteKnowledgeLibrary] ([SiteKnowledgeLibraryID], [SiteContentID],[Topic],[Subtopic],[CreatedBy])
	SELECT KL.[KnowledgeLibraryID],KL.[SiteContentID],KL.[Topic],KL.[Subtopic],KL.[CreatedBy] 
	FROM [Innova_Staging].cms.[KnowledgeLibrary] KL
		JOIN cms.SiteContent SC ON KL.SiteContentID = SC.SiteContentID
GO

SET IDENTITY_INSERT [cms].[SiteKnowledgeLibrary] OFF
GO

-- Site Content Status
INSERT INTO [cms].[SiteContentStatus] ([SiteContentStatusID],[StatusDescription])
     SELECT [SiteContentStatusID],[StatusDescription] 
	 FROM [Innova_Staging].[cms].[SiteContentStatus]

-- Site Content Type
INSERT INTO [cms].[SiteContentType] ([SiteContentTypeID],[ContentTypeName],[ContentTypeDescription])
	SELECT [SiteContentTypeID],[ContentTypeName],[ContentTypeDescription] 
	FROM [Innova_Staging].[cms].[SiteContentType]
	WHERE [SiteContentTypeID] IN ( 1, 2, 3 )

-- Site Document Type
INSERT INTO [cms].[SiteDocumentType] ([SiteDocumentTypeID],[DocumentTypeName],[DocumentTypeExtension],[MIMEType])
	SELECT [SiteDocumentTypeID],[DocumentTypeName],[DocumentTypeExtension],[MIMEType] 
	FROM [Innova_Staging].[cms].[SiteDocumentType]

-- Site Template
INSERT INTO [cms].[SiteTemplate] ([SiteTemplateID] ,[SiteID] ,[TemplateName] ,[TemplateDescription] ,[DefaultContent] ,[LayoutPath] ,[CreateUserID] ,[CreateDate], [ModifyUserID],[ModifyDate])
     SELECT [SiteTemplateID] ,[SiteID] ,[TemplateName] ,[TemplateDescription] ,[DefaultContent] ,[LayoutPath],
		ISNULL(SC.CreateUser, 0 ),
		SC.[CreateDate], 
		ISNULL(SC.ModifyUser, 0 ),
		SC.[ModifyDate] 
	 FROM [Innova_Staging].cms.[SiteTemplate] SC
	 WHERE SC.SiteID IN (1,2,3)
GO

-- Third Party Resource
SET IDENTITY_INSERT [cms].[SiteThirdPartyResource] ON
GO

INSERT INTO [cms].[SiteThirdPartyResource] ([SiteThirdPartyResourceID],[Name],[Description],[AddressLine1],[AddressLine2],[City],[State],[PostalCode],[Country],[PhoneNo],[PhoneNoExt],[FaxNo],[Email],[WebsiteUrl],[Services],[CreateUserID],[CreateDate],[CreateDateUTC],[ModifyUserID],[ModifyDate],[ModifyDateUTC])
	SELECT [ThirdPartyResourceID],TPR.[Name],TPR.[Description],
	TPR.[AddressLine1],TPR.[AddressLine2],TPR.[City],TPR.[State],TPR.[PostalCode],	
	CASE WHEN ISNULL(TPR.[Country], '') = '' AND ISNULL(TPR.[State], '') <> '' THEN 'US' ELSE TPR.[Country] END,
	TPR.[PhoneNo],TPR.[PhoneNoExt],TPR.[FaxNo],TPR.[Email],TPR.[WebsiteUrl],TPR.[Services],
	ISNULL(TPR.CreateUser, 0 ),
	TPR.[CreateDate],
	DATEADD( hh, 7, TPR.[CreateDate] ),
	ISNULL(TPR.ModifyUser, 0 ),
	TPR.[ModifyDate],
	DATEADD( hh, 7, TPR.[ModifyDate] )  
	FROM [Innova_Staging].[cms].[ThirdPartyResource] TPR
	WHERE TPR.SiteID IN (1,2,3)
GO

SET IDENTITY_INSERT [cms].[SiteThirdPartyResource] OFF
GO

USE [Innova]
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_Site_SiteID' )
BEGIN
	ALTER TABLE [cms].[SiteContent] ADD CONSTRAINT [FK_SiteContent_Site_SiteID] FOREIGN KEY([SiteID]) REFERENCES [cms].[Site] ([SiteID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_FileInfo_FileID' )
BEGIN
	ALTER TABLE [cms].[SiteContent] ADD CONSTRAINT [FK_SiteContent_FileInfo_FileID] FOREIGN KEY([FileID]) REFERENCES [dbo].[FileInfo] ([FileID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteContentType_SiteContentTypeID' )
BEGIN
	ALTER TABLE [cms].[SiteContent] ADD CONSTRAINT [FK_SiteContent_SiteContentType_SiteContentTypeID] FOREIGN KEY([SiteContentTypeID]) REFERENCES [cms].[SiteContentType] ([SiteContentTypeID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteDocumentType_SiteDocumentTypeID' )
BEGIN
	ALTER TABLE [cms].[SiteContent] ADD CONSTRAINT [FK_SiteContent_SiteDocumentType_SiteDocumentTypeID] FOREIGN KEY([SiteDocumentTypeID]) REFERENCES [cms].[SiteDocumentType] ([SiteDocumentTypeID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteContentStatus_SiteContentStatusID' )
BEGIN
	ALTER TABLE [cms].[SiteContent] ADD CONSTRAINT [FK_SiteContent_SiteContentStatus_SiteContentStatusID] FOREIGN KEY([SiteContentStatusID]) REFERENCES [cms].[SiteContentStatus] ([SiteContentStatusID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContent_SiteContentParentID' )
BEGIN
	ALTER TABLE [cms].[SiteContent] ADD  CONSTRAINT [FK_SiteContent_SiteContentParentID] FOREIGN KEY([SiteContentParentID]) REFERENCES [cms].[SiteContent] ([SiteContentID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteTemplate_Site_SiteID' )
BEGIN
	ALTER TABLE [cms].[SiteTemplate] ADD  CONSTRAINT [FK_SiteTemplate_Site_SiteID] FOREIGN KEY([SiteID]) REFERENCES [cms].[Site] ([SiteID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteKnowledgeLibrary_SiteContentID' )
BEGIN
	ALTER TABLE [cms].[SiteKnowledgeLibrary] ADD CONSTRAINT FK_SiteKnowledgeLibrary_SiteContentID FOREIGN KEY( SiteContentID ) REFERENCES [cms].SiteContent( SiteContentID )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentProfileType_SiteContentID' )
BEGIN
	ALTER TABLE [cms].[SiteContentProfileType] ADD CONSTRAINT [FK_SiteContentProfileType_SiteContentID] FOREIGN KEY([SiteContentID]) REFERENCES [cms].[SiteContent] ( [SiteContentID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentProfileType_ProfileTypeID' )
BEGIN
	ALTER TABLE [cms].[SiteContentProfileType] ADD CONSTRAINT [FK_SiteContentProfileType_ProfileTypeID] FOREIGN KEY([ProfileTypeID]) REFERENCES [usr].[ProfileType] ( [ProfileTypeID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteKnowledgeLibraryTopic_Site' )
BEGIN
	ALTER TABLE [cms].[SiteKnowledgeLibraryTopic] ADD CONSTRAINT [FK_SiteKnowledgeLibraryTopic_Site] FOREIGN KEY( SiteID ) REFERENCES [cms].[Site]( SiteID )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentAffiliate_AffiliateID' )
BEGIN
	ALTER TABLE [cms].[SiteContentAffiliate] ADD CONSTRAINT [FK_SiteContentAffiliate_AffiliateID] FOREIGN KEY([AffiliateID]) REFERENCES [dbo].[Affiliate] ( [AffiliateID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersion_SiteContentID' )
BEGIN
	ALTER TABLE [cms].[SiteContentVersion] ADD CONSTRAINT [FK_SiteContentVersion_SiteContentID] FOREIGN KEY([SiteContentID]) REFERENCES [cms].[SiteContent] ( [SiteContentID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersion_SiteTemplateID' )
BEGIN
	ALTER TABLE [cms].[SiteContentVersion] ADD CONSTRAINT [FK_SiteContentVersion_SiteTemplateID] FOREIGN KEY([SiteTemplateID]) REFERENCES [cms].[SiteTemplate] ( [SiteTemplateID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteThirdPartyResourceAffiliate_ThirdPartyResourceID' )
BEGIN
	ALTER TABLE [cms].[SiteThirdPartyResourceAffiliate] ADD CONSTRAINT [FK_SiteThirdPartyResourceAffiliate_ThirdPartyResourceID] FOREIGN KEY([SiteThirdPartyResourceID]) REFERENCES [cms].[SiteThirdPartyResource] ( [SiteThirdPartyResourceID] ) ON DELETE CASCADE
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteThirdPartyResourceAffiliate_AffiliateID' )
BEGIN
	ALTER TABLE [cms].[SiteThirdPartyResourceAffiliate] ADD CONSTRAINT [FK_SiteThirdPartyResourceAffiliate_AffiliateID] FOREIGN KEY([AffiliateID]) REFERENCES [dbo].[Affiliate] ( [AffiliateID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersionAffiliate_AffiliateID' )
BEGIN
	ALTER TABLE [cms].[SiteContentVersionAffiliate] ADD CONSTRAINT [FK_SiteContentVersionAffiliate_AffiliateID] FOREIGN KEY([AffiliateID]) REFERENCES [dbo].[Affiliate] ( [AffiliateID] )
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersionAffiliate_SiteContentVersionID' )
BEGIN
	ALTER TABLE [cms].[SiteContentVersionAffiliate] ADD CONSTRAINT [FK_SiteContentVersionAffiliate_SiteContentVersionID] FOREIGN KEY([SiteContentVersionID]) REFERENCES [cms].[SiteContentVersion] ( [SiteContentVersionID] )
END
GO


-- Add ProfileType associations
DECLARE @profileTypeId INT

SELECT @profileTypeId = ProfileTypeID FROM Innova.usr.ProfileType WHERE [Name] = 'Financial Advisor'

INSERT INTO cms.SiteContentProfileType ( SiteContentID, ProfileTypeID )
	SELECT SiteContentID, @profileTypeId
	FROM cms.SiteContent 
	WHERE [Title] IN ( 'My Business Plans', 'Business-Planning Wizard', 'Business Valuation', 'Succession Dashboard' )
GO

-- Knowledge Library Topics
INSERT INTO [cms].[SiteKnowledgeLibraryTopic] ( [SiteID], [Topic], [Subtopic] )
	SELECT 2, 'Business Development', 'Client Acquisition' UNION
	SELECT 2, 'Business Development', 'Marketing' UNION
	SELECT 2, 'Business Development', 'Wealth Management' UNION

	SELECT 2, 'Business Management', 'Financial Management' UNION
	SELECT 2, 'Business Management', 'Strategic Planning' UNION

	SELECT 2, 'Human Capital', 'Employment Basics' UNION
	SELECT 2, 'Human Capital', 'Hiring and Onboarding' UNION
	SELECT 2, 'Human Capital', 'Organization Design' UNION
	SELECT 2, 'Human Capital', 'Performance Management' UNION	

	SELECT 2, 'Operational Efficiency', 'Business Processing' UNION
	SELECT 2, 'Operational Efficiency', 'Client Service' UNION
	SELECT 2, 'Operational Efficiency', 'Technology' UNION

	SELECT 2, 'Succession Planning', 'Deal Structure' UNION
	SELECT 2, 'Succession Planning', 'Due Diligence' UNION
	SELECT 2, 'Succession Planning', 'Education' UNION
	SELECT 2, 'Succession Planning', 'Funding' UNION
	SELECT 2, 'Succession Planning', 'Identifying Your Partner' UNION
	SELECT 2, 'Succession Planning', 'Sample Documentation' UNION
	SELECT 2, 'Succession Planning', 'Transition' UNION
	SELECT 2, 'Succession Planning', 'Valuation'
GO

-- Third Party Resource Services
INSERT INTO [cms].[SiteThirdPartyResourceService] ([SiteThirdPartyResourceServiceID],[ServiceName])
     SELECT 1, 'Business Development'
	 UNION SELECT 2, 'Business Management'
	 UNION SELECT 3, 'Human Capital'
	 UNION SELECT 4, 'Operational Efficiency'
	 UNION SELECT 5, 'Succession Planning'
GO


-- General Updates
IF EXISTS ( SELECT * FROM cms.SiteContent WHERE Permalink = '/pentameter/business-valuation' )
BEGIN
	UPDATE cms.SiteContent 
	SET Permalink = '/pentameter/succession-planning/business-valuation'
	WHERE Permalink = '/pentameter/business-valuation'

	UPDATE SCV
	SET SCV.ContentText = REPLACE( SCV.ContentText, '/pentameter/business-valuation', '/pentameter/succession-planning/business-valuation' )
	FROM
		cms.SiteContentVersion SCV
		JOIN cms.SiteContent SC
			ON SCV.SiteContentID = SC.SiteContentID	
	WHERE SC.Permalink = '/pentameter/succession-planning'

END


IF @@SERVERNAME LIKE '%DCOLSQL00%'
	UPDATE cms.Site SET [DomainName] = 'localhost:44303'

IF @@SERVERNAME LIKE '%QCOLSQL00%'
	UPDATE cms.Site SET [DomainName] = 'qa-guideport.firstallied.com'

IF @@SERVERNAME LIKE '%CTQ%'
	UPDATE cms.Site SET [DomainName] = 'qa.guideportcfg.com'

IF @@SERVERNAME LIKE '%CTP%'
	UPDATE cms.Site SET [DomainName] = 'www.guideportcfg.com'

GO

IF NOT EXISTS( SELECT 1 FROM [cms].[SiteContent] WHERE Title = 'Business Assessment' AND SiteID = 2 )
BEGIN
	-- Business Assessment Survey
	INSERT INTO [cms].[SiteContent]
		([SiteContentParentID],[SiteID],[SiteContentStatusID]
		,[SiteContentTypeID],[SiteDocumentTypeID]
		,[Title],[Description],[Permalink]
		,[SortOrder],[MenuVisible],[MenuIconCssClass]
		,[MenuTarget],[PublishDateUTC]
		,[CreateUserID],[CreateDate],[CreateDateUTC]
		,[ModifyUserID],[ModifyDate],[ModifyDateUTC])
		VALUES
		(
			NULL, 2, 1, 
			2, 5,
			'Business Assessment', NULL, '/pentameter/survey',
			100, 0, NULL,
			NULL, GetUtcDate(),
			0, GetDate(), GetUtcDate(),
			0, GetDate(), GetUtcDate()
		)
END
GO

IF NOT EXISTS( SELECT 1 FROM [cms].[SiteContent] WHERE Title = 'Advisor View' AND SiteID = 1 )
BEGIN
	INSERT INTO [cms].[SiteContent]
		([SiteContentParentID],[SiteID],[SiteContentStatusID]
		,[SiteContentTypeID],[SiteDocumentTypeID]
		,[Title],[Description],[Permalink]
		,[SortOrder],[MenuVisible],[MenuIconCssClass]
		,[MenuTarget],[PublishDateUTC]
		,[CreateUserID],[CreateDate],[CreateDateUTC]
		,[ModifyUserID],[ModifyDate],[ModifyDateUTC])
		VALUES
		(
			NULL, 1, 1, 
			2, 5,
			'Advisor View', NULL, '/advisor-view',
			10, 0, NULL,
			NULL, GetUtcDate(),
			0, GetDate(), GetUtcDate(),
			0, GetDate(), GetUtcDate()
		)
END
GO

UPDATE [cms].[SiteContentType] SET [ContentTypeName] = 'Content' WHERE [ContentTypeName] = 'ContentPage'
UPDATE [cms].[SiteContentType] SET [ContentTypeName] = 'Static' WHERE [ContentTypeName] = 'StaticPage'
GO

-- Publish succession files
DECLARE @parentId INT,
		@utcNow DATETIME = GetUtcDate()

SELECT @parentId = SiteContentID 
FROM cms.SiteContent 
WHERE Title = 'Succession Planning' AND SiteContentParentID IS NULL
AND SiteContentStatusID = 1
AND SiteContentTypeID = 1

UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Succession Dashboard'
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Planning' AND SiteContentParentID = @parentId
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Enrollment' AND SiteContentParentID = @parentId
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Qualified Buyer Program' AND SiteContentParentID = @parentId
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Services' AND SiteContentParentID = @parentId
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Planning Roadmap' AND SiteContentParentID = @parentId
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Transition Support' AND SiteContentParentID = @parentId

UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Continuity Planning'
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Succession Planning'
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Business Acquisition'

UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Create a Continuity Plan'
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Create a Succession Plan'
UPDATE cms.SiteContent SET SiteContentStatusID = 1, PublishDateUtc = @utcNow WHERE Title = 'Business Acquisition'

SELECT @parentId = SiteContentID FROM cms.SiteContent WHERE Title = 'Succession Dashboard'

UPDATE cms.SiteContent SET SiteContentParentID = @parentId WHERE Title IN ('Planning','Enrollment','Qualified Buyer Program')
GO

UPDATE cms.SiteContent SET Title = 'CWS&reg; 13 Wealth Management Issues'
WHERE Permalink = '/pentameter/business-development/client-acquisition'

UPDATE cms.SiteContent SET Title = 'Certified Wealth Strategist&reg;'
WHERE Permalink = '/pentameter/business-development/wealth-management/certified-wealth-strategist'