USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwUserProfileType' )
	DROP VIEW [usr].[vwUserProfileType]
GO

/*
	SELECT * FROM usr.vwUserProfileType
 */
 IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwUserProfileType]
		AS
			SELECT 
				ProfileTypeID = PT.PR_Type_ID,
				Name = PT.PR_Type_Name,
				Description = PT.PR_Type_Name
			FROM 
				UserProfileDM.dbo.Profile_Type PT WITH( NoLock )'
END
GO

