USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteKnowledgeLibraryTopic]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteKnowledgeLibraryTopic]
	(
		[SiteKnowledgeLibraryTopicID] [int] IDENTITY(1,1) NOT NULL,
		[SiteID] [int] NOT NULL,
		[Topic] [varchar](255) NOT NULL,
		[Subtopic] [varchar](255) NULL,
		CONSTRAINT [PK_SiteKnowledgeLibraryTopic] PRIMARY KEY CLUSTERED ( [SiteKnowledgeLibraryTopicID] ASC )
	) ON [PRIMARY]
END
GO


