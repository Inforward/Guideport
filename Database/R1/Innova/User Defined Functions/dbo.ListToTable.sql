USE [Innova]
GO

IF EXISTS (SELECT * FROM sys.objects  WHERE object_id = OBJECT_ID(N'[dbo].[ListToTable]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ListToTable]
GO

CREATE FUNCTION [dbo].[ListToTable] 
(
	@list TEXT
)
RETURNS @table TABLE( ID INT PRIMARY KEY IDENTITY( 1, 1 ) NOT NULL, Value VARCHAR( 128 ) NOT NULL )
AS  
BEGIN 
	DECLARE @index INT, @pos INT, @str VARCHAR( 8000 ), @length INT

	SELECT @length = DATALENGTH( @list )

    SET @pos = 0
	SET @index = PATINDEX( '%,%', SUBSTRING( @list, @pos + 1, (@length - @pos) ) )

    WHILE @pos < @index 
    BEGIN
            IF @pos < @index
				SET @str = SUBSTRING( @list, @pos, @index - @pos )
            ELSE
				SET @str = SUBSTRING( @list, @pos, @length )

			SET @str = LTRIM (RTRIM( @str ) )

            INSERT INTO @table( Value ) VALUES( @str )

            SET @pos = @index + 1
			SET @index = @index + PATINDEX( '%,%', SUBSTRING( @list, @pos, ( @length - @pos ) ) )

    END

	INSERT @table ( Value )
	SELECT SUBSTRING( @list, @pos, ( @length - @pos ) + 1 )

    RETURN
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT SELECT ON [dbo].[ListToTable]  TO svc_Innova_qa
GO


