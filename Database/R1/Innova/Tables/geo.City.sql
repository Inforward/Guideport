USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'geo' ) 
	EXEC sp_executesql N'CREATE SCHEMA geo'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[geo].[City]') AND type in (N'U'))
BEGIN

	CREATE TABLE [geo].[City]
	(
		[CityID] int NOT NULL IDENTITY(1,1),
		[Name] [varchar](500) NOT NULL,
		[StateProvinceID] [int] NULL,
		[CountryID] [int] NULL,
		[TimeZoneID] [int] NULL,
		[Latitude] [real] NULL,
		[Longitude] [real] NULL,		
		CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED( CityID )
	) ON [PRIMARY]

END

GO


