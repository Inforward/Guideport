USE Innova
GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleUser_ApplicationAccess' )
	ALTER TABLE [usr].[ApplicationRoleUser] DROP  CONSTRAINT [FK_ApplicationRoleUser_ApplicationAccess]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleUser_ApplicationRole' )
	ALTER TABLE [usr].[ApplicationRoleUser] DROP  CONSTRAINT [FK_ApplicationRoleUser_ApplicationRole]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleAccess_ApplicationAccess' )
	ALTER TABLE [usr].[ApplicationRoleAccess] DROP  CONSTRAINT [FK_ApplicationRoleAccess_ApplicationAccess]

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleAccess_ApplicationRole' )
	ALTER TABLE [usr].[ApplicationRoleAccess] DROP  CONSTRAINT [FK_ApplicationRoleAccess_ApplicationRole]

DELETE usr.LicenseUser
DELETE usr.License
DELETE usr.BranchUser
DELETE usr.Branch
DELETE usr.ApplicationRoleUser
DELETE usr.ApplicationRole
DELETE usr.ApplicationRoleAccess
DELETE usr.ApplicationAccess
DELETE usr.[User]

-- User
INSERT INTO usr.[User]
(
	UserID, UserProfileTypeID, AffiliateID, ProfileID, UserStatusID, UserStatusName, Email, 
	FirstName, MiddleName, LastName, DisplayFirstName, DisplayLastName, DBAName, 
	Address1, Address2, City, State, ZipCode, Country, PrimaryPhone, HomePhone, WorkPhone, Fax, 
	SecurityProfileStartDate, StartDate, TerminateDate, CreateDate, ModifyDate, DeleteDate
)
	SELECT 
		U.UserID,
		PT.ProfileTypeID,
		AffiliateID = 1,
		ProfileID = ISNULL( CAST( U.PR_ID AS VARCHAR( 20 ) ), 'UserID_' + CAST( U.UserID AS VARCHAR( 10 ) ) ),
		UserStatusID = U.StatusID,
		UserStatusName = US.StatusName,
		Email = CASE WHEN ISNULL( PR.PR_Email, '' ) <> '' THEN PR.PR_Email ELSE P.[Email] END,
		FirstName = ISNULL( PR.PR_FName, U.FirstName ),
		MiddleName = ISNULL( PR.PR_MName, U.MiddleName ),
		LastName = ISNULL( PR.PR_LName, U.LastName ),
		DisplayFirstName = COALESCE( PR.PR_AliasFName, PR.PR_FName, U.FirstName ),
		DisplayLastName = COALESCE( PR.PR_AliasLName, PR.PR_LName, U.LastName ),
		DBAName = DBA.OB_Title,
		Address1 = ISNULL( BR.BR_BAddr1, P.[Address1] ),
		Address2 = ISNULL( BR.BR_BAddr2, P.[Address2] ),
		City = ISNULL( BR.BR_BCity, P.[City] ),
		State = ISNULL( BR.BR_BState, P.[State] ),
		ZipCode = ISNULL( BR.BR_BZip, P.[ZipCode] ),
		Country = ISNULL( BR.BR_BCountry, p.[Country] ),
		PrimaryPhone = COALESCE( BR.BR_Phone, P.WorkPhone, P.HomePhone ),
		P.HomePhone,
		P.WorkPhone,
		P.Fax,
		SecurityProfileStartDate = PR.PR_StartDate,
		U.StartDate,
		U.TerminateDate,
		U.CreateDate,
		U.ModifyDate,
		U.DeleteDate
	FROM 
		[Innova_Staging].dbo.AE_User U WITH( NoLock )
			-- Eliminate Duplicate PR_IDs
			LEFT JOIN ( SELECT MIN( UserID ) MinUserID, PR_ID FROM [Innova_Staging].dbo.AE_User WHERE PR_ID IS NOT NULL GROUP BY PR_ID ) AS D
				ON U.PR_ID = D.PR_ID AND U.UserID = D.MinUserID
			LEFT JOIN [Innova_Staging].dbo.AE_Person P WITH( NoLock )
				ON U.PersonID = P.PersonID
			LEFT JOIN [Innova_Staging].dbo.SecLic_Profiles PR WITH( NoLock )
				ON U.PR_ID = PR.PR_ID
			LEFT JOIN [Innova_Staging].dbo.SecLic_ProfileBranch PB WITH( NoLock )
				ON PR.PR_ID = PB.PR_ID 
					AND PB.PB_Deleted IS NULL
					AND PB.PB_TerminatedDate IS NULL
					AND PB.PB_PrimaryBranch = 'Y'
			LEFT JOIN [Innova_Staging].dbo.SecLic_Branch BR WITH( NoLock ) 
				ON PB.BR_ID = BR.BR_ID
			LEFT JOIN [Innova_Staging].dbo.AE_UserStatus US WITH( NoLock )
				ON U.StatusID = US.StatusID	
			JOIN usr.ProfileType PT WITH( NoLock ) ON
					   CASE 
							WHEN PR.EC_ID IN (1, 23, 19) THEN 'Financial Advisor' 
							WHEN PR.EC_ID IN (2, 4, 27) THEN 'Branch Assistant' 
							WHEN PR.EC_ID IN (3, 5, 20, 24, 25) THEN 'Employee' 
							ELSE 'Other' 
						END = PT.Name
			OUTER APPLY
			(
				SELECT TOP 1 SOB.OB_Title
				FROM [Innova_Staging].dbo.SecLic_OutsideBusiness SOB WITH( NoLock )
				WHERE U.PR_ID = SOB.PR_ID
					AND SOB.OB_TypeID = 1
					AND SOB.OB_TerminatedDate IS NULL
				ORDER BY SOB.PR_ID DESC
			) AS DBA
	WHERE 1 = 1
		AND ( U.PR_ID IS NULL OR D.MinUserID IS NOT NULL )

