USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContent]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteContent]
	(
		[SiteContentID] [int] IDENTITY(1,1) NOT NULL,
		[SiteContentParentID] [int] NULL,
		[SiteID] [int] NOT NULL,
		[SiteContentStatusID] [int] NOT NULL,
		[SiteContentTypeID] [int] NOT NULL,
		[SiteDocumentTypeID] [int] NOT NULL,
		[FileID] [int] NULL,
		[Title] [varchar](256) NOT NULL,
		[Description] [varchar](1000) NULL,
		[Permalink] [varchar](1000) NULL,
		[SortOrder] [int] NOT NULL,
		[MenuVisible] [bit] NOT NULL,
		[MenuIconCssClass] [varchar](64) NULL,
		[MenuTarget] [varchar](64) NULL,
		[PublishDateUTC] [datetime] NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		[ModifyDateUTC] [datetime] NOT NULL,
		CONSTRAINT [PK_SiteContent] PRIMARY KEY CLUSTERED  ( [SiteContentID] ASC ) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ContentFilePath' AND [object_id] = OBJECT_ID(N'[cms].[SiteContent]'))
BEGIN
    ALTER TABLE [cms].[SiteContent] DROP COLUMN [ContentFilePath]
END

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ContentFileSizeBytes' AND [object_id] = OBJECT_ID(N'[cms].[SiteContent]'))
BEGIN
    ALTER TABLE [cms].[SiteContent] DROP COLUMN [ContentFileSizeBytes]
END