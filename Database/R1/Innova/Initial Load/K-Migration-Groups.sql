USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Group_GroupType_GroupTypeID' )
	ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_GroupType_GroupTypeID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupGroup_Group_MemberGroupID' )
	ALTER TABLE [dbo].[GroupGroup] DROP CONSTRAINT [FK_GroupGroup_Group_MemberGroupID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupGroup_Group_GroupID' )
	ALTER TABLE [dbo].[GroupGroup] DROP CONSTRAINT [FK_GroupGroup_Group_GroupID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupUserAccess_Group_GroupID' )
	ALTER TABLE [dbo].[GroupUserAccess] DROP CONSTRAINT [FK_GroupUserAccess_Group_GroupID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupUser_Group_GroupID' )
	ALTER TABLE [dbo].[GroupUser] DROP CONSTRAINT [FK_GroupUser_Group_GroupID]
GO

DELETE FROM [dbo].[Group]
DELETE FROM [dbo].[GroupGroup]
DELETE FROM [dbo].[GroupUserAccess]
DELETE FROM [dbo].[GroupUser]
GO


-- BCG Groups
INSERT INTO [dbo].[Group]
(
	[Name],
	[IsReadOnly],
	[CreateUserID],
	[CreateDate],
	[CreateDateUTC],
	[ModifyUserID],
	[ModifyDate],
	[ModifyDateUTC]
)
	SELECT
		Name = T.Team_Name,
		IsReadOnly = 1,
		CreateUserID = U.UserID,
		CreateDate = T.Team_CreatedDate, 
		CreateDateUTC = DATEADD( hh, 7, T.Team_CreatedDate ), 
		ModifyUserID = U.UserID,
		ModifyDate = ISNULL( T.Team_LastModifyDate, T.Team_CreatedDate ),
		ModifyDateUTC = DATEADD( hh, 7, ISNULL( T.Team_LastModifyDate, T.Team_CreatedDate ) )
	FROM
		[Innova_Staging].dbo.Users URS WITH( NoLock )
		JOIN [Innova_Staging].dbo.TeamMember TM WITH( NoLock )
			ON URS.UR_ID = TM.UR_ID
				AND TM.TM_DeletedDate IS NULL
		JOIN [Innova_Staging].dbo.Team T WITH( NoLock )
			ON TM.Team_ID = T.Team_ID
				AND T.Team_Name LIKE 'BCG%'
				AND T.Team_DeletedDate IS NULL
		JOIN dbo.vwUser U WITH( NoLock )
			ON CAST(URS.PR_ID AS VARCHAR) = U.ProfileID
				AND U.AffiliateID = 1

-- BCG Group Accessibility
INSERT INTO [dbo].[GroupUserAccess] ( GroupID, UserID, IsReadOnly )
	SELECT 
		GroupID = G.GroupID, 
		UserID = TEAMS.UserID,
		IsReadOnly = 1
	FROM
		dbo.[Group] G WITH ( NoLock )
		JOIN
		(
			SELECT
				DISTINCT
				U.UserID,
				T.Team_Name AS [Name]
			FROM
				[Innova_Staging].dbo.Users URS WITH( NoLock )
				JOIN [Innova_Staging].dbo.TeamMember TM WITH( NoLock )
					ON URS.UR_ID = TM.UR_ID
						AND TM.TM_DeletedDate IS NULL
				JOIN [Innova_Staging].dbo.Team T WITH( NoLock )
					ON TM.Team_ID = T.Team_ID
						AND T.Team_Name LIKE 'BCG%'
						AND T.Team_DeletedDate IS NULL
				JOIN [Innova_Staging].dbo.TeamProfile TP WITH( NoLock )
					ON T.Team_ID = TP.Team_ID
						AND TP.T_DeletedDate IS NULL
				JOIN dbo.vwUser U WITH( NoLock )
					ON CAST(URS.PR_ID AS VARCHAR) = U.ProfileID 
						AND U.AffiliateID = 1
		) TEAMS
			ON G.Name = TEAMS.Name


-- BCG Group Members
INSERT INTO [dbo].[GroupUser] ( [GroupID], [MemberUserID] )
	SELECT 
		GroupID = G.GroupID, 
		MemberUserID = TEAMS.UserID
	FROM
		dbo.[Group] G WITH ( NoLock )
		JOIN
		(
			SELECT 
				DISTINCT
				T.Team_Name AS [Name], 
				U.UserID
			FROM
				[Innova_Staging].dbo.Users URS WITH( NoLock )
				JOIN [Innova_Staging].dbo.TeamMember TM WITH( NoLock )
					ON URS.UR_ID = TM.UR_ID
						AND TM.TM_DeletedDate IS NULL
				JOIN [Innova_Staging].dbo.Team T WITH( NoLock )
					ON TM.Team_ID = T.Team_ID
						AND T.Team_Name LIKE 'BCG%'
						AND T.Team_DeletedDate IS NULL
				JOIN [Innova_Staging].dbo.TeamProfile TP WITH( NoLock )
					ON T.Team_ID = TP.Team_ID
						AND TP.T_DeletedDate IS NULL
				JOIN [dbo].[vwUser] U WITH( NoLock )
					ON CAST(TP.PR_ID AS VARCHAR) = U.ProfileID
						AND U.AffiliateID = 1
		) TEAMS
			ON G.Name = TEAMS.Name

-- Update BusinessConsultantUserIDs
UPDATE
	U
SET
	BusinessConsultantUserID = GUA.UserID
FROM
	usr.[User] U WITH( NoLock )
		JOIN dbo.GroupUser GU WITH( NoLock )
			ON U.UserID = GU.MemberUserID
		JOIN dbo.GroupUserAccess GUA WITH( NoLock )
			ON GU.GroupID = GUA.GroupID

GO


IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupGroup_Group_MemberGroupID' )
	ALTER TABLE [dbo].[GroupGroup]  ADD  CONSTRAINT [FK_GroupGroup_Group_MemberGroupID] FOREIGN KEY([MemberGroupID]) REFERENCES [dbo].[Group] ([GroupID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupGroup_Group_GroupID' )
	ALTER TABLE [dbo].[GroupGroup]  ADD  CONSTRAINT [FK_GroupGroup_Group_GroupID] FOREIGN KEY([GroupID]) REFERENCES [dbo].[Group] ([GroupID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupUserAccess_Group_GroupID' )
	ALTER TABLE [dbo].[GroupUserAccess]  ADD  CONSTRAINT [FK_GroupUserAccess_Group_GroupID] FOREIGN KEY([GroupID]) REFERENCES [dbo].[Group] ([GroupID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_GroupUser_Group_GroupID' )
	ALTER TABLE [dbo].[GroupUser]  ADD  CONSTRAINT [FK_GroupUser_Group_GroupID] FOREIGN KEY([GroupID]) REFERENCES [dbo].[Group] ([GroupID])

GO


