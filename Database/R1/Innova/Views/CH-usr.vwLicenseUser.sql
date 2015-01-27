USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwLicenseUser' )
	DROP VIEW [usr].[vwLicenseUser]
GO

/*
	SELECT * FROM usr.vwLicenseUser
 */
 IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwLicenseUser]
		AS
			SELECT 	
				LicenseID = U_LP.LI_License_ID,
				UserID = U_LP.PR_Profile_Master_ID,
				EffectiveDate = U_LP.LP_License_Effective_Date,
				TerminatedDate = U_LP.LP_License_Expiration_Date
			FROM
				UserProfileDM.dbo.License_Profile U_LP WITH( NoLock )'
END
GO
