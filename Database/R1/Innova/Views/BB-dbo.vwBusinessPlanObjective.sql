USE Innova
GO

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwBusinessPlanObjective]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[vwBusinessPlanObjective] AS SELECT 1 AS [One]'


DECLARE @sql NVARCHAR( MAX )

SET @sql =
N'ALTER VIEW [dbo].[vwBusinessPlanObjective]
AS
	SELECT
		AEU.ProfileID AS [PR_ID], 
		BP.UserID, 
		BP.Year, 
		BPO.BusinessPlanObjectiveID, 
		BPO.Name, 
		BPO.Value, 
		BPO.DataType, 
		BPO.BaselineValue,
		BPO.BaselineDate,
		BPO.EstimatedCompletionDate, 
		PercentComplete = 
			CASE 
				WHEN BPV.PercentComplete >= 100 THEN 100 
				ELSE CAST( BPV.PercentComplete AS INT )
			END,
		BPO.SortOrder,
		BPO.CreateUserID, 
		BPO.CreateDate, 
		BPO.CreateDateUTC, 
		BPO.ModifyUserID, 
		BPO.ModifyDate,
		BPO.ModifyDateUTC
	FROM
		dbo.BusinessPlan BP WITH( NoLock )
		JOIN dbo.BusinessPlanObjective BPO WITH( NoLock ) 
			ON BP.BusinessPlanID = BPO.BusinessPlanID 
		JOIN ' + CASE WHEN DB_ID( 'UserProfileDM' ) IS NOT NULL AND @@SERVERNAME <> 'DCOLSQL00' THEN 'usr' ELSE 'dbo' END + '.vwUser AEU WITH( NoLock ) 
			ON BP.UserID = AEU.UserID
		JOIN dbo.vwBusinessPlanObjectiveValues BPV
			ON BPV.BusinessPlanObjectiveID = BPO.BusinessPlanObjectiveID
	WHERE
		BP.DeleteDate IS NULL' 

EXEC dbo.sp_executesql @sql

GO