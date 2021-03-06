USE [Innova]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanTactic_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]'))
ALTER TABLE [dbo].[BusinessPlanTactic] DROP CONSTRAINT [FK_dbo.BusinessPlanTactic_dbo.BusinessPlan_BusinessPlanID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessPlanSwot_BusinessPlan1]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]'))
ALTER TABLE [dbo].[BusinessPlanSwot] DROP CONSTRAINT [FK_BusinessPlanSwot_BusinessPlan1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanTactic_BusinessPlanTacticID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]'))
ALTER TABLE [dbo].[BusinessPlanStrategyTactic] DROP CONSTRAINT [FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanTactic_BusinessPlanTacticID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]'))
ALTER TABLE [dbo].[BusinessPlanStrategyTactic] DROP CONSTRAINT [FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategy_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]'))
ALTER TABLE [dbo].[BusinessPlanStrategy] DROP CONSTRAINT [FK_dbo.BusinessPlanStrategy_dbo.BusinessPlan_BusinessPlanID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]'))
ALTER TABLE [dbo].[BusinessPlanObjectiveStrategy] DROP CONSTRAINT [FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanObjective_BusinessPlanObjectiveID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]'))
ALTER TABLE [dbo].[BusinessPlanObjectiveStrategy] DROP CONSTRAINT [FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanObjective_BusinessPlanObjectiveID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjective_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]'))
ALTER TABLE [dbo].[BusinessPlanObjective] DROP CONSTRAINT [FK_dbo.BusinessPlanObjective_dbo.BusinessPlan_BusinessPlanID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AffiliateBusinessPlanObjective_BusinessPlanObjectiveID]') AND parent_object_id = OBJECT_ID(N'[dbo].[AffiliateBusinessPlanObjective]'))
ALTER TABLE [dbo].[AffiliateBusinessPlanObjective] DROP CONSTRAINT [FK_AffiliateBusinessPlanObjective_BusinessPlanObjectiveID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeRole_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeRole] DROP CONSTRAINT [FK_dbo.BusinessPlanEmployeeRole_dbo.BusinessPlan_BusinessPlanID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployeeRole_BusinessPlanEmployeeRoleID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeEmployeeRole] DROP CONSTRAINT [FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployeeRole_BusinessPlanEmployeeRoleID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployee_BusinessPlanEmployeeID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeEmployeeRole] DROP CONSTRAINT [FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployee_BusinessPlanEmployeeID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployee_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]'))
ALTER TABLE [dbo].[BusinessPlanEmployee] DROP CONSTRAINT [FK_dbo.BusinessPlanEmployee_dbo.BusinessPlan_BusinessPlanID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessPlanEmployee_ParentID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]'))
ALTER TABLE [dbo].[BusinessPlanEmployee] DROP CONSTRAINT [FK_BusinessPlanEmployee_ParentID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__Edita__6AA692EC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanTactic] DROP CONSTRAINT [DF__BusinessP__Edita__6AA692EC]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__Edita__69B26EB3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanStrategy] DROP CONSTRAINT [DF__BusinessP__Edita__69B26EB3]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__AutoT__63057124]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanObjective] DROP CONSTRAINT [DF__BusinessP__AutoT__63057124]
END

GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__Perce__15C5FB1B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanObjective] DROP CONSTRAINT [DF__BusinessP__Perce__15C5FB1B]
END

