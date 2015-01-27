USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rpt].[Filter]') AND type in (N'U'))
BEGIN
	CREATE TABLE [rpt].[Filter]
	(
		FilterID				INT NOT NULL,
		Name					VARCHAR( 50 ) NOT NULL,
		Label					VARCHAR( 50 ) NOT NULL,
		IsRequired				BIT NOT NULL CONSTRAINT [DF_Filter_IsRequired] DEFAULT 0,
		DataTypeName			VARCHAR( 50 ) NOT NULL,
		ParameterName			VARCHAR( 50 ) NOT NULL,
		InputType				VARCHAR( 50 ) NOT NULL,
		CONSTRAINT [PK_Filter]  PRIMARY KEY CLUSTERED ( [FilterID] ASC ),
	) ON [PRIMARY]
END
GO
