USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'GetBusinessPlanProgression' and schema_id = SCHEMA_ID( 'rpt' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
	DROP PROCEDURE rpt.GetBusinessPlanProgression
END
GO

/*
declare @totalRowCount INT = 0
exec rpt.GetBusinessPlanProgression
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
CREATE PROCEDURE rpt.GetBusinessPlanProgression
(
	@firstName VARCHAR( 100 ) = NULL, 
	@lastName VARCHAR( 100 ) = NULL,
	@groupIDList VARCHAR( MAX ) = NULL,
	@affiliateIDList VARCHAR( MAX ) = NULL,
	@includeTerminated BIT = NULL,
	@excludeAdvisorsWithNoData BIT = NULL,
	@year INT = NULL,
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
		@year = ISNULL( @year, YEAR( GETDATE() ) ),
		@sortColumnList = ISNULL( @sortColumnList, 'AdvisorName' ),
		@sortDirectionList = ISNULL( @sortDirectionList, 'ASC' )

	;WITH BUSINESS_PLAN AS
	(
		SELECT
			BP.UserID,
			BP.BusinessPlanID,
			BusinessPlanYear = BP.[Year],
			ObjectiveName = O.Name,
			ObjectiveValue = 
					CASE O.DataType
						WHEN 'decimal' THEN '$' + CONVERT( VARCHAR, CAST( O.Value AS MONEY ), 1 )
						WHEN 'integer' THEN REPLACE( CONVERT( VARCHAR, CAST( O.Value AS MONEY ), 1 ), '.00', '')
						WHEN 'percent' THEN CONVERT( VARCHAR, CAST( O.Value AS MONEY ), 1 ) + ' %'
						ELSE O.Value
					END,
			ObjectiveValueRaw = O.Value,
			CompleteDate = O.EstimatedCompletionDate,
			PercentComplete = CASE WHEN PercentComplete > 0 THEN CAST( PercentComplete AS FLOAT ) / 100 ELSE 0 END
		FROM
			dbo.BusinessPlan BP WITH( NoLock )
				JOIN dbo.BusinessPlanObjective O WITH( NoLock )
					ON BP.BusinessPlanID = O.BusinessPlanID
		WHERE
			BP.[Year] = @year
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
		BP.BusinessPlanYear,
		BP.ObjectiveName,
		BP.ObjectiveValue,
		BP.ObjectiveValueRaw,
		BP.CompleteDate,
		BP.PercentComplete
	INTO
		#RESULT
	FROM
		dbo.vwUserSummary U
			LEFT JOIN BUSINESS_PLAN BP
				ON U.UserID = BP.UserID
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
		AND ( @excludeAdvisorsWithNoData = 0 OR BP.BusinessPlanID IS NOT NULL )

	SET @totalRowCount = @@ROWCOUNT

	EXECUTE rpt.GetPagedResults '#RESULT', @storedProcedureName, @sortColumnList, @sortDirectionList, @pageSize, @pageNumber, @debug

	SET NOCOUNT OFF
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.GetBusinessPlanProgression TO svc_Innova_qa
GO
