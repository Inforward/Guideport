USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[ConfigurationSetting]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[ConfigurationSetting] 
	(
		ConfigurationSettingID		INT NOT NULL IDENTITY( 1, 1 ),
		ConfigurationID				INT NOT NULL,
		EnvironmentID				INT NOT NULL,
		SettingID					INT NOT NULL,
		Value						VARCHAR( MAX ) NOT NULL,
		[CreateUserID]				INT  NOT NULL CONSTRAINT DF_ConfigurationSetting_CreateUserID DEFAULT 0,
		[CreateDate]				DATETIME NOT NULL CONSTRAINT DF_ConfigurationSetting_CreateDate DEFAULT GETDATE(),
		[CreateDateUTC]				DATETIME NOT NULL CONSTRAINT DF_ConfigurationSetting_CreateDateUTC DEFAULT GETUTCDATE(),
		[ModifyUserID]				INT NOT NULL CONSTRAINT DF_ConfigurationSetting_ModifyUserID DEFAULT 0,
		[ModifyDate]				DATETIME NOT NULL CONSTRAINT DF_ConfigurationSetting_ModifyDate DEFAULT GETDATE(),
		[ModifyDateUTC]				DATETIME NOT NULL CONSTRAINT DF_ConfigurationSetting_ModifyDateUTC DEFAULT GETUTCDATE(),
		CONSTRAINT [PK_ConfigurationSetting] PRIMARY KEY CLUSTERED( ConfigurationSettingID ),
		CONSTRAINT [UX_ConfigurationSetting_ConfigurationID_EnvironmentID_SettingID] UNIQUE( ConfigurationID, EnvironmentID, SettingID )
	)
END
GO
