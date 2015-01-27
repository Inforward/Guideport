USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'usr' ) 
	EXEC sp_executesql N'CREATE SCHEMA usr'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[ApplicationRole]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[ApplicationRole]
	(
		ApplicationRoleID	INT NOT NULL,
		Name			VARCHAR( 50 ) NOT NULL,
		DisplayName		VARCHAR( 50 ) NOT NULL,
		Description		VARCHAR( 512 ) NOT NULL,
		CONSTRAINT [PK_ApplicationRole] PRIMARY KEY CLUSTERED( ApplicationRoleID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

ALTER TABLE [usr].[ApplicationRole] ALTER COLUMN [Description] VARCHAR( 512 ) NOT NULL
GO
