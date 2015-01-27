USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwUserStatus' )
	DROP VIEW [usr].[vwUserStatus]
GO

/*
	SELECT * FROM usr.vwUserStatus
 */
 IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwUserStatus]
		AS
			SELECT 
				[UserStatusID] = PS.PR_Status_ID, 
				[UserStatusName] = PS.PR_Status_Name
			FROM 
				UserProfileDM.dbo.Profile_Status PS WITH( NoLock )'
END
GO


