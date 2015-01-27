USE [Innova]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[svc].[Ruleset]') AND type in (N'U'))
BEGIN
	DROP TABLE [svc].[Ruleset]
END

GO