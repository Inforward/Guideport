USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwGroup' )
	DROP VIEW [dbo].[vwGroup]
GO

/*
	SELECT * FROM dbo.vwGroup
 */

--CREATE VIEW [dbo].[vwGroup]
--AS

--	SELECT
--		GroupID = T.Team_ID,
--		GroupTypeID = 1,
--		GroupTypeName = 'Business Consulting Group',
--		AffiliateID = 1,
--		AffiliateName = 'First Allied',
--		Name = T.Team_Name,
--		OwnerUserID = U.UserID,
--		CreateDate = T.Team_CreatedDate, 
--		ModifyDate = T.Team_LastModifyDate, 
--		DeleteDate = T.Team_DeletedDate
--	FROM
--		FASI_ISecurity.dbo.Users URS WITH( NoLock )
--		JOIN FASI_Tracking.dbo.TeamMember TM WITH( NoLock )
--			ON URS.UR_ID = TM.UR_ID
--				AND TM.TM_DeletedDate IS NULL
--		JOIN FASI_Tracking.dbo.Team T WITH( NoLock )
--			ON TM.Team_ID = T.Team_ID
--				AND T.Team_Name LIKE 'BCG%'
--				AND T.Team_DeletedDate IS NULL
--		JOIN AE_Security.dbo.AE_User U WITH( NoLock )
--			ON URS.PR_ID = U.PR_ID
--GO


