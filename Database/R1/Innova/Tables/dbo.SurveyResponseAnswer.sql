USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyResponseAnswer]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyResponseAnswer]
	(
		[SurveyResponseAnswerID] [int] IDENTITY(1,1) NOT NULL,
		[SurveyResponseID] [int] NOT NULL,
		[SurveyQuestionID] [int] NOT NULL,
		[Answer] [varchar](max) NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		CONSTRAINT [PK_SurveyResponseAnswer] PRIMARY KEY CLUSTERED ( [SurveyResponseAnswerID] ASC )
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyResponseAnswer_SurveyQuestion' )
	ALTER TABLE [dbo].[SurveyResponseAnswer] ADD  CONSTRAINT [FK_SurveyResponseAnswer_SurveyQuestion] FOREIGN KEY([SurveyQuestionID]) REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestionID])
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyResponseAnswer_SurveyResponse' )
	ALTER TABLE [dbo].[SurveyResponseAnswer] ADD  CONSTRAINT [FK_SurveyResponseAnswer_SurveyResponse] FOREIGN KEY([SurveyResponseID]) REFERENCES [dbo].[SurveyResponse] ([SurveyResponseID])
GO
