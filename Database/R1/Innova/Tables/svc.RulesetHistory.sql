USE [Innova]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[svc].[RulesetHistory]') AND type in (N'U'))
BEGIN
	DROP TABLE [svc].[RulesetHistory]
END
GO