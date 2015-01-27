USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[Environment]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[Environment] 
	(
		EnvironmentID		INT NOT NULL,
		Name				VARCHAR( 50 ) NOT NULL,
		[Description]		VARCHAR( 100 ) NOT NULL,
		CONSTRAINT [PK_Environment] PRIMARY KEY CLUSTERED( EnvironmentID )
	)
END
GO
