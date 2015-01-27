USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwLicenseUser' )
	DROP VIEW [dbo].[vwLicenseUser]
GO

/*
	SELECT * FROM dbo.vwLicenseUser
 */
CREATE VIEW [dbo].[vwLicenseUser]
AS
	SELECT 	
		LicenseID,
		UserID,
		EffectiveDate,
		TerminatedDate
	FROM 
		usr.LicenseUser WITH( NoLock )
GO
