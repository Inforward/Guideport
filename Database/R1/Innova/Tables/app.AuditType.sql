USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[AuditType]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[AuditType]
	(
		AuditTypeID		TINYINT NOT NULL,
		Name			VARCHAR( 50 ),
		[Description]	VARCHAR( 250 ),
		CONSTRAINT [PK_AuditType] PRIMARY KEY CLUSTERED( AuditTypeID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
