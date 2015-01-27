USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanningWizardPhase]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[PlanningWizardPhase]
(
	[PlanningWizardPhaseID] [int] NOT NULL,
	[PlanningWizardID] [int] NOT NULL,
	[Name] [varchar](128) NOT NULL,
	[NameHtml] [varchar](256) NOT NULL,
	[Description] [varchar](256) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_PlanningWizardPhase] PRIMARY KEY CLUSTERED 
(
	[PlanningWizardPhaseID] ASC
)
) ON [PRIMARY]
END
GO