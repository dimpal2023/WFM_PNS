--gatepass changes
alter table dbo.TAB_GATEPASS add DEPT_ID uniqueidentifier not null
alter table dbo.TAB_GATEPASS add WF_EMP_TYPE smallint not null
alter table dbo.TAB_GATEPASS add [CREATED_DATE] [datetime] NULL
alter table dbo.TAB_GATEPASS add [CREATED_BY] [varchar](100) NULL
alter table dbo.TAB_GATEPASS add [UPDATED_DATE] [datetime] NULL
alter table dbo.TAB_GATEPASS add [UPDATED_BY] [varchar](100) NULL

ALTER TABLE [dbo].[TAB_GATEPASS]  WITH CHECK ADD FOREIGN KEY([DEPT_ID])
REFERENCES [dbo].[TAB_DEPARTMENT_MASTER] ([DEPT_ID])
GO
ALTER TABLE [dbo].[TAB_GATEPASS]  WITH CHECK ADD FOREIGN KEY([WF_EMP_TYPE])
REFERENCES [dbo].[TAB_WORFORCE_TYPE] ([WF_EMP_TYPE])
GO