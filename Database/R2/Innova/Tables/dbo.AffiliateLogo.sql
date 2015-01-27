USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateLogo_Affiliate_AffiliateID' )
	ALTER TABLE [dbo].[AffiliateLogo] DROP  CONSTRAINT [FK_AffiliateLogo_Affiliate_AffiliateID]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AffiliateLogo]') AND type in (N'U'))
	DROP TABLE [dbo].[AffiliateLogo]
GO

CREATE TABLE [dbo].[AffiliateLogo]
(
	[AffiliateID] [int] NOT NULL,
	[AffiliateLogoTypeID] [int] NOT NULL,
	[FileID] [int] NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,
	CONSTRAINT [PK_AffiliateLogo] PRIMARY KEY CLUSTERED ( [AffiliateID] ASC, [AffiliateLogoTypeID] ASC )
) ON [PRIMARY]

GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateLogo_Affiliate_AffiliateID' )
BEGIN
	ALTER TABLE [dbo].[AffiliateLogo] ADD  CONSTRAINT [FK_AffiliateLogo_Affiliate_AffiliateID] FOREIGN KEY([AffiliateID]) REFERENCES [dbo].[Affiliate] ([AffiliateID])
END
GO