-- Roles
INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (1, N'Portal.ContentAdmin', 'Content Admin', N'Enables a user to create and modify CMS related content within the Admin Console as well as within the Succession Planning tools.')

INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (2, N'Portal.AdvisorView', 'Advisor View', N'Enables a user to access Advisor View and impersonate a Financial Advisor.')

INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (3, N'Portal.SurveyAdmin', 'Survey Admin', N'Enables a user to modify surveys within the Admin Console.')

INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (4, N'Portal.Reporting', 'Reporting', N'Enables a user to access the Reporting section of the Admin Console.')

INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (5, N'Portal.UserAdmin', 'User Admin', N'Enables a user to access and modify user data within the Admin Console.')

INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (6, N'Portal.AffiliateAdmin', 'Affiliate Admin', N'Enables a user to access and modify affiliate data within the Admin Console.')

INSERT [usr].[ApplicationRole] ([ApplicationRoleID], [Name], [DisplayName], [Description]) 
VALUES (7, N'Portal.GroupAdmin', 'Group Admin', N'Enables a user to access and modify group data within the Admin Console.')

-- Role Access
INSERT INTO [usr].[ApplicationAccess] ([ApplicationAccessID],[Name]) VALUES( 1, 'Unrestricted' )
INSERT INTO [usr].[ApplicationAccess] ([ApplicationAccessID],[Name]) VALUES( 2, 'Restricted' )

-- Roles and Role Access Mapping
INSERT INTO [usr].[ApplicationRoleAccess] ( [ApplicationRoleID], [ApplicationAccessID], [Description] ) -- Reporting, Unrestricted
	VALUES( 4, 1, 'Allows a user to report on all users and groups.' ) 

INSERT INTO [usr].[ApplicationRoleAccess] ( [ApplicationRoleID], [ApplicationAccessID], [Description] )  -- Reporting, Restricted
	VALUES( 4, 2, 'Requires a user to report on only the groups they are associated to.' )

INSERT INTO [usr].[ApplicationRoleAccess] ( [ApplicationRoleID], [ApplicationAccessID], [Description] ) -- AdvisorView, Unrestricted
	VALUES( 2, 1, 'Allows a user to impersonate any advisor.' ) 

