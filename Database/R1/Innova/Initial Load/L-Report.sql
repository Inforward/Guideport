USE [Innova]
GO

DECLARE @baseUrl VARCHAR(100) = 'https://localhost:44303'

IF @@SERVERNAME = 'QCOLSQL00'
	SET @baseUrl = 'https://qa-guideport.firstallied.com'

IF @@SERVERNAME LIKE '%CTQ%'
	SET @baseUrl = 'https://qa.guideportcfg.com'

IF @@SERVERNAME LIKE '%CTP%'
	SET @baseUrl = 'https://www.guideportcfg.com'


DELETE rpt.ViewColumn
DELETE rpt.ViewFilter
DELETE rpt.[View]
DELETE rpt.[Column]
DELETE rpt.Filter
DELETE rpt.Category

-- View Categories
INSERT INTO rpt.Category( CategoryID, ParentCategoryID, Name )
SELECT 1, NULL, 'Business Assessment'
UNION SELECT 2, NULL, 'Business Planning'
UNION SELECT 3, NULL, 'Succession Planning'

-- Filters
INSERT INTO rpt.Filter( FilterID, Name, Label, IsRequired, DataTypeName, ParameterName, InputType )
SELECT 1, 'FirstName', 'First Name', 0, 'System.String', '@firstName', 'TextBox'
UNION SELECT 2, 'LastName', 'Last Name', 0, 'System.String', '@lastName', 'TextBox'
UNION SELECT 3, 'Group', 'Select Group...', 0, 'System.String', '@groupIDList', 'DropDownList'
UNION SELECT 4, 'BrokerDealer', 'Select Affiliate...', 0, 'System.String', '@affiliateIDList', 'DropDownList'
UNION SELECT 5, 'IncludeTerminated', 'Include Terminated Users', 0, 'System.Boolean', '@includeTerminated', 'CheckBox'
UNION SELECT 6, 'ExcludeAdvisorsWithNoData', 'Exclude Users With No Data', 0, 'System.Boolean', '@excludeAdvisorsWithNoData', 'CheckBox'
UNION SELECT 7, 'StartDate', 'Start Date', 0, 'System.DateTime', '@startDate', 'DatePicker'
UNION SELECT 8, 'EndDate', 'End Date', 0, 'System.DateTime', '@endDate', 'DatePicker'
UNION SELECT 9, 'Year', 'Select Year...', 0, 'System.Int32', '@year', 'DropDownList'

