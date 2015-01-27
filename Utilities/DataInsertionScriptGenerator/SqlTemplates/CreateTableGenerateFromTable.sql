USE [__DatabaseName__]

SET ARITHABORT ON

CREATE TABLE #SQL( ID INT IDENTITY( 1, 1 ), Command VARCHAR( MAX ) )
DECLARE @schemaName VARCHAR( 50 ), @tableNamePrefix VARCHAR( 50 ), @tableName VARCHAR( 50 ), @excludeTables VARCHAR( MAX ), @includeTables VARCHAR( MAX )

-- Set your table criteria
SELECT @schemaName = '__SchemaName__', @tableNamePrefix = '__TableNamePrefix__', @excludeTables = '__ExcludeTables__', @includeTables = '__IncludeTables__'

INSERT INTO #SQL SELECT 'USE [__StagingDatabaseName__]' + CHAR( 13 ) + CHAR( 10 ) + 'GO' + CHAR( 13 ) + CHAR( 10 )

DECLARE TableCursor CURSOR LOCAL FORWARD_ONLY READ_ONLY FOR
SELECT T.TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES T
WHERE T.TABLE_SCHEMA = @schemaName
	AND T.TABLE_NAME LIKE @tableNamePrefix + '%'
	AND T.TABLE_NAME NOT IN ( SELECT Value FROM [Forms].dbo.ListToTable( @excludeTables ) )
	AND ( @includeTables = '' OR T.TABLE_NAME IN ( SELECT Value FROM [Forms].dbo.ListToTable( @includeTables ) ) )

IF @schemaName <> 'dbo'
	INSERT INTO #SQL SELECT 'IF NOT EXISTS ( SELECT  schema_name FROM information_schema.schemata WHERE schema_name = ''' + @schemaName + ''' ) '
							+ CHAR( 13 ) + CHAR( 10 ) + 'BEGIN'
							+ CHAR( 13 ) + CHAR( 10 ) + CHAR( 9 ) + 'EXEC sp_executesql N''CREATE SCHEMA [' + @schemaName + ']''' + CHAR( 9 ) + CHAR( 13 ) + CHAR( 10 )
							+ 'END' + CHAR( 13 ) + CHAR( 10 ) + 'GO' + CHAR( 13 ) + CHAR( 10 )

OPEN TableCursor
FETCH NEXT FROM TableCursor INTO @tableName

