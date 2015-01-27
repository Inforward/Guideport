USE [Innova]
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'UX_Site_SiteName' )
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_Site_SiteName] ON [cms].[Site] ( [SiteName] ASC )
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'UX_Site_DomainName_ApplicationRootPath' )
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_Site_DomainName_ApplicationRootPath] ON [cms].[Site] ( [DomainName] ASC, [ApplicationRootPath] ASC )
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'UX_SiteTemplate_SiteID_TemplateName' )
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_SiteTemplate_SiteID_TemplateName] ON [cms].[SiteTemplate] ( [SiteID] ASC, [TemplateName] ASC )
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'UX_SiteKnowledgeLibrary_SiteContentID' )
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_SiteKnowledgeLibrary_SiteContentID] ON [cms].[SiteKnowledgeLibrary] ( [SiteContentID] ASC )
END
GO
