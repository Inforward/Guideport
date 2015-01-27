USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentProfileType]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteContentProfileType]
	(
		[SiteContentID] [int] NOT NULL,
		[ProfileTypeID] [int] NOT NULL,
		CONSTRAINT [PK_SiteContentProfileType] PRIMARY KEY CLUSTERED ( [SiteContentID] ASC,	[ProfileTypeID] ASC )
	) ON [PRIMARY]

END
GO


