USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwUser' )
	DROP VIEW [usr].[vwUser]
GO

/*
	SELECT * FROM usr.vwUser
	WHERE ProfileID = '4031'
 */

IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwUser]
		AS

			SELECT 
				UserID = U_P.PR_Profile_Master_ID,
				ProfileTypeID = U_P.PR_Type_ID,
				ProfileTypeName = U_PT.PR_Type_Name,
				AffiliateID = ISNULL( A.AffiliateID, 0 ),
				AffiliateName = ISNULL( A.Name, ''Unknown'' ),
				ProfileID = U_P.PR_ID,
				UserStatusID = U_P.PR_Status_ID,
				UserStatusName = U_PS.PR_Status_Name,
				Email = U_P.PR_Business_Email,
				FirstName = U_P.PR_First_Name,
				MiddleName = U_P.PR_Middle_Name,
				LastName = U_P.PR_Last_Name,
				DisplayFirstName = U_P.PR_Display_First_Name,
				DisplayMiddleName = U_P.PR_Display_Middle_Name,
				DisplayLastName = U_P.PR_Display_Last_Name,
				DisplayName = ISNULL( U_P.PR_Display_First_Name, '''' ) + '' '' + ISNULL( U_P.PR_Display_Last_Name, '''' ),
				DBAName = U_P.PR_DBA_Name,
				Address1 = U_P.PR_Business_Address_1,
				Address2 = U_P.PR_Business_Address_2,
				City = U_P.PR_Business_City,
				ZipCode = U_P.PR_Business_Zip,
				[State] = U_P.PR_Business_State,
				Country = U_P.PR_Business_Country,
				PrimaryPhone = U_P.PR_Primary_Phone,
				PrimaryPhoneExtension = U_P.PR_Primary_Phone_Extension,
				HomePhone = U_P.PR_Home_Phone,
				WorkPhone = U_P.PR_Work_Phone,
				WorkPhoneExtension = U_P.PR_Work_Phone_Extension,
				Fax = U_P.PR_Business_Fax,
				SecurityProfileStartDate = U_P.PR_Start_Date,
				StartDate = U_P.PR_Start_Date,
				TerminateDate = U_P.PR_Termination_Date,
				CreateDate = U_P.PR_Create_Date,
				ModifyDate = ISNULL( U_P.PR_Modify_Date, U_P.PR_Create_Date ),
				DeleteDate = U_P.PR_Delete_Date,

				UBM.GDC_T12,
				UBM.GDC_PriorYear,
				UBM.AUM,
				UBM.AUM_Split,
				UBM.ReturnOnAUM,
				UBM.NoOfClients,
				UBM.NoOfAccounts,
				UBM.RevenueRecurring,
				UBM.RevenueNonRecurring,
				UBM.BusinessValuationLow,
				UBM.BusinessValuationHigh,
				UBM.AccountValueTotal,
				UBM.AccountValueAverage,
				MetricsUpdateDate = UBM.UpdateDate,
				BusinessConsultantUserID = U_P.PR_Business_Consultant_Profile_Master_ID,					
				BusinessConsultantDisplayName = ISNULL( U_BCU.PR_Display_First_Name, '''' ) + '' '' + ISNULL( U_BCU.PR_Display_Last_Name, '''' ),
				BusinessConsultantEmail = U_BCU.PR_Business_Email
			FROM 
				 UserProfileDM.dbo.[Profile] U_P WITH( NoLock )
					JOIN UserProfileDM.dbo.[Profile_Type] U_PT WITH( NoLock )
						ON U_P.PR_Type_ID = U_PT.PR_Type_ID
					JOIN UserProfileDM.dbo.[Profile_Status] U_PS WITH( NoLock )
						ON U_P.PR_Status_ID = U_PS.PR_Status_ID
					LEFT JOIN UserProfileDM.dbo.[Profile] U_BCU WITH( NoLock )
						ON U_P.PR_Business_Consultant_Profile_Master_ID = U_BCU.PR_Profile_Master_ID
					LEFT JOIN dbo.Affiliate A WITH( NoLock )
						ON U_P.BD_ID = A.ExternalID
					LEFT JOIN usr.BusinessMetric UBM WITH( NoLock )
						ON U_P.PR_Profile_Master_ID = UBM.UserID'
END
GO
