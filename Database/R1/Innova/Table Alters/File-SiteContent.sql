USE [Innova]
GO

IF NOT EXISTS
(
	SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_FileInfo_FileID'
)
BEGIN
	ALTER TABLE dbo.FileInfo ADD CONSTRAINT FK_FileInfo_FileID FOREIGN KEY( FileID ) REFERENCES dbo.[File]( FileID )
END
GO

IF NOT EXISTS
(
	SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'SiteContent' AND TABLE_SCHEMA = 'cms' AND COLUMN_NAME = 'FileID'
)
BEGIN
	ALTER TABLE cms.SiteContent ADD FileID INT NULL
END
GO