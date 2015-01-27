USE [Innova]
GO

DELETE FROM [dbo].[SurveyResponseHistory]
DELETE FROM [dbo].[SurveyResponseAnswer]
DELETE FROM [dbo].[SurveyResponse]
DELETE FROM [dbo].[SurveyQuestionAnswerSuggestedContent]
DELETE FROM [dbo].[SurveyQuestionAnswer]
DELETE FROM [dbo].[SurveyQuestion]
DELETE FROM [dbo].[SurveyPageScoreRange]
DELETE FROM [dbo].[SurveyPage]
DELETE FROM [dbo].[Survey]
GO


SET IDENTITY_INSERT [dbo].[SurveyPageScoreRange] OFF
SET IDENTITY_INSERT [dbo].[SurveyQuestionAnswer] OFF
SET IDENTITY_INSERT [dbo].[SurveyQuestionAnswerSuggestedContent]  OFF
GO

-- Survey
INSERT INTO [dbo].[Survey] 
(
	[SurveyID],
	[SurveyName],
	[SurveyDescription],
	[RulesetCoreName],
	[RulesetValidationName],	
	[CompleteMessage],
	[CompleteRedirectUrl],
	[StatusCalculator],
	[NotificationType],
	[SuggestedContentSiteID],
	[StatusLabel],
	[IsAutoCompleteEnabled],
	[IsReviewVisible],
	[IsStatusVisible],
	[IsActive],
	[ReviewTabText]
)
SELECT
	[SurveyID],
	[SurveyName],
	[SurveyIntroduction],
	[ControlRuleset],
	[ValidationRuleset],		
	[CompleteMessage],
	[CompleteRedirectUrl],
	[StatusCalculator],
	[CompletionNotificationType],
	[SuggestedContentSiteID],
	CASE WHEN [SurveyName] = 'Qualified Buyer Program' THEN 'Qualified' ELSE 'Status' END,
	[AutoComplete],
	[UseSummary],
	CASE WHEN [SurveyName] IN ('Enrollment', 'Qualified Buyer Program') THEN 1 ELSE 0 END,
	CASE WHEN [SurveyName] IN ('Guided Retirement Solutions Assessment') THEN 0 ELSE 1 END,
	'Review'
FROM
	[Innova_Staging].dbo.Survey

GO

INSERT INTO [dbo].[SurveyPage] ([SurveyPageID],[SurveyID],[PageName],[SortOrder],[IsVisible],[Tooltip])
	SELECT [SurveyPageID],[SurveyID],[PageName],[PageOrder],[IsVisible],[Tooltip] FROM [Innova_Staging].dbo.SurveyPages
GO	 

SET IDENTITY_INSERT [dbo].[SurveyPageScoreRange] ON
GO
INSERT INTO [dbo].[SurveyPageScoreRange] ([SurveyPageScoreRangeID],[SurveyPageID],[MinScore],[MaxScore],[Description])
	SELECT [SurveyPageScoreRangeID],[SurveyPageID],[MinScore],[MaxScore],[Description] FROM [Innova_Staging].dbo.SurveyPageScoreRanges
GO
SET IDENTITY_INSERT [dbo].[SurveyPageScoreRange] OFF
GO

INSERT INTO [dbo].[SurveyQuestion] 
(
	[SurveyQuestionID],
	[SurveyPageID],
	[QuestionName],
	[QuestionText],
	[QuestionType],
	[LayoutType],
	[SortOrder],
	[MaxLength],
	[IsRequired],
    [IsVisible],
    [IsDisabled]
)
SELECT
	[SurveyQuestionID],
	[SurveyPageID],
	[QuestionName],
	[Question],
	[QuestionType],
	[LayoutType],
	[DisplayOrder],
	[MaxLength],
	[IsRequired],
    [IsVisible],
    ISNULL( [IsDisabled], 0 )
FROM
	[Innova_Staging].dbo.SurveyQuestions
GO


SET IDENTITY_INSERT [dbo].[SurveyQuestionAnswer] ON
GO

INSERT INTO [dbo].[SurveyQuestionAnswer] ([SurveyQuestionAnswerID],[SurveyQuestionID],[AnswerText],[ReviewAnswerText],[SortOrder],[AnswerWeight])
	SELECT [SurveyQuestionAnswerID],[SurveyQuestionID],[Answer],[ReviewAnswer],[AnswerOrder],[AnswerWeight] FROM [Innova_Staging].dbo.SurveyQuestionAnswers
GO

SET IDENTITY_INSERT [dbo].[SurveyQuestionAnswer] OFF
GO

SET IDENTITY_INSERT [dbo].[SurveyQuestionAnswerSuggestedContent] ON
GO

INSERT INTO [dbo].[SurveyQuestionAnswerSuggestedContent] ([SurveyQuestionAnswerSuggestedContentID],[SurveyQuestionAnswerID],[SiteContentID],[CreateUserID],[CreateDate],[CreateDateUTC])		
	SELECT 
		[SurveyAnswerSuggestedContentID],
		[SurveyQuestionAnswerID],
		[SiteContentID],
		ISNULL( SC.CreateUser, 0 ),
		SC.[CreateDate],
		DATEADD( hh, 7, SC.[CreateDate] )
	FROM 
		[Innova_Staging].[dbo].[SurveyAnswerSuggestedContent] SC
