USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'rpt' ) 
	EXEC sp_executesql N'CREATE SCHEMA rpt'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rpt].[Category]') AND type in (N'U'))
BEGIN
	CREATE TABLE [rpt].[Category]
	(
		CategoryID			INT NOT NULL,
		ParentCategoryID	INT,
		Name					VARCHAR( 50 ) NOT NULL,
		CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ( [CategoryID] ASC ),
		CONSTRAINT [FK_Category_ParentCategoryID] FOREIGN KEY( ParentCategoryID )
			REFERENCES [rpt].[Category]( CategoryID )
	) ON [PRIMARY]
END
GO
