USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rpt].[ViewColumn]') AND type in (N'U'))
BEGIN
	CREATE TABLE [rpt].[ViewColumn]
	(
		ViewID					INT NOT NULL,
		ColumnID				INT NOT NULL,
		Ordinal					INT NOT NULL,
		Template				VARCHAR(1000) NULL,
		DataFormat				VARCHAR(50) NULL,
		Width					INT NULL,
		IsSortable				BIT NOT NULL,
		IsEnabled				BIT NOT NULL,
		IsLocked				BIT NOT NULL,
		CONSTRAINT [PK_ViewColumn] PRIMARY KEY( ViewID, ColumnID )
	) ON [PRIMARY]
END
GO
