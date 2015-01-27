USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'grp' ) 
	EXEC sp_executesql N'CREATE SCHEMA grp'
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID(N'grp.RemoveMemberUsers'))
	DROP PROCEDURE [grp].[RemoveMemberUsers]
GO

CREATE PROC [grp].[RemoveMemberUsers]
(
	@groupId INT,
	@userIdList VARCHAR( MAX )
)
AS
BEGIN

	DELETE 
	FROM 
		dbo.GroupUser
	WHERE
		GroupID = @groupId
		AND [MemberUserID] IN 
		( 
			SELECT 
				CAST(U.Value AS INT ) 
			FROM 
				dbo.ListToTable( @userIdList ) U
		)
		
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [grp].[RemoveMemberUsers] TO svc_Innova_qa
GO