USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanningWizardActionItem]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[PlanningWizardActionItem]
	(
		[PlanningWizardActionItemID] [int] NOT NULL,
		[PlanningWizardStepID] [int] NOT NULL,
		[ActionItemText] [varchar](512) NOT NULL,
		[SortOrder] [int] NOT NULL,
		[ResourcesContent] [varchar](max) NULL,
		[ModifyUserID] [int] NOT NULL,
		[ModifyDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_PlanningWizardActionItem] PRIMARY KEY CLUSTERED 
	(
		[PlanningWizardActionItemID] ASC
	)
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO