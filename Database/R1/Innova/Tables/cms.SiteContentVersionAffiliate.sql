USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentVersionAffiliate]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteContentVersionAffiliate]
	(
		[SiteContentVersionID] [int] NOT NULL,
		[AffiliateID] [int] NOT NULL,
		CONSTRAINT [PK_SiteContentVersionAffiliate] PRIMARY KEY ( [SiteContentVersionID] ASC, [AffiliateID] ASC  )
	) ON [PRIMARY]

END
GO