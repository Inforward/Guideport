USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwUser' )
	DROP VIEW [dbo].[vwUser]
GO

/*
	SELECT * FROM dbo.vwUser
	WHERE ProfileiD = '3424'

	FASI_FirstAllied..sp_columns SecLic_AEView
 */
CREATE VIEW [dbo].[vwUser]
AS

	SELECT 
		U.UserID,
		PT.ProfileTypeID,
		ProfileTypeName = PT.Name,
		AffiliateID = U.AffiliateID,
		AffiliateName = A.Name,
		ProfileID = U.ProfileID,
		U.UserStatusID,
		U.UserStatusName,
		U.Email,
		U.FirstName,
		U.MiddleName,
		U.LastName,
		U.DisplayFirstName,
		U.DisplayLastName,
		DisplayName = ISNULL( U.DisplayFirstName, '' ) + ' ' + ISNULL( U.DisplayLastName, '' ),
		U.DBAName,
		U.Address1,
		U.Address2,
		U.City,
		U.ZipCode,
		U.[State],
		U.Country,
		U.PrimaryPhone,
		U.HomePhone,
		U.WorkPhone,
		U.Fax,
		U.SecurityProfileStartDate,
		U.StartDate,
		U.TerminateDate,
		U.CreateDate,
		U.ModifyDate,
		U.DeleteDate,

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

		U.BusinessConsultantUserID,
		BusinessConsultantDisplayName = ISNULL( U_BC.DisplayFirstName, '' ) + ' ' + ISNULL( U_BC.DisplayLastName, '' ),
		BusinessConsultantEmail = U_BC.Email
	FROM 
		usr.[User] U WITH( NoLock )
			JOIN dbo.Affiliate A WITH( NoLock )
				ON U.AffiliateID = A.AffiliateID		
			JOIN usr.ProfileType PT WITH( NoLock )
				ON U.UserProfileTypeID = PT.ProfileTypeID
			LEFT JOIN usr.[User] U_BC WITH( NoLock )
				ON U.BusinessConsultantUserID = U_BC.UserID
			LEFT JOIN usr.BusinessMetric UBM WITH( NoLock )
				ON U.UserID = UBM.UserID

GO
