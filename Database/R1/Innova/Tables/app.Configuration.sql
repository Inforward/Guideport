USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[Configuration]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[Configuration] 
	(
		ConfigurationID	INT NOT NULL,
		ConfigurationTypeID INT NOT NULL,
		Name			VARCHAR( 100 ) NOT NULL,
		[Description]	VARCHAR( 1000 ) NOT NULL,
		CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED( ConfigurationID ),
		CONSTRAINT [UX_Configuration_ConfigurationTypeID_Name] UNIQUE( ConfigurationTypeID, Name )
	)
END
GO
