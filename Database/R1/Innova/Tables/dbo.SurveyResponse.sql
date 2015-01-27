USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyResponse]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyResponse]
	(
		[SurveyResponseID] [int] IDENTITY(1,1) NOT NULL,
		[SurveyID] [int] NOT NULL,	
		[UserID] [int] NOT NULL,
		[SelectedSurveyPageID] [int] NOT NULL,
		[CurrentScore] [decimal](3, 2) NULL,
		[PercentComplete] [decimal](3, 2) NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,	
		[CreateDateUTC] [datetime] NOT NULL,	
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		[ModifyDateUTC] [datetime] NOT NULL,
		[CompleteUserID] [int] NULL,
		[CompleteDate] [datetime] NULL,		
		[CompleteDateUTC] [datetime] NULL,	
		[OldID] [int] NULL,	
		CONSTRAINT [PK_SurveyResponse] PRIMARY KEY CLUSTERED (	[SurveyResponseID] ASC )
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OldID' AND [object_id] = OBJECT_ID(N'[dbo].[SurveyResponse]'))
    ALTER TABLE [dbo].[SurveyResponse] ADD [OldID] INT NULL
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyResponse_Survey' )
	ALTER TABLE [dbo].[SurveyResponse] ADD CONSTRAINT [FK_SurveyResponse_Survey] FOREIGN KEY([SurveyID]) REFERENCES [dbo].[Survey] ([SurveyID])
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyResponse_SurveyPage' )
	ALTER TABLE [dbo].[SurveyResponse]  ADD  CONSTRAINT [FK_SurveyResponse_SurveyPage] FOREIGN KEY([SelectedSurveyPageID]) REFERENCES [dbo].[SurveyPage] ([SurveyPageID]) 
GO

