USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeatureSetting]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[FeatureSetting]
	(
		[FeatureSettingID] [int] NOT NULL,
		[FeatureID] [int] NOT NULL,
		[Name] [varchar](64) NOT NULL,
		[Description] [varchar](512) NOT NULL,
		[PlaceholderValue] [varchar](512) NOT NULL,
		[VisibleState] [varchar](12) NOT NULL,
		[IsRequired] [bit] NOT NULL,
		[ValidationRegEx] [varchar](512) NULL,
		CONSTRAINT [PK_FeatureSetting] PRIMARY KEY CLUSTERED ( [FeatureSettingID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_FeatureSetting_Feature' )
BEGIN
	ALTER TABLE [dbo].[FeatureSetting] ADD  CONSTRAINT [FK_FeatureSetting_Feature] FOREIGN KEY([FeatureID]) REFERENCES [dbo].[Feature] ([FeatureID])
END
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateFeatureSetting_FeatureSetting' )
BEGIN
	ALTER TABLE [dbo].[AffiliateFeatureSetting] ADD  CONSTRAINT [FK_AffiliateFeatureSetting_FeatureSetting] FOREIGN KEY([FeatureSettingID]) REFERENCES [dbo].[FeatureSetting] ([FeatureSettingID])
END
GO