USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyResponseHistory]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[SurveyResponseHistory]
	(
		[SurveyResponseHistoryID] [int] IDENTITY(1,1) NOT NULL,
		[SurveyResponseID] [int] NOT NULL,
		[SurveyPageID] [int] NOT NULL,
		[ResponseDate] [datetime] NOT NULL,
		[Score] [decimal](4, 3) NULL,
		[IsLatestScore] [bit] NOT NULL,
		[PercentComplete] [decimal](4, 3) NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,		
		[ModifyDateUTC] [datetime] NOT NULL,	
		CONSTRAINT [PK_SurveyResponseHistory] PRIMARY KEY CLUSTERED ( [SurveyResponseHistoryID] ASC	),
		CONSTRAINT [IX_SurveyResponseHistory_SurveyResponseID_ResponseDate_SurveyPageID] UNIQUE NONCLUSTERED ( [SurveyResponseID] ASC, [ResponseDate] ASC, [SurveyPageID] ASC )
	) ON [PRIMARY]

END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyResponseHistory_SurveyPageID' )
	ALTER TABLE [dbo].[SurveyResponseHistory] ADD  CONSTRAINT [FK_SurveyResponseHistory_SurveyPageID] FOREIGN KEY([SurveyPageID]) REFERENCES [dbo].[SurveyPage] ([SurveyPageID])
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyResponseHistory_SurveyResponseID' )
	ALTER TABLE [dbo].[SurveyResponseHistory] ADD  CONSTRAINT [FK_SurveyResponseHistory_SurveyResponseID] FOREIGN KEY([SurveyResponseID]) REFERENCES [dbo].[SurveyResponse] ([SurveyResponseID])
GO
