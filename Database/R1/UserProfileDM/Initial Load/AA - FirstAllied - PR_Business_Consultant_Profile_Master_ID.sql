USE UserProfileDM
GO

UPDATE
	P
SET
	PR_Business_Consultant_Profile_Master_ID = TEAMS.PR_Business_Consultant_Profile_Master_ID
FROM
	dbo.[Profile] P WITH( NoLock )
		JOIN
		(
			SELECT 
				DISTINCT
				T.Team_Name AS [Name], 
				PR_Business_Consultant_Profile_Master_ID = BCU.PR_Profile_Master_ID,
				MU.PR_Profile_Master_ID
			FROM
				[Innova_Staging].dbo.Users URS WITH( NoLock )
				JOIN [Innova_Staging].dbo.TeamMember TM WITH( NoLock )
					ON URS.UR_ID = TM.UR_ID
						AND TM.TM_DeletedDate IS NULL
				JOIN [Innova_Staging].dbo.Team T WITH( NoLock )
					ON TM.Team_ID = T.Team_ID
						AND T.Team_Name LIKE 'BCG%'
						AND T.Team_DeletedDate IS NULL
				JOIN [Innova_Staging].dbo.TeamProfile TP WITH( NoLock )
					ON T.Team_ID = TP.Team_ID
						AND TP.T_DeletedDate IS NULL
				JOIN dbo.[Profile] MU WITH( NoLock )
					ON CAST(TP.PR_ID AS VARCHAR) = MU.PR_ID
						AND MU.BD_ID = 2 -- First Allied
				JOIN dbo.[Profile] BCU WITH( NoLock )
					ON CAST(URS.PR_ID AS VARCHAR) = BCU.PR_ID
						AND BCU.BD_ID = 2 -- First Allied
		) TEAMS
			ON P.PR_Profile_Master_ID = TEAMS.PR_Profile_Master_ID
	
GO
