USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'geo' ) 
	EXEC sp_executesql N'CREATE SCHEMA geo'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[geo].[StateProvince]') AND type in (N'U'))
BEGIN

CREATE TABLE [geo].[StateProvince]
(
	[StateProvinceID] [int] NOT NULL IDENTITY(1,1),
	[CountryID] [int] NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[StateCode] [varchar](32) NOT NULL,
	CONSTRAINT [PK_StateProvince] PRIMARY KEY CLUSTERED  ( [StateProvinceID] ASC )
) ON [PRIMARY]

END
GO

/*
ALTER TABLE [dbo].[StateProvince] ADD CONSTRAINT [FK_StateProvince_Country] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([CountryID])
GO

ALTER TABLE [dbo].[StateProvince] CHECK CONSTRAINT [FK_StateProvince_Country]
GO
*/