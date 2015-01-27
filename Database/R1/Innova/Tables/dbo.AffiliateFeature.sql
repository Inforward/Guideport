USE [Innova]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AffiliateFeature]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[AffiliateFeature]
	(
		[AffiliateFeatureID] [int] NOT NULL IDENTITY(1,1),
		[AffiliateID] [int] NOT NULL,
		[FeatureID] [int] NOT NULL,
		[IsEnabled] [bit] NOT NULL,
		CreateUserID			INT NOT NULL CONSTRAINT [DF_AffiliateFeature_CreateUserID] DEFAULT 0,
		CreateDate				DATETIME NOT NULL CONSTRAINT [DF_AffiliateFeature_CreateDate] DEFAULT GETDATE(),
		CreateDateUTC			DATETIME NOT NULL CONSTRAINT [DF_AffiliateFeature_CreateDateUTC] DEFAULT GETUTCDATE(),
		ModifyUserID			INT NOT NULL CONSTRAINT [DF_AffiliateFeature_ModifyUserID] DEFAULT 0,
		ModifyDate				DATETIME NOT NULL CONSTRAINT [DF_AffiliateFeature_ModifyDate] DEFAULT GETDATE(),
		ModifyDateUTC			DATETIME NOT NULL CONSTRAINT [DF_AffiliateFeature_ModifyDateUTC] DEFAULT GETUTCDATE(),
		CONSTRAINT [UX_AffiliateID_FeatureID] UNIQUE ( AffiliateID, FeatureID ), 
		CONSTRAINT [PK_AffiliateFeature] PRIMARY KEY CLUSTERED ( [AffiliateFeatureID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateFeature_Affiliate' )
BEGIN
	ALTER TABLE [dbo].[AffiliateFeature] ADD  CONSTRAINT [FK_AffiliateFeature_Affiliate] FOREIGN KEY([AffiliateID]) REFERENCES [dbo].[Affiliate] ([AffiliateID])
END
GO