INSERT INTO [usr].[ApplicationRoleAccess] ( [ApplicationRoleID], [ApplicationAccessID], [Description] ) -- AdvisorView, Restricted
	VALUES( 2, 2, 'User can only impersonate advisors who are within the groups the user is associated to.' ) 

-- Assign Roles to Users
IF @@SERVERNAME IN ('DCOLSQL00','QCOLSQL00')
BEGIN
	INSERT INTO [usr].[ApplicationRoleUser] ( UserID, ApplicationRoleID, ApplicationAccessID, CreateDate )
	SELECT U.UserID, AR.[ApplicationRoleID], 1, GetDate()
	FROM usr.[User] U, [usr].[ApplicationRole] AR
	WHERE 
		(U.FirstName = 'Phillip' AND LastName = 'Panuco')
		OR (U.FirstName = 'Roanne' AND LastName = 'Martinez')
		OR (U.FirstName = 'Tzu' AND LastName = 'Lee')
		OR (U.FirstName = 'Carissa' AND LastName = 'Townsend')
		OR (U.FirstName = 'Logan' AND LastName = 'Moss')
		OR (U.FirstName = 'Richard' AND LastName = 'Whitworth')
		OR (U.FirstName = 'Chris' AND LastName = 'Haas')
		OR (U.FirstName = 'David' AND LastName = 'Edquilang')
		OR (U.FirstName = 'Yang' AND LastName = 'Ciccimaro')
END

IF @@SERVERNAME LIKE 'CTQ%' OR @@SERVERNAME LIKE 'CTP%'
BEGIN
	INSERT INTO [usr].[ApplicationRoleUser] ( UserID, ApplicationRoleID, ApplicationAccessID, CreateDate )
	SELECT U.UserID, AR.[ApplicationRoleID], 1, GetDate()
	FROM usr.[vwUser] U, [usr].[ApplicationRole] AR
	WHERE 
		(U.FirstName = 'Phillip' AND LastName = 'Panuco')
		OR (U.FirstName = 'Roanne' AND LastName = 'Martinez')
		OR (U.FirstName = 'Tzu' AND LastName = 'Lee')
		OR (U.FirstName = 'Carissa' AND LastName = 'Townsend')
		OR (U.FirstName = 'Logan' AND LastName = 'Moss')
		OR (U.FirstName = 'Richard' AND LastName = 'Whitworth')
		OR (U.FirstName = 'Chris' AND LastName = 'Haas')
		OR (U.FirstName = 'David' AND LastName = 'Edquilang')
		OR (U.FirstName = 'Yang' AND LastName = 'Ciccimaro')
END


INSERT INTO usr.[Branch]
( 
	BranchID, AffiliateID, BranchNo, Address1, Address2, City, State, ZipCode, Country, 
	MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, MailingCountry,
	Phone, Fax, CreateDate, DeleteDate 
)
SELECT
	BranchID = B.BR_ID,
	AffiliateID = 1,
	BranchNo = BR_Num, 
	Address1 = BR_BAddr1, 
	Address2 = BR_BAddr2, 
	City = BR_Bcity, 
	State = BR_Bstate, 
	ZipCode = BR_Bzip, 
	Country = BR_Bcountry, 
	MailingAddress1 = BR_MAddr1, 
	MailingAddress2 = BR_MAddr2, 
	MailingCity = BR_Mcity, 
	MailingState = BR_Mstate, 
	MailingZipCode = BR_Mzip, 
	MailingCountry = BR_Mcountry, 
	Phone = BR_Phone, 
	Fax = BR_Fax, 
	CreateDate = ISNULL( BR_CreatedTime, GETDATE() ),
	DeleteDate = BR_Deleted
FROM 
	[Innova_Staging].dbo.SecLic_Branch B WITH( NoLock )

INSERT INTO usr.[BranchUser]
( BranchID, UserID, IsPrimary, TerminatedDate, CreateDate, ModifyDate )
SELECT 
	BranchID = PB.BR_ID, 
	UserID = U.UserID,
	IsPrimary = CASE WHEN PB.PB_PrimaryBranch = 'Y' THEN 1 ELSE 0 END, 
	TerminatedDate = PB.PB_TerminatedDate, 
	CreateDate = ISNULL( PB.PB_CreatedTime, GETDATE() ), 
	ModifyDate = ISNULL( PB.PB_CreatedTime, GETDATE() )
