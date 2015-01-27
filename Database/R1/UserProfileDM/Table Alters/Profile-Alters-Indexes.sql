USE [UserProfileDM]
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Profile' 
								AND COLUMN_NAME = 'PR_Business_Consultant_Profile_Master_ID' )
BEGIN
	ALTER TABLE dbo.[Profile] ADD PR_Business_Consultant_Profile_Master_ID INT
END
GO

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE 
						WHERE TABLE_SCHEMA = 'dbo' AND TABLE_Name = 'Profile' 
								AND COLUMN_NAME = 'PR_Business_Consultant_Profile_Master_ID' )
BEGIN
	ALTER TABLE dbo.[Profile] ADD CONSTRAINT Business_Consultant_FK FOREIGN KEY( PR_Business_Consultant_Profile_Master_ID ) 
		REFERENCES dbo.[Profile]( PR_Profile_Master_ID )
END
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Profile' )
	AND EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Profile' 
							AND COLUMN_NAME = 'PR_Business_Consultant_Profile_Master_ID' )
	AND NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_Profile_PR_Business_Consultant_Profile_Master_ID' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Profile_PR_Business_Consultant_Profile_Master_ID] ON [dbo].[Profile](PR_Business_Consultant_Profile_Master_ID)
END
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Profile' )
	AND NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_Profile_BD_ID_PR_ID' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Profile_BD_ID_PR_ID] ON [dbo].[Profile] ([BD_ID], [PR_ID])
END
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Profile' )
	AND NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_Profile_PR_Type_ID_BD_ID_PR_Termination_Date' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Profile_PR_Type_ID_BD_ID_PR_Termination_Date]
		ON [dbo].[Profile] ([PR_Type_ID], [BD_ID], [PR_Termination_Date] )
END
GO

