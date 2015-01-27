USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'usr' ) 
	EXEC sp_executesql N'CREATE SCHEMA usr'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[ApplicationRoleAccess]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[ApplicationRoleAccess]
	(
		ApplicationRoleID	INT NOT NULL,
		ApplicationAccessID	INT NOT NULL,
		[Description]		VARCHAR( 512 ) NOT NULL,
		CONSTRAINT [PK_ApplicationRoleAccess] PRIMARY KEY CLUSTERED( ApplicationRoleID, ApplicationAccessID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns  WHERE [name] = N'Description' AND [object_id] = OBJECT_ID(N'[usr].[ApplicationRoleAccess]'))
BEGIN
    ALTER TABLE [usr].[ApplicationRoleAccess] ADD [Description] VARCHAR( 512 )
END