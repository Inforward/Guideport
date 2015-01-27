USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwGroupUser' )
	DROP VIEW [dbo].[vwGroupUser]
GO

/*
	SELECT * FROM dbo.vwGroupUser
 */
--CREATE VIEW [dbo].[vwGroupUser]
--AS

--	SELECT 
--		GroupID = T.Team_ID, 
--		UserID = U.UserID
--	FROM
--		FASI_ISecurity.dbo.Users URS WITH( NoLock )
--		JOIN FASI_Tracking.dbo.TeamMember TM WITH( NoLock )
--			ON URS.UR_ID = TM.UR_ID
--				AND TM.TM_DeletedDate IS NULL
--		JOIN FASI_Tracking.dbo.Team T WITH( NoLock )
--			ON TM.Team_ID = T.Team_ID
--				AND T.Team_Name LIKE 'BCG%'
--				AND T.Team_DeletedDate IS NULL
--		JOIN FASI_Tracking.dbo.TeamProfile TP WITH( NoLock )
--			ON T.Team_ID = TP.Team_ID
--				AND TP.T_DeletedDate IS NULL
--		JOIN AE_Security.dbo.AE_User U WITH( NoLock )
--			ON TP.PR_ID = U.PR_ID
--GO


