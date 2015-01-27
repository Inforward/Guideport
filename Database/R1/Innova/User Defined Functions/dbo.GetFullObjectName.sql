USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE object_id = object_id(N'dbo.GetFullObjectName') and type in (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
	DROP FUNCTION dbo.GetFullObjectName
END
GO

CREATE FUNCTION dbo.GetFullObjectName 
(
	@objectID INT
)
RETURNS VARCHAR( 1000 ) 
AS  
BEGIN 
	RETURN OBJECT_SCHEMA_NAME( @objectID ) + '.' + OBJECT_NAME( @objectID )
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON dbo.GetFullObjectName TO svc_Innova_qa
GO