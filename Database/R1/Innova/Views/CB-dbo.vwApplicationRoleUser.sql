USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwApplicationRoleUser' )
	DROP VIEW [dbo].[vwApplicationRoleUser]
GO