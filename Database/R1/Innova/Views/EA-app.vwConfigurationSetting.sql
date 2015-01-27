USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'app' AND TABLE_NAME = 'vwConfigurationSetting' )
	DROP VIEW [app].[vwConfigurationSetting]
GO

/*
	SELECT * FROM app.vwConfigurationSetting
 */
CREATE VIEW [app].[vwConfigurationSetting]
AS
	SELECT 
		CS.ConfigurationSettingID,
		ConfigurationTypeName = ST.Name,
		ConfigurationName = A.Name,
		EnvironmentName = E.Name,
		SettingName = S.Name,
		CS.Value,
		SettingDescription = S.[Description],
		A.ConfigurationID,
		ConfigurationDescription = A.[Description],
		E.EnvironmentID,
		EnvironmentDescription = E.[Description],
		S.SettingID,
		ST.ConfigurationTypeID,
		ConfigurationTypeDescription = ST.[Description],
		CS.CreateUserID,
		CS.CreateDate,
		CS.CreateDateUTC,
		CS.ModifyUserID,
		CS.ModifyDate,
		CS.ModifyDateUTC		
	FROM 
		app.ConfigurationSetting AS CS WITH( NoLock )
			JOIN app.[Configuration] A WITH( NoLock )
				ON CS.ConfigurationID = A.ConfigurationID
			JOIN app.Setting S WITH( NoLock )
				ON CS.SettingID = S.SettingID
			JOIN app.ConfigurationType ST WITH( NoLock )
				ON S.ConfigurationTypeID = ST.ConfigurationTypeID
			JOIN app.Environment E WITH( NoLock )
				ON CS.EnvironmentID = E.EnvironmentID
GO
