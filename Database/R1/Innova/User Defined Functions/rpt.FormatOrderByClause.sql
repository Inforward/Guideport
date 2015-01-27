USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE object_id = object_id(N'rpt.FormatOrderByClause') and type in (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
	DROP FUNCTION rpt.FormatOrderByClause
END
GO

CREATE FUNCTION rpt.FormatOrderByClause 
(
	@reportStoredProcedureName VARCHAR( 100 ), @sortColumnList VARCHAR( MAX ), @sortDirectionList VARCHAR( MAX )
)
RETURNS VARCHAR( 1000 ) 
AS  
BEGIN 
	DECLARE @orderBySql VARCHAR( 1000 ) = '', @validSortColumnList VARCHAR( MAX ) = ''
	
	SELECT @validSortColumnList = COALESCE( @validSortColumnList + ',', '' ) + C.DataField
	FROM rpt.[View] V WITH( NoLock )
		JOIN rpt.ViewColumn VC WITH( NoLock ) ON V.ViewID = VC.ViewID
		JOIN rpt.[Column] C WITH( NoLock ) ON VC.ColumnID = C.ColumnID
	WHERE V.StoredProcedureName = @reportStoredProcedureName
	
	-- Defaults
	SELECT
		@sortColumnList = ISNULL( @sortColumnList, '' ),
		@sortDirectionList = ISNULL( @sortDirectionList, '' )
		
	IF @sortColumnList <> ''
	BEGIN	
		SET @orderBySql = ''
		SELECT 
			@orderBySql = @orderBySql + CASE WHEN @orderBySql <> '' THEN ', ' ELSE '' END + ISNULL( S.Value, '' ) + ' ' + ISNULL( SD.Value, 'ASC' )
		FROM 
			dbo.ListToTable( @validSortColumnList ) AS S
			JOIN dbo.ListToTable( @sortColumnList ) AS SC
				ON SC.Value = S.Value
			LEFT JOIN dbo.ListToTable( @sortDirectionList ) AS SD
				ON SC.ID = SD.ID
		ORDER BY SC.ID
	END
	
	RETURN @orderBySql
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.FormatOrderByClause TO svc_Innova_qa
GO