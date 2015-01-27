USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rpt].[View]') AND type in (N'U'))
BEGIN
	CREATE TABLE [rpt].[View]
	(
		ViewID					INT NOT NULL,
		CategoryID				INT NOT NULL,
		Name					VARCHAR( 50 ) NOT NULL,
		FullName				VARCHAR( 100 ) NOT NULL,
		StoredProcedureName		VARCHAR( 50 ) NOT NULL,
		PageSize				INT NULL,
		IsPageable				BIT NOT NULL CONSTRAINT [DF_View_IsPageable] DEFAULT 1,
		IsSortable				BIT NOT NULL CONSTRAINT [DF_View_IsSortable] DEFAULT 1,
		IsEnabled				BIT NOT NULL CONSTRAINT [DF_View_IsEnabled] DEFAULT 1,
		CONSTRAINT [PK_View] PRIMARY KEY CLUSTERED ( [ViewID] ASC )
	) ON [PRIMARY]
END
GO
