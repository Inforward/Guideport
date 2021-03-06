USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[LicenseUser]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[LicenseUser]
	(		
		LicenseID			INT NOT NULL,
		UserID				INT NOT NULL,
		EffectiveDate		DATETIME,
		TerminatedDate		DATETIME
		CONSTRAINT [PK_LicenseUser] PRIMARY KEY CLUSTERED( LicenseID, UserID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	) ON [PRIMARY]
END
GO
