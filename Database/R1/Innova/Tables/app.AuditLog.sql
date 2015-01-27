USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM information_schema.schemata WHERE   schema_name = 'app' ) 
	EXEC sp_executesql N'CREATE SCHEMA app'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[AuditLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[AuditLog]
	(
		AuditLogID		INT NOT NULL IDENTITY(1, 1 ),
		AuditTypeID		TINYINT NOT NULL,
		TableName		VARCHAR( 50 ),
		TableIDValue	INT,
		RelatedKeyName	VARCHAR( 50 ),
		RelatedKeyValue	INT,
		UserID			INT NOT NULL CONSTRAINT [DF_AuditLog_UserID] DEFAULT 0,
		OldData			VARCHAR( MAX ),
		NewData			VARCHAR( MAX ),
		AuditDate		DATETIME NOT NULL CONSTRAINT [DF_AuditLog_AuditDate] DEFAULT GETDATE(),
		CreateDate	DATETIME NOT NULL CONSTRAINT [DF_AuditLog_CreateDate] DEFAULT GETDATE()
		CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED( AuditLogID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
