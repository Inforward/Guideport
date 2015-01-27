USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'geo' ) 
	EXEC sp_executesql N'CREATE SCHEMA geo'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[geo].[TimeZone]') AND type in (N'U'))
BEGIN

	CREATE TABLE [geo].[TimeZone]
	(
		[TimeZoneID] [int] NOT NULL IDENTITY( 1, 1 ),
		[Name] [varchar](64) NOT NULL,
		[OffsetGMT] [float] NOT NULL,
		[OffsetDST] [float] NOT NULL,
		CONSTRAINT [PK_TimeZone] PRIMARY KEY CLUSTERED (	[TimeZoneID] ASC )
	) ON [PRIMARY]

END

GO