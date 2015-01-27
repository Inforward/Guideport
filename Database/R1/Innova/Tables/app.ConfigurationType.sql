USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[ConfigurationType]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[ConfigurationType] 
	(
		ConfigurationTypeID	INT NOT NULL,
		Name			VARCHAR( 50 ) NOT NULL,
		[Description]	VARCHAR( 1000 ) NOT NULL,
		AssemblyName	VARCHAR( 250 ),
		ClassName		VARCHAR( 250 ),
		CONSTRAINT [PK_ConfigurationType] PRIMARY KEY CLUSTERED( ConfigurationTypeID )
	)
END
GO
