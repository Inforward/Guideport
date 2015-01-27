USE [Innova]
GO

IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'usr' AND TABLE_NAME = 'vwBranch' )
	DROP VIEW [usr].[vwBranch]
GO

/*
	SELECT * FROM usr.vwBranch
 */
 IF DB_ID( 'UserProfileDM' ) IS NOT NULL
 BEGIN
	EXEC sp_executesql
		N'CREATE VIEW [usr].[vwBranch]
			AS

			SELECT
				BranchID = U_B.BR_Branch_Master_ID,
				AffiliateID = ISNULL( A.AffiliateID, 0 ),
				AffiliateName = ISNULL( A.Name, ''Unknown'' ),
				BranchNo = U_B.BR_Number,
				Address1 = U_B.BR_Branch_Address_1,
				Address2 = U_B.BR_Branch_Address_2,
				City = U_B.BR_Branch_City,
				[State] = U_B.BR_Branch_State,
				Country = U_B.BR_Branch_Country,
				ZipCode = U_B.BR_Branch_Zip,
				MailingAddress1 = U_B.BR_Mail_Address_1,
				MailingAddress2 = U_B.BR_Mail_Address_2,
				MailingCity = U_B.BR_Mail_City,
				MailingState = U_B.BR_Mail_State,
				MailingCountry = U_B.BR_Mail_Country,
				MailingZipCode = U_B.BR_Mail_Zip,
				Phone = U_B.BR_Branch_Phone,
				Fax = U_B.BR_Branch_Fax,
				CreateDate = U_B.BR_Create_Date,
				ModifyDate = U_B.BR_Modify_Date,
				DeleteDate = U_B.BR_Modify_Date -- ???
			FROM 
				UserProfileDM.dbo.Branch U_B WITH( NoLock )
					LEFT JOIN dbo.Affiliate A WITH( NoLock )
						ON U_B.BD_ID = A.ExternalID'
END
GO
