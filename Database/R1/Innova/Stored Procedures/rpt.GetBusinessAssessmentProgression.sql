USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'GetBusinessAssessmentProgression' and schema_id = SCHEMA_ID( 'rpt' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
	DROP PROCEDURE rpt.GetBusinessAssessmentProgression
END
GO

/*
declare @totalRowCount INT = 0
exec rpt.GetBusinessAssessmentProgression
	@firstName = N'', @lastName = '',
	@groupIDList = '', @affiliateIDList = NULL,
	@includeTerminated = 0, @excludeAdvisorsWithNoData = 1,
	@startDate = '1/1/2014', @endDate = '1/7/2015',
	@pageSize = NULL, @pageNumber = NULL, 
	@sortColumnList = 'AdvisorName,OverallScore', 
	@sortDirectionList = 'ASC,ASC',
	@debug = 1,
	@totalRowCount = @totalRowCount OUTPUT
select @totalRowCount
*/
CREATE PROCEDURE rpt.GetBusinessAssessmentProgression
(
	@firstName VARCHAR( 100 ) = NULL, 
	@lastName VARCHAR( 100 ) = NULL,
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
		@firstName = ISNULL( @firstName, '' ),
		@lastName = ISNULL( @lastName, '' ),
		@groupIDList = ISNULL( @groupIDList, '' ),
		@affiliateIDList = ISNULL( @affiliateIDList, '' ),
		@includeTerminated = ISNULL( @includeTerminated, 0 ),
		@excludeAdvisorsWithNoData = ISNULL( @excludeAdvisorsWithNoData, 0 ),
		@startDate = ISNULL( @startDate, @DATE_MIN ),
		@endDate = ISNULL( DATEADD( DAY, DATEDIFF( DAY, 0, @endDate ), 1 ), @DATE_MAX ),
		@sortColumnList = ISNULL( @sortColumnList, 'AdvisorName' ),
		@sortDirectionList = ISNULL( @sortDirectionList, 'ASC' )

	;WITH SURVEY AS
	(
		SELECT
			S.SurveyName,
			SR.SurveyResponseID,
			SR.UserID,
			ResponseDateFirst = MIN( SRH.ResponseDate ),
			ResponseDate = MAX( SRH.ResponseDate )
		FROM
			dbo.SurveyResponse SR WITH( NoLock )
				JOIN dbo.Survey S WITH( NoLock )
					ON SR.SurveyID = S.SurveyID
				JOIN dbo.SurveyResponseHistory SRH WITH( NoLock )
					ON SRH.SurveyResponseID = SR.SurveyResponseID
		WHERE 1 = 1
			AND S.SurveyName = 'Business Assessment'
			AND SRH.ResponseDate >= @startDate AND SRH.ResponseDate < @endDate
		GROUP BY
			S.SurveyName, SR.SurveyResponseID, SR.UserID
	),
	FIRST AS
	(
		SELECT 
			S.SurveyResponseID,
			BusinessDevelopmentScore = SUM( CASE WHEN SP.PageName = 'Business Development' THEN D.DisplayScore ELSE 0 END ),
			OperationalEfficiencyScore = SUM( CASE WHEN SP.PageName = 'Operational Efficiency' THEN D.DisplayScore ELSE 0 END ),
			HumanCapitalScore = SUM( CASE WHEN SP.PageName = 'Human Capital' THEN D.DisplayScore ELSE 0 END ),
			BusinessManagementScore = SUM( CASE WHEN SP.PageName = 'Business Management' THEN D.DisplayScore ELSE 0 END ),
			SuccessionPlanningScore = SUM( CASE WHEN SP.PageName = 'Succession Planning' THEN D.DisplayScore ELSE 0 END )
		FROM
			SURVEY S
				JOIN dbo.SurveyResponseHistory SRH WITH( NoLock )
					ON S.SurveyResponseID = SRH.SurveyResponseID
				JOIN dbo.SurveyPage SP WITH( NoLock )
					ON SRH.SurveyPageID = SP.SurveyPageID
				CROSS APPLY ( SELECT DisplayScore = CASE WHEN SRH.Score IS NULL THEN NULL ELSE FLOOR( SRH.Score * 100 ) / 100 END ) AS D
		WHERE 1 = 1
			AND SRH.ResponseDate = S.ResponseDateFirst
		GROUP BY
			 S.SurveyResponseID
	),
	LAST AS
	(
		SELECT 
			S.SurveyResponseID,
			BusinessDevelopmentScore = SUM( CASE WHEN SP.PageName = 'Business Development' THEN D.DisplayScore ELSE 0 END ),
			BusinessDevelopmentPercentComplete = SUM( CASE WHEN SP.PageName = 'Business Development' THEN SRH.PercentComplete ELSE 0 END ),
			OperationalEfficiencyScore = SUM( CASE WHEN SP.PageName = 'Operational Efficiency' THEN D.DisplayScore ELSE 0 END ),
			OperationalEfficiencyPercentComplete = SUM( CASE WHEN SP.PageName = 'Operational Efficiency' THEN SRH.PercentComplete ELSE 0 END ),
			HumanCapitalScore = SUM( CASE WHEN SP.PageName = 'Human Capital' THEN D.DisplayScore ELSE 0 END ),
			HumanCapitalPercentComplete = SUM( CASE WHEN SP.PageName = 'Human Capital' THEN SRH.PercentComplete ELSE 0 END ),
			BusinessManagementScore = SUM( CASE WHEN SP.PageName = 'Business Management' THEN D.DisplayScore ELSE 0 END ),
			BusinessManagementPercentComplete = SUM( CASE WHEN SP.PageName = 'Business Management' THEN SRH.PercentComplete ELSE 0 END ),
			SuccessionPlanningScore = SUM( CASE WHEN SP.PageName = 'Succession Planning' THEN D.DisplayScore ELSE 0 END ),
			SuccessionPlanningPercentComplete = SUM( CASE WHEN SP.PageName = 'Succession Planning' THEN SRH.PercentComplete ELSE 0 END )
		FROM
			SURVEY S
				JOIN dbo.SurveyResponseHistory SRH WITH( NoLock )
					ON S.SurveyResponseID = SRH.SurveyResponseID
				JOIN dbo.SurveyPage SP WITH( NoLock )
					ON SRH.SurveyPageID = SP.SurveyPageID
				CROSS APPLY ( SELECT DisplayScore = CASE WHEN SRH.Score IS NULL THEN NULL ELSE FLOOR( SRH.Score * 100 ) / 100 END ) AS D
		WHERE 1 = 1
			AND SRH.ResponseDate = S.ResponseDate
		GROUP BY
			 S.SurveyResponseID
	),
	CHANGE AS
	(
		SELECT
			S.SurveyName,
			S.SurveyResponseID,
			S.UserID,
			S.ResponseDateFirst,
			S.ResponseDate,
			L.BusinessDevelopmentScore,
			L.BusinessDevelopmentPercentComplete,
			BusinessDevelopmentChange = CASE WHEN ISNULL( F.BusinessDevelopmentScore, 0 ) <= 0 OR L.BusinessDevelopmentScore IS NULL THEN NULL ELSE L.BusinessDevelopmentScore - F.BusinessDevelopmentScore END,
			L.OperationalEfficiencyScore,
			L.OperationalEfficiencyPercentComplete,
			OperationalEfficiencyChange = CASE WHEN ISNULL( F.OperationalEfficiencyScore, 0 ) <= 0 OR L.OperationalEfficiencyScore IS NULL THEN NULL ELSE L.OperationalEfficiencyScore - F.OperationalEfficiencyScore END,
			L.HumanCapitalScore,
			L.HumanCapitalPercentComplete,
			HumanCapitalChange = CASE WHEN ISNULL( F.HumanCapitalScore, 0 ) <= 0 OR L.HumanCapitalScore IS NULL THEN NULL ELSE L.HumanCapitalScore - F.HumanCapitalScore END,
			L.BusinessManagementScore,
			L.BusinessManagementPercentComplete,
			BusinessManagementChange = CASE WHEN ISNULL( F.BusinessManagementScore, 0 ) <= 0 OR L.BusinessManagementScore IS NULL THEN NULL ELSE L.BusinessManagementScore - F.BusinessManagementScore END,
			L.SuccessionPlanningScore,
			L.SuccessionPlanningPercentComplete,
			SuccessionPlanningChange = CASE WHEN ISNULL( F.SuccessionPlanningScore, 0 ) <= 0 OR L.SuccessionPlanningScore IS NULL THEN NULL ELSE L.SuccessionPlanningScore - F.SuccessionPlanningScore END
		FROM
			SURVEY S
				JOIN LAST L
					ON S.SurveyResponseID = L.SurveyResponseID
				LEFT JOIN FIRST F
					ON S.SurveyResponseID = F.SurveyResponseID
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
		C.SurveyName,
		C.BusinessDevelopmentScore,
		C.BusinessDevelopmentPercentComplete,
		C.BusinessDevelopmentChange,
		C.OperationalEfficiencyScore,
		C.OperationalEfficiencyPercentComplete,
		C.OperationalEfficiencyChange,
		C.HumanCapitalScore,
		C.HumanCapitalPercentComplete,
		C.HumanCapitalChange,
		C.BusinessManagementScore,
		C.BusinessManagementPercentComplete,
		C.BusinessManagementChange,
		C.SuccessionPlanningScore,
		C.SuccessionPlanningPercentComplete,
		C.SuccessionPlanningChange,
		C.ResponseDateFirst,
		C.ResponseDate
	INTO
		#RESULT
	FROM
		dbo.vwUserSummary U
			LEFT JOIN CHANGE C
				ON U.UserID = C.UserID
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
		AND ( @excludeAdvisorsWithNoData = 0 OR C.SurveyResponseID IS NOT NULL )

	SET @totalRowCount = @@ROWCOUNT

	EXECUTE rpt.GetPagedResults '#RESULT', @storedProcedureName, @sortColumnList, @sortDirectionList, @pageSize, @pageNumber, @debug

	SET NOCOUNT OFF
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.GetBusinessAssessmentProgression TO svc_Innova_qa
GO
