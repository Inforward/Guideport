USE [__DatabaseName__]

SET ARITHABORT ON

CREATE TABLE #SQL( ID INT IDENTITY( 1, 1 ), Command VARCHAR( MAX ) )
DECLARE @schemaName VARCHAR( 50 ), @tableNamePrefix VARCHAR( 50 ), @tableName VARCHAR( 50 ), @tableEachWhere VARCHAR( MAX ), @excludeTables VARCHAR( MAX ), @includeTables VARCHAR( MAX )

-- Set your table criteria
SELECT @schemaName = '__SchemaName__', @tableNamePrefix = '__TableNamePrefix__', @excludeTables = '__ExcludeTables__', @includeTables = '__IncludeTables__'

INSERT INTO #SQL SELECT 'USE [__StagingDatabaseName__]' + CHAR( 13 ) + CHAR( 10 ) + 'GO' + CHAR( 13 ) + CHAR( 10 )

-- Disable Constraints
SET @tableEachWhere = 'AND O.id IN ( SELECT object_id FROM sys.tables WHERE name LIKE ''' + @tableNamePrefix + '%'' 
					   AND schema_id = SCHEMA_ID( ''' + @schemaName + ''' ) )
					   AND O.name NOT IN ( ''' + REPLACE( @excludeTables, ',', ''',''' ) + ''' )'
					   + CASE WHEN @includeTables <> '' THEN ' AND O.name IN ( ''' + REPLACE( @includeTables, ',', ''',''' ) + ''' )' ELSE '' END

INSERT INTO #SQL SELECT '-- Disable Constraints'
EXECUTE sp_MSforeachtable
	@command1 = 'INSERT INTO #SQL SELECT ''ALTER TABLE ? NOCHECK CONSTRAINT ALL''',
	@whereand = @tableEachWhere

INSERT INTO #SQL SELECT 'GO' + CHAR( 13 ) + CHAR( 10 )

-- Truncate Tables
INSERT INTO #SQL SELECT '' UNION ALL SELECT '-- Truncate Tables'
EXECUTE sp_MSforeachtable
	@command1 = 'INSERT INTO #SQL SELECT ''DELETE FROM ?''',
	@whereand = @tableEachWhere

INSERT INTO #SQL SELECT 'GO' + CHAR( 13 ) + CHAR( 10 )

DECLARE TableCursor CURSOR LOCAL FORWARD_ONLY READ_ONLY FOR
SELECT T.TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES T
WHERE T.TABLE_SCHEMA = @schemaName
	AND T.TABLE_NAME LIKE @tableNamePrefix + '%'
	AND T.TABLE_NAME NOT IN ( SELECT Value FROM [Forms].dbo.ListToTable( @excludeTables ) )
	AND ( @includeTables = '' OR T.TABLE_NAME IN ( SELECT Value FROM [Forms].dbo.ListToTable( @includeTables ) ) )

OPEN TableCursor
FETCH NEXT FROM TableCursor INTO @tableName

