﻿ALTER TABLE dbo.TAB_TRAINNING_WORKFORCE_MAPPING  ADD Remark varchar(500)
ALTER TABLE dbo.TAB_WORKFORCE_EXIT_APPROVER  ADD REMARK varchar(500)
ALTER TABLE dbo.TAB_WORKFORCE_EXIT  ADD COMPANY_ID [uniqueidentifier]

EXEC sp_RENAME 'TAB_WORKFLOW_MAPPING_MASTER.DEPT_ID' , 'ROLE_ID', 'COLUMN'
EXEC sp_RENAME 'TAB_WORKFLOW_MAPPING_MASTER.WF_ID' , 'USER_ID', 'COLUMN'

CREATE TABLE [dbo].[TAB_MRF_APPROVER](
	[MRF_APPROVER_ID] [uniqueidentifier] NOT NULL,
	[MRP_INETRNAL_ID] [uniqueidentifier] NOT NULL,
	[APPROVE_BY] [uniqueidentifier] NULL,
	[APPROVE_DATE] [datetime] NULL,
	[APPROVER_REMARK] [varchar](500) NULL,
	[APPROVER_STATUS] [char](1) NULL,
 CONSTRAINT [PK_TAB_MRF_APPROVER] PRIMARY KEY CLUSTERED 
(
	[MRF_APPROVER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


