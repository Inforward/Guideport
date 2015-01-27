USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupGroup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GroupGroup]
	(
		GroupID			INT NOT NULL,
		MemberGroupID	INT NOT NULL,
		CONSTRAINT [PK_GroupGroup] PRIMARY KEY CLUSTERED( GroupID, MemberGroupID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