-- Columns
INSERT INTO rpt.[Column]( ColumnID, Title, DataField, DataFormat, Width, DataTypeName )
SELECT 1, 'Advisor', 'AdvisorName', NULL, 200, 'System.String'
UNION SELECT 2, 'Consultant', 'BusinessConsultantName', NULL, 150, 'System.String'
UNION SELECT 3, 'Overall', 'OverallScore', '{0:P0}', 85, 'System.Decimal'
UNION SELECT 4, 'Business Development', 'BusinessDevelopmentScore', '{0:P0}', 200, 'System.Decimal'
UNION SELECT 5, 'Operational Efficiency', 'OperationalEfficiencyScore', '{0:P0}', 190, 'System.Decimal'
UNION SELECT 6, 'Human Capital', 'HumanCapitalScore', '{0:P0}', 160, 'System.Decimal'
UNION SELECT 7, 'Business Management', 'BusinessManagementScore', '{0:P0}', 190, 'System.Decimal'
UNION SELECT 8, 'Succession Planning', 'SuccessionPlanningScore', '{0:P0}', 180, 'System.Decimal'
UNION SELECT 9, '% Complete', 'PercentComplete', '{0:P0}', 125, 'System.Decimal'
UNION SELECT 10, 'Terminate Date', 'TerminateDate', '{0:MM/dd/yyyy}', 140, 'System.DateTime'
UNION SELECT 11, 'Start Date', 'StartDate', '{0:MM/dd/yyyy}', 120, 'System.DateTime'
UNION SELECT 12, 'T12 GDC', 'GDC_T12', '{0:N2}', 150, 'System.Decimal'
UNION SELECT 13, 'Profile ID', 'ProfileID', NULL, 120, 'System.String'
UNION SELECT 14, 'Business Development (&plusmn;)', 'BusinessDevelopmentChange', '{0:P0}', 200, 'System.Decimal'
UNION SELECT 15, 'Operational Efficiency (&plusmn;)', 'OperationalEfficiencyChange', '{0:P0}', 200, 'System.Decimal'
UNION SELECT 16, 'Human Capital (&plusmn;)', 'HumanCapitalChange', '{0:P0}', 150, 'System.Decimal'
UNION SELECT 17, 'Business Management (&plusmn;)', 'BusinessManagementChange', '{0:P0}', 200, 'System.Decimal'
UNION SELECT 18, 'Succession Planning (&plusmn;)', 'SuccessionPlanningChange', '{0:P0}', 200, 'System.Decimal'
UNION SELECT 19, 'First Assessment Date', 'ResponseDateFirst', '{0:MM/dd/yyyy}', 185, 'System.DateTime'
UNION SELECT 20, 'Last Assessment Date', 'ResponseDate', '{0:MM/dd/yyyy}', 185, 'System.DateTime'
UNION SELECT 21, 'Year', 'BusinessPlanYear', NULL, 150, 'System.Int32'
UNION SELECT 22, 'Objective', 'ObjectiveName', NULL, 250, 'System.String'
UNION SELECT 23, 'Goal', 'ObjectiveValue', NULL, 400, 'System.String'
UNION SELECT 24, 'Completed', 'CompleteDate', '{0:MM/dd/yyyy}', 150, 'System.DateTime'
UNION SELECT 25, 'Email', 'AdvisorEmail', NULL, 200, 'System.String'
UNION SELECT 26, 'PhoneNo', 'AdvisorPhoneNo', NULL, 150, 'System.String'
UNION SELECT 27, 'State', 'AdvisorState', NULL, 80, 'System.String'
UNION SELECT 28, 'Activity', 'Activity', NULL, 100, 'System.String'
UNION SELECT 29, 'Created', 'CreateDate', '{0:MM/dd/yyyy}', 100, 'System.DateTime'
UNION SELECT 30, 'Last Modified', 'ModifyDate', '{0:MM/dd/yyyy}', 150, 'System.DateTime'
UNION SELECT 31, 'Continuity', 'Continuity', NULL, 250, 'System.String'
UNION SELECT 32, 'Continuity On File', 'ContinuityOnFile', NULL, 200, 'System.String'
UNION SELECT 33, 'Succession', 'Succession', NULL, 250, 'System.String'
UNION SELECT 34, 'Acquisition', 'Acquisition', NULL, 250, 'System.String'
UNION SELECT 35, 'Qualified Buyer', 'QualifiedBuyer', NULL, 200, 'System.String'
UNION SELECT 36, 'Succession Sell Time Frame', 'SuccessionSellTimeframe', NULL, 250, 'System.String'
UNION SELECT 37, 'Succession Buy Time Frame', 'SuccessionBuyTimeframe', NULL, 250, 'System.String'
UNION SELECT 38, 'Acquisition Buy Time Frame', 'AcquisitionBuyTimeframe', NULL, 250, 'System.String'
UNION SELECT 39, 'Affiliate', 'BrokerDealer', NULL, 200, 'System.String'
UNION SELECT 40, 'AUM', 'AUM', '{0:N2}', 150, 'System.Decimal'
UNION SELECT 41, 'Question', 'QuestionText', NULL, 400, 'System.String'
UNION SELECT 42, '# Answered', 'TotalAnswers', NULL, 50, 'System.Int32'
UNION SELECT 43, 'Avg. Score', 'AverageScore', '{0:N2}', 50, 'System.Decimal'
UNION SELECT 44, 'Ordinal', 'QuestionOrdinal', '{0:N2}', 50, 'System.Decimal'
UNION SELECT 45, 'Page', 'PageName', NULL, 100, 'System.String'

-- Views
INSERT INTO rpt.[View]( ViewID, CategoryID, Name, FullName, StoredProcedureName, IsEnabled, IsPageable, IsSortable, PageSize )
SELECT 1, 1, 'BusinessAssessmentScores', 'Business Assessment Scores', 'rpt.GetBusinessAssessmentScores', 1, 1, 1, 250
UNION SELECT 2, 1, 'BusinessAssessmentProgression', 'Business Assessment Progression', 'rpt.GetBusinessAssessmentProgression', 1, 1, 1, 250
UNION SELECT 3, 2, 'BusinessPlanProgression', 'Business Plan Progression', 'rpt.GetBusinessPlanProgression', 1, 1, 1, 250
UNION SELECT 4, 3, 'EnrollmentActivity', 'Enrollment Activity', 'rpt.GetEnrollmentActivity', 1, 1, 1, 250
UNION SELECT 5, 3, 'BusinessAssessmentSummary', 'Business Assessment Summary', 'rpt.GetBusinessAssessmentSummary', 1, 1, 1, 250

-- ViewFilters
INSERT INTO rpt.[ViewFilter]( ViewID, FilterID )
-- BusinessAssessmentScores
SELECT 1, F.FilterID FROM rpt.Filter F 
WHERE F.Name IN ( 'FirstName', 'LastName', 'Group', 'BrokerDealer', 'IncludeTerminated', 'ExcludeAdvisorsWithNoData' )
-- BusinessAssessmentProgression
UNION SELECT 2, F.FilterID FROM rpt.Filter F 
WHERE F.Name IN ( 'FirstName', 'LastName', 'Group', 'BrokerDealer', 'IncludeTerminated', 'ExcludeAdvisorsWithNoData', 'StartDate', 'EndDate' )
-- BusinessPlanProgression
UNION SELECT 3, F.FilterID FROM rpt.Filter F 
WHERE F.Name IN ( 'FirstName', 'LastName', 'Group', 'BrokerDealer', 'IncludeTerminated', 'ExcludeAdvisorsWithNoData', 'Year' )
-- EnrollmentActivity
UNION SELECT 4, F.FilterID FROM rpt.Filter F 
WHERE F.Name IN ( 'FirstName', 'LastName', 'Group', 'BrokerDealer', 'IncludeTerminated', 'ExcludeAdvisorsWithNoData', 'StartDate', 'EndDate' )
-- BusinessAssessmentSummary
UNION SELECT 5, F.FilterID FROM rpt.Filter F 
WHERE F.Name IN ( 'Group', 'BrokerDealer', 'IncludeTerminated', 'ExcludeAdvisorsWithNoData', 'StartDate', 'EndDate' )

