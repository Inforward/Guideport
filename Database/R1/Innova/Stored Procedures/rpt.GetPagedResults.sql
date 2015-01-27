USE Innova
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'rpt.GetPagedResults') AND type in (N'P', N'PC'))
    DROP PROCEDURE rpt.GetPagedResults
GO

CREATE PROCEDURE rpt.GetPagedResults
(
      @tempTableName VARCHAR( 50 ), @callingStoredProcedureName VARCHAR( 100 ), 
      @sortColumnList VARCHAR( MAX ), @sortDirectionList VARCHAR( MAX ), @pageSize INT, @pageNumber INT,
      @debug BIT = 0
)
AS
BEGIN
    DECLARE @sql NVARCHAR( MAX ), @firstRow INT, @lastRow INT, @orderBySql VARCHAR( MAX )

    -- Get Paging Values
    SET @orderBySql = rpt.FormatOrderByClause( @callingStoredProcedureName, @sortColumnList, @sortDirectionList )
    SELECT @firstRow = [FirstRow], @lastRow = [LastRow], @pageSize = PageSize, @pageNumber = PageNumber
    FROM dbo.GetPageRowNumbers( @pageSize, @pageNumber )

    --Return Results
    SET @sql = '					
        WITH RESULT AS
        (
            SELECT
                R.*,
                ROW_NUMBER() OVER( ORDER BY ' + @orderBySql + ' ) AS [RowNumber]				
            FROM
                ' + @tempTableName + ' R
        )
        SELECT
            TOP ( @pageSize )
            R.*
        FROM
            RESULT R
        WHERE
            R.RowNumber BETWEEN @firstRow AND @lastRow
        ORDER BY
            R.RowNumber'

    IF @debug = 1
    BEGIN
        EXEC dbo.PrintLong @sql
    END

    EXECUTE sp_executesql @stmt = @sql,
        @params = N'@pageSize INT, @firstRow INT, @lastRow INT',
        @pageSize = @pageSize, @firstRow = @firstRow, @lastRow = @lastRow

END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON rpt.GetPagedResults TO svc_Innova_qa
GO