WHILE( @@FETCH_STATUS = 0 )
BEGIN

	DECLARE @columns TABLE( ID INT IDENTITY( 1, 1), ColumnType INT, ColumnName VARCHAR( 128 ), IsIdentity INT, Ordinal INT )
	DECLARE	@id INT, @maxID INT, @hasIdentity INT
	DECLARE @cmd0 VARCHAR( MAX ) = '', @cmd1 VARCHAR( MAX ) = '', @cmd2 VARCHAR( MAX ) = '', @cmd3 VARCHAR( MAX ) = ''

	INSERT INTO @columns( ColumnType, ColumnName, IsIdentity, Ordinal )
	SELECT 
		ColumnType = CASE WHEN C.DATA_TYPE LIKE '%CHAR%' THEN 1 WHEN C.DATA_TYPE LIKE '%DATE%' THEN 2 WHEN C.DATA_TYPE LIKE '%TEXT%' THEN 3 ELSE 0 END, 
		ColumnName = C.COLUMN_NAME,
		IsIdentity = COLUMNPROPERTY( OBJECT_ID( C.TABLE_SCHEMA + '.' + C.TABLE_NAME ), C.COLUMN_NAME, 'IsIdentity' ),
		Ordinal = C.ORDINAL_POSITION
	FROM INFORMATION_SCHEMA.COLUMNS C
	WHERE C.TABLE_NAME = @tableName AND C.TABLE_SCHEMA = @schemaName
	ORDER BY C.ORDINAL_POSITION

	IF NOT EXISTS( SELECT * FROM @columns )
	BEGIN
		RAISERROR( 'No columns found for table %s', 16, -1, @tableName )
		RETURN
	END

	SELECT @id = 0, @maxID = MAX( ID ), @hasIdentity = MAX( IsIdentity )
	FROM @columns

	SET @cmd0 = 'SELECT ''---------- BEGIN ' + @schemaName + '.' + @tableName + ' ----------'' UNION ALL '
	SET @cmd0 += 'SELECT ''SET NOCOUNT ON'' UNION ALL '

	IF @hasIdentity = 1
	BEGIN
		SET @cmd0 += 'SELECT ''SET IDENTITY_INSERT [' + @schemaName + '].[' + @tableName + '] ON'' UNION ALL '
		SET @cmd3 = ' UNION ALL SELECT ''SET IDENTITY_INSERT [' + @schemaName + '].[' + @tableName + '] OFF'''
	END

	SELECT	@cmd1 = 'SELECT ''INSERT INTO [' + @schemaName + '].[' + @tableName + ']( '
	SELECT	@cmd2 = ' + ''SELECT '' + '

	WHILE @id < @maxID
	BEGIN
		SELECT @id = MIN( ID ) FROM @columns WHERE ID > @id

		SELECT @cmd1 = @cmd1 + '[' + ColumnName + '], '
		FROM @columns
		WHERE ID = @id

		SELECT @cmd2 = @cmd2
				+ ' CASE WHEN [' + ColumnName + '] IS NULL '
				+	' THEN ''NULL'' '
				+	' ELSE '
				+	  CASE WHEN ColumnType = 1 THEN  ''''''''' + REPLACE( [' + ColumnName + '], '''''''', '''''''''''' ) + ''''''''' 
						WHEN ColumnType = 2 THEN  ''''''''' + ' + 'CONVERT( VARCHAR( 40 ), [' + ColumnName + '] )' + ' + '''''''''
						WHEN ColumnType = 3 THEN  ''''''''' + REPLACE( CAST( [' + ColumnName + '] AS VARCHAR( MAX ) ), '''''''', '''''''''''' ) + ''''''''' 		
						ELSE 'CONVERT( VARCHAR( 40 ), [' + ColumnName + '] )' END
				+ ' END + '', '' + '
		FROM @columns
		WHERE id = @id
	END

	SET @cmd3 += ' UNION ALL SELECT ''---------- END ' + @schemaName + '.' + @tableName + ' ----------'''

	SET @cmd1 = LEFT( @cmd1, LEN( @cmd1 ) - 1 ) + ' ) '' '
	SET @cmd2 = LEFT( @cmd2, LEN( @cmd2 ) - 8 ) + ' + CHAR( 13 ) + CHAR( 10 ) + ''GO'' + CHAR( 13 ) + CHAR( 10 )' + ' FROM [' + @schemaName + '].[' + @tableName + '] WITH( NoLock )'

	SET @cmd1 = REPLACE( @cmd1, ', ) ', ' ) ')
	SET @cmd2 = REPLACE( @cmd2, '+ FROM ', ' FROM ')

	-- SELECT '/*' + @cmd0 + @cmd1 + @cmd2 + @cmd3 + '*/'

	INSERT INTO #SQL
	EXEC( @cmd0 + @cmd1 + @cmd2 + @cmd3 )

	INSERT INTO #SQL SELECT 'GO' + CHAR( 13 ) + CHAR( 10 )
	INSERT INTO #SQL SELECT ''

	DELETE @columns

	FETCH NEXT FROM TableCursor INTO @tableName
END

CLOSE TableCursor
DEALLOCATE TableCursor

-- Re-enable Constraints
INSERT INTO #SQL SELECT '-- Re-enable Constraints'
EXECUTE sp_MSforeachtable
	@command1 = 'INSERT INTO #SQL SELECT ''ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL''',
	@whereand = @tableEachWhere

INSERT INTO #SQL SELECT 'GO' + CHAR( 13 ) + CHAR( 10 )

SELECT Command
FROM #SQL
ORDER BY ID

DROP TABLE #SQL
