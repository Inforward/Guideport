USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cms].[SiteDocumentType]') AND type in (N'U'))
BEGIN

	CREATE TABLE [cms].[SiteDocumentType]
	(
		[SiteDocumentTypeID] [int] NOT NULL,
		[DocumentTypeName] [varchar](64) NOT NULL,
		[DocumentTypeExtension] [varchar](64) NOT NULL,
		[MIMEType] [varchar](64) NOT NULL,
		CONSTRAINT [PK_cms.SiteDocumentType] PRIMARY KEY CLUSTERED  ( [SiteDocumentTypeID] ASC )
	) ON [PRIMARY]

END
GO