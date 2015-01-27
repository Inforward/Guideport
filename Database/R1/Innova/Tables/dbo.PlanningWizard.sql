USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanningWizard]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[PlanningWizard]
	(
		[PlanningWizardID] [int] NOT NULL,
		[Name] [varchar](32) NOT NULL,
		[Description] [varchar](max) NOT NULL,
		[CompleteMessage] [varchar](512) NOT NULL,
		CONSTRAINT [PK_PlanningWizard] PRIMARY KEY CLUSTERED ( [PlanningWizardID] ASC )
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO



