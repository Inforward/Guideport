USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'grp' ) 
	EXEC sp_executesql N'CREATE SCHEMA grp'
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE object_id = OBJECT_ID(N'grp.AddMemberGroups'))
	DROP PROCEDURE [grp].[AddMemberGroups]
GO

CREATE PROC [grp].[AddMemberGroups]
(
	@groupId INT,
	@groupIdList VARCHAR( MAX )
)
AS
BEGIN

	DECLARE @Groups TABLE ( GroupID INT )

	INSERT INTO @Groups ( GroupID )
		SELECT 
			CAST(U.Value AS INT ) 
		FROM 
			dbo.ListToTable( @groupIdList ) U

	INSERT INTO dbo.GroupGroup ( GroupID, MemberGroupID )
		SELECT
			@groupId,
			G.GroupID
		FROM
			dbo.[Group] G
			JOIN @Groups GRPS
				ON G.GroupID = GRPS.GroupID
			LEFT JOIN dbo.GroupGroup GG
				ON GG.GroupID = @groupId
					AND GG.MemberGroupID = GRPS.GroupID
		WHERE
			GG.MemberGroupID IS NULL
		
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON [grp].[AddMemberGroups] TO svc_Innova_qa
GO