USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'GetEnrollmentActivity' and schema_id = SCHEMA_ID( 'rpt' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
    DROP PROCEDURE rpt.GetEnrollmentActivity
END
GO

/*
declare @totalRowCount INT = 0
exec rpt.GetEnrollmentActivity
    @firstName = N'', @lastName = '',
    @groupIDList = '', @affiliateIDList = NULL,
    @includeTerminated = 0, @excludeAdvisorsWithNoData = 1,
	@startDate = '1/1/2014', @endDate = '1/1/2015',
    @pageSize = NULL, @pageNumber = NULL, 
    @sortColumnList = 'AdvisorName,OverallScore', 
    @sortDirectionList = 'ASC,ASC',
    @debug = 1,
    @totalRowCount = @totalRowCount OUTPUT
select @totalRowCount
*/
CREATE PROCEDURE rpt.GetEnrollmentActivity
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

    ;WITH ENROLLMENT AS
    (
        SELECT
            SR.UserID,
            SR.SurveyResponseID,
            CreateDate = SR.CreateDateUTC,
            CompleteDate = SR.CompleteDateUTC,
            ModifyDate = SR.ModifyDateUTC,
            Activity = MAX( CASE WHEN SR.CreateDate = SR.ModifyDate THEN 'Created' ELSE 'Modified' END ),
            Continuity = MAX( CASE WHEN SQ.QuestionName = 'Continuity' THEN SRA.Answer ELSE NULL END ),
            ContinuityOnFile = MAX( CASE WHEN SQ.QuestionName = 'ContinuityProvide' THEN SRA.Answer ELSE NULL END ),
            Succession = MAX( CASE WHEN SQ.QuestionName = 'Succession' THEN SRA.Answer ELSE NULL END ),
            Acquisition = MAX( CASE WHEN SQ.QuestionName = 'Acquisition' THEN SRA.Answer ELSE NULL END ),
            QualifiedBuyer = MAX( CASE WHEN SQ.QuestionName = 'NeedAcquisitionFunding' THEN SRA.Answer ELSE NULL END ),
            SuccessionSellTimeframe = MAX( CASE WHEN SQ.QuestionName = 'SuccessionTime' THEN SRA.Answer ELSE NULL END ),
            SuccessionBuyTimeframe = MAX( CASE WHEN SQ.QuestionName = 'TimeframeS' THEN SRA.Answer ELSE NULL END ),
            AcquisitionBuyTimeframe = MAX( CASE WHEN SQ.QuestionName = 'TimeframeB' THEN SRA.Answer ELSE NULL END )
        FROM
            dbo.SurveyResponse SR WITH( NoLock )
                JOIN dbo.Survey S WITH( NoLock )
                    ON SR.SurveyID = S.SurveyID
                JOIN dbo.SurveyResponseAnswer SRA WITH( NoLock )
                    ON SR.SurveyResponseID = SRA.SurveyResponseID
                JOIN dbo.SurveyQuestion SQ WITH( NoLock )
                    ON SRA.SurveyQuestionID = SQ.SurveyQuestionID
        WHERE 1 = 1
            AND S.SurveyName = 'Enrollment'
            AND SR.ModifyDate >= @startDate AND SR.ModifyDate < @endDate
        GROUP BY
            SR.UserID, SR.SurveyResponseID, SR.CreateDateUTC, SR.CompleteDateUTC, SR.ModifyDateUTC
    )

    SELECT
		AdvisorUserID = U.UserID,
		AdvisorName = U.UserName,
		AdvisorPhoneNo = U.UserPhoneNo,
		AdvisorEmail = U.UserEmail,
		AdvisorState = U.UserState,
		U.BusinessConsultantName,
		BrokerDealer = U.AffiliateName,
		U.TerminateDate,
		U.StartDate,
		U.GDC_T12,
		U.AUM,
		U.ProfileID,
        E.SurveyResponseID,
        E.CreateDate,
        E.CompleteDate,
        E.ModifyDate,
        E.Activity,
        E.Continuity,
        E.ContinuityOnFile,
        E.Succession,
        E.Acquisition,
        E.QualifiedBuyer,
        E.SuccessionSellTimeframe,
        E.SuccessionBuyTimeframe,
        E.AcquisitionBuyTimeframe
    INTO
        #RESULT
    FROM
        dbo.vwUserSummary U
            LEFT JOIN ENROLLMENT E
                ON U.UserID = E.UserID
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
        AND ( @excludeAdvisorsWithNoData = 0 OR E.SurveyResponseID IS NOT NULL )

    SET @totalRowCount = @@ROWCOUNT

    EXECUTE rpt.GetPagedResults '#RESULT', @storedProcedureName, @sortColumnList, @sortDirectionList, @pageSize, @pageNumber, @debug

    SET NOCOUNT OFF
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.GetEnrollmentActivity TO svc_Innova_qa

GO
