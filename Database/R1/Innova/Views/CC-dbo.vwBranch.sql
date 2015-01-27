USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwBranch' )
	DROP VIEW [dbo].[vwBranch]
GO

/*
	SELECT * FROM dbo.vwBranch
 */
CREATE VIEW [dbo].[vwBranch]
AS

	SELECT
		B.BranchID,
		B.AffiliateID,
		AffiliateName = A.Name,
		B.BranchNo, 
		B.Address1, 
		B.Address2, 
		B.City, 
		B.State, 
		B.Country, 
		B.ZipCode, 
		B.MailingAddress1, 
		B.MailingAddress2, 
		B.MailingCity, 
		B.MailingState, 
		B.MailingCountry, 
		B.MailingZipCode, 
		B.Phone, 
		B.Fax, 
		B.CreateDate,
		B.DeleteDate
	FROM 
		usr.Branch B WITH( NoLock )
			JOIN dbo.Affiliate A WITH( NoLock )
				ON B.AffiliateID = A.AffiliateID

GO


