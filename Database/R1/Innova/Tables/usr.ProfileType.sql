USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteContentProfileType_ProfileTypeID' )
	ALTER TABLE [cms].[SiteContentProfileType] DROP CONSTRAINT [FK_SiteContentProfileType_ProfileTypeID]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserProfileType]') AND type in (N'U'))
	DROP TABLE [dbo].[UserProfileType]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[ProfileType]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[ProfileType]
	(
		ProfileTypeID	INT NOT NULL,
		Name		VARCHAR( 50 ),
		Description VARCHAR( 100 ),
		CONSTRAINT [PK_ProfileType] PRIMARY KEY CLUSTERED( ProfileTypeID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