WHILE( @@FETCH_STATUS = 0 )
BEGIN

	DECLARE  @object_id INT, @object_name SYSNAME

	SELECT 
		@object_name = '[' + s.name + '].[' + o.name + ']',
		@object_id = o.[object_id]
	FROM sys.objects o WITH (NOWAIT)
	JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
	WHERE s.name = @schemaName AND o.name = @tableName
		AND o.[type] = 'U'
		AND o.is_ms_shipped = 0

	DECLARE @SQL NVARCHAR(MAX) = ''

	SELECT @SQL = 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[' + @schemaName + '].[' + @tableName + ']'') AND type in (N''U''))' + CHAR( 13 ) + CHAR( 10 ) 
				+ 'BEGIN' + CHAR( 13 ) + CHAR( 10 )
				+ CHAR( 9 ) + 'DROP TABLE [' + @schemaName + '].[' + @tableName + ']' + CHAR( 13 ) + CHAR( 10 )
				+ 'END' + CHAR( 13 ) + CHAR( 10 )
				+ 'GO' + CHAR( 13 ) + CHAR( 10 ) + CHAR( 13 ) + CHAR( 10 )

	;WITH index_column AS 
	(
		SELECT 
			  ic.[object_id]
			, ic.index_id
			, ic.is_descending_key
			, ic.is_included_column
			, c.name
		FROM sys.index_columns ic WITH (NOWAIT)
		JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id
		WHERE ic.[object_id] = @object_id
	),
	fk_columns AS 
	(
		 SELECT 
			  k.constraint_object_id
			, cname = c.name
			, rcname = rc.name
		FROM sys.foreign_key_columns k WITH (NOWAIT)
		JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id AND rc.column_id = k.referenced_column_id 
		JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id
		WHERE k.parent_object_id = @object_id
	)

	SELECT @SQL += 'CREATE TABLE ' + @object_name + CHAR( 13 ) + CHAR( 10 ) + '(' + CHAR( 13 ) + CHAR( 10 ) + STUFF((
		SELECT CHAR(9) + ', [' + c.name + '] ' + 
			CASE WHEN c.is_computed = 1
				THEN 'AS ' + cc.[definition] 
				ELSE UPPER(tp.name) + 
					CASE WHEN tp.name IN ('varchar', 'char', 'varbinary', 'binary')
						   THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(5)) END + ')'
						 WHEN tp.name IN ('nvarchar', 'nchar')
						   THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(5)) END + ')'
						 WHEN tp.name IN ('datetime2', 'time2', 'datetimeoffset') 
						   THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'
						 WHEN tp.name = 'decimal' 
						   THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'
						ELSE ''
					END +
					CASE WHEN c.collation_name IS NOT NULL THEN ' COLLATE ' + c.collation_name ELSE '' END +
					CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END +
					CASE WHEN dc.[definition] IS NOT NULL THEN ' DEFAULT' + dc.[definition] ELSE '' END + 
					CASE WHEN ic.is_identity = 1 THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS CHAR(1)) + ')' ELSE '' END 
			END + CHAR( 13 ) + CHAR( 10 )
		FROM sys.columns c WITH (NOWAIT)
		JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id
		LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id] AND c.column_id = cc.column_id
		LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0 AND c.[object_id] = dc.parent_object_id AND c.column_id = dc.parent_column_id
		LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1 AND c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id
		WHERE c.[object_id] = @object_id
		ORDER BY c.column_id
		FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ')
		+ ISNULL((SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' + 
						(SELECT STUFF((
							 SELECT ', [' + c.name + '] ' + CASE WHEN ic.is_descending_key = 1 THEN 'DESC' ELSE 'ASC' END
							 FROM sys.index_columns ic WITH (NOWAIT)
							 JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id
							 WHERE ic.is_included_column = 0
								 AND ic.[object_id] = k.parent_object_id 
								 AND ic.index_id = k.unique_index_id     
							 FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''))
				+ ')' + CHAR( 13 ) + CHAR( 10 )
				FROM sys.key_constraints k WITH (NOWAIT)
				WHERE k.parent_object_id = @object_id 
					AND k.[type] = 'PK'), '') + ')'  + CHAR( 13 ) + CHAR( 10 )
/*
		+ ISNULL((SELECT (
			SELECT CHAR(13) +
				 'ALTER TABLE ' + @object_name + ' WITH' 
				+ CASE WHEN fk.is_not_trusted = 1 
					THEN ' NOCHECK' 
					ELSE ' CHECK' 
				  END + 
				  ' ADD CONSTRAINT [' + fk.name  + '] FOREIGN KEY(' 
				  + STUFF((
					SELECT ', [' + k.cname + ']'
					FROM fk_columns k
					WHERE k.constraint_object_id = fk.[object_id]
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
				   + ')' +
				  ' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name + '] ('
				  + STUFF((
					SELECT ', [' + k.rcname + ']'
					FROM fk_columns k
					WHERE k.constraint_object_id = fk.[object_id]
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
				   + ')'
				+ CASE 
					WHEN fk.delete_referential_action = 1 THEN ' ON DELETE CASCADE' 
					WHEN fk.delete_referential_action = 2 THEN ' ON DELETE SET NULL'
					WHEN fk.delete_referential_action = 3 THEN ' ON DELETE SET DEFAULT' 
					ELSE '' 
				  END
				+ CASE 
					WHEN fk.update_referential_action = 1 THEN ' ON UPDATE CASCADE'
					WHEN fk.update_referential_action = 2 THEN ' ON UPDATE SET NULL'
					WHEN fk.update_referential_action = 3 THEN ' ON UPDATE SET DEFAULT'  
					ELSE '' 
				  END 
				+ CHAR( 13 ) + CHAR( 10 ) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name  + ']' + CHAR( 13 ) + CHAR( 10 )
			FROM sys.foreign_keys fk WITH (NOWAIT)
			JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id
			WHERE fk.parent_object_id = @object_id
			FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')
		+ ISNULL(((SELECT
			 CHAR(13) + 'CREATE' + CASE WHEN i.is_unique = 1 THEN ' UNIQUE' ELSE '' END 
					+ ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' +
					STUFF((
					SELECT ', [' + c.name + ']' + CASE WHEN c.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END
					FROM index_column c
					WHERE c.is_included_column = 0
						AND c.index_id = i.index_id
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')'  
					+ ISNULL(CHAR(13) + 'INCLUDE (' + 
						STUFF((
						SELECT ', [' + c.name + ']'
						FROM index_column c
						WHERE c.is_included_column = 1
							AND c.index_id = i.index_id
						FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '')  + CHAR( 13 ) + CHAR( 10 )
			FROM sys.indexes i WITH (NOWAIT)
			WHERE i.[object_id] = @object_id
				AND i.is_primary_key = 0
				AND i.[type] = 2
			FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')
		), '')
*/

	SELECT @SQL += 'GO' + CHAR( 13 ) + CHAR( 10 )

	INSERT INTO #SQL
	SELECT @SQL

	FETCH NEXT FROM TableCursor INTO @tableName
END

CLOSE TableCursor
DEALLOCATE TableCursor

SELECT Command
FROM #SQL
ORDER BY ID

DROP TABLE #SQL

