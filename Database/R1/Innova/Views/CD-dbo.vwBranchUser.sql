USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwBranchUser' )
	DROP VIEW [dbo].[vwBranchUser]
GO

/*
	SELECT * FROM dbo.vwBranchUser
 */
CREATE VIEW [dbo].[vwBranchUser]
AS

	SELECT 
		BU.UserID,
		B.AffiliateID,
		AffiliateName = A.Name,
		BU.BranchID, 
		BU.IsPrimary,
		BU.TerminatedDate,
		BU.CreateDate,
		BU.ModifyDate
	FROM 
		usr.BranchUser BU WITH( NoLock )
			JOIN usr.Branch B WITH( NoLocK )
				ON BU.BrancHID = B.BranchID
			JOIN dbo.Affiliate A WITH( NoLock )
				ON B.AffiliateID = A.AffiliateID
GO
