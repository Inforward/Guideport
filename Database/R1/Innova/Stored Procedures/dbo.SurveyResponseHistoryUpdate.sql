USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'SurveyResponseHistoryUpdate' and schema_id = SCHEMA_ID( 'dbo' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
	DROP PROCEDURE [dbo].[SurveyResponseHistoryUpdate]
END
GO


CREATE PROCEDURE [dbo].[SurveyResponseHistoryUpdate]
(
	@surveyResponseID INT,
	@surveyPageIdList VARCHAR( 8000 ),
	@scoreList VARCHAR( 8000 ),
	@percentCompleteList VARCHAR( 8000 ),
	@responseDate DATETIME,
	@modifyUser INT,
	@modifyDate DATETIME,
	@modifyDateUTC DATETIME
)
AS
BEGIN
	
	;WITH INCOMING
	AS
	(
		SELECT SurveyPageID = CAST( SPL.Value AS INT ), Score = CAST( SL.Value AS DECIMAL( 4, 3 ) ), PercentComplete = CAST( PCL.Value AS DECIMAL( 4, 3 ) )
		FROM dbo.ListToTable( @surveyPageIdList ) SPL
				JOIN dbo.ListToTable( @scoreList ) SL ON SPL.ID = SL.ID
				JOIN dbo.ListToTable( @percentCompleteList ) PCL ON SPL.ID = PCL.ID
	)
	MERGE dbo.SurveyResponseHistory AS SRH
	USING INCOMING ON SRH.SurveyResponseID = @surveyResponseID AND INCOMING.SurveyPageID = SRH.SurveyPageID AND SRH.ResponseDate = @responseDate
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			SurveyResponseID, SurveyPageID, ResponseDate, Score, IsLatestScore, PercentComplete,
			CreateUserID, CreateDate, CreateDateUTC, ModifyUserID, ModifyDate, ModifyDateUTC
		)
		VALUES
		(
			@surveyResponseID, INCOMING.SurveyPageID, @responseDate, INCOMING.Score, 1, INCOMING.PercentComplete,
			@modifyUser, @modifyDate, @modifyDateUTC, @modifyUser, @modifyDate, @modifyDateUTC
		)
	WHEN MATCHED THEN
		UPDATE
		SET Score = INCOMING.Score,
			PercentComplete = INCOMING.PercentComplete,
			ModifyUserID = @modifyUser, 
			ModifyDate = @modifyDate,
			ModifyDateUTC = @modifyDateUTC;

	-- Mark any previous day scores as not the latest
	UPDATE SRH
	SET IsLatestScore = 0
	FROM dbo.SurveyResponseHistory SRH WITH( NoLock )
		JOIN dbo.ListToTable( @surveyPageIdList ) SPL ON SRH.SurveyPageID = CAST( SPL.Value AS INT )
	WHERE SRH.SurveyResponseID = @surveyResponseID
		AND SRH.ResponseDate <> @responseDate
		AND IsLatestScore = 1
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [dbo].[SurveyResponseHistoryUpdate] TO svc_Innova_qa
GO