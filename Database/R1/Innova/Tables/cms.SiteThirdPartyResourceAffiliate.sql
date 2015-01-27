USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteThirdPartyResourceAffiliate]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteThirdPartyResourceAffiliate]
	(
		[SiteThirdPartyResourceID] [int] NOT NULL,
		[AffiliateID] [int] NOT NULL,
		CONSTRAINT [PK_SiteThirdPartyResourceAffiliate] PRIMARY KEY CLUSTERED ( [SiteThirdPartyResourceID] ASC,	[AffiliateID] ASC )
	) ON [PRIMARY]

END
GO

