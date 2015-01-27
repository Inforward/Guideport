USE [Innova]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyQuestion]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SurveyQuestion]
	(
		[SurveyQuestionID] [int] NOT NULL,	
		[SurveyPageID] [int] NOT NULL,	
		[QuestionName] [varchar](50) NULL,
		[QuestionText] [varchar](max) NOT NULL,
		[QuestionType] [varchar](100) NOT NULL,
		[QuestionWeight] [decimal](2, 2) NULL,
		[LayoutType] [varchar](100) NULL,
		[SortOrder] [int] NOT NULL,
		[MaxLength] [int] NULL,
		[IsRequired] [bit] NOT NULL,
		[IsVisible] [bit] NOT NULL,
		[IsDisabled] [bit] NOT NULL,
		CONSTRAINT [PK_SurveyQuestion] PRIMARY KEY CLUSTERED (	[SurveyQuestionID] ASC )
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GO

IF EXISTS( SELECT * FROM sys.foreign_keys WHERE name = 'FK_SurveyQuestions_SurveyPages' )
	ALTER TABLE [dbo].[SurveyQuestion] ADD  CONSTRAINT [FK_SurveyQuestions_SurveyPages] FOREIGN KEY([SurveyPageID]) REFERENCES [dbo].[SurveyPages] ([SurveyPageID]) 
GO