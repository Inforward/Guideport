USE [Innova]
GO

IF NOT EXISTS ( SELECT  schema_name FROM information_schema.schemata WHERE   schema_name = 'app' ) 
	EXEC sp_executesql N'CREATE SCHEMA app'
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[app].[EntityFrameworkViewCache]') AND type in (N'U'))
BEGIN
	CREATE TABLE [app].[EntityFrameworkViewCache] 
	(
		[ConceptualModelContainerName] [nvarchar](255) not null,
		[StoreModelContainerName] [nvarchar](255) not null,
		[ViewDefinitions] [nvarchar](max) null,
		[LastUpdated] [datetimeoffset](7) not null,
		CONSTRAINT [PK_EntityFrameworkViewCache] PRIMARY KEY CLUSTERED([ConceptualModelContainerName], [StoreModelContainerName])
	)
END
GO
