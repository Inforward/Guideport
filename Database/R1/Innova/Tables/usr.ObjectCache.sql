USE [Innova]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserObjectCache]') AND type in (N'U'))
	DROP TABLE [dbo].[UserObjectCache]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[ObjectCache]') AND type in (N'U'))
BEGIN

	CREATE TABLE [usr].[ObjectCache]
	(
		[UserID] [int] NOT NULL,
		[Key] [varchar](128) NOT NULL,
		[Value] [varchar](max) NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		CONSTRAINT [PK_ObjectCache] PRIMARY KEY CLUSTERED (	[UserID] ASC,	[Key] ASC )
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO
