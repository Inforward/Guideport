USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[Branch]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[Branch]
	(
		BranchID			INT NOT NULL,
		AffiliateID			INT NOT NULL,
		BranchNo			VARCHAR( 10 ),
		Address1			VARCHAR( 50 ),
		Address2			VARCHAR( 50 ),
		City				VARCHAR( 30 ),
		State				VARCHAR( 2 ),
		ZipCode				VARCHAR( 10 ),
		Country				VARCHAR( 2 ),
		MailingAddress1		VARCHAR( 50 ),
		MailingAddress2		VARCHAR( 50 ),
		MailingCity			VARCHAR( 30 ),
		MailingState		VARCHAR( 2 ),
		MailingZipCode		VARCHAR( 10 ),
		MailingCountry		VARCHAR( 2 ),
		Phone				VARCHAR( 15 ),
		Fax					VARCHAR( 15 ),		
		CreateDate			DATETIME NOT NULL CONSTRAINT [DF_Branch_CreateDate] DEFAULT GETDATE(),
		DeleteDate			DATETIME
		CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED( BranchID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
