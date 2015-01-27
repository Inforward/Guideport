USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'grp' ) 
	EXEC sp_executesql N'CREATE SCHEMA grp'
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID(N'grp.RemoveMemberGroups'))
	DROP PROCEDURE [grp].[RemoveMemberGroups]
GO

CREATE PROC [grp].[RemoveMemberGroups]
(
	@groupId INT,
	@groupIdList VARCHAR( MAX )
)
AS
BEGIN

	DELETE 
	FROM 
		dbo.GroupGroup
	WHERE
		GroupID = @groupId
		AND [MemberGroupID] IN 
		( 
			SELECT 
				CAST(Value AS INT ) 
			FROM 
				dbo.ListToTable( @groupIdList ) G
		)
		
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [grp].[RemoveMemberGroups] TO svc_Innova_qa
GO