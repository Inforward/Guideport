USE [Innova]
GO

-- Add OldID columns to each Response table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OldID' AND [object_id] = OBJECT_ID(N'[dbo].[SurveyResponse]'))
    ALTER TABLE [dbo].[SurveyResponse] ADD [OldID] INT NULL
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OldID' AND [object_id] = OBJECT_ID(N'[dbo].[SurveyResponseAnswer]'))
    ALTER TABLE [dbo].[SurveyResponseAnswer] ADD [OldID] INT NULL
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OldID' AND [object_id] = OBJECT_ID(N'[dbo].[SurveyResponseHistory]'))
    ALTER TABLE [dbo].[SurveyResponseHistory] ADD [OldID] INT NULL
GO

-- Remove previously migrated records
DELETE FROM [dbo].[SurveyResponseHistory] WHERE OldID IS NOT NULL OR SurveyResponseID IN ( SELECT SurveyResponseID FROM [dbo].[SurveyResponse] WHERE OldID IS NOT NULL ) 
DELETE FROM [dbo].[SurveyResponseAnswer] WHERE OldID IS NOT NULL OR SurveyResponseID IN ( SELECT SurveyResponseID FROM [dbo].[SurveyResponse] WHERE OldID IS NOT NULL ) 
DELETE FROM [dbo].[SurveyResponse] WHERE OldID IS NOT NULL


INSERT INTO [dbo].[SurveyResponse] 
(
	[OldID],
	[SurveyID],
	[UserID],
	[SelectedSurveyPageID],
	[CurrentScore],
	[PercentComplete],
	[CreateUserID],
	[CreateDate],
	[CreateDateUtc],
	[ModifyUserID],
	[ModifyDate],
	[ModifyDateUtc],
	[CompleteUserID],
	[CompleteDate],
	[CompleteDateUtc]
)
SELECT
	[SurveyResponseID],
	[SurveyID],
	[UserID],
	[SurveyPageID],
	[Score],
	[PercentComplete],
	CreateUser,
	[CreateDate],
	DATEADD( hh, 7, [CreateDate] ),
	[ModifyUser],
	[ModifyDate],
	DATEADD( hh, 7, [ModifyDate] ),
	[CompleteUser],
	[CompleteDate],
	DATEADD( hh, 7, [CompleteDate] )
FROM
	[Innova_Staging].dbo.SurveyResponse S

GO

INSERT INTO [dbo].[SurveyResponseAnswer] ([OldID],[SurveyResponseID],[SurveyQuestionID],[Answer],[CreateUserID],[CreateDate],[CreateDateUtc])
	SELECT 
		SA.[SurveyAnswerID],
		SR.[SurveyResponseID],
		SA.[SurveyQuestionID],
		SA.[Answer],
		SA.CreateUser,
		SA.[CreateDate],
		DATEADD(hh, 7, SA.[CreateDate] )
	FROM
		[Innova_Staging].dbo.SurveyAnswer SA
		JOIN dbo.SurveyResponse SR 
			ON SA.SurveyResponseID = SR.OldID
GO

INSERT INTO [dbo].[SurveyResponseHistory]
( 
	[OldID],
	[SurveyResponseID],
	[SurveyPageID],
	[ResponseDate],
	[Score],
	[IsLatestScore],
	[PercentComplete],
	[CreateUserID],
	[CreateDate],
	[CreateDateUtc],
	[ModifyUserID],
	[ModifyDate],
	[ModifyDateUtc]
)
SELECT
	SRH.SurveyResponseHistoryID,
	SR.[SurveyResponseID],
	SRH.[SurveyPageID],
	SRH.[ResponseDate],
	SRH.[Score],
	SRH.[IsLatestScore],
	SRH.[PercentComplete],
	SRH.[CreateUser],
	SRH.[CreateDate],
	DATEADD( hh, 7, SRH.[CreateDate] ),
	SRH.[ModifyUser],
	SRH.[ModifyDate],
	DATEADD( hh, 7, SRH.[ModifyDate] )
FROM
	[Innova_Staging].dbo.SurveyResponseHistory SRH
	JOIN dbo.SurveyResponse SR 
		ON SRH.SurveyResponseID = SR.OldID