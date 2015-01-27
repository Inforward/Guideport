USE [Innova]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyQuestionAnswerSuggestedContent]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyQuestionAnswerSuggestedContent]
	(
		[SurveyQuestionAnswerSuggestedContentID] [int] IDENTITY(1,1) NOT NULL,
		[SurveyQuestionAnswerID] [int] NOT NULL,
		[SiteContentID] [int] NOT NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		CONSTRAINT [PK_SurveyQuestionAnswerSuggestedContent] PRIMARY KEY CLUSTERED (	[SurveyQuestionAnswerSuggestedContentID] ASC )
	) ON [PRIMARY]
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyQuestionAnswerSuggestedContent_SurveyQuestionAnswer' )
	ALTER TABLE [dbo].[SurveyQuestionAnswerSuggestedContent] ADD  CONSTRAINT [FK_SurveyQuestionAnswerSuggestedContent_SurveyQuestionAnswer] FOREIGN KEY([SurveyQuestionAnswerID]) REFERENCES [dbo].[SurveyQuestionAnswer] ([SurveyQuestionAnswerID])
GO