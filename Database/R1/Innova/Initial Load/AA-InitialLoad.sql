USE [Innova]
GO

IF NOT EXISTS( SELECT * FROM usr.[ProfileType] )
	INSERT INTO usr.ProfileType( ProfileTypeID, Name, Description )
	SELECT 0, 'Other', 'Other'
	UNION SELECT 1, 'Employee', 'Employee'
	UNION SELECT 2, 'Financial Advisor', 'Financial Advisor'
	UNION SELECT 3, 'Branch Assistant', 'Branch Assistant'
GO

IF NOT EXISTS( SELECT * FROM app.[EventType] )
	INSERT INTO app.EventType( EventTypeID, Name )
	SELECT 1, 'Information'
	UNION SELECT 2, 'Warning'
	UNION SELECT 3, 'Error'
GO

IF NOT EXISTS( SELECT * FROM app.[AuditType] )
	INSERT INTO app.AuditType( AuditTypeID, Name, [Description] )
	SELECT 1, 'Insert', 'Record inserted.'
	UNION SELECT 2, 'Update', 'Record updated.'
	UNION SELECT 3, 'Delete', 'Record Deleted.'
GO