USE Innova
GO

UPDATE
	dbo.BusinessPlanObjective
SET
	Name = 'Total Assets'
WHERE
	Name = 'Assets Under Management'
	AND AutoTrackingEnabled = 1

UPDATE
	dbo.BusinessPlanObjective
SET
	Name = 'Return on Total Assets'
WHERE
	Name = 'Return on Assets Under Management'
	AND AutoTrackingEnabled = 1

GO
