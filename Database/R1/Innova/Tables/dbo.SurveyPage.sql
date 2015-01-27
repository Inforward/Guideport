USE [Innova]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyPage]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyPage]
	(
		[SurveyPageID] [int] NOT NULL,
		[SurveyID] [int] NOT NULL,
		[PageName] [varchar](256) NOT NULL,
		[SortOrder] [int] NOT NULL,
		[IsVisible] [bit] NOT NULL,
		[Tooltip] [varchar](512) NULL,
		CONSTRAINT [PK_SurveyPage] PRIMARY KEY CLUSTERED (	[SurveyPageID] ASC )
	) ON [PRIMARY]
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyPage_Survey' )
	ALTER TABLE [dbo].[SurveyPage]  ADD  CONSTRAINT [FK_SurveyPage_Survey] FOREIGN KEY([SurveyID]) REFERENCES [dbo].[Survey] ([SurveyID])
GO


