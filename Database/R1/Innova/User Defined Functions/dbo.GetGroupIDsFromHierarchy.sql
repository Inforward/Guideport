USE Innova
GO

IF EXISTS( SELECT * FROM sys.objects WHERE object_id = object_id(N'grp.GetGroupIDsFromHierarchy') and type in (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
	DROP FUNCTION grp.GetGroupIDsFromHierarchy
END
GO

/*
exec sp_executesql N'grp.GetGroupsFromHierarchy @groupIDList',N'@groupIDList nvarchar(10)',@groupIDList=N'2147483647'

	SELECT * FROM grp.GetGroupIDsFromHierarchy( '245697859' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '23' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '26' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '27' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '28' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '23,26' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '23,26,27' )
	SELECT * FROM grp.GetGroupIDsFromHierarchy( '23,26,27,28' )

	SELECT * FROM grp.[Group]
 */
CREATE FUNCTION grp.GetGroupIDsFromHierarchy 
(
	@groupIDList VARCHAR( MAX )
)
RETURNS @table TABLE( GroupID INT PRIMARY KEY CLUSTERED ) 
AS  
BEGIN

	;WITH GRP AS
	(

		SELECT G.GroupID,  [Level] = 1
		FROM dbo.[Group] G WITH( NoLock )
			JOIN dbo.ListToTable( @groupIDList ) AS GIL
				ON G.GroupID = CAST( GIL.Value AS INT )

		UNION ALL

		SELECT G2.GroupID, [Level] = G.[Level] + 1
		FROM dbo.[Group] G2
			JOIN dbo.GroupGroup GG WITH( NoLock )
				ON G2.GroupID = GG.MemberGroupID
			JOIN GRP G
				ON G.GroupID = GG.GroupID
		WHERE G2.GroupID <> GG.GroupID
	)
	
	INSERT INTO @table( GroupID )
	SELECT DISTINCT GroupID
	FROM GRP

	RETURN
END
GO

IF EXISTS( SELECT * FROM sys.database_principals WHERE name = 'svc_innova_qa' )
    GRANT SELECT ON [grp].[GetGroupIDsFromHierarchy] TO svc_Innova_qa
GO