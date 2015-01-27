USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteThirdPartyResource]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteThirdPartyResource]
	(
		[SiteThirdPartyResourceID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [varchar](256) NULL,
		[Description] [varchar](1000) NULL,
		[AddressLine1] [varchar](100) NULL,
		[AddressLine2] [varchar](100) NULL,
		[City] [varchar](50) NULL,
		[State] [varchar](50) NULL,
		[PostalCode] [varchar](12) NULL,
		[Country] [varchar](50) NULL,
		[PhoneNo] [varchar](50) NULL,
		[PhoneNoExt] [varchar](25) NULL,
		[FaxNo] [varchar](50) NULL,
		[Email] [varchar](100) NULL,
		[WebsiteUrl] [varchar](256) NULL,
		[Services] [varchar](1000) NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		[ModifyDateUTC] [datetime] NOT NULL,
		CONSTRAINT [PK_SiteThirdPartyResource] PRIMARY KEY CLUSTERED ( [SiteThirdPartyResourceID] ASC )
	) ON [PRIMARY]
END
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ThirdPartyResource_SiteID' )
	ALTER TABLE [cms].[SiteThirdPartyResource] DROP CONSTRAINT [FK_ThirdPartyResource_SiteID]
GO


IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SiteThirdPartyResource_SiteID' )
	ALTER TABLE [cms].[SiteThirdPartyResource] DROP CONSTRAINT [FK_SiteThirdPartyResource_SiteID]
GO


IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SiteID' AND [object_id] = OBJECT_ID(N'[cms].[SiteThirdPartyResource]'))
BEGIN
    ALTER TABLE [cms].[SiteThirdPartyResource] DROP COLUMN [SiteID]
END
GO
