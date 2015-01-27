USE Innova
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintLong]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PrintLong]
GO

CREATE PROCEDURE [dbo].[PrintLong]
(
      @String NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE
        @CurrentEnd BIGINT,
        @offset TINYINT

    SET @String = replace(  replace(@String, CHAR(13) + CHAR(10), CHAR(10))   , CHAR(13), CHAR(10))

    WHILE LEN(@String) > 1
    BEGIN

        IF CHARINDEX(CHAR(10), @String) between 1 AND 4000
        BEGIN

            SET @CurrentEnd =  CHARINDEX(CHAR(10), @String) -1
            SET @offset = 2
        END
        ELSE
        BEGIN
            SET @CurrentEnd = 4000
            SET @offset = 1
        END

        PRINT SUBSTRING(@String, 1, @CurrentEnd)

        SET @string = SUBSTRING(@String, @CurrentEnd+@offset, 1073741822)

    END
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [dbo].[PrintLong] TO svc_Innova_qa
GO

