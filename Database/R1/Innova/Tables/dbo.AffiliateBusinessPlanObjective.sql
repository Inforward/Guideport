USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AffiliateBusinessPlanObjective]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[AffiliateBusinessPlanObjective]
	(
		AffiliateID				INT NOT NULL,
		BusinessPlanObjectiveID	INT NOT NULL,
		AutoTrackingEnabled		BIT NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_AutoTrackingEnabled] DEFAULT 0,
		CreateUserID			INT NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_CreateUserID] DEFAULT 0,
		CreateDate				DATETIME NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_CreateDate] DEFAULT GETDATE(),
		CreateDateUTC			DATETIME NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_CreateDateUTC] DEFAULT GETUTCDATE(),
		ModifyUserID			INT NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_ModifyUserID] DEFAULT 0,
		ModifyDate				DATETIME NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_ModifyDate] DEFAULT GETDATE(),
		ModifyDateUTC			DATETIME NOT NULL CONSTRAINT [DF_AffiliateBusinessPlanObjective_ModifyDateUTC] DEFAULT GETUTCDATE(),
		CONSTRAINT [PK_AffiliateBusinessPlanObjective] PRIMARY KEY CLUSTERED( AffiliateID, BusinessPlanObjectiveID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
