USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rpt].[ViewFilter]') AND type in (N'U'))
BEGIN
	CREATE TABLE [rpt].[ViewFilter]
	(
		ViewID					INT NOT NULL,
		FilterID				INT NOT NULL,
		CONSTRAINT [PK_ViewFilter] PRIMARY KEY( ViewID, FilterID )
	) ON [PRIMARY]
END
GO
