USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanningWizardProgress]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[PlanningWizardProgress]
	(
		[PlanningWizardProgressID] [int] IDENTITY(1,1) NOT NULL,
		[PlanningWizardID] [int] NOT NULL,
		[UserID] [int] NOT NULL,
		[CurrentPlanningWizardPhaseID] [int] NOT NULL,
		[PercentComplete] decimal(5,2) NOT NULL,
		[ProgressXml] [xml] NOT NULL,
		[CreateUserID] [int] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[CreateDateUTC] [datetime] NOT NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
		[ModifyDateUTC] [datetime] NOT NULL,
		CONSTRAINT [PK_PlanningWizardProgress] PRIMARY KEY CLUSTERED ( [PlanningWizardProgressID] ASC )
	) ON [PRIMARY]

END
GO