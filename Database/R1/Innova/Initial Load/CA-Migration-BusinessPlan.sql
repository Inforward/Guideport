USE [Innova]
GO

-- This script truncates all BusinessPlan tables and reseeds default options
DELETE FROM [dbo].[AffiliateBusinessPlanObjective]
DELETE FROM [dbo].[BusinessPlanStrategyTactic]
DELETE FROM [dbo].[BusinessPlanTactic]
DELETE FROM [dbo].[BusinessPlanObjectiveStrategy]
DELETE FROM [dbo].[BusinessPlanStrategy]
DELETE FROM [dbo].[BusinessPlanObjective]
DELETE FROM [dbo].[BusinessPlanEmployeeEmployeeRole]
DELETE FROM [dbo].[BusinessPlanEmployee]
DELETE FROM [dbo].[BusinessPlanEmployeeRole]
DELETE FROM [dbo].[BusinessPlanSwot]
DELETE FROM [dbo].[BusinessPlan]
GO

SET IDENTITY_INSERT dbo.BusinessPlanEmployeeRole ON
INSERT INTO dbo.BusinessPlanEmployeeRole
( BusinessPlanEmployeeRoleID, BusinessPlanID, Name, Description, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC, 
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC,
	SortOrder )
SELECT BusinessPlanEmployeeRoleID, B.BusinessPlanID, Name, Description, 
	CreateUser, 
	B.CreateDate, 
	DATEADD( hh, 7, B.CreateDate ),
	ModifyUser, 
	B.ModifyDate, 
	DATEADD( hh, 7, B.ModifyDate ),
	SortOrder
FROM [Innova_Staging].dbo.BusinessPlanEmployeeRole B WITH( NoLock )
WHERE B.BusinessPlanID IS NULL
SET IDENTITY_INSERT dbo.BusinessPlanEmployeeRole OFF

SET IDENTITY_INSERT dbo.BusinessPlanObjective ON
INSERT INTO dbo.BusinessPlanObjective
( BusinessPlanObjectiveID, BusinessPlanID, Name, Value, DataType, PercentComplete, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC,
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC,
	EstimatedCompletionDate, 
	SortOrder, 
	AutoTrackingEnabled, 
	BaselineValue, 
	BaselineDate )
SELECT BusinessPlanObjectiveID, B.BusinessPlanID, Name, Value, DataType, PercentComplete, 
	CreateUser, 
	B.CreateDate, 
	DATEADD( hh, 7, B.CreateDate ),
	ModifyUser, 
	B.ModifyDate, 
	DATEADD( hh, 7, B.ModifyDate ),
	EstimatedCompletionDate, 
	SortOrder, 
	AutoTrackingEnabled, 
	BaselineValue, 
	BaselineDate
FROM [Innova_Staging].dbo.BusinessPlanObjective B WITH( NoLock )
WHERE B.BusinessPlanID IS NULL
SET IDENTITY_INSERT dbo.BusinessPlanObjective OFF

SET IDENTITY_INSERT dbo.BusinessPlanStrategy ON
INSERT INTO dbo.BusinessPlanStrategy
( BusinessPlanStrategyID, BusinessPlanID, Name, Description, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC, 
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC, 
	SortOrder, 
	Editable )
SELECT BusinessPlanStrategyID, B.BusinessPlanID, Name, Description, 
	CreateUser, 
	B.CreateDate, 
	DATEADD( hh, 7, B.CreateDate ),
	ModifyUser, 
	B.ModifyDate, 
	DATEADD( hh, 7, B.ModifyDate ),
	SortOrder, 
	Editable
FROM [Innova_Staging].dbo.BusinessPlanStrategy B WITH( NoLock )
WHERE B.BusinessPlanID IS NULL
SET IDENTITY_INSERT dbo.BusinessPlanStrategy OFF

SET IDENTITY_INSERT dbo.BusinessPlanTactic ON
INSERT INTO dbo.BusinessPlanTactic
( BusinessPlanTacticID, BusinessPlanID, Name, Description, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC,
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC, 
	SortOrder, 
	CompletedDate, 
	Editable )
SELECT BusinessPlanTacticID, B.BusinessPlanID, Name, Description, 
	CreateUser, 
	B.CreateDate, 
	DATEADD( hh, 7, B.CreateDate ),
	ModifyUser, 
	B.ModifyDate, 
	DATEADD( hh, 7, B.ModifyDate ),
	SortOrder, 
	CompletedDate, 
	Editable
FROM [Innova_Staging].dbo.BusinessPlanTactic B WITH( NoLock )
WHERE B.BusinessPlanID IS NULL
SET IDENTITY_INSERT dbo.BusinessPlanTactic OFF

INSERT INTO dbo.AffiliateBusinessPlanObjective( AffiliateID, BusinessPlanObjectiveID, AutoTrackingEnabled )
SELECT 
	A.AffiliateID, O.BusinessPlanObjectiveID, 
	-- Disable auto-tracking for Cetera BDs
	CASE WHEN A.AffiliateID IN ( 2, 3, 4, 5 ) THEN 0 ELSE O.AutoTrackingEnabled END
FROM dbo.Affiliate A WITH( NoLock )
	CROSS JOIN dbo.BusinessPlanObjective O WITH( NoLock )
WHERE O.BusinessPlanID IS NULL 
	AND O.AutoTrackingEnabled = 1

GO
