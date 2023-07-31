CREATE TABLE [dbo].[TAB_TOOL_TALK_DAILY_CHECKLIST](
	[ID] uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
	[TOOL_TALK_ID] uniqueidentifier NOT NULL,
	[WF_ID] uniqueidentifier NOT NULL,
	[DEPT_ID] uniqueidentifier NOT NULL,
	[DATE] datetime NOT NULL DEFAULT (getdate()),
	[CREATED_DATE] [datetime] NULL DEFAULT (getdate()),
	[CREATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TAB_TOOL_TALK_DAILY_CHECKLIST] WITH CHECK ADD FOREIGN KEY([TOOL_TALK_ID])
REFERENCES [dbo].[TAB_TOOL_TALK_MASTER] ([ID])
GO

ALTER TABLE [dbo].[TAB_TOOL_TALK_DAILY_CHECKLIST] WITH CHECK ADD FOREIGN KEY([WF_ID])
REFERENCES [dbo].[TAB_WORKFORCE_MASTER] ([WF_ID])
GO

ALTER TABLE [dbo].[TAB_TOOL_TALK_DAILY_CHECKLIST] WITH CHECK ADD FOREIGN KEY([DEPT_ID])
REFERENCES [dbo].[TAB_DEPARTMENT_MASTER] ([DEPT_ID])
GO

insert into dbo.TAB_SUBMENU  values('01B3E7D0-2CFA-4957-81A5-02BA74D8C2C4', '88CC250A-D681-458D-BFE9-5B9A60515FB5', 'Mark Daily Check List', 'Mark Check List - Top Left Sub Menu', 'ToolTalk', 'CreateDailyCheckList', 1, 5)
insert into dbo.TAB_SUBMENU  values('74CF8DA0-6088-4B19-8B0F-44C98732D1BB', '88CC250A-D681-458D-BFE9-5B9A60515FB5', 'Daily Check Lists', 'Daily Check Lists - Top Left Sub Menu', 'ToolTalk', 'DailyCheckLists', 1, 6)

insert into dbo.TAB_MENU_ROLE_MAPPING values(NEWID(), '88CC250A-D681-458D-BFE9-5B9A60515FB5', '01B3E7D0-2CFA-4957-81A5-02BA74D8C2C4', '1F72634E-9B41-EB11-9471-8CDCD4D2C4BC')
insert into dbo.TAB_MENU_ROLE_MAPPING values(NEWID(), '88CC250A-D681-458D-BFE9-5B9A60515FB5', '74CF8DA0-6088-4B19-8B0F-44C98732D1BB', '1F72634E-9B41-EB11-9471-8CDCD4D2C4BC')

