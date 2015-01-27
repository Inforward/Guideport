USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteMenuIcon]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteMenuIcon]
	(
		[SiteMenuIconID] [int] NOT NULL,
		[IconName] [varchar](50) NOT NULL,
		[IconCssClass] [varchar](100) NOT NULL,
		CONSTRAINT [PK_SiteMenuIcon] PRIMARY KEY CLUSTERED ( [SiteMenuIconID] ASC )
	) ON [PRIMARY]
END

GO