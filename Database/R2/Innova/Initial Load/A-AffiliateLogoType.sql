USE [Innova]
GO

IF NOT EXISTS( SELECT 1 FROM [dbo].[AffiliateLogoType] )
BEGIN
	INSERT INTO [dbo].[AffiliateLogoType] ( [AffiliateLogoTypeID], [Name], [Description] )
		SELECT 1, 'Tile', 'Small logo, currently used on the Login Page'
		UNION SELECT 2, 'PDF Header', 'Medium logo, currently used within the header of some PDF exports'
		UNION SELECT 3, 'PDF Cover Sheet', 'Large logo, currently used on the cover sheet of some PDF exports'
END
GO
