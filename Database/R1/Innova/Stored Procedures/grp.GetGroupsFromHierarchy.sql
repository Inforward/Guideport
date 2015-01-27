USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE name = 'GetGroupsFromHierarchy' and schema_id = SCHEMA_ID( 'grp' ) and type_desc = 'SQL_STORED_PROCEDURE' )
BEGIN
	DROP PROCEDURE grp.GetGroupsFromHierarchy
END
GO

/*
	exec grp.GetGroupsFromHierarchy @groupIDList = '23'
*/
CREATE PROCEDURE grp.GetGroupsFromHierarchy
(
	@groupIDList VARCHAR( MAX )
) 
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		G.*
	FROM dbo.[Group] G WITH( NoLock )
		JOIN  grp.GetGroupIDsFromHierarchy( @groupIDList ) H
			ON H.GroupID = G.GroupID

	SET NOCOUNT OFF
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT EXECUTE ON grp.GetGroupsFromHierarchy TO svc_Innova_qa
GO
