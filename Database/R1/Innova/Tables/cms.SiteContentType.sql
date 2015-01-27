USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentType]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteContentType]
	(
		[SiteContentTypeID] [int] NOT NULL,
		[ContentTypeName] [varchar](64) NOT NULL,
		[ContentTypeDescription] [varchar](128) NOT NULL,
		CONSTRAINT [PK_cms.SiteContentType] PRIMARY KEY CLUSTERED (	[SiteContentTypeID] ASC )
	) ON [PRIMARY]
END
GO