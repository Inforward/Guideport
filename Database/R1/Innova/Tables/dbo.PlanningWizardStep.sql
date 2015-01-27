USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanningWizardStep]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[PlanningWizardStep]
	(
		[PlanningWizardStepID] [int] NOT NULL,
		[PlanningWizardPhaseID] [int] NOT NULL,
		[StepNo] [int] NOT NULL,
		[StepWeight] [decimal](2, 2) NOT NULL,
		[Name] [varchar](64) NOT NULL,
		[Description] [varchar](max) NOT NULL,
	 CONSTRAINT [PK_PlanningWizardStep] PRIMARY KEY CLUSTERED 
	(
		[PlanningWizardStepID] ASC
	)
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO