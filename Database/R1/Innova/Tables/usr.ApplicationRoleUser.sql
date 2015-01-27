USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[ApplicationRoleUser]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[ApplicationRoleUser]
	(
		UserID					INT NOT NULL,
		ApplicationRoleID		INT NOT NULL,
		ApplicationAccessID INT NOT NULL,
		CreateUserID			INT NOT NULL CONSTRAINT [DF_ApplicationRoleUser_CreateUserID] DEFAULT 0,
		CreateDate				DATETIME NOT NULL CONSTRAINT [DF_ApplicationRoleUser_CreateDate] DEFAULT GETDATE(),
		CONSTRAINT [PK_ApplicationRoleUser] PRIMARY KEY CLUSTERED( UserID, ApplicationRoleID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
