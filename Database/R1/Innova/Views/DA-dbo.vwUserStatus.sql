USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwUserStatus' )
	DROP VIEW [dbo].[vwUserStatus]
GO

/*
	SELECT * FROM dbo.vwUserStatus
 */
CREATE VIEW [dbo].[vwUserStatus]
AS
	SELECT 
		DISTINCT 
		[UserStatusID], 
		[UserStatusName] 
	FROM 
		[dbo].[vwUser]
GO
