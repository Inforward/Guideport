USE [Innova]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AffiliateFeatureSetting]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[AffiliateFeatureSetting]
	(
		[AffiliateFeatureID] [int] NOT NULL,
		[FeatureSettingID] [int] NOT NULL,
		[Value] [varchar](Max) NULL,
		CONSTRAINT [PK_AffiliateFeatureSetting] PRIMARY KEY CLUSTERED ( [AffiliateFeatureID] ASC, [FeatureSettingID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateFeatureSetting_AffiliateFeature' )
BEGIN
	ALTER TABLE [dbo].[AffiliateFeatureSetting] ADD  CONSTRAINT [FK_AffiliateFeatureSetting_AffiliateFeature] FOREIGN KEY([AffiliateFeatureID]) REFERENCES [dbo].[AffiliateFeature] ([AffiliateFeatureID])
END
GO

