USE [Innova]
GO

DELETE FROM [dbo].[PlanningWizardProgress]
DELETE FROM [dbo].[PlanningWizardActionItem]
DELETE FROM [dbo].[PlanningWizardStep]
DELETE FROM [dbo].[PlanningWizardPhase]
DELETE FROM [dbo].[PlanningWizard]
GO

INSERT INTO [dbo].[PlanningWizard] ([PlanningWizardID],[Name],[Description],[CompleteMessage])
	SELECT [WorkflowID], [Name], [PageDescription], [CompleteMessage] FROM [Innova_Staging].[dbo].[Workflow]
GO

INSERT INTO [dbo].[PlanningWizardPhase] ([PlanningWizardPhaseID],[PlanningWizardID],[Name],[NameHtml],[Description],[SortOrder])
	SELECT 
		[WorkflowPhaseID], 
		[WorkflowID], 
		[Name], 
		[NameHtml] = CASE [Name]
						WHEN 'Find a Partner' THEN 'Find a<br/>Partner'
						WHEN 'Find a Seller' THEN 'Find a<br/>Seller'
						WHEN 'Deal Terms & Financing' THEN 'Deal Terms<br/>& Financing'
						WHEN 'Education & Optimization' THEN 'Education<br/>& Optimization'
						WHEN 'Transaction Execution' THEN 'Transaction<br/>Execution'
						WHEN 'Transition Support & Maximizing Value' THEN 'Transition Support<br/>& Maximizing Value'
						ELSE [Name]
					END,
		[Description], 
		[PhaseOrdinal] 
	FROM 
		[Innova_Staging].[dbo].[WorkflowPhase]
GO

INSERT INTO [dbo].[PlanningWizardStep] 
(
	[PlanningWizardStepID],
    [PlanningWizardPhaseID],
    [StepNo],
    [StepWeight],
    [Name],
    [Description]
)
SELECT
	[WorkflowStepID],
	[WorkflowPhaseID],
	[StepNo],
	[StepWeight],
	[Name],
	CASE WHEN ISNULL([LearnWhyContent], '') <> '' THEN [LearnWhyContent] ELSE [Description] END
FROM
	[Innova_Staging].[dbo].[WorkflowStep]
GO

INSERT INTO [dbo].[PlanningWizardActionItem] ([PlanningWizardActionItemID],[PlanningWizardStepID],[ActionItemText],[SortOrder],[ResourcesContent],ModifyUserID,ModifyDate)
	SELECT [WorkflowActionItemID],[WorkflowStepID],[ActionItemText],[ActionItemOrdinal],[ResourcesContent],0,GetDate() FROM [Innova_Staging].[dbo].[WorkflowActionItem]
GO

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_PlanningWizardPhase_PlanningWizard_PlanningWizardID' )
	ALTER TABLE [dbo].[PlanningWizardPhase] ADD  CONSTRAINT [FK_PlanningWizardPhase_PlanningWizard_PlanningWizardID] FOREIGN KEY([PlanningWizardID]) REFERENCES [dbo].[PlanningWizard] ([PlanningWizardID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_PlanningWizardStep_PlanningWizardPhase_PlanningWizardPhaseID' )
	ALTER TABLE [dbo].[PlanningWizardStep] ADD  CONSTRAINT [FK_PlanningWizardStep_PlanningWizardPhase_PlanningWizardPhaseID] FOREIGN KEY([PlanningWizardPhaseID]) REFERENCES [dbo].[PlanningWizardPhase] ([PlanningWizardPhaseID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_PlanningWizardActionItem_PlanningWizardStep_PlanningWizardStepID' )
	ALTER TABLE [dbo].[PlanningWizardActionItem] ADD  CONSTRAINT [FK_PlanningWizardActionItem_PlanningWizardStep_PlanningWizardStepID] FOREIGN KEY([PlanningWizardStepID]) REFERENCES [dbo].[PlanningWizardStep] ([PlanningWizardStepID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_PlanningWizardProgress_PlanningWizard_PlanningWizardID' )
	ALTER TABLE [dbo].[PlanningWizardProgress] ADD  CONSTRAINT [FK_PlanningWizardProgress_PlanningWizard_PlanningWizardID] FOREIGN KEY([PlanningWizardID]) REFERENCES [dbo].[PlanningWizard] ([PlanningWizardID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_PlanningWizardProgress_PlanningWizardPhase_PlanningWizardPhaseID' )
	ALTER TABLE [dbo].[PlanningWizardProgress] ADD  CONSTRAINT [FK_PlanningWizardProgress_PlanningWizardPhase_PlanningWizardPhaseID] FOREIGN KEY([CurrentPlanningWizardPhaseID]) REFERENCES [dbo].[PlanningWizardPhase] ([PlanningWizardPhaseID])

GO

UPDATE [dbo].[PlanningWizardStep] 
	SET [Description] = REPLACE( [Description], '<p>', ' ' )
WHERE
	[Description] LIKE '%<p>%'

UPDATE [dbo].[PlanningWizardStep] 
	SET [Description] = REPLACE( [Description], '</p>', '' )
WHERE
	[Description] LIKE '%</p>%'

GO

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Use this Checklist - <a target="_blank" href="/Content/Detail/27548">Seller''s Guideline & Due Diligence Checklist</a>'
WHERE [PlanningWizardActionItemID] = 58

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Use this Checklist - <a target="_blank" href="/Content/Detail/27548">Seller''s Guideline & Due Diligence Checklist</a>'
WHERE [PlanningWizardActionItemID] = 108

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Read this article - <a target="_blank" href="/Content/Detail/27623">Considerations for Maximizing Advisory Firm Value</a>'
WHERE [PlanningWizardActionItemID] = 99

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Read this article - <a target="_blank" href="/Content/Detail/29352">Succession Planning & Acquisition Insights (Newsletters) VOL 2</a>'
WHERE [PlanningWizardActionItemID] = 104

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Read this article - <a target="_blank" href="/Content/Detail/35504">Be Proactive in Succession</a>'
WHERE [PlanningWizardActionItemID] = 113

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Use this Checklist - <a target="_blank" href="/Content/Detail/27548">Seller''s Guideline & Due Diligence Checklist</a>'
WHERE [PlanningWizardActionItemID] = 117

UPDATE [Innova].[dbo].[PlanningWizardActionItem]
SET ResourcesContent = 'Use this Template - <a target="_blank" href="/Content/Detail/27554">Template for Creating a Five-Year Business Value Plan</a><p>Read this article - <a target="_blank" href="/Content/Detail/28441">How to Create Business Value Using "The GAP Principle"</a></p><p>Read this article – <a target="_blank" href="/Content/Detail/27623">Considerations for Maximizing Advisory Firm Value</a></p><p>Use this Template - <a target="_blank" href="/Content/Detail/27192">Creating a Growth Incentive Plan (GIP) for your Business</a></p>'
WHERE [PlanningWizardActionItemID] = 42

GO

