USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'grp' ) 
	EXEC sp_executesql N'CREATE SCHEMA grp'
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID(N'grp.DeleteGroup'))
	DROP PROCEDURE [grp].[DeleteGroup]
GO

CREATE PROC [grp].[DeleteGroup]
(
	@groupId INT
)
AS
BEGIN

	DELETE FROM dbo.[GroupUserAccess] WHERE GroupID = @groupId

	DELETE FROM dbo.[GroupUser] WHERE GroupID = @groupId

	DELETE FROM dbo.[GroupGroup] WHERE GroupID = @groupId
	
	DELETE FROM dbo.[GroupGroup] WHERE MemberGroupID = @groupId

	DELETE FROM dbo.[Group] WHERE GroupID = @groupId
	
		
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [grp].[DeleteGroup] TO svc_Innova_qa
GO