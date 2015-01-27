USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'geo' ) 
	EXEC sp_executesql N'CREATE SCHEMA geo'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[geo].[Country]') AND type in (N'U'))
BEGIN

	CREATE TABLE [geo].[Country]
	(
		[CountryID] [int] NOT NULL IDENTITY( 1, 1 ),
		[Name] [varchar](64) NOT NULL,
		[CountryCode] [varchar](2) NOT NULL,
		[PostalCodeRegEx] [varchar](256) NULL,
		CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED (	[CountryID] ASC )
	) ON [PRIMARY]

END

GO