USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'app' ) 
	EXEC sp_executesql N'CREATE SCHEMA app'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[Ruleset]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[Ruleset]
	(
		[Name] [varchar](500) NOT NULL,
		[MajorVersion] [int] NOT NULL,
		[MinorVersion] [int] NOT NULL,
		[RuleSet] [varchar](max) NOT NULL,
		[Status] [smallint] NULL,
		[AssemblyPath] [varchar](max) NULL,
		[ActivityName] [varchar](max) NULL,
		[ModifiedDate] [datetime] NULL,
		[ModifiedBy] [varchar](1000) NULL,
		CONSTRAINT [PK_Ruleset] PRIMARY KEY CLUSTERED ( [Name] ASC,	[MajorVersion] ASC,	[MinorVersion] ASC )
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GO