USE [Innova]
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlan' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlan ADD OldID INT
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlanEmployee' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlanEmployee ADD OldID INT
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlanEmployeeRole' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlanEmployeeRole ADD OldID INT
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlanObjective' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlanObjective ADD OldID INT
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlanStrategy' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlanStrategy ADD OldID INT
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlanTactic' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlanTactic ADD OldID INT
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='BusinessPlanSwot' AND COLUMN_NAME = 'OldID' )
	ALTER TABLE dbo.BusinessPlanSwot ADD OldID INT
GO

-- Remove previously migrated records
DELETE FROM [dbo].[BusinessPlanSwot] WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlanStrategyTactic] WHERE BusinessPlanStrategyID IN ( SELECT BusinessPlanStrategyID FROM dbo.BusinessPlanStrategy WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlanObjectiveStrategy] WHERE BusinessPlanObjectiveID IN ( SELECT BusinessPlanObjectiveID FROM dbo.BusinessPlanObjective WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL ) )
DELETE FROM [dbo].[BusinessPlanTactic] WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlanStrategy] WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlanObjective] WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlanEmployeeEmployeeRole] WHERE BusinessPlanEmployeeID IN ( SELECT BusinessPlanEmployeeID FROM dbo.BusinessPlanEmployee WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL ) )
DELETE FROM [dbo].[BusinessPlanEmployeeRole] WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlanEmployee] WHERE OldID IS NOT NULL OR BusinessPlanID IN ( SELECT BusinessPlanID FROM dbo.BusinessPlan WHERE OldID IS NOT NULL )
DELETE FROM [dbo].[BusinessPlan] WHERE OldID IS NOT NULL
GO

-- Insert BusinessPlan records
INSERT INTO dbo.BusinessPlan
( 
	OldID,
	UserID, 
	Year, 
	MissionWhat, 
	MissionHow, 
	MissionWhy, 
	VisionOneYear, 
	VisionFiveYear, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC, 
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC, 
	DeleteUserID, 
	DeleteDate,
	DeleteDateUTC )
SELECT 
	S_BP.BusinessPlanID,
	ISNULL(U.UserID, S_BP.UserID), 
	S_BP.[Year], S_BP.MissionWhat, S_BP.MissionHow, S_BP.MissionWhy, S_BP.VisionOneYear, S_BP.VisionFiveYear,
	S_BP.CreateUser, 
	S_BP.CreateDate, 
	DATEADD( hh, 7, S_BP.CreateDate ),
	S_BP.ModifyUser, 
	S_BP.ModifyDate, 
	DATEADD( hh, 7, S_BP.ModifyDate ),
	S_BP.DeleteUser, 
	S_BP.DeleteDate,
	DATEADD( hh, 7, S_BP.DeleteDate )
FROM 
	[Innova_Staging].dbo.BusinessPlan S_BP WITH( NoLock )
	LEFT JOIN Innova_Staging.dbo.AE_User A WITH( NoLock )
		ON S_BP.[UserID] = A.UserID
	LEFT JOIN [dbo].vwUserSummary U WITH( NoLock )
		ON A.PR_ID = U.ProfileID
			AND U.AffiliateID = 1
			AND ISNUMERIC(U.ProfileID) = 1


-- Insert BusinessPlanEmployee records
INSERT INTO dbo.BusinessPlanEmployee
( OldID, BusinessPlanID, FirstName, MiddleInitial, LastName, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC,
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC
)
SELECT S_BPE.BusinessPlanEmployeeID, BP.BusinessPlanID, S_BPE.FirstName, MiddleInitial, S_BPE.LastName, 
	S_BPE.CreateUser, 
	S_BPE.CreateDate, 
	DATEADD( hh, 7, S_BPE.CreateDate ),
	S_BPE.ModifyUser, 
	S_BPE.ModifyDate, 
	DATEADD( hh, 7, S_BPE.ModifyDate )
FROM [Innova_Staging].dbo.BusinessPlanEmployee S_BPE WITH( NoLock )
	JOIN dbo.BusinessPlan BP WITH( NoLock )
		ON S_BPE.BusinessPlanID = BP.OldID

-- Update BusinessPlanEmployeeParentID
UPDATE BPE
SET BusinessPlanEmployeeParentID = BPE_PARENT.BusinessPlanEmployeeID
FROM dbo.BusinessPlanEmployee BPE WITH( NoLock )
	JOIN [Innova_Staging].dbo.BusinessPlanEmployee S_BPE WITH( NoLock )
		ON BPE.OldID = S_BPE.BusinessPlanEmployeeID
	JOIN dbo.BusinessPlanEmployee BPE_PARENT WITH( NoLock )
		ON S_BPE.BusinessPlanEmployeeParentID = BPE_PARENT.OldID

-- Insert BusinessPlanEmployeeRole records
INSERT INTO dbo.BusinessPlanEmployeeRole
( OldID, BusinessPlanID, Name, Description, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC, 
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC,
	SortOrder )
SELECT S_BPER.BusinessPlanEmployeeRoleID, BP.BusinessPlanID, S_BPER.Name, S_BPER.Description, 
	S_BPER.CreateUser, 
	S_BPER.CreateDate, 
	DATEADD( hh, 7, S_BPER.CreateDate ),
	S_BPER.ModifyUser, 
	S_BPER.ModifyDate, 
	DATEADD( hh, 7, S_BPER.ModifyDate ),
	SortOrder
FROM [Innova_Staging].dbo.BusinessPlanEmployeeRole S_BPER WITH( NoLock )
	JOIN dbo.BusinessPlan BP WITH( NoLock )
		ON S_BPER.BusinessPlanID = BP.OldID

-- Insert BuinessPlanEmployeeEmployeeRole records
INSERT INTO dbo.BusinessPlanEmployeeEmployeeRole
( BusinessPlanEmployeeID, BusinessPlanEmployeeRoleID )
SELECT BPE.BusinessPlanEmployeeID, BPER.BusinessPlanEmployeeRoleID
FROM [Innova_Staging].dbo.BusinessPlanEmployeeEmployeeRole S_BPEER WITH( NoLock )
	JOIN dbo.BusinessPlanEmployee BPE WITH( NoLock )
		ON S_BPEER.BusinessPlanEmployeeID = BPE.OldID
	JOIN dbo.BusinessPlanEmployeeRole BPER WITH( NoLock )
		ON S_BPEER.BusinessPlanEmployeeRoleID = BPER.OldID

-- Insert BusinessPlanObjective records
INSERT INTO dbo.BusinessPlanObjective
( OldID, BusinessPlanID, Name, Value, DataType, PercentComplete, 
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
SELECT S_BPO.BusinessPlanObjectiveID, BP.BusinessPlanID, S_BPO.Name, S_BPO.Value, S_BPO.DataType, S_BPO.PercentComplete, 
	S_BPO.CreateUser, 
	S_BPO.CreateDate, 
	DATEADD( hh, 7, S_BPO.CreateDate ),
	S_BPO.ModifyUser, 
	S_BPO.ModifyDate, 
	DATEADD( hh, 7, S_BPO.ModifyDate ),
	S_BPO.EstimatedCompletionDate, 
	S_BPO.SortOrder, 
	S_BPO.AutoTrackingEnabled, 
	S_BPO.BaselineValue, 
	S_BPO.BaselineDate
FROM [Innova_Staging].dbo.BusinessPlanObjective S_BPO WITH( NoLock )
	JOIN dbo.BusinessPlan BP WITH( NoLock )
			ON S_BPO.BusinessPlanID = BP.OldID

-- Insert BusinessPlanStrategy records
INSERT INTO dbo.BusinessPlanStrategy
( OldID, BusinessPlanID, Name, Description, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC, 
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC, 
	SortOrder, 
	Editable )
SELECT S_BPS.BusinessPlanStrategyID, BP.BusinessPlanID, S_BPS.Name, S_BPS.Description, 
	S_BPS.CreateUser, 
	S_BPS.CreateDate, 
	DATEADD( hh, 7, S_BPS.CreateDate ),
	S_BPS.ModifyUser, 
	S_BPS.ModifyDate, 
	DATEADD( hh, 7, S_BPS.ModifyDate ),
	S_BPS.SortOrder, 
	S_BPS.Editable
FROM [Innova_Staging].dbo.BusinessPlanStrategy S_BPS WITH( NoLock )
	JOIN dbo.BusinessPlan BP WITH( NoLock )
			ON S_BPS.BusinessPlanID = BP.OldID

-- Insert BusinessPlanTactic records
INSERT INTO dbo.BusinessPlanTactic
( OldID, BusinessPlanID, Name, Description, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC,
	ModifyUserID, 
	ModifyDate, 
	ModifyDateUTC, 
	SortOrder, 
	CompletedDate, 
	Editable )
SELECT S_BPT.BusinessPlanTacticID, BP.BusinessPlanID, S_BPT.Name, S_BPT.Description, 
	S_BPT.CreateUser, 
	S_BPT.CreateDate, 
	DATEADD( hh, 7, S_BPT.CreateDate ),
	S_BPT.ModifyUser, 
	S_BPT.ModifyDate, 
	DATEADD( hh, 7, S_BPT.ModifyDate ),
	S_BPT.SortOrder, 
	S_BPT.CompletedDate, 
	S_BPT.Editable
FROM [Innova_Staging].dbo.BusinessPlanTactic S_BPT WITH( NoLock )
	JOIN dbo.BusinessPlan BP WITH( NoLock )
			ON S_BPT.BusinessPlanID = BP.OldID

-- Insert BusinessPlanObjectiveStrategy records
INSERT INTO dbo.BusinessPlanObjectiveStrategy
( BusinessPlanObjectiveID, BusinessPlanStrategyID )
SELECT BPO.BusinessPlanObjectiveID, BPS.BusinessPlanStrategyID
FROM [Innova_Staging].dbo.BusinessPlanObjectiveStrategy S_BPOS WITH( NoLock )
	JOIN dbo.BusinessPlanObjective BPO WITH( NoLock )
		ON S_BPOS.BusinessPlanObjectiveID = BPO.OldID
	JOIN dbo.BusinessPlanStrategy BPS WITH( NoLock )
		ON S_BPOS.BusinessPlanStrategyID = BPS.OldID

-- Insert BusinessPlanStrategyTactic records
INSERT INTO dbo.BusinessPlanStrategyTactic
( BusinessPlanStrategyID, BusinessPlanTacticID)
SELECT BPS.BusinessPlanStrategyID, BPT.BusinessPlanTacticID
FROM [Innova_Staging].dbo.BusinessPlanStrategyTactic S_BPST WITH( NoLock )
	JOIN dbo.BusinessPlanStrategy BPS WITH( NoLock )
		ON S_BPST.BusinessPlanStrategyID = BPS.OldID
	JOIN dbo.BusinessPlanTactic BPT WITH( NoLock )
		ON S_BPST.BusinessPlanTacticID = BPT.OldID

-- Insert BusinessPlanStrategySwot records
INSERT INTO dbo.BusinessPlanSwot
( OldID, BusinessPlanID, SwotType, SwotDescription, 
	CreateUserID, 
	CreateDate, 
	CreateDateUTC,
	ModifyUserID,
	ModifyDate, 
	ModifyDateUTC
	 )
SELECT S_BPSWOT.BusinessPlanSwotID, BP.BusinessPlanID, S_BPSWOT.SwotType, S_BPSWOT.SwotDescription, 
	S_BPSWOT.CreateUser, 
	S_BPSWOT.CreateDate, 
	DATEADD( hh, 7, S_BPSWOT.CreateDate ),
	S_BPSWOT.ModifyUser,
	S_BPSWOT.ModifyDate,
	DATEADD( hh, 7, S_BPSWOT.ModifyDate )	
FROM [Innova_Staging].dbo.BusinessPlanSwot S_BPSWOT WITH( NoLock )
	JOIN dbo.BusinessPlan BP WITH( NoLock )
			ON S_BPSWOT.BusinessPlanID = BP.OldID
GO
