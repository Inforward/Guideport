USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rpt].[Column]') AND type in (N'U'))
BEGIN
	CREATE TABLE [rpt].[Column]
	(
		[ColumnID] [int] NOT NULL,
		[Title] [varchar](50) NOT NULL,
		[DataField] [varchar](100) NOT NULL,
		[DataFormat] [varchar](50) NULL,
		[DataTypeName] [varchar](50) NOT NULL,
		[Width] [int] NULL,
		CONSTRAINT [PK_Column] PRIMARY KEY CLUSTERED ( [ColumnID] ASC )  ON [PRIMARY]
	) ON [PRIMARY]
END
GO
