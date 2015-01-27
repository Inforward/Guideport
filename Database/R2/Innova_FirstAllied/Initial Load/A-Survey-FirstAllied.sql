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

DECLARE @SurveyResponses TABLE ( SurveyResponseID INT )

INSERT INTO @SurveyResponses
	SELECT SurveyResponseID FROM dbo.SurveyResponse WHERE OldID IS NOT NULL
	UNION 
	SELECT SurveyResponseID FROM dbo.SurveyResponse WHERE UserID IN ( SELECT UserID FROM dbo.SurveyResponse WHERE OldID IS NOT NULL )

-- Remove any records entered by users whose records we are about to migrate
DELETE FROM [dbo].[SurveyResponseHistory] WHERE SurveyResponseID IN ( SELECT SurveyResponseID FROM @SurveyResponses )
DELETE FROM [dbo].[SurveyResponseAnswer] WHERE SurveyResponseID IN ( SELECT SurveyResponseID FROM @SurveyResponses )
DELETE FROM [dbo].[SurveyResponse] WHERE SurveyResponseID IN ( SELECT SurveyResponseID FROM @SurveyResponses )
GO

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
	S.[SurveyResponseID],
	S.[SurveyID],
	ISNULL( U.UserID, S.[UserID] ),
	S.[SurveyPageID],
	S.[Score],
	S.[PercentComplete],
	S.CreateUser,
	S.[CreateDate],
	DATEADD( hh, 7, S.[CreateDate] ),
	S.[ModifyUser],
	S.[ModifyDate],
	DATEADD( hh, 7, S.[ModifyDate] ),
	S.[CompleteUser],
	S.[CompleteDate],
	DATEADD( hh, 7, S.[CompleteDate] )
FROM
	[Innova_Staging].dbo.SurveyResponse S
	LEFT JOIN Innova_Staging.dbo.AE_User A WITH( NoLock )
		ON S.[UserID] = A.UserID
	LEFT JOIN [dbo].vwUserSummary U WITH( NoLock )
		ON A.PR_ID = U.ProfileID
			AND U.AffiliateID = 1
			AND ISNUMERIC(U.ProfileID) = 1

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


