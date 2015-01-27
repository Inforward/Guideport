USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Affiliate]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Affiliate]
	(
		AffiliateID			INT NOT NULL,
		Name				VARCHAR( 50 ) NOT NULL,
		ShortName			VARCHAR( 10 ) NOT NULL,
		ExternalID			INT,
		Phone				VARCHAR( 25 ),
		WebsiteUrl			VARCHAR( 50 ),
		Address1			VARCHAR( 100 ),
		Address2			VARCHAR( 100 ),
		City				VARCHAR( 50 ),
		State				VARCHAR( 2 ),
		ZipCode				VARCHAR( 50 ),
		Country				VARCHAR( 50 ),
		SAMLSourceDomain	VARCHAR( 50 ),
		SAMLConfigurationID INT,
		SAMLDisplayOrder	INT,
		CreateUserID		INT NOT NULL CONSTRAINT [DF_Affiliate_CreateUserID] DEFAULT 0,
		CreateDate			DATETIME NOT NULL CONSTRAINT [DF_Affiliate_CreateDate] DEFAULT GETDATE(),
		CreateDateUTC		DATETIME NOT NULL CONSTRAINT [DF_Affiliate_CreateDateUTC] DEFAULT GETUTCDATE(),
		ModifyUserID		INT NOT NULL CONSTRAINT [DF_Affiliate_ModifyUserID] DEFAULT 0,
		ModifyDate			DATETIME NOT NULL CONSTRAINT [DF_Affiliate_ModifyDate] DEFAULT GETDATE(),
		ModifyDateUTC		DATETIME NOT NULL CONSTRAINT [DF_Affiliate_ModifyDateUTC] DEFAULT GETUTCDATE()
		CONSTRAINT [PK_Affiliate] PRIMARY KEY CLUSTERED( AffiliateID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO