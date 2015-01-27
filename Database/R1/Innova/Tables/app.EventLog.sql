USE [Innova]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventLog]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[EventLog]	
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[EventLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[EventLog]
	(
		EventLogID	INT NOT NULL IDENTITY(1, 1 ),
		EventTypeID	TINYINT NOT NULL,
		EventDate	DATETIME NOT NULL CONSTRAINT [DF_EventLog_EventDate] DEFAULT GETDATE(),
		ServerName	VARCHAR( 100 ),
		ServerIP	VARCHAR( 25 ),
		RemoteIP	VARCHAR( 25 ),
		Message		VARCHAR( 2000 ),
		ErrorCode	INT,
		ErrorText	VARCHAR( 2000 ),
		RequestMethod VARCHAR( 50 ),
		ScriptName	VARCHAR( 300 ),
		QueryString VARCHAR( 1000 ),
		PostData	VARCHAR( MAX ),
		Referer		VARCHAR( 300 ),
		BrowserType	VARCHAR( 300 ),
		Source		VARCHAR( 2000 ),
		StackTrace	VARCHAR( MAX ),
		UserID		INT,
		CreateDate	DATETIME NOT NULL CONSTRAINT [DF_EventLog_CreateDate] DEFAULT GETDATE(),
		CONSTRAINT [PK_EventLog] PRIMARY KEY CLUSTERED( EventLogID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

ALTER TABLE [app].[EventLog] ALTER COLUMN [Referer] VARCHAR(2000)
GO

ALTER TABLE [app].[EventLog] ALTER COLUMN [ErrorText] VARCHAR(2000)
GO
