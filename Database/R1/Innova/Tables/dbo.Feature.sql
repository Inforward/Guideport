USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feature]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[Feature]
	(
		[FeatureID] [int] NOT NULL,
		[Name] [varchar](64) NOT NULL,
		[Description] [varchar](512) NOT NULL,
		[PlaceholderValue] [varchar](512) NOT NULL,
		CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED ( [FeatureID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateFeature_Feature' )
BEGIN
	ALTER TABLE [dbo].[AffiliateFeature] ADD  CONSTRAINT [FK_AffiliateFeature_Feature] FOREIGN KEY([FeatureID]) REFERENCES [dbo].[Feature] ([FeatureID])
END
GO

IF OBJECT_ID('DF_Feature_PlaceholderValue', 'D') IS NOT NULL 
BEGIN
	ALTER TABLE [dbo].[Feature] DROP CONSTRAINT DF_Feature_PlaceholderValue
END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PlaceholderValue' AND [object_id] = OBJECT_ID(N'[dbo].[Feature]'))
BEGIN
    ALTER TABLE [dbo].[Feature] DROP COLUMN [PlaceholderValue]
END
GO