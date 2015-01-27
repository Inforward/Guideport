USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'GetBusinessAssessmentSummary' and schema_id = SCHEMA_ID( 'rpt' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
	DROP PROCEDURE rpt.GetBusinessAssessmentSummary
END
GO

/*
declare @totalRowCount INT = 0
exec rpt.GetBusinessAssessmentSummary
	@groupIDList = '', @affiliateIDList = '2',
	@includeTerminated = NULL, @excludeAdvisorsWithNoData = 0,
	@startDate = '1/1/2014', @endDate = '1/7/2015',
	@pageSize = NULL, @pageNumber = NULL, 
	@sortColumnList = NULL, 
	@sortDirectionList = NULL,
	@debug = 1,
	@totalRowCount = @totalRowCount OUTPUT
select @totalRowCount
*/
CREATE PROCEDURE rpt.GetBusinessAssessmentSummary
(
	@groupIDList VARCHAR( MAX ) = NULL,
	@affiliateIDList VARCHAR( MAX ) = NULL,
	@includeTerminated BIT = NULL,
	@excludeAdvisorsWithNoData BIT = NULL,
	@startDate DATETIME = NULL,
	@endDate DATETIME = NULL,
	@pageSize INT = NULL,
	@pageNumber INT = NULL,
	@sortColumnList VARCHAR( MAX ) = NULL,
	@sortDirectionList VARCHAR( MAX ) = NULL,
	@debug BIT = 0,
	@totalRowCount INT OUTPUT
) 
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @storedProcedureName VARCHAR( 100 ) = dbo.GetFullObjectName( @@PROCID ),
			@DATE_MIN DATETIME = CAST( '1/1/1753' AS DATETIME ), @DATE_MAX DATETIME = CAST( '12/31/9999 23:59:59:997' AS DATETIME )

	-- Default Values
	SELECT
		@groupIDList = ISNULL( @groupIDList, '' ),
		@affiliateIDList = ISNULL( @affiliateIDList, '' ),
		@includeTerminated = ISNULL( @includeTerminated, 0 ),
		@excludeAdvisorsWithNoData = ISNULL( @excludeAdvisorsWithNoData, 0 ),
		@startDate = ISNULL( @startDate, @DATE_MIN ),
		@endDate = ISNULL( DATEADD( DAY, DATEDIFF( DAY, 0, @endDate ), 1 ), @DATE_MAX ),
		@sortColumnList = ISNULL( @sortColumnList, 'QuestionOrdinal' ),
		@sortDirectionList = ISNULL( @sortDirectionList, 'ASC' )

	;WITH SURVEY AS
	(
		SELECT
			QuestionOrdinal = CAST( O.PageOrder + O.QuestionOrder AS DECIMAL( 3, 2 ) ),
			SP.PageName,
			SQ.SurveyQuestionID,
			SQ.QuestionName,
			SQ.QuestionText
		FROM
			dbo.Survey S WITH( NoLock )
				JOIN dbo.SurveyPage SP WITH( NoLock )
					ON S.SurveyID = SP.SurveyID
				JOIN dbo.SurveyQuestion SQ WITH( NoLock )
					ON SP.SurveyPageID = SQ.SurveyPageID
				CROSS APPLY
				( 
					SELECT PageOrder = CAST( ( SP.SortOrder / 10 ) AS DECIMAL( 4, 2 ) ), 
						   QuestionOrder = ( CAST( SQ.SortOrder AS DECIMAL( 5, 2 ) ) / 1000 )
				) AS O
		WHERE 1 = 1
			AND S.SurveyName = 'Business Assessment'
	),	
	SURVEY_SAYS AS
	(
		SELECT
			SRA.SurveyQuestionID,
			SR.SurveyID,
			SR.UserID,
			AnswerScore = CASE WHEN SQA.AnswerWeight IS NOT NULL THEN SQA.AnswerWeight ELSE NULL END
		FROM
			dbo.vwUserSummary U WITH( NoLock )
				JOIN dbo.SurveyResponse SR WITH( NoLock )
					ON U.UserID = SR.UserID
				JOIN dbo.Survey S WITH( NoLock )
					ON SR.SurveyID = S.SurveyID
						AND S.SurveyName = 'Business Assessment'
				JOIN dbo.SurveyResponseAnswer SRA WITH( NoLock )
					ON SR.SurveyResponseID = SRA.SurveyResponseID
				JOIN dbo.SurveyQuestionAnswer SQA WITH( NoLock )
					ON SRA.SurveyQuestionID = SQA.SurveyQuestionID
						AND SRA.Answer = SQA.AnswerText					
				LEFT JOIN dbo.ListToTable( @affiliateIDList ) AIL
					ON U.AffiliateID = CAST( AIL.Value AS INT )
				LEFT JOIN
				(
					SELECT GU.MemberUserID, GroupCount = COUNT( * )
					FROM dbo.GroupUser GU WITH( NoLock )
						JOIN grp.GetGroupIDsFromHierarchy( @groupIDList ) GH
							ON GU.GroupID = GH.GroupID
					GROUP BY GU.MemberUserID
				) AS G
					ON U.UserID = G.MemberUserID
		WHERE 1 = 1
			AND SR.ModifyDate >= @startDate AND SR.ModifyDate < @endDate
			AND ( @groupIDList = '' OR G.MemberUserID IS NOT NULL )
			AND ( @affiliateIDList = '' OR AIL.Value IS NOT NULL )
			AND ( @includeTerminated = 1 OR U.TerminateDate IS NULL )
	)

	SELECT
		S.PageName,
		S.QuestionName,
		S.QuestionText,
		S.QuestionOrdinal,
		TotalAnswers = COUNT( SR.AnswerScore ),
		AverageScore = AVG( SR.AnswerScore )
	INTO
		#RESULT
	FROM
		SURVEY S
			LEFT JOIN SURVEY_SAYS SR
				ON S.SurveyQuestionID = SR.SurveyQuestionID
	WHERE 1 = 1
		AND ( @excludeAdvisorsWithNoData = 0 OR SR.SurveyQuestionID IS NOT NULL )

	GROUP BY
		S.PageName, S.QuestionName, S.QuestionText, S.QuestionOrdinal

	SET @totalRowCount = @@ROWCOUNT

	EXECUTE rpt.GetPagedResults '#RESULT', @storedProcedureName, @sortColumnList, @sortDirectionList, @pageSize, @pageNumber, @debug

	SET NOCOUNT OFF
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.GetBusinessAssessmentSummary TO svc_Innova_qa
GO
