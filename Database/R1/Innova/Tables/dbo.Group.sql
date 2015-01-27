USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Group]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Group]
	(
		GroupID				INT IDENTITY( 1, 1 ) NOT NULL,
		Name				VARCHAR( 256 ) NOT NULL,
		[Description]		VARCHAR( 1024 ) NULL,
		IsReadOnly			BIT NOT NULL,
		CreateUserID		INT NOT NULL,
		CreateDate			DATETIME NOT NULL,
		CreateDateUTC		DATETIME NOT NULL,
		ModifyUserID		INT NOT NULL,
		ModifyDate			DATETIME NOT NULL,
		ModifyDateUTC		DATETIME NOT NULL,
		CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED( GroupID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'UX_Group_Name' )
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_Group_Name] ON [dbo].[Group] ( [Name] ASC )
END
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Group_GroupType_GroupTypeID' )
	ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_GroupType_GroupTypeID]
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'GroupTypeID' AND [object_id] = OBJECT_ID(N'[dbo].[Group]'))
BEGIN
    ALTER TABLE [dbo].[Group] DROP COLUMN [GroupTypeID]
END

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OwnerUserID' AND [object_id] = OBJECT_ID(N'[dbo].[Group]'))
BEGIN
    ALTER TABLE [dbo].[Group] DROP COLUMN [OwnerUserID]
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Description' AND [object_id] = OBJECT_ID(N'[dbo].[Group]'))
BEGIN
    ALTER TABLE [dbo].[Group] ADD [Description] VARCHAR( 1024 ) NULL
END