GO

SET IDENTITY_INSERT [dbo].[SurveyQuestionAnswerSuggestedContent] OFF
GO





-- Data Updates
UPDATE dbo.SurveyQuestion SET 
	LayoutType='section',
	QuestionText = 'My Branch and Personnel Information'
WHERE 
	QuestionName = 'BranchHeader'

UPDATE dbo.SurveyQuestion SET 
	LayoutType='section',
	QuestionText = 'My Client Information'
WHERE 
	QuestionName = 'MyClientHeader'

UPDATE dbo.SurveyQuestion SET 
	LayoutType='section',
	QuestionText = 'My Product/Investment Mix Information'
WHERE 
	QuestionName = 'ProductHeader'

UPDATE dbo.SurveyQuestion SET 
	LayoutType='section',
	QuestionText = 'Miscellaneous'
WHERE 
	QuestionName = 'MiscHeader'
GO

UPDATE dbo.Survey SET
	NotificationType = 'BusinessConsultantEmail'
WHERE
	SurveyName = 'Enrollment'

GO


USE Innova
GO

UPDATE SQA
	SET AnswerWeight = CASE WHEN AnswerText = 'Yes' THEN  0.10 ELSE 0 END
FROM
	dbo.Survey S
	JOIN dbo.SurveyPage SP
		ON S.SurveyID = SP.SurveyID
	JOIN dbo.SurveyQuestion SQ
		ON SP.SurveyPageID = SQ.SurveyPageID
	JOIN dbo.SurveyQuestionAnswer SQA
		ON SQ.SurveyQuestionID = SQA.SurveyQuestionID
WHERE
	S.SurveyName = 'Qualified Buyer Program'
GO

UPDATE dbo.Survey 
	SET [StatusCalculator] = 'WeightedAnswer' 
WHERE SurveyName = 'Qualified Buyer Program'
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'QuestionWeight' AND [object_id] = OBJECT_ID(N'SurveyQuestion'))
	ALTER TABLE dbo.SurveyQuestion DROP COLUMN QuestionWeight
GO

-- Fix typos
UPDATE dbo.SurveyQuestion
SET QuestionText = REPLACE( QuestionText, 'sucession', 'succession' )
WHERE QuestionText LIKE '%sucession%'


UPDATE dbo.SurveyQuestionAnswer 
SET AnswerText = 'Succession Planning and Business Acquisition webpage'
WHERE AnswerText = 'Sucession Planning and Business Acquisition webpage'

GO

-- Update Question verbiage to remove references to First Allied
UPDATE dbo.SurveyQuestion
SET QuestionText = 'We use a technology platform that sends out emails and/or social media updates to consistently communicate with clients and prospects'
WHERE QuestionText = 'We use First Allied''s Advisor Marketing System (AMS) or a similar technology platform to consistently communicate with clients and prospects'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'I have a written, executable Continuity Plan on file with my broker/dealer'
WHERE QuestionText = 'I have a written, executable Continuity Plan on file with First Allied'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'I have a written, executable Succession Plan on file with my broker/dealer'
WHERE QuestionText = 'I have a written, executable Succession Plan on file with First Allied'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'I have completed the Enrollment survey on the Succession Dashboard.'
WHERE QuestionText = 'I have enrolled on First Allied''s Succession Planning and Business Acquisition website'


UPDATE dbo.SurveyQuestion
SET QuestionName = 'EnrollmentComplete'
WHERE QuestionText = 'I have completed the Enrollment survey on the Succession Dashboard.'


-- Update Question to rename AUM to Total Assets
UPDATE dbo.SurveyQuestion
SET QuestionText = 'What is the expected minimum Total Assets of your potential continuity partner?'
WHERE QuestionText = 'What is the expected minimum Assets Under Management (AUM) of your potential continuity partner?'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'What is the maximum Total Assets of your potential continuity partner?'
WHERE QuestionText = 'What is the maximum Assets Under Management (AUM) of your potential continuity partner?'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'What is the expected minimum Total Assets of your potential succession partner?'
WHERE QuestionText = 'What is the expected minimum Assets Under Management (AUM) of your potential succession partner?'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'What is the maximum Total Assets of your potential succession partner?'
WHERE QuestionText = 'What is the maximum Assets Under Management (AUM) of your potential succession partner?'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'What is the maximum Total Assets you are interested in buying?'
WHERE QuestionText = 'What is the maximum Assets Under Management (AUM) you are interested in buying?'

UPDATE dbo.SurveyQuestion
SET QuestionText = 'Please indicate your estimated Total Assets.'
WHERE QuestionText = 'Please indicate your estimated Assets Under Management (AUM).'

GO

-- Fix Questions with funky characters
UPDATE dbo.SurveyQuestion 
SET QuestionText = 'How did you hear about First Allied''s continuity/ succession/ business acquisition programs? Select all that apply.' 
WHERE SurveyQuestionID = 378
GO