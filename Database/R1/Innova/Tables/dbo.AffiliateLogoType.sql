USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AffiliateLogoType]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[AffiliateLogoType]
	(
		[AffiliateLogoTypeID] [int] NOT NULL,
		[Name] [varchar](64) NOT NULL,
		[Description] [varchar](512) NOT NULL,		
		CONSTRAINT [PK_AffiliateLogoType] PRIMARY KEY CLUSTERED ( [AffiliateLogoTypeID] ASC )
	) ON [PRIMARY]

END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateLogo_AffiliateLogoType_AffiliateLogoTypeID' )
BEGIN
	ALTER TABLE [dbo].[AffiliateLogo] ADD  CONSTRAINT [FK_AffiliateLogo_AffiliateLogoType_AffiliateLogoTypeID] FOREIGN KEY([AffiliateLogoTypeID]) REFERENCES [dbo].[AffiliateLogoType] ([AffiliateLogoTypeID])
END
GO