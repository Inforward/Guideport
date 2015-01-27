USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentHistory_SiteContent' )
	ALTER TABLE [cms].[SiteContentHistory] DROP CONSTRAINT [FK_SiteContentHistory_SiteContent]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentHistory]') AND type in (N'U'))
BEGIN
	DROP TABLE [cms].[SiteContentHistory]
END
GO