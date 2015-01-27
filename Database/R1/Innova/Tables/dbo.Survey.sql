USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Survey]
	(
		[SurveyID] [int] NOT NULL,
		[SurveyName] [varchar](64) NOT NULL,
		[SurveyDescription] [varchar](max) NULL,
		[RulesetCoreName] [varchar](1000) NULL,		
		[RulesetValidationName] [varchar](1000) NULL,		
		[CompleteMessage] [varchar](max) NULL,
		[CompleteRedirectUrl] [varchar](64) NULL,		
		[StatusCalculator] [varchar](32) NULL,		
		[NotificationType] [varchar](64) NULL,
		[SuggestedContentSiteID] [int] NULL,
		[StatusLabel] [varchar](64) NULL,
		[IsAutoCompleteEnabled] [bit] NOT NULL,
		[IsReviewVisible] [bit] NOT NULL,
		[IsStatusVisible] [bit] NOT NULL,
		[IsActive] [bit] NOT NULL,
		[ReviewTabText] [varchar](64) NOT NULL,
		CreateUserID		INT NOT NULL CONSTRAINT [DF_Survey_CreateUserID] DEFAULT 0,
		CreateDate			DATETIME NOT NULL CONSTRAINT [DF_Survey_CreateDate] DEFAULT GETDATE(),
		CreateDateUTC		DATETIME NOT NULL CONSTRAINT [DF_Survey_CreateDateUTC] DEFAULT GETUTCDATE(),
		ModifyUserID		INT NOT NULL CONSTRAINT [DF_Survey_ModifyUserID] DEFAULT 0,
		ModifyDate			DATETIME NOT NULL CONSTRAINT [DF_Survey_ModifyDate] DEFAULT GETDATE(),
		ModifyDateUTC		DATETIME NOT NULL CONSTRAINT [DF_Survey_ModifyDateUTC] DEFAULT GETUTCDATE(),
		CONSTRAINT [PK_Survey] PRIMARY KEY CLUSTERED (	[SurveyID] ASC )
	) 
	ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS ( SELECT 1 FROM sys.indexes WHERE name='UX_Survey_SurveyName' AND object_id = OBJECT_ID('[dbo].[Survey]') )
	CREATE UNIQUE NONCLUSTERED INDEX [UX_Survey_SurveyName] ON [dbo].[Survey] ( [SurveyName] ASC ) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'IsActive' AND [object_id] = OBJECT_ID(N'[dbo].[Survey]'))
BEGIN
    ALTER TABLE dbo.Survey ADD IsActive BIT NOT NULL DEFAULT(1)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ReviewTabText' AND [object_id] = OBJECT_ID(N'[dbo].[Survey]'))
BEGIN
    ALTER TABLE dbo.Survey ADD [ReviewTabText] varchar(64) NULL
END
GO

IF EXISTS( SELECT 1 FROM dbo.Survey WHERE ReviewTabText IS NULL )
BEGIN
	UPDATE dbo.Survey SET ReviewTabText = 'Review' WHERE ReviewTabText IS NULL
END
GO

ALTER TABLE dbo.Survey ALTER COLUMN [ReviewTabText] varchar(64) NOT NULL
GO

