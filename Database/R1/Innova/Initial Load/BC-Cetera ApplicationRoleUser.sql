USE Innova
GO

-- Cetera Users
INSERT INTO [usr].[ApplicationRoleUser] ( UserID, ApplicationRoleID, ApplicationAccessID, CreateDate )
SELECT U.UserID, AR.[ApplicationRoleID], 1, GetDate()
FROM usr.vwUser U, [usr].[ApplicationRole] AR
WHERE (U.FirstName = 'CRISTA' AND LastName = 'BOEHM')

GO
