USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwApplicationRole' )
	DROP VIEW [dbo].[vwApplicationRole]
GO