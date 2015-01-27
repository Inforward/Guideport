USE Innova
GO

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwBusinessPlanObjectiveValues]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[vwBusinessPlanObjectiveValues] AS
	SELECT
		BusinessPlanObjectiveID,
		Name,
		PercentComplete = 
		ROUND
		(
			(
				CASE 
					WHEN [Name] = ''Assets Under Management'' THEN
						( AUMCurrent - AUMBaseline ) / CASE WHEN ( AUMGoal - AUMBaseline ) <> 0 THEN ( AUMGoal - AUMBaseline ) ELSE 1 END
					WHEN [Name] = ''Gross Dealer Concession'' THEN
						( GDCCurrent - GDCBaseline ) / CASE WHEN ( GDCGoal - GDCBaseline ) <> 0 THEN ( GDCGoal - GDCBaseline ) ELSE 1 END
					WHEN [Name] = ''Return on Assets Under Management'' THEN
						( ReturnOnAUMCurrent - ReturnOnAUMBaseline ) / CASE WHEN ( ReturnOnAUMGoal - ReturnOnAUMBaseline ) <> 0 THEN ( ReturnOnAUMGoal - ReturnOnAUMBaseline ) ELSE 1 END
					WHEN [Name] = ''Business Value'' THEN
						( BusinessValueCurrent - BusinessValueBaseline ) / CASE WHEN ( BusinessValueGoal - BusinessValueBaseline ) <> 0 THEN ( BusinessValueGoal - BusinessValueBaseline ) ELSE 1 END
					WHEN [Name] = ''Average Client Account Size'' THEN
						( AverageAccountSizeCurrent - AverageAccountSizeBaseline ) / CASE WHEN ( AverageAccountSizeGoal - AverageAccountSizeBaseline ) <> 0 THEN ( AverageAccountSizeGoal - AverageAccountSizeBaseline ) ELSE 1 END
					WHEN [Name] = ''Number of Clients'' THEN
						( NoOfClientsCurrent - NoOfClientsBaseline ) / CASE WHEN ( NoOfClientsGoal - NoOfClientsBaseline ) <> 0 THEN ( NoOfClientsGoal - NoOfClientsBaseline ) ELSE 1 END
				ELSE
					CAST( PercentComplete AS DECIMAL ) / 100
				END
			) * 100, 0),
		AUMGoal,
		AUMBaseline,
		AUMCurrent,
		GDCGoal,
		GDCBaseline,
		GDCCurrent,
		ReturnOnAUMGoal,
		ReturnOnAUMBaseline,
		ReturnOnAUMCurrent,
		BusinessValueGoal,
		BusinessValueBaseline,
		BusinessValueCurrent,
		AverageAccountSizeGoal,
		AverageAccountSizeBaseline,
		AverageAccountSizeCurrent,
		NoOfClientsGoal,
		NoOfClientsBaseline,
		NoOfClientsCurrent
	FROM
		(
		SELECT
			BPO.BusinessPlanObjectiveID, 
			BPO.Name,
			BPO.PercentComplete,

			AUMGoal = CASE WHEN [Name] = ''Assets Under Management'' THEN CAST( ISNULL( BPO.Value, ''0'' ) AS DECIMAL ) ELSE 0 END, 
			AUMBaseline = CASE WHEN [Name] = ''Assets Under Management'' THEN CAST( ISNULL( BPO.BaselineValue, ''0'' ) AS DECIMAL ) ELSE 0 END,
			AUMCurrent = CAST( ISNULL( AM.AUM, 0 ) AS DECIMAL ),

			GDCGoal = CASE WHEN [Name] = ''Gross Dealer Concession'' THEN CAST( ISNULL( BPO.Value, ''0'' ) AS DECIMAL ) ELSE 0 END, 
			GDCBaseline = CASE WHEN [Name] = ''Gross Dealer Concession'' THEN CAST( ISNULL( BPO.BaselineValue, ''0'' ) AS DECIMAL ) ELSE 0 END,
			GDCCurrent = CAST( ISNULL( AM.GDC_T12, 0 ) AS DECIMAL ),

			ReturnOnAUMGoal = CASE WHEN [Name] = ''Return on Assets Under Management'' THEN CAST( ISNULL( BPO.Value, ''0'' ) AS DECIMAL(18,2) ) ELSE 0 END, 
			ReturnOnAUMBaseline = CASE WHEN [Name] = ''Return on Assets Under Management'' THEN CAST( ISNULL( BPO.BaselineValue, ''0'' ) AS DECIMAL(18,2) ) ELSE 0 END,
			ReturnOnAUMCurrent = CAST( ISNULL( AM.ReturnOnAUM, 0 ) AS DECIMAL(18,2) ),

			BusinessValueGoal = CASE WHEN [Name] = ''Business Value'' THEN CAST( ISNULL( BPO.Value, ''0'' ) AS DECIMAL(18,2) ) ELSE 0 END, 
			BusinessValueBaseline = CASE WHEN [Name] = ''Business Value'' THEN CAST( ISNULL( BPO.BaselineValue, ''0'' ) AS DECIMAL(18,2) ) ELSE 0 END,
			BusinessValueCurrent = CAST( ISNULL( AM.BusinessValuationLow, 0 ) AS DECIMAL(18,2) ),

			AverageAccountSizeGoal = CASE WHEN [Name] = ''Average Client Account Size'' THEN CAST( ISNULL( BPO.Value, ''0'' ) AS DECIMAL ) ELSE 0 END, 
			AverageAccountSizeBaseline = CASE WHEN [Name] = ''Average Client Account Size'' THEN CAST( ISNULL( BPO.BaselineValue, ''0'' ) AS DECIMAL ) ELSE 0 END,
			AverageAccountSizeCurrent = CAST( ISNULL( AM.AccountValueAverage, 0 ) AS DECIMAL ),

			NoOfClientsGoal = CASE WHEN [Name] = ''Number of Clients'' THEN CAST( ISNULL( BPO.Value, ''0'' ) AS DECIMAL ) ELSE 0 END, 
			NoOfClientsBaseline = CASE WHEN [Name] = ''Number of Clients'' THEN CAST( ISNULL( BPO.BaselineValue, ''0'' ) AS DECIMAL ) ELSE 0 END,
			NoOfClientsCurrent = CAST( ISNULL( AM.NoOfClients, 0 ) AS DECIMAL )

		FROM
			dbo.BusinessPlan BP WITH( NoLock )
			JOIN dbo.BusinessPlanObjective BPO WITH( NoLock ) 
				ON BP.BusinessPlanID = BPO.BusinessPlanID
			LEFT JOIN usr.BusinessMetric AM WITH( NoLock )
				ON BP.UserID = AM.UserID
		WHERE
			BP.DeleteDate IS NULL

		) AS GOALS

' 
GO