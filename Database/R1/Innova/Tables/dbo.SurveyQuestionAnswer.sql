USE [Innova]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyQuestionAnswer]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyQuestionAnswer]
	(
		[SurveyQuestionAnswerID] [int] IDENTITY(1,1) NOT NULL,
		[SurveyQuestionID] [int] NOT NULL,
		[AnswerText] [varchar](max) NOT NULL,
		[ReviewAnswerText] [varchar](max) NULL,
		[SortOrder] [int] NOT NULL,
		[AnswerWeight] [decimal](10, 2) NULL,		
		CONSTRAINT [PK_SurveyQuestionAnswer] PRIMARY KEY CLUSTERED 	(		[SurveyQuestionAnswerID] ASC 	)
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyQuestionAnswer_SurveyQuestion' )
	ALTER TABLE [dbo].[SurveyQuestionAnswer]  ADD CONSTRAINT [FK_SurveyQuestionAnswer_SurveyQuestion] FOREIGN KEY([SurveyQuestionID]) REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestionID])
GO