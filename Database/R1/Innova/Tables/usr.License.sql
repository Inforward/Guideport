USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[License]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[License]
	(
		LicenseID			INT  NOT NULL,
		LicenseTypeID		INT NOT NULL,
		LicenseTypeName		VARCHAR( 50 ) NOT NULL,
		LicenseExamTypeID	INT NOT NULL,
		LicenseExamTypeName VARCHAR( 50 ) NOT NULL,
		RegistrationCategory VARCHAR( 5 ),
		Description			VARCHAR( 50 ),
		CONSTRAINT [PK_License] PRIMARY KEY CLUSTERED( LicenseID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	) ON [PRIMARY]
END
GO
