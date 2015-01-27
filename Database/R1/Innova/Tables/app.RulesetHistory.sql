USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'app' ) 
	EXEC sp_executesql N'CREATE SCHEMA app'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[RulesetHistory]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[RulesetHistory]
	(
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [varchar](500) NOT NULL,
		[MajorVersion] [int] NOT NULL,
		[MinorVersion] [int] NOT NULL,
		[RuleSet] [ntext] NOT NULL,
		[Status] [smallint] NULL,
		[AssemblyPath] [varchar](max) NULL,
		[ActivityName] [varchar](max) NULL,
		[ModifiedDate] [datetime] NULL,
		[ModifiedBy] [varchar](1000) NULL,
		CONSTRAINT [PK_RulesetHistory] PRIMARY KEY CLUSTERED (	[ID] ASC )
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO