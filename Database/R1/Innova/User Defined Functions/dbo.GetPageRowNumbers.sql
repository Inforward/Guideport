USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE object_id = object_id(N'dbo.GetPageRowNumbers') and type in (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
	DROP FUNCTION [dbo].[GetPageRowNumbers]
END
GO

CREATE FUNCTION dbo.GetPageRowNumbers
(
	@pageSize INT, @pageNumber INT
)
RETURNS @table TABLE( [FirstRow] INT, [LastRow] INT, PageSize INT, PageNumber INT )
AS  
BEGIN
	DECLARE @firstRow INT = 1, @lastRow INT = 2147483647, @INT_MAX INT = 2147483647

	-- Defaults
	SELECT @pageSize = ISNULL( @pageSize, 0 ), @pageNumber = ISNULL( @pageNumber, 0 )
	
	IF @pageSize = 0
	BEGIN
		SET @pageSize = @INT_MAX
	END

	IF @pageNumber = 0
	BEGIN
		SET @pageNumber = 1
	END

	IF @pageSize <> @INT_MAX AND @pageNumber > 0 
	BEGIN
		SET @firstRow = ( @pageSize * ( @pageNumber - 1 ) ) + 1
		SET @lastRow = @pageSize * @pageNumber
	END
	
	INSERT @table( [FirstRow], [LastRow], PageSize, PageNumber )
	VALUES( @firstRow, @lastRow, @pageSize, @pageNumber )
	
	RETURN
END

GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT SELECT ON [dbo].[GetPageRowNumbers] TO svc_Innova_qa
GO