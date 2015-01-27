USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'GetBusinessAssessmentScores' and schema_id = SCHEMA_ID( 'rpt' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
	DROP PROCEDURE rpt.GetBusinessAssessmentScores
END
GO

/*
declare @totalRowCount INT = 0
exec rpt.GetBusinessAssessmentScores
	@firstName = N'', @lastName = '',
	@groupIDList = '', @affiliateIDList = NULL,
	@includeTerminated = 0, @excludeAdvisorsWithNoData = 1,
	@pageSize = NULL, @pageNumber = NULL, 
	@sortColumnList = 'AdvisorName,OverallScore', 
	@sortDirectionList = 'ASC,ASC',
	@debug = 1,
	@totalRowCount = @totalRowCount OUTPUT
select @totalRowCount
*/
CREATE PROCEDURE rpt.GetBusinessAssessmentScores
(
	@firstName VARCHAR( 100 ) = NULL, 
	@lastName VARCHAR( 100 ) = NULL,
	@groupIDList VARCHAR( MAX ) = NULL,
	@affiliateIDList VARCHAR( MAX ) = NULL,
	@includeTerminated BIT = NULL,
	@excludeAdvisorsWithNoData BIT = NULL,
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

	DECLARE @storedProcedureName VARCHAR( 100 ) = dbo.GetFullObjectName( @@PROCID )

	-- Default Values
	SELECT
		@firstName = ISNULL( @firstName, '' ),
		@lastName = ISNULL( @lastName, '' ),
		@groupIDList = ISNULL( @groupIDList, '' ),
		@affiliateIDList = ISNULL( @affiliateIDList, '' ),
		@includeTerminated = ISNULL( @includeTerminated, 0 ),
		@excludeAdvisorsWithNoData = ISNULL( @excludeAdvisorsWithNoData, 0 ),
		@sortColumnList = ISNULL( @sortColumnList, 'AdvisorName' ),
		@sortDirectionList = ISNULL( @sortDirectionList, 'ASC' )

	;WITH SURVEY AS
	(
		SELECT
			S.SurveyName,
			SR.SurveyResponseID,
			SR.UserID,
			OverallScore = SR.CurrentScore,
			SR.PercentComplete,
			BusinessDevelopmentScore = SUM( CASE WHEN SP.PageName = 'Business Development' THEN D.DisplayScore ELSE 0 END ),
			OperationalEfficiencyScore = SUM( CASE WHEN SP.PageName = 'Operational Efficiency' THEN D.DisplayScore ELSE 0 END ),
			HumanCapitalScore = SUM( CASE WHEN SP.PageName = 'Human Capital' THEN D.DisplayScore ELSE 0 END ),
			BusinessManagementScore = SUM( CASE WHEN SP.PageName = 'Business Management' THEN D.DisplayScore ELSE 0 END ),
			SuccessionPlanningScore = SUM( CASE WHEN SP.PageName = 'Succession Planning' THEN D.DisplayScore ELSE 0 END )
		FROM
			dbo.SurveyResponse SR WITH( NoLock )
				JOIN dbo.Survey S WITH( NoLock )
					ON SR.SurveyID = S.SurveyID
						AND S.SurveyName = 'Business Assessment'
				JOIN dbo.SurveyResponseHistory SRH WITH( NoLock )
					ON SRH.SurveyResponseID = SR.SurveyResponseID
						AND SRH.IsLatestScore = 1
				JOIN dbo.SurveyPage SP WITH( NoLock )
					ON SRH.SurveyPageID = SP.SurveyPageID
				CROSS APPLY ( SELECT DisplayScore = CASE WHEN SRH.Score IS NULL THEN NULL ELSE FLOOR( SRH.Score * 100 ) / 100 END ) AS D
		GROUP BY
			S.SurveyName, SR.SurveyResponseID, SR.UserID, SR.CurrentScore, SR.PercentComplete
	)
		
	SELECT
		AdvisorUserID = U.UserID,
		AdvisorName = U.UserName,
		AdvisorPhoneNo = U.UserPhoneNo,
		U.BusinessConsultantName,
		BrokerDealer = U.AffiliateName,
		U.TerminateDate,
		U.StartDate,
		U.GDC_T12,
		U.ProfileID,
		S.SurveyName,
		S.OverallScore,
		S.BusinessDevelopmentScore,
		S.OperationalEfficiencyScore,
		S.HumanCapitalScore,
		S.BusinessManagementScore,
		S.SuccessionPlanningScore,
		S.PercentComplete
	INTO
		#RESULT
	FROM
		dbo.vwUserSummary U
			LEFT JOIN SURVEY S
				ON U.UserID = S.UserID
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
		AND ( @groupIDList = '' OR G.MemberUserID IS NOT NULL )
		AND ( @affiliateIDList = '' OR AIL.Value IS NOT NULL )
		AND ( @firstName = '' OR U.DisplayFirstName LIKE @firstName + '%' )
		AND ( @lastName = '' OR U.DisplayLastName LIKE @lastName + '%' )
		AND ( @includeTerminated = 1 OR U.TerminateDate IS NULL )
		AND ( @excludeAdvisorsWithNoData = 0 OR S.SurveyResponseID IS NOT NULL )

	SET @totalRowCount = @@ROWCOUNT

	EXECUTE rpt.GetPagedResults '#RESULT', @storedProcedureName, @sortColumnList, @sortDirectionList, @pageSize, @pageNumber, @debug

	SET NOCOUNT OFF
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.GetBusinessAssessmentScores TO svc_Innova_qa
GO
