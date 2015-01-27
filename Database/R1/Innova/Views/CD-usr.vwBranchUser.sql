USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwBranchUser' )
	DROP VIEW [usr].[vwBranchUser]
GO

/*
	SELECT * FROM usr.vwBranchUser
 */
 IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwBranchUser]
		AS

			SELECT 
				UserID = U_BP.PR_Profile_Master_ID,
				AffiliateID = ISNULL( A.AffiliateID, 0 ),
				AffiliateName = ISNULL( A.Name, ''Unknown'' ),
				BranchID = U_BP.BR_Branch_Master_ID,
				IsPrimary = U_BP.BP_Primary_Branch_Ind,
				TerminatedDate = U_BP.BP_Termination_Date,
				CreateDate = U_BP.BP_Create_Date,
				ModifyDate = U_BP.BP_Modify_Date
			FROM 
				UserProfileDM.dbo.Branch_Profile U_BP WITH( NoLock )
					JOIN UserProfileDM.dbo.Branch U_B WITH( NoLocK )
						ON U_BP.BR_Branch_Master_ID = U_B.BR_Branch_Master_ID
					LEFT JOIN dbo.Affiliate A WITH( NoLock )
						ON U_B.BD_ID = A.ExternalID'
END
GO
