USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentStatus]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteContentStatus]
	(
		[SiteContentStatusID] [int] NOT NULL,
		[StatusDescription] [varchar](64) NOT NULL,
		CONSTRAINT [PK_SiteContentStatus] PRIMARY KEY CLUSTERED ( [SiteContentStatusID] ASC )
	) ON [PRIMARY]
END

GO