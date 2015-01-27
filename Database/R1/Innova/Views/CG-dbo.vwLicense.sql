USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwLicense' )
	DROP VIEW [dbo].[vwLicense]
GO

/*
	SELECT * FROM dbo.vwLicense
 */
CREATE VIEW [dbo].[vwLicense]
AS

	SELECT 
		LicenseID,
		LicenseTypeID,
		LicenseTypeName,
		LicenseExamTypeID,
		LicenseExamTypeName,
		RegistrationCategory,
		Description
	FROM 
		usr.License WITH( NoLock )

GO


