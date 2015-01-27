USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteThirdPartyResourceService]') AND type in (N'U'))
BEGIN
	CREATE TABLE [cms].[SiteThirdPartyResourceService]
	(
		[SiteThirdPartyResourceServiceID] [int] NOT NULL,
		[ServiceName] [varchar](256) NULL,
		CONSTRAINT [PK_SiteThirdPartyResourceService] PRIMARY KEY CLUSTERED ( [SiteThirdPartyResourceServiceID] ASC )
	) ON [PRIMARY]
END
GO