-- ViewColumns
INSERT INTO rpt.[ViewColumn]( ViewID, ColumnID, Ordinal, DataFormat, Width, IsSortable, IsLocked, IsEnabled, Template )
-- BusinessAssessmentScores
SELECT 1, C.ColumnID, 1, NULL, NULL, 1, 1, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AdvisorName'
UNION SELECT 1, C.ColumnID, 2, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessConsultantName'
UNION SELECT 1, C.ColumnID, 3, NULL, NULL, 1, 0, 1, '#if(OverallScore){#<a href=''' + @baseUrl + '/Survey/Print/#=SurveyName#/#=AdvisorUserID#'' title=''Click to view the survey'' target=''_blank''>#= OverallScore #</a> #}#' FROM rpt.[Column] C WHERE DataField = 'OverallScore'
UNION SELECT 1, C.ColumnID, 4, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessDevelopmentScore'
UNION SELECT 1, C.ColumnID, 5, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'OperationalEfficiencyScore'
UNION SELECT 1, C.ColumnID, 6, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'HumanCapitalScore'
UNION SELECT 1, C.ColumnID, 7, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessManagementScore'
UNION SELECT 1, C.ColumnID, 8, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'SuccessionPlanningScore'
UNION SELECT 1, C.ColumnID, 9, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'PercentComplete'
UNION SELECT 1, C.ColumnID, 10, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BrokerDealer'
UNION SELECT 1, C.ColumnID, 11, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'TerminateDate'
UNION SELECT 1, C.ColumnID, 12, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'StartDate'
UNION SELECT 1, C.ColumnID, 13, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'GDC_T12'
UNION SELECT 1, C.ColumnID, 14, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ProfileID'
-- BusinessAssessmentProgression
UNION SELECT 2, C.ColumnID, 1, NULL, NULL, 1, 1, 1, '#if(SurveyName){#<a href=''' + @baseUrl + '/Survey/Print/#=SurveyName#/#=AdvisorUserID#'' title=''Click to view the survey'' target=''_blank''>#=AdvisorName#</a>#}else{# #=AdvisorName# #}#' FROM rpt.[Column] C WHERE DataField = 'AdvisorName'
UNION SELECT 2, C.ColumnID, 2, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessConsultantName'
UNION SELECT 2, C.ColumnID, 3, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessDevelopmentScore'
UNION SELECT 2, C.ColumnID, 4, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessDevelopmentChange'
UNION SELECT 2, C.ColumnID, 5, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'OperationalEfficiencyScore'
UNION SELECT 2, C.ColumnID, 6, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'OperationalEfficiencyChange'
UNION SELECT 2, C.ColumnID, 7, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'HumanCapitalScore'
UNION SELECT 2, C.ColumnID, 8, NULL, NULL, 1, 0, 1,  NULL FROM rpt.[Column] C WHERE DataField = 'HumanCapitalChange'
UNION SELECT 2, C.ColumnID, 9, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessManagementScore'
UNION SELECT 2, C.ColumnID, 10, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessManagementChange'
UNION SELECT 2, C.ColumnID, 11, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'SuccessionPlanningScore'
UNION SELECT 2, C.ColumnID, 12, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'SuccessionPlanningChange'
UNION SELECT 2, C.ColumnID, 13, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ResponseDateFirst'
UNION SELECT 2, C.ColumnID, 14, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ResponseDate'
UNION SELECT 2, C.ColumnID, 15, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BrokerDealer'
UNION SELECT 2, C.ColumnID, 16, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'TerminateDate'
UNION SELECT 2, C.ColumnID, 17, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'StartDate'
UNION SELECT 2, C.ColumnID, 18, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'GDC_T12'
UNION SELECT 2, C.ColumnID, 19, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ProfileID'
-- BusinessPlanProgression
UNION SELECT 3, C.ColumnID, 1, NULL, NULL, 1, 1, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AdvisorName'
UNION SELECT 3, C.ColumnID, 2, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessConsultantName'
UNION SELECT 3, C.ColumnID, 3, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessPlanYear'
UNION SELECT 3, C.ColumnID, 4, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ObjectiveName'
UNION SELECT 3, C.ColumnID, 5, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ObjectiveValue'
UNION SELECT 3, C.ColumnID, 6, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'CompleteDate'
UNION SELECT 3, C.ColumnID, 7, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'PercentComplete'
UNION SELECT 3, C.ColumnID, 8, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BrokerDealer'
UNION SELECT 3, C.ColumnID, 9, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'TerminateDate'
UNION SELECT 3, C.ColumnID, 10, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'StartDate'
UNION SELECT 3, C.ColumnID, 11, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'GDC_T12'
UNION SELECT 3, C.ColumnID, 12, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ProfileID'
-- EnrollmentActivity
UNION SELECT 4, C.ColumnID, 1, NULL, NULL, 1, 1, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AdvisorName'
UNION SELECT 4, C.ColumnID, 2, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BusinessConsultantName'
UNION SELECT 4, C.ColumnID, 3, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AdvisorEmail'
UNION SELECT 4, C.ColumnID, 4, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AdvisorPhone'
UNION SELECT 4, C.ColumnID, 5, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AdvisorState'
UNION SELECT 4, C.ColumnID, 6, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'Activity'
UNION SELECT 4, C.ColumnID, 7, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'CreateDate'
UNION SELECT 4, C.ColumnID, 8, NULL, 110, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'CompleteDate'
UNION SELECT 4, C.ColumnID, 9, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ModifyDate'
UNION SELECT 4, C.ColumnID, 10, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'Continuity'
UNION SELECT 4, C.ColumnID, 11, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ContinuityOnFile'
UNION SELECT 4, C.ColumnID, 12, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'Succession'
UNION SELECT 4, C.ColumnID, 13, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'Acquisition'
UNION SELECT 4, C.ColumnID, 14, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'QualifiedBuyer'
UNION SELECT 4, C.ColumnID, 15, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'SuccessionSellTimeframe'
UNION SELECT 4, C.ColumnID, 16, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'SuccessionBuyTimeframe'
UNION SELECT 4, C.ColumnID, 17, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AcquisitionBuyTimeframe'
UNION SELECT 4, C.ColumnID, 18, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'BrokerDealer'
UNION SELECT 4, C.ColumnID, 19, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'GDC_T12'
UNION SELECT 4, C.ColumnID, 20, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AUM'
UNION SELECT 4, C.ColumnID, 21, NULL, NULL, 1, 0, 1,NULL FROM rpt.[Column] C WHERE DataField = 'TerminateDate'
UNION SELECT 4, C.ColumnID, 22, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'ProfileID'
-- BusinessAssessmentSummary
UNION SELECT 5, C.ColumnID, 1, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'QuestionOrdinal'
UNION SELECT 5, C.ColumnID, 2, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'PageName'
UNION SELECT 5, C.ColumnID, 3, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'QuestionText'
UNION SELECT 5, C.ColumnID, 4, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'TotalAnswers'
UNION SELECT 5, C.ColumnID, 5, NULL, NULL, 1, 0, 1, NULL FROM rpt.[Column] C WHERE DataField = 'AverageScore'

GO