GO
/****** Object:  Index [IX_BusinessPlanTactic_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]') AND name = N'IX_BusinessPlanTactic_BusinessPlanID_Name')
DROP INDEX [IX_BusinessPlanTactic_BusinessPlanID_Name] ON [dbo].[BusinessPlanTactic]
GO
/****** Object:  Index [IX_BusinessPlanTactic_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]') AND name = N'IX_BusinessPlanTactic_BusinessPlanID')
DROP INDEX [IX_BusinessPlanTactic_BusinessPlanID] ON [dbo].[BusinessPlanTactic]
GO
/****** Object:  Index [IX_BusinessPlanSwot_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]') AND name = N'IX_BusinessPlanSwot_BusinessPlanID')
DROP INDEX [IX_BusinessPlanSwot_BusinessPlanID] ON [dbo].[BusinessPlanSwot]
GO
/****** Object:  Index [IX_BusinessPlanTacticID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]') AND name = N'IX_BusinessPlanTacticID')
DROP INDEX [IX_BusinessPlanTacticID] ON [dbo].[BusinessPlanStrategyTactic]
GO
/****** Object:  Index [IX_BusinessPlanStrategyID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]') AND name = N'IX_BusinessPlanStrategyID')
DROP INDEX [IX_BusinessPlanStrategyID] ON [dbo].[BusinessPlanStrategyTactic]
GO
/****** Object:  Index [IX_BusinessPlanStrategy_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]') AND name = N'IX_BusinessPlanStrategy_BusinessPlanID_Name')
DROP INDEX [IX_BusinessPlanStrategy_BusinessPlanID_Name] ON [dbo].[BusinessPlanStrategy]
GO
/****** Object:  Index [IX_BusinessPlanStrategy_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]') AND name = N'IX_BusinessPlanStrategy_BusinessPlanID')
DROP INDEX [IX_BusinessPlanStrategy_BusinessPlanID] ON [dbo].[BusinessPlanStrategy]
GO
/****** Object:  Index [IX_BusinessPlanObjectiveStrategy_BusinessPlanStrategyID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]') AND name = N'IX_BusinessPlanObjectiveStrategy_BusinessPlanStrategyID')
DROP INDEX [IX_BusinessPlanObjectiveStrategy_BusinessPlanStrategyID] ON [dbo].[BusinessPlanObjectiveStrategy]
GO
/****** Object:  Index [IX_BusinessPlanObjectiveStrategy_BusinessPlanObjectiveID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]') AND name = N'IX_BusinessPlanObjectiveStrategy_BusinessPlanObjectiveID')
DROP INDEX [IX_BusinessPlanObjectiveStrategy_BusinessPlanObjectiveID] ON [dbo].[BusinessPlanObjectiveStrategy]
GO
/****** Object:  Index [IX_BusinessPlanObjective_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]') AND name = N'IX_BusinessPlanObjective_BusinessPlanID_Name')
DROP INDEX [IX_BusinessPlanObjective_BusinessPlanID_Name] ON [dbo].[BusinessPlanObjective]
GO
/****** Object:  Index [IX_BusinessPlanObjective_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]') AND name = N'IX_BusinessPlanObjective_BusinessPlanID')
DROP INDEX [IX_BusinessPlanObjective_BusinessPlanID] ON [dbo].[BusinessPlanObjective]
GO
/****** Object:  Index [IX_BusinessPlanHistory_UserID_Year]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanHistory]') AND name = N'IX_BusinessPlanHistory_UserID_Year')
DROP INDEX [IX_BusinessPlanHistory_UserID_Year] ON [dbo].[BusinessPlanHistory]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeRole_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeRole_BusinessPlanID_Name')
DROP INDEX [IX_BusinessPlanEmployeeRole_BusinessPlanID_Name] ON [dbo].[BusinessPlanEmployeeRole]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeRole_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeRole_BusinessPlanID')
DROP INDEX [IX_BusinessPlanEmployeeRole_BusinessPlanID] ON [dbo].[BusinessPlanEmployeeRole]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeRoleID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeRoleID')
DROP INDEX [IX_BusinessPlanEmployeeRoleID] ON [dbo].[BusinessPlanEmployeeEmployeeRole]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeID')
DROP INDEX [IX_BusinessPlanEmployeeID] ON [dbo].[BusinessPlanEmployeeEmployeeRole]
GO
/****** Object:  Index [IX_BusinessPlanEmployee_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]') AND name = N'IX_BusinessPlanEmployee_BusinessPlanID')
DROP INDEX [IX_BusinessPlanEmployee_BusinessPlanID] ON [dbo].[BusinessPlanEmployee]
GO
/****** Object:  Index [IX_BusinessPlan_UserID_Year]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlan]') AND name = N'IX_BusinessPlan_UserID_Year')
DROP INDEX [IX_BusinessPlan_UserID_Year] ON [dbo].[BusinessPlan]
GO
/****** Object:  View [dbo].[vwBusinessPlanObjective]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwBusinessPlanObjective]'))
DROP VIEW [dbo].[vwBusinessPlanObjective]
GO
/****** Object:  View [dbo].[vwBusinessPlanObjectiveValues]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwBusinessPlanObjectiveValues]'))
DROP VIEW [dbo].[vwBusinessPlanObjectiveValues]
GO
/****** Object:  Table [dbo].[BusinessPlanTactic]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanTactic]
GO
/****** Object:  Table [dbo].[BusinessPlanSwot]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanSwot]
GO
/****** Object:  Table [dbo].[BusinessPlanStrategyTactic]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanStrategyTactic]
GO
/****** Object:  Table [dbo].[BusinessPlanStrategy]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanStrategy]
GO
/****** Object:  Table [dbo].[BusinessPlanObjectiveStrategy]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanObjectiveStrategy]
GO
/****** Object:  Table [dbo].[BusinessPlanObjective]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanObjective]
GO
/****** Object:  Table [dbo].[AffiliateBusinessPlanObjective]    Script Date: 12/15/2014 04:26:07 PM By Dave E ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AffiliateBusinessPlanObjective]') AND type in (N'U'))
DROP TABLE [dbo].[AffiliateBusinessPlanObjective]
GO
/****** Object:  Table [dbo].[BusinessPlanHistory]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanHistory]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanHistory]
GO
/****** Object:  Table [dbo].[BusinessPlanEmployeeRole]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanEmployeeRole]
GO
/****** Object:  Table [dbo].[BusinessPlanEmployeeEmployeeRole]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanEmployeeEmployeeRole]
GO
/****** Object:  Table [dbo].[BusinessPlanEmployee]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlanEmployee]
GO
/****** Object:  Table [dbo].[BusinessPlan]    Script Date: 9/25/2014 12:45:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlan]') AND type in (N'U'))
DROP TABLE [dbo].[BusinessPlan]
GO
/****** Object:  Table [dbo].[BusinessPlan]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlan](
	[BusinessPlanID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[MissionWhat] [varchar](max) NULL,
	[MissionHow] [varchar](max) NULL,
	[MissionWhy] [varchar](max) NULL,
	[VisionOneYear] [varchar](max) NULL,
	[VisionFiveYear] [varchar](max) NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,
	[DeleteUserID] [int] NULL,
	[DeleteDate] [datetime] NULL,
	[DeleteDateUTC] [datetime] NULL,
 CONSTRAINT [PK_dbo.BusinessPlan] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanEmployee]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanEmployee](
	[BusinessPlanEmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPlanEmployeeParentID] [int] NULL,
	[BusinessPlanID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[MiddleInitial] [varchar](1) NULL,
	[LastName] [varchar](50) NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,
	
 CONSTRAINT [PK_dbo.BusinessPlanEmployee] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanEmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanEmployeeEmployeeRole]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanEmployeeEmployeeRole](
	[BusinessPlanEmployeeID] [int] NOT NULL,
	[BusinessPlanEmployeeRoleID] [int] NOT NULL,
 CONSTRAINT [PK_BusinessPlanEmployeeEmployeeRole] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanEmployeeID] ASC,
	[BusinessPlanEmployeeRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[BusinessPlanEmployeeRole]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanEmployeeRole](
	[BusinessPlanEmployeeRoleID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPlanID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](1000) NULL,
	[SortOrder] [int] NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,	
 CONSTRAINT [PK_BusinessPlanEmployeeRole] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanEmployeeRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanHistory]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanObjective]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanObjective](
	[BusinessPlanObjectiveID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPlanID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](1000) NULL,
	[DataType] [varchar](50) NOT NULL,
	[PercentComplete] [int] NOT NULL,
	[EstimatedCompletionDate] [datetime] NULL,
	[SortOrder] [int] NULL,
	[AutoTrackingEnabled] [bit] NOT NULL,
	[BaselineValue] [varchar](1000) NULL,
	[BaselineDate] [datetime] NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,	
	[ModifyDateUTC] [datetime] NOT NULL,	
 CONSTRAINT [PK_dbo.BusinessPlanObjective] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanObjectiveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanObjectiveStrategy]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanObjectiveStrategy](
	[BusinessPlanObjectiveID] [int] NOT NULL,
	[BusinessPlanStrategyID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BusinessPlanObjectiveStrategy] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanObjectiveID] ASC,
	[BusinessPlanStrategyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[BusinessPlanStrategy]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanStrategy](
	[BusinessPlanStrategyID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPlanID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](1000) NULL,
	[SortOrder] [int] NULL,
	[Editable] [bit] NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.BusinessPlanStrategy] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanStrategyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanStrategyTactic]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanStrategyTactic](
	[BusinessPlanStrategyID] [int] NOT NULL,
	[BusinessPlanTacticID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BusinessPlanStrategyTactic] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanStrategyID] ASC,
	[BusinessPlanTacticID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[BusinessPlanSwot]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanSwot](
	[BusinessPlanSwotID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPlanID] [int] NOT NULL,
	[SwotType] [varchar](11) NOT NULL,
	[SwotDescription] [varchar](max) NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,	
	[CreateDateUTC] [datetime] NOT NULL,	
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,
 CONSTRAINT [PK_BusinessPlanSwot] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanSwotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPlanTactic]    Script Date: 9/25/2014 12:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessPlanTactic](
	[BusinessPlanTacticID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPlanID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](1000) NULL,
	[SortOrder] [int] NULL,
	[CompletedDate] [datetime] NULL,
	[Editable] [bit] NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateDateUTC] [datetime] NOT NULL,
	[ModifyUserID] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyDateUTC] [datetime] NOT NULL,	
 CONSTRAINT [PK_dbo.BusinessPlanTactic] PRIMARY KEY CLUSTERED 
(
	[BusinessPlanTacticID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO




/****** Object:  Index [IX_BusinessPlan_UserID_Year]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlan]') AND name = N'IX_BusinessPlan_UserID_Year')
CREATE NONCLUSTERED INDEX [IX_BusinessPlan_UserID_Year] ON [dbo].[BusinessPlan]
(
	[UserID] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanEmployee_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]') AND name = N'IX_BusinessPlanEmployee_BusinessPlanID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanEmployee_BusinessPlanID] ON [dbo].[BusinessPlanEmployee]
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanEmployeeID] ON [dbo].[BusinessPlanEmployeeEmployeeRole]
(
	[BusinessPlanEmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeRoleID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeRoleID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanEmployeeRoleID] ON [dbo].[BusinessPlanEmployeeEmployeeRole]
(
	[BusinessPlanEmployeeRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanEmployeeRole_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeRole_BusinessPlanID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanEmployeeRole_BusinessPlanID] ON [dbo].[BusinessPlanEmployeeRole]
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_BusinessPlanEmployeeRole_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]') AND name = N'IX_BusinessPlanEmployeeRole_BusinessPlanID_Name')
CREATE UNIQUE NONCLUSTERED INDEX [IX_BusinessPlanEmployeeRole_BusinessPlanID_Name] ON [dbo].[BusinessPlanEmployeeRole]
(
	[BusinessPlanID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_BusinessPlanObjective_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]') AND name = N'IX_BusinessPlanObjective_BusinessPlanID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanObjective_BusinessPlanID] ON [dbo].[BusinessPlanObjective]
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_BusinessPlanObjective_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]') AND name = N'IX_BusinessPlanObjective_BusinessPlanID_Name')
CREATE UNIQUE NONCLUSTERED INDEX [IX_BusinessPlanObjective_BusinessPlanID_Name] ON [dbo].[BusinessPlanObjective]
(
	[BusinessPlanID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanObjectiveStrategy_BusinessPlanObjectiveID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]') AND name = N'IX_BusinessPlanObjectiveStrategy_BusinessPlanObjectiveID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanObjectiveStrategy_BusinessPlanObjectiveID] ON [dbo].[BusinessPlanObjectiveStrategy]
(
	[BusinessPlanObjectiveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanObjectiveStrategy_BusinessPlanStrategyID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]') AND name = N'IX_BusinessPlanObjectiveStrategy_BusinessPlanStrategyID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanObjectiveStrategy_BusinessPlanStrategyID] ON [dbo].[BusinessPlanObjectiveStrategy]
(
	[BusinessPlanStrategyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanStrategy_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]') AND name = N'IX_BusinessPlanStrategy_BusinessPlanID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanStrategy_BusinessPlanID] ON [dbo].[BusinessPlanStrategy]
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_BusinessPlanStrategy_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]') AND name = N'IX_BusinessPlanStrategy_BusinessPlanID_Name')
CREATE UNIQUE NONCLUSTERED INDEX [IX_BusinessPlanStrategy_BusinessPlanID_Name] ON [dbo].[BusinessPlanStrategy]
(
	[BusinessPlanID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanStrategyID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]') AND name = N'IX_BusinessPlanStrategyID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanStrategyID] ON [dbo].[BusinessPlanStrategyTactic]
(
	[BusinessPlanStrategyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanTacticID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]') AND name = N'IX_BusinessPlanTacticID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanTacticID] ON [dbo].[BusinessPlanStrategyTactic]
(
	[BusinessPlanTacticID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanSwot_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]') AND name = N'IX_BusinessPlanSwot_BusinessPlanID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanSwot_BusinessPlanID] ON [dbo].[BusinessPlanSwot]
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BusinessPlanTactic_BusinessPlanID]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]') AND name = N'IX_BusinessPlanTactic_BusinessPlanID')
CREATE NONCLUSTERED INDEX [IX_BusinessPlanTactic_BusinessPlanID] ON [dbo].[BusinessPlanTactic]
(
	[BusinessPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_BusinessPlanTactic_BusinessPlanID_Name]    Script Date: 9/25/2014 12:45:07 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]') AND name = N'IX_BusinessPlanTactic_BusinessPlanID_Name')
CREATE UNIQUE NONCLUSTERED INDEX [IX_BusinessPlanTactic_BusinessPlanID_Name] ON [dbo].[BusinessPlanTactic]
(
	[BusinessPlanID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__Perce__15C5FB1B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanObjective] ADD  DEFAULT ((0)) FOR [PercentComplete]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__AutoT__63057124]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanObjective] ADD  DEFAULT ((0)) FOR [AutoTrackingEnabled]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__Edita__69B26EB3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanStrategy] ADD  DEFAULT ((1)) FOR [Editable]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__BusinessP__Edita__6AA692EC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessPlanTactic] ADD  DEFAULT ((1)) FOR [Editable]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessPlanEmployee_ParentID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]'))
ALTER TABLE [dbo].[BusinessPlanEmployee]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPlanEmployee_ParentID] FOREIGN KEY([BusinessPlanEmployeeParentID])
REFERENCES [dbo].[BusinessPlanEmployee] ([BusinessPlanEmployeeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessPlanEmployee_ParentID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]'))
ALTER TABLE [dbo].[BusinessPlanEmployee] CHECK CONSTRAINT [FK_BusinessPlanEmployee_ParentID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployee_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]'))
ALTER TABLE [dbo].[BusinessPlanEmployee]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanEmployee_dbo.BusinessPlan_BusinessPlanID] FOREIGN KEY([BusinessPlanID])
REFERENCES [dbo].[BusinessPlan] ([BusinessPlanID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployee_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployee]'))
ALTER TABLE [dbo].[BusinessPlanEmployee] CHECK CONSTRAINT [FK_dbo.BusinessPlanEmployee_dbo.BusinessPlan_BusinessPlanID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployee_BusinessPlanEmployeeID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeEmployeeRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployee_BusinessPlanEmployeeID] FOREIGN KEY([BusinessPlanEmployeeID])
REFERENCES [dbo].[BusinessPlanEmployee] ([BusinessPlanEmployeeID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployee_BusinessPlanEmployeeID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeEmployeeRole] CHECK CONSTRAINT [FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployee_BusinessPlanEmployeeID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployeeRole_BusinessPlanEmployeeRoleID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeEmployeeRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployeeRole_BusinessPlanEmployeeRoleID] FOREIGN KEY([BusinessPlanEmployeeRoleID])
REFERENCES [dbo].[BusinessPlanEmployeeRole] ([BusinessPlanEmployeeRoleID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployeeRole_BusinessPlanEmployeeRoleID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeEmployeeRole] CHECK CONSTRAINT [FK_dbo.BusinessPlanEmployeeEmployeeRole_dbo.BusinessPlanEmployeeRole_BusinessPlanEmployeeRoleID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeRole_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanEmployeeRole_dbo.BusinessPlan_BusinessPlanID] FOREIGN KEY([BusinessPlanID])
REFERENCES [dbo].[BusinessPlan] ([BusinessPlanID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanEmployeeRole_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanEmployeeRole]'))
ALTER TABLE [dbo].[BusinessPlanEmployeeRole] CHECK CONSTRAINT [FK_dbo.BusinessPlanEmployeeRole_dbo.BusinessPlan_BusinessPlanID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjective_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]'))
ALTER TABLE [dbo].[BusinessPlanObjective]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanObjective_dbo.BusinessPlan_BusinessPlanID] FOREIGN KEY([BusinessPlanID])
REFERENCES [dbo].[BusinessPlan] ([BusinessPlanID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjective_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjective]'))
ALTER TABLE [dbo].[BusinessPlanObjective] CHECK CONSTRAINT [FK_dbo.BusinessPlanObjective_dbo.BusinessPlan_BusinessPlanID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanObjective_BusinessPlanObjectiveID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]'))
ALTER TABLE [dbo].[BusinessPlanObjectiveStrategy]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanObjective_BusinessPlanObjectiveID] FOREIGN KEY([BusinessPlanObjectiveID])
REFERENCES [dbo].[BusinessPlanObjective] ([BusinessPlanObjectiveID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanObjective_BusinessPlanObjectiveID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]'))
ALTER TABLE [dbo].[BusinessPlanObjectiveStrategy] CHECK CONSTRAINT [FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanObjective_BusinessPlanObjectiveID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]'))
ALTER TABLE [dbo].[BusinessPlanObjectiveStrategy]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanStrategy_BusinessPlanStrategyID] FOREIGN KEY([BusinessPlanStrategyID])
REFERENCES [dbo].[BusinessPlanStrategy] ([BusinessPlanStrategyID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanObjectiveStrategy]'))
ALTER TABLE [dbo].[BusinessPlanObjectiveStrategy] CHECK CONSTRAINT [FK_dbo.BusinessPlanObjectiveStrategy_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategy_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]'))
ALTER TABLE [dbo].[BusinessPlanStrategy]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanStrategy_dbo.BusinessPlan_BusinessPlanID] FOREIGN KEY([BusinessPlanID])
REFERENCES [dbo].[BusinessPlan] ([BusinessPlanID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategy_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategy]'))
ALTER TABLE [dbo].[BusinessPlanStrategy] CHECK CONSTRAINT [FK_dbo.BusinessPlanStrategy_dbo.BusinessPlan_BusinessPlanID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]'))
ALTER TABLE [dbo].[BusinessPlanStrategyTactic]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanStrategy_BusinessPlanStrategyID] FOREIGN KEY([BusinessPlanStrategyID])
REFERENCES [dbo].[BusinessPlanStrategy] ([BusinessPlanStrategyID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]'))
ALTER TABLE [dbo].[BusinessPlanStrategyTactic] CHECK CONSTRAINT [FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanStrategy_BusinessPlanStrategyID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanTactic_BusinessPlanTacticID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]'))
ALTER TABLE [dbo].[BusinessPlanStrategyTactic]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanTactic_BusinessPlanTacticID] FOREIGN KEY([BusinessPlanTacticID])
REFERENCES [dbo].[BusinessPlanTactic] ([BusinessPlanTacticID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanTactic_BusinessPlanTacticID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanStrategyTactic]'))
ALTER TABLE [dbo].[BusinessPlanStrategyTactic] CHECK CONSTRAINT [FK_dbo.BusinessPlanStrategyTactic_dbo.BusinessPlanTactic_BusinessPlanTacticID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessPlanSwot_BusinessPlan1]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]'))
ALTER TABLE [dbo].[BusinessPlanSwot]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPlanSwot_BusinessPlan1] FOREIGN KEY([BusinessPlanID])
REFERENCES [dbo].[BusinessPlan] ([BusinessPlanID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BusinessPlanSwot_BusinessPlan1]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanSwot]'))
ALTER TABLE [dbo].[BusinessPlanSwot] CHECK CONSTRAINT [FK_BusinessPlanSwot_BusinessPlan1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanTactic_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]'))
ALTER TABLE [dbo].[BusinessPlanTactic]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BusinessPlanTactic_dbo.BusinessPlan_BusinessPlanID] FOREIGN KEY([BusinessPlanID])
REFERENCES [dbo].[BusinessPlan] ([BusinessPlanID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.BusinessPlanTactic_dbo.BusinessPlan_BusinessPlanID]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessPlanTactic]'))
ALTER TABLE [dbo].[BusinessPlanTactic] CHECK CONSTRAINT [FK_dbo.BusinessPlanTactic_dbo.BusinessPlan_BusinessPlanID]
GO
