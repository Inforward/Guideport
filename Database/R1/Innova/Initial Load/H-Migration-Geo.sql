USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_City_StateProvince_StateProvinceID' )
	ALTER TABLE [geo].[City] DROP CONSTRAINT [FK_City_StateProvince_StateProvinceID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_City_Country_CountryID' )
	ALTER TABLE [geo].[City] DROP CONSTRAINT [FK_City_Country_CountryID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_City_TimeZone_TimeZoneID' )
	ALTER TABLE [geo].[City] DROP  CONSTRAINT [FK_City_TimeZone_TimeZoneID]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_StateProvince_Country_CountryID' )
	ALTER TABLE [geo].[StateProvince] DROP CONSTRAINT [FK_StateProvince_Country_CountryID]
GO

DELETE FROM [geo].[City]
DELETE FROM [geo].[TimeZone]
DELETE FROM [geo].[StateProvince]
DELETE FROM [geo].[Country]
GO

-- Countries
SET IDENTITY_INSERT [geo].[Country] ON

INSERT INTO [geo].[Country] ([CountryID],[Name],[CountryCode],[PostalCodeRegEx])
	SELECT [CountryID],[Name],[Abbreviation],[PostalCodeRegEx] FROM [Innova_Staging].dbo.Country ORDER BY [Name]

SET IDENTITY_INSERT [geo].[Country] OFF

-- State Provinces

INSERT INTO [geo].[StateProvince] ([CountryID],[Name],[StateCode])
	SELECT 
		DISTINCT
		CN.CountryID,
		A1.asciiname AS [Name],
		C.admin1 AS [StateCode]
	FROM
		[Innova_Staging].dbo.City C WITH (NOLOCK)
		JOIN [Innova_Staging].dbo.admin1codes A1 WITH (NOLOCK) 
			ON A1.Code = C.countrycode + '.' + C.admin1
		JOIN geo.Country CN
			ON C.CountryCode = CN.CountryCode
	ORDER BY		
		A1.asciiname

-- Timezones
INSERT INTO [geo].[TimeZone] ( [Name], [OffsetGMT], [OffsetDST] )
	SELECT 
		[TimeZoneId], CAST( [GMToffset20120101] AS FLOAT ), CAST( [DSToffset20120701] AS FLOAT )
	FROM
		[Innova_Staging].dbo.Timezones


INSERT INTO [geo].[City] ([Name],[StateProvinceID],[CountryID],[Latitude],[Longitude],[TimeZoneID])
	SELECT 
		C.asciiname AS [Name],
		SP.StateProvinceID,
		CN.CountryID,
		C.Latitude,
		C.Longitude,
		T.TimezoneID
	FROM 
		[Innova_Staging].dbo.City C
		JOIN geo.StateProvince SP
			ON C.admin1 = SP.StateCode
		JOIN geo.Country CN
			ON C.CountryCode = CN.CountryCode
				AND SP.CountryID = CN.CountryID
		JOIN geo.Timezone T
			ON C.Timezone = T.[Name]
	ORDER BY
		C.CountryCode, C.admin1, C.asciiname

GO

-- Update state codes
UPDATE SP
	SET SP.[StateCode] = G.Abbreviation
FROM
	[geo].[StateProvince] SP
	JOIN [Innova_Staging].dbo.StateProvince G
		ON SP.CountryID = G.CountryID
			AND SP.Name = G.Name



IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_City_StateProvince_StateProvinceID' )
	ALTER TABLE [geo].[City] ADD  CONSTRAINT [FK_City_StateProvince_StateProvinceID] FOREIGN KEY([StateProvinceID]) REFERENCES [geo].[StateProvince] ([StateProvinceID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_City_Country_CountryID' )
	ALTER TABLE [geo].[City] ADD  CONSTRAINT [FK_City_Country_CountryID] FOREIGN KEY([CountryID]) REFERENCES [geo].[Country] ([CountryID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_City_TimeZone_TimeZoneID' )
	ALTER TABLE [geo].[City] ADD  CONSTRAINT [FK_City_TimeZone_TimeZoneID] FOREIGN KEY([TimeZoneID]) REFERENCES [geo].[TimeZone] ([TimeZoneID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_StateProvince_Country_CountryID' )
	ALTER TABLE [geo].[StateProvince] ADD  CONSTRAINT [FK_StateProvince_Country_CountryID] FOREIGN KEY([CountryID]) REFERENCES [geo].[Country] ([CountryID])