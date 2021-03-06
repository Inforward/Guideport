USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[File]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[File]
	(
		FileID			INT NOT NULL IDENTITY( 1, 1 ),
		Data			VARBINARY( MAX ) NOT NULL,
		CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED( FileID ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
