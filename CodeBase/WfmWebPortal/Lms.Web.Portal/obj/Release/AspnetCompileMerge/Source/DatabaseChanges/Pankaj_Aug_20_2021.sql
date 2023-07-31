﻿ALTER TABLE [dbo].[TAB_MRF_DETAILS]  ADD HIRING_QUANTITY INT DEFAULT(0) NOT NULL

CREATE TABLE [dbo].[TAB_CONSTANT_DATA](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WORKFORC_COUNT] INT DEFAULT(0) NOT NULL
 CONSTRAINT PK_TAB_CONSTANT_DATA PRIMARY KEY CLUSTERED 
(
	ID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
INSERT INTO TAB_CONSTANT_DATA VALUES(2000)

ALTER TABLE TAB_WORKFORCE_SALARY_MASTER
ADD CONSTRAINT PK_TAB_WORKFORCE_SALARY_MASTER_WF_ID PRIMARY KEY(WF_ID)