USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteTemplate]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteTemplate]
	(
		[SiteTemplateID] [int] NOT NULL,
		[SiteID] [int] NOT NULL,
		[TemplateName] [varchar](256) NOT NULL,
		[TemplateDescription] [varchar](512) NOT NULL,
		[DefaultContent] [varchar](max) NULL,
		[LayoutPath] [varchar](1000) NOT NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		CONSTRAINT [PK_SiteTemplate] PRIMARY KEY CLUSTERED 	(	[SiteTemplateID] ASC 	)
	) ON [PRIMARY]
END
