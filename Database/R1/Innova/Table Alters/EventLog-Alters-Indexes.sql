USE [Innova]
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_EventLog_CreateDate' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_EventLog_CreateDate] ON [app].[EventLog] ( [CreateDate] DESC )
END
GO

IF NOT EXISTS( SELECT * FROM sys.indexes WHERE name = 'IX_EventLog_ServerName_EventDate' )
BEGIN
	CREATE NONCLUSTERED INDEX [IX_EventLog_ServerName_EventDate] ON [app].[EventLog] ( [ServerName] ASC, [EventDate] DESC )
END
GO

IF NOT EXISTS
(
	SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_EventLog_EventTypeID'
)
BEGIN
	ALTER TABLE app.EventLog ADD CONSTRAINT FK_EventLog_EventTypeID FOREIGN KEY( EventTypeID ) REFERENCES app.EventType( EventTypeID )
END
GO
