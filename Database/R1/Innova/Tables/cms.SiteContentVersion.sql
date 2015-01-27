USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentVersion]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteContentVersion]
	(
		[SiteContentVersionID] [int] IDENTITY(1,1) NOT NULL,
		[SiteContentID] [int] NOT NULL,
		[SiteTemplateID] [int] NOT NULL,
		[VersionName] [varchar](128) NOT NULL,
		[ContentText] varchar(max) NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		[ModifyDateUTC] [datetime] NOT NULL,
		CONSTRAINT [UX_SiteContentVersion_SiteContentID_VersionName] UNIQUE ( SiteContentID, VersionName ), 
		CONSTRAINT [PK_SiteContentVersion] PRIMARY KEY ( [SiteContentVersionID] ASC )
	) ON [PRIMARY]

END
GO

IF EXISTS( SELECT * FROM sys.indexes WHERE name='UX_SiteContentVersion_SiteContentVersionTypeID_SiteContentID_AffiliateID' AND [object_id] = OBJECT_ID('[cms].[SiteContentVersion]') )
	ALTER TABLE [cms].[SiteContentVersion] DROP CONSTRAINT [UX_SiteContentVersion_SiteContentVersionTypeID_SiteContentID_AffiliateID]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersion_SiteContentVersionTypeID' )
	ALTER TABLE [cms].[SiteContentVersion] DROP CONSTRAINT [FK_SiteContentVersion_SiteContentVersionTypeID]
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'VersionID' AND [object_id] = OBJECT_ID(N'[cms].[SiteContentVersion]'))
    ALTER TABLE [cms].[SiteContentVersion] DROP COLUMN [VersionID]
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SiteContentVersionTypeID' AND [object_id] = OBJECT_ID(N'[cms].[SiteContentVersion]'))
    ALTER TABLE [cms].[SiteContentVersion] DROP COLUMN [SiteContentVersionTypeID]
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name='UX_SiteContentVersion_SiteContentID_VersionName' AND [object_id] = OBJECT_ID('[cms].[SiteContentVersion]') )
	ALTER TABLE [cms].[SiteContentVersion] ADD  CONSTRAINT [UX_SiteContentVersion_SiteContentID_VersionName] UNIQUE NONCLUSTERED ( [SiteContentID] ASC, [VersionName] ASC ) ON [PRIMARY]
GO

