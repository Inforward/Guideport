USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwLicense' )
	DROP VIEW [usr].[vwLicense]
GO

/*
	SELECT * FROM usr.vwLicense
 */
IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwLicense]
		AS

			SELECT 
				LicenseID = U_L.LI_License_ID,
				LicenseTypeID = U_L.LI_License_Type_ID,
				LicenseTypeName = U_L.LI_License_Type_Name,
				LicenseExamTypeID = U_L.LI_License_Exam_Type_ID,
				LicenseExamTypeName = U_L.LI_License_Exam_Type_Name,
				RegistrationCategory = U_L.LI_Registration_Category,
				[Description] = U_L.LI_Registration_Category_Name
			FROM 
				UserProfileDM.dbo.License U_L WITH( NoLock )'
END
GO
