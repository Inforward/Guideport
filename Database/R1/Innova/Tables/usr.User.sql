USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[User]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[User]
	(
		UserID				INT NOT NULL,
		UserProfileTypeID		INT NOT NULL,
		AffiliateID			INT NOT NULL,
		ProfileID			VARCHAR( 20 ) NOT NULL,
		UserStatusID		INT NOT NULL,
		UserStatusName		VARCHAR( 50 ),
		Email				VARCHAR( 100 ),
		FirstName			VARCHAR( 50 ) NOT NULL,
		MiddleName			VARCHAR( 50 ),
		LastName			VARCHAR( 50 ) NOT NULL,
		DisplayFirstName	VARCHAR( 50 ) NOT NULL,
		DisplayLastName		VARCHAR( 50 ) NOT NULL,
		DBAName				VARCHAR( 250 ),
		Address1			VARCHAR( 100 ),
		Address2			VARCHAR( 100 ),
		City				VARCHAR( 50 ),
		State				VARCHAR( 2 ),
		ZipCode				VARCHAR( 10 ),
		Country				VARCHAR( 5 ),
		PrimaryPhone		VARCHAR( 25 ),
		HomePhone			VARCHAR( 25 ),
		WorkPhone			VARCHAR( 25 ),
		Fax					VARCHAR( 25 ),
		SecurityProfileStartDate DATETIME,
		StartDate			DATETIME NOT NULL,
		TerminateDate		DATETIME,
		CreateDate			DATETIME NOT NULL CONSTRAINT [DF_User_CreateDate] DEFAULT GETDATE(),
		ModifyDate			DATETIME NOT NULL CONSTRAINT [DF_User_ModifyDate] DEFAULT GETDATE(),
		DeleteDate			DATETIME,
		CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED( UserID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [AK_User_AffiliateID_ProfileID] UNIQUE( AffiliateID, ProfileID )
	) ON [PRIMARY]
END
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User' AND TABLE_SCHEMA = 'usr' )
	AND NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_User_ProfileID' )
BEGIN
	CREATE NONCLUSTERED INDEX IX_User_ProfileID ON [usr].[User] ([ProfileID])
END
GO
