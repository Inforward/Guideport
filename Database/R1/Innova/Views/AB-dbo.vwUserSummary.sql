USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwUserSummary' )
	DROP VIEW [dbo].[vwUserSummary]
GO

DECLARE @sql NVARCHAR( MAX )

SET @sql = 
		N'CREATE VIEW [dbo].[vwUserSummary]
		AS
			SELECT
				UserID = AU.UserID,
				UserName = AU.DisplayName,
				UserPhoneNo = AU.PrimaryPhone,
				UserEmail = AU.Email,
				UserState = AU.[State],
				BusinessConsultantName = AU.BusinessConsultantDisplayName,
				AU.TerminateDate,
				AU.StartDate,
				AU.GDC_T12,
				AU.AUM,
				AU.ProfileID,
				AU.DisplayFirstName,
				AU.DisplayLastName,
				AU.AffiliateID,
				AffiliateName = A.Name
			FROM
				' + CASE WHEN DB_ID( 'UserProfileDM' ) IS NOT NULL AND @@SERVERNAME <> 'DCOLSQL00' THEN 'usr' ELSE 'dbo' END + '.vwUser AU WITH( NoLock )
					LEFT JOIN dbo.Affiliate A WITH( NoLock )
						ON AU.AffiliateID = A.AffiliateID'

EXEC sp_executesql @sql

GO
