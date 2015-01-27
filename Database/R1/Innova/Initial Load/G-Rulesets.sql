USE [Innova]
GO

DELETE FROM [app].[RulesetHistory]
DELETE FROM [app].[Ruleset]
GO

INSERT INTO [app].[Ruleset]
        ([Name]
        ,[MajorVersion]
        ,[MinorVersion]
        ,[RuleSet]
        ,[Status]
        ,[AssemblyPath]
        ,[ActivityName]
        ,[ModifiedDate]
        ,[ModifiedBy])
     SELECT
		[Name]
        ,[MajorVersion]
        ,[MinorVersion]
        ,[RuleSet]
        ,[Status]
        ,[AssemblyPath]
        ,[ActivityName]
        ,[ModifiedDate]
        ,[ModifiedBy]
	FROM
		[Innova_Staging].dbo.[RS_Sets]
	WHERE
		[Name] IN ('Retirement.Assessment','SuccessionPlanning.Survey', 'SuccessionPlanning.Validation')
GO

