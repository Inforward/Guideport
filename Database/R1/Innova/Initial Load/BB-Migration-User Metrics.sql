USE [Innova]
GO

-- Drop User Related FKs
IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_BusinessPlan_UserID' )
	ALTER TABLE [dbo].[BusinessPlan] DROP  CONSTRAINT [FK_BusinessPlan_UserID]


DELETE usr.[BusinessMetric]
DELETE usr.[ObjectCache]


-- UserBusinessMetric
INSERT INTO usr.BusinessMetric
( UserID, GDC_T12, GDC_PriorYear, AUM, AUM_Split, ReturnOnAUM, NoOfClients, NoOfAccounts, RevenueRecurring, RevenueNonRecurring, BusinessValuationLow, BusinessValuationHigh, AccountValueTotal, AccountValueAverage, UpdateDate )
SELECT P.UserID, GDC_T12, GDC_PriorYear, AUM, AUM_Split, ReturnOnAUM, NoOfClients, NoOfAccounts, RevenueRecurring, RevenueNonRecurring, BusinessValuationLow, BusinessValuationHigh, AccountValueTotal, AccountValueAverage, UpdateDate
FROM [Innova_Staging].dbo.AdvisorMetrics B WITH( NoLock )
	JOIN [Innova_Staging].dbo.[AE_User] P WITH( NoLock ) ON B.PR_ID = P.PR_ID

GO