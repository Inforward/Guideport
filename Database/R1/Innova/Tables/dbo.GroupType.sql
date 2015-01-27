USE [Innova]
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_Group_GroupType_GroupTypeID' )
	ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_GroupType_GroupTypeID]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupType]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[GroupType]
END
GO