FROM 
	[Innova_Staging].dbo.SecLic_ProfileBranch PB WITH( NoLock )
		JOIN usr.[User] U WITH( NoLock )
			ON PB.PR_ID = U.ProfileID
				AND ISNUMERIC( U.ProfileID ) = 1
		-- Eliminate Duplicates
		JOIN ( SELECT MIN( PB_ID ) MinPBID, BR_ID, PR_ID FROM [Innova_Staging].dbo.SecLic_ProfileBranch GROUP BY BR_ID, PR_ID ) AS D
			ON PB.BR_ID = D.BR_ID AND PB.PR_ID = D.PR_ID AND PB.PB_ID = D.MinPBID

INSERT INTO usr.[License]( LicenseID, LicenseTypeID, LicenseTypeName, LicenseExamTypeID, LicenseExamTypeName, RegistrationCategory, Description )
SELECT 
	LicenseID = L.LI_ID,
	LicenseTypeID = L.LT_ID, 
	LicenseTypeName = LT.LT_Description,
	LicenseExamTypeID = L.ET_ID, 
	LicenseExamTypeName = ET.ET_Description,
	RegistrationCategory = L.LI_RegCategory, 
	Description = L.LI_Description		
FROM 
	[Innova_Staging].dbo.SecLic_License L WITH( NoLock )
		JOIN [Innova_Staging].dbo.SecLic_LicenseType LT WITH( NoLock )
			ON L.LT_ID = LT.LT_ID
		JOIN [Innova_Staging].dbo.SecLic_ExamType ET WITH( NoLock )
			ON L.ET_ID = ET.ET_ID

INSERT INTO usr.[LicenseUser]
( LicenseID, UserID, EffectiveDate, TerminatedDate )
SELECT 	
	LicenseID = SL.LI_ID, 
	UserID = U.UserID,
	EffectiveDate = SL.SL_EffectiveDate, 
	TerminatedDate = SL.SL_TerminatedDate
FROM 
	[Innova_Staging].dbo.SecLic_SecLicense SL WITH( NoLock )
		JOIN usr.[User] U WITH( NoLock )
			ON SL.PR_ID = U.ProfileID
				AND ISNUMERIC( U.ProfileID ) = 1
		-- Eliminate Duplicates
		JOIN ( SELECT MIN( SL_ID ) MinSLID, LI_ID, PR_ID FROM [Innova_Staging].dbo.SecLic_SecLicense GROUP BY LI_ID, PR_ID ) AS D
			ON SL.LI_ID = D.LI_ID AND SL.PR_ID = D.PR_ID AND SL.SL_ID = D.MinSLID


GO

		
IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleUser_ApplicationAccess' )
	ALTER TABLE [usr].[ApplicationRoleUser] ADD  CONSTRAINT [FK_ApplicationRoleUser_ApplicationAccess] FOREIGN KEY([ApplicationAccessID]) REFERENCES [usr].[ApplicationAccess] ([ApplicationAccessID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleUser_ApplicationRole' )
	ALTER TABLE [usr].[ApplicationRoleUser] ADD CONSTRAINT [FK_ApplicationRoleUser_ApplicationRole] FOREIGN KEY([ApplicationRoleID]) REFERENCES [usr].[ApplicationRole] ([ApplicationRoleID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleAccess_ApplicationAccess' )
	ALTER TABLE [usr].[ApplicationRoleAccess] ADD  CONSTRAINT [FK_ApplicationRoleAccess_ApplicationAccess] FOREIGN KEY([ApplicationAccessID]) REFERENCES [usr].[ApplicationAccess] ([ApplicationAccessID])

IF NOT EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_ApplicationRoleAccess_ApplicationRole' )
	ALTER TABLE [usr].[ApplicationRoleAccess] ADD  CONSTRAINT [FK_ApplicationRoleAccess_ApplicationRole] FOREIGN KEY([ApplicationRoleID]) REFERENCES [usr].[ApplicationRole] ([ApplicationRoleID])

GO