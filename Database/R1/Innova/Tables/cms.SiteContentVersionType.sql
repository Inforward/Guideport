USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentVersion_SiteContentVersionTypeID' )
	ALTER TABLE [cms].[SiteContentVersion] DROP CONSTRAINT [FK_SiteContentVersion_SiteContentVersionTypeID]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentVersionType]') AND type in (N'U'))
BEGIN
	DROP TABLE [cms].[SiteContentVersionType]
END
GO

