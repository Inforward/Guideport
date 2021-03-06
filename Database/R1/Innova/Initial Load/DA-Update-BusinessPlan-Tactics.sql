USE [Innova]
GO

DELETE FROM [dbo].[BusinessPlanTactic] WHERE BusinessPlanID IS NULL
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Ask for Client Referrals', 
N'- Create a Process
- Build an Information File to Collect Data
- Select a Strategy
- Take Action and Follow Up', 4, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Bring in Partner', 
N'- State Why a Partner Would Improve Your Business
- List the Desirable Qualities of a Partner
- Evaluate the Potential Problems of Partnership
- Begin a Process for Identifying a Partner
- Establish Relationships with Potential Partners
- Discuss Partnership Arrangement
- Agree on Partnership Structure
- Formalize Partnership', 18, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Change Investment Management', 
N'- List Shortcomings of Current Offering
- Evaluate Whether Shortcomings Can Reasonably Be Solved
- Evaluate Options for Change
- Learn New Options
- Implement New Investment Management', 16, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Change Legal Structure', 
N'- Understand Why Your Current Structure Needs to Be Changed
- Learn About Alternative Legal Structures
- Identify a Legal Structure That Addresses Your Needs
- Hire an Attorney to Draft the Appropriate Documents
- File Documents to Formalize the Change in Legal Structure
- Make Changes to Any Advertising, Signage, Stationery or Business Cards', 22, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Compensation Modeling', 
N'- List Compensation for All Employees 
- Categorize Employees Based on Title or Responsibilities 
- Search for Industry Data on Compensation ranges
- Compare Industry Data Versus Actual Compensation ', 9, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Complete Continuity Plan', 
N'- Visit the Succession Planning Measure on Pentameter™
- Fill out the Enrollment form
- Use the Step-by-Step Process to Learn How to Create a Continuity Plan
- Decide What Your Continuity Plan Needs
- Engage with an Attorney Who Specializes in Continuity Planning
- Use Your Broker/Dealer’s Resources to Create a Plan
- File Your Continuity Plan with Your Broker/Dealer', 20, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Complete Succession Plan', 
N'- Visit the Succession Planning Measure on Pentameter™
- Fill Out the Enrollment Form
- Use the Step-by-Step Process to Learn How to Create a Succession Plan
- Decide What Your Succession Plan Needs
- Engage with an Attorney Who Specializes in Succession Planning
- Use Your Broker/Dealer’s Resources to Create a Plan', 19, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Develop Niche', 
N'- Identify a Client Need You Can Solve
- Identify Groups Who Have This Need
- Identify Current Clients in These Groups
- Marry Your Personal Interests with Groups That Have This Need
- Network Your Way into a Few Groups
- Get to Know the People
- Demonstrate Your Skills', 12, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Establish Benchmarks', 
N'- Search for Industry Data on Financial Indicators
- Determine the Key Financial Indicators of Your Business
- Compare Your Data to That of Like-Size Companies', 7, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Expand Client Communication', 
N'- Segment Book of Business
- Define Client Communication Standards
- Understand Client Expectations
- Investigate Various Communication Media
- Select Communication Media to Use
- Select Which Media You Will Use with Which Type of Communication
- Use Software to Automate Communication
- Send Communication
- Get Feedback', 11, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Expand Geographic Footprint', 
N'- Identify Desired Geography
- Evaluate Business Infrastructure
- Evaluate Time Commitment to New Geography
- Decide Whether a Physical Location Is Necessary', 13, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Host Seminars', 
N'- Buy Mailing List
- Set Seminar Date
- Search for Venue
- Set Agenda
- Invite Speakers
- Design Invitations
- Obtain Advertising Compliance Approval
- Print Invitations
- Mail Invitations
- Schedule Office Meetings', 1, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Implement Business Continuity Plan', 
N'- Visit http://www.ready.gov/business/implementation/continuity
- Do a Business Impact Analysis
- Devise Recovery Strategies
- Develop a Plan
- Conduct Testing and Exercises', 21, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO
INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Invest in Advertising', 
N'- Identify Target Market
- Identify Media Focused on Target Market
- Set Budget for Campaign
- Create Content for Advertisement
- Obtain Advertising Compliance Approval', 3, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Invest in Digital Media', 
N'- Investigate the Various Platforms
- Select Platform
- Create Content
- Obtain Advertising Compliance Approval
- Systematize Usage', 5, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Invest in Software', 
N'- Understand Where You Need to Create Efficiencies
- Research Software Packages That Address Your Inefficiencies
- Demo Available Packages
- Purchase Software
- Get Training
- Institutionalize Software Implementation
- Evaluate Utilization', 6, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO
INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Offer New Products or Services', 
N'- Evaluate Shortages in Current Offering
- Identify Desired Outcome If Changes Are Made
- Evaluate Offerings in the Marketplace
- Select New Products
- Establish Relationships with Product Companies
- Learn Product Details and Sales Stories
- Offer Products to Clients They Would Help', 14, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Pay Off Debt', 
N'- Organize Debt Structure
- Evaluate Resources or Options to Pay Off Debt
- Anticipate Timeline to Pay Off Debt
- Calculate Opportunity Cost and Time Value of Debt Payoff
- Make Payment or Begin Payment Plan', 17, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Perform Process Mapping', 
N'- List Repeated Actions
- Identify Most Important Actions
- Document List of Tasks Required to Complete Actions
- Identify Owners of Tasks
- Set Timelines
- Evaluate Opportunities for Improvement
- Create New Processes for Actions
- Input into Software System to Create Scale and Efficiency', 8, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Send Client Surveys', 
N'- Define What You Want to Learn From Your Clients
- Write Survey Questions
- Obtain Advertising Compliance Approval
- Select Survey Software
- Input Questions and Client Email into Survey Software
- Select Survey Dates
- Send Survey
- Read Survey Data
- Make Changes Based on Survey Data', 10, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Send Direct Mail Campaigns', 
N'- Buy Mailing List
- Design Mailer
- Obtain Advertising Compliance Approval
- Print Mailer
- Send Mailer', 2, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Engage in Training/Education', 
N'- Identify Subject You Wish to Learn
- State How Knowing Subject Will Help Your Business
- Understand How Much Time You Have to Commit
- Search for Providers That Teach the Subject
- Evaluate Providers Based on Aspects Important to You
- Decide Whether the Investment of Time and Money Will Pay Off
- Pursue Learning If There Is a Positive Payoff', 15, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO

INSERT [dbo].[BusinessPlanTactic] ([BusinessPlanID], [Name], [Description], [SortOrder], [CreateUserID], [CreateDate], [CreateDateUTC], [ModifyUserID], [ModifyDate], [ModifyDateUTC], [CompletedDate], [Editable]) 
VALUES ( NULL, N'Re-design Website', 
N'- Rethink the Concept Behind the Website
- Review the Content
- Review the Organization of Content
- Re-Design Website or Do a Complete End-to-End Rebuilt with New Content', 15, 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), 0, CAST(0x0000A34000F73140 AS DateTime), CAST(0x0000A34000F73140 AS DateTime), NULL, 0)
GO
