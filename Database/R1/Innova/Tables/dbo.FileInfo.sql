USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileInfo]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[FileInfo]
	(
		FileID			INT NOT NULL,
		Name			VARCHAR( 200 ) NOT NULL,
		Extension		VARCHAR( 50 ) NOT NULL,
		SizeBytes		INT NOT NULL,
		CreateUserID	INT NOT NULL CONSTRAINT [DF_FileInfo_CreateUserID] DEFAULT 0,
		CreateDate		DATETIME NOT NULL CONSTRAINT [DF_FileInfo_CreateDate] DEFAULT GETDATE(),
		CONSTRAINT [PK_FileInfo] PRIMARY KEY CLUSTERED( FileID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_AffiliateLogo_FileInfo_FileID' )
BEGIN
	ALTER TABLE [dbo].[AffiliateLogo] ADD  CONSTRAINT [FK_AffiliateLogo_FileInfo_FileID] FOREIGN KEY([FileID]) REFERENCES [dbo].[FileInfo] ([FileID])
END
GO
