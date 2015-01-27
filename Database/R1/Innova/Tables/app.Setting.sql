USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[Setting]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[Setting] 
	(
		SettingID			INT NOT NULL IDENTITY(1, 1),
		ConfigurationTypeID	INT NOT NULL,
		Name				VARCHAR( 100 ) NOT NULL,
		[Description]		VARCHAR( 1000 ) NOT NULL,
		[DataTypeName]		VARCHAR( 50 ) NOT NULL,
		[IsRequired]		BIT NOT NULL CONSTRAINT [DF_Setting_IsRequired] DEFAULT 0,
		CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED( SettingID ),
		CONSTRAINT [UX_Setting_ConfigurationTypeID_Name] UNIQUE( ConfigurationTypeID, Name )
	)
END
GO
