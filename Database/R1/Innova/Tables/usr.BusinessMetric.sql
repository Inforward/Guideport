USE Innova
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBusinessMetric]') AND type in (N'U'))
	DROP TABLE [dbo].[UserBusinessMetric]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[usr].[BusinessMetric]') AND type in (N'U'))
BEGIN
	CREATE TABLE [usr].[BusinessMetric]
	(
		[UserID] [int] NOT NULL,
		[GDC_T12] [float] NULL,
		[GDC_PriorYear] [float] NULL,
		[AUM] [decimal](38, 6) NULL,
		[AUM_Split] [decimal](38, 11) NULL,
		[ReturnOnAUM] [float] NULL,
		[NoOfClients] [int] NULL,
		[NoOfAccounts] [int] NULL,
		[RevenueRecurring] [float] NULL,
		[RevenueNonRecurring] [float] NULL,
		[BusinessValuationLow] [float] NULL,
		[BusinessValuationHigh] [float] NULL,
		[AccountValueTotal] [decimal](38, 6) NULL,
		[AccountValueAverage] [decimal](38, 6) NULL,
		[UpdateDate] [datetime] NOT NULL,
		CONSTRAINT [PK_BusinessMetric] PRIMARY KEY CLUSTERED ( [UserID] ASC )
	) ON [PRIMARY]
END
GO