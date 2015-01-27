USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteContentAffiliate]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteContentAffiliate]
	(
		[SiteContentID] [int] NOT NULL,
		[AffiliateID] [int] NOT NULL,
		CONSTRAINT [PK_SiteContentAffiliate] PRIMARY KEY CLUSTERED ( [SiteContentID] ASC,	[AffiliateID] ASC )
	) ON [PRIMARY]

END
GO

