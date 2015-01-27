USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyPageScoreRange]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyPageScoreRange]
	(
		[SurveyPageScoreRangeID] [int] IDENTITY(1,1) NOT NULL,
		[SurveyPageID] [int] NOT NULL,
		[MinScore] [decimal](4, 3) NOT NULL,
		[MaxScore] [decimal](4, 3) NOT NULL,
		[Description] [varchar](1000) NOT NULL,
		CreateUserID		INT NOT NULL CONSTRAINT [DF_SurveyPageScoreRange_CreateUserID] DEFAULT 0,
		CreateDate			DATETIME NOT NULL CONSTRAINT [DF_SurveyPageScoreRange_CreateDate] DEFAULT GETDATE(),
		CreateDateUTC		DATETIME NOT NULL CONSTRAINT [DF_SurveyPageScoreRange_CreateDateUTC] DEFAULT GETUTCDATE(),
		ModifyUserID		INT NOT NULL CONSTRAINT [DF_SurveyPageScoreRange_ModifyUserID] DEFAULT 0,
		ModifyDate			DATETIME NOT NULL CONSTRAINT [DF_SurveyPageScoreRange_ModifyDate] DEFAULT GETDATE(),
		ModifyDateUTC		DATETIME NOT NULL CONSTRAINT [DF_SurveyPageScoreRange_ModifyDateUTC] DEFAULT GETUTCDATE(),
		CONSTRAINT [PKSurveyPageScoreRange] PRIMARY KEY CLUSTERED (	[SurveyPageScoreRangeID] ASC ),
		CONSTRAINT [IX_SurveyPageScoreRange_SurveyPageID_MinScore_MaxScore] UNIQUE NONCLUSTERED  (	[SurveyPageID] ASC,	[MinScore] ASC,	[MaxScore] ASC )
	) ON [PRIMARY]
END
GO