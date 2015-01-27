USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteKnowledgeLibrary]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteKnowledgeLibrary]
	(
		[SiteKnowledgeLibraryID] [int] IDENTITY(1,1) NOT NULL,
		[SiteContentID] [int] NOT NULL,
		[Topic] [varchar](255) NULL,
		[Subtopic] [varchar](255) NULL,
		[CreatedBy] [varchar](255) NULL,
		CONSTRAINT [PK_SiteKnowledgeLibrary] PRIMARY KEY CLUSTERED ( [SiteKnowledgeLibraryID] ASC )
	) ON [PRIMARY]

END
GO
