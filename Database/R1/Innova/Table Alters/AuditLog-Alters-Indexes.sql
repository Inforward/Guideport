USE [Innova]
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_AuditLog_AuditDate' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_AuditLog_AuditDate] ON [app].[AuditLog] ( [AuditDate] DESC )
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_AuditLog_TableName_TableIDValue_AuditDate' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_AuditLog_TableName_TableIDValue_AuditDate] ON [app].[AuditLog] ( TableName, TableIDValue, [AuditDate] DESC )
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_AuditLog_UserID_AuditDate' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_AuditLog_UserID_AuditDate] ON [app].[AuditLog] ( UserID,  [AuditDate] DESC )
END
GO

IF NOT EXISTS
(
	SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_AuditLog_AuditTypeID'
)
BEGIN
	ALTER TABLE app.AuditLog ADD CONSTRAINT FK_AuditLog_AuditTypeID FOREIGN KEY( AuditTypeID ) REFERENCES app.AuditType( AuditTypeID )
END
GO
