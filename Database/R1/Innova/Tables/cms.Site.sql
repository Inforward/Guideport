USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'cms' ) 
	EXEC sp_executesql N'CREATE SCHEMA cms'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[Site]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[Site]
	(
		[SiteID] [int] NOT NULL,
		[SiteName] [varchar](64) NOT NULL,
		[SiteDescription] [varchar](256) NOT NULL,
		[DomainName] [varchar](256) NOT NULL,
		[ApplicationRootPath] [varchar](100) NOT NULL,
		[DefaultSiteTemplateID] [int] NULL,
		[DefaultSiteContentID] [int] NULL,
		CONSTRAINT [PK_Site] PRIMARY KEY CLUSTERED ( [SiteID] ASC )
	) ON [PRIMARY]
END
GO


IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Site_SiteGroup_SiteGroupID' )
	ALTER TABLE [cms].[Site] DROP CONSTRAINT [FK_Site_SiteGroup_SiteGroupID]
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SiteGroupID' AND [object_id] = OBJECT_ID(N'[cms].[Site]'))
    ALTER TABLE [cms].[Site] DROP COLUMN [SiteGroupID]
GO


