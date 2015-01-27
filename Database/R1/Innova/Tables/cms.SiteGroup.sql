USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Site_SiteGroup_SiteGroupID' )
	ALTER TABLE [cms].[Site] DROP CONSTRAINT [FK_Site_SiteGroup_SiteGroupID]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteGroup]') AND type in (N'U'))
	DROP TABLE [cms].[SiteGroup]
GO