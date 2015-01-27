USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'grp' ) 
	EXEC sp_executesql N'CREATE SCHEMA grp'
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID(N'grp.AddMemberUsers'))
	DROP PROCEDURE [grp].[AddMemberUsers]
GO

CREATE PROC [grp].[AddMemberUsers]
(
	@groupId INT,
	@userIdList VARCHAR( MAX )
)
AS
BEGIN

	DECLARE @Users TABLE ( UserID INT )

	INSERT INTO @Users ( UserID )
		SELECT 
			CAST(U.Value AS INT ) 
		FROM 
			dbo.ListToTable( @userIdList ) U

	INSERT INTO dbo.GroupUser ( GroupID, MemberUserID )
		SELECT
			@groupId,
			U.UserID
		FROM
			dbo.vwUserSummary U
			JOIN @Users URS
				ON U.UserID = URS.UserID
			LEFT JOIN dbo.GroupUser GU
				ON GU.GroupID = @groupId
					AND GU.MemberUserID = URS.UserID
		WHERE
			GU.MemberUserID IS NULL
		
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [grp].[AddMemberUsers] TO svc_Innova_qa
GO