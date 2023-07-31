alter table TAB_GATEPASS add BUILDING_ID [uniqueidentifier]

alter table [dbo].[TAB_TRAINNING_WORKFORCE] add BUILDING_ID [uniqueidentifier]

alter table [dbo].[TAB_TRAINNING_WORKFORCE_MAPPING] add TRAINNER_NAME VARCHAR(50)

alter table [dbo].[TAB_TRAINNING_WORKFORCE_MAPPING] add PRESENTED_EMP VARCHAR(MAX)

alter table TAB_TOOL_TALK_MASTER add  BUILDING_ID [uniqueidentifier]

alter table [dbo].[TAB_SHIFT_MASTER] add ID int

alter table TAB_TOOL_TALK_DAILY_CHECKLIST add SHIFT_AUTOID int,BUILDING_ID [uniqueidentifier]

alter table TAB_TOOL_TALK_DAILY_CHECKLIST add EMP_NAME VARCHAR(MAX)

alter table TAB_TOOL_TALK_DAILY_CHECKLIST add WF_ID uniqueidentifier
alter table TAB_TOOL_TALK_DAILY_CHECKLIST add DELIVERED_BY VARCHAR(100)

Alter table [dbo].[TAB_SHIFT_MASTER] add SHIFT_CODE VARCHAR(100)
Alter table [dbo].[TAB_SHIFT_MASTER] add BreakStartTime VARCHAR(100),BreakEndTime VARCHAR(100)

alter table [dbo].[New_TAB_BIOMETRIC] add GatePassMin as dbo.fn_GetGatePasshour(AttendanceLogId)

-----------------------------test--------------------------------------------------
alter table [dbo].[New_TAB_BIOMETRIC] add CountPunch as dbo.fn_Count_TotalPunch(AttendanceLogId)

ALTER TABLE [dbo].[New_TAB_BIOMETRIC]
DROP COLUMN GatePassMin;

---------------------------------------------------------------------------------------

alter table [dbo].[TAB_BIOMETRIC] add Present nvarchar(max),
Absent nvarchar(max),
Status nvarchar(max),
OverTime nvarchar(max),
EarlyBy nvarchar(max),
LateBy nvarchar(max),
InTime nvarchar(max),
OutTime nvarchar(max),
Duration nvarchar(max),
PunchRecords nvarchar(max)




-------------------------------------------------------------------------------

select * from TAB_BIOMETRIC where convert(date,ATTENDANCE_DATE) between CONVERT(date,'2022-06-1') and CONVERT(date,'2022-06-30') and
 BIOMETRIC_CODE=11016 order by ATTENDANCE_DATE asc

select * from New_TAB_BIOMETRIC where convert(date,AttendanceDate) between CONVERT(date,'2022-06-1') and CONVERT(date,'2022-06-30') and
 EmployeeId=11016 order by AttendanceDate asc

select EmployeeId,AttendanceDate,Present,Absent,Status,OverTime,[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords]
into #tmp
from New_TAB_BIOMETRIC where convert(date,AttendanceDate) between CONVERT(date,'2022-05-1') and CONVERT(date,'2022-05-31') and
 EmployeeId=11016 order by AttendanceDate asc

update tb set tb.[Present]=tm.Present,tb.[Absent]=tm.Absent,tb.[Status]=tm.Status
,tb.[OverTime]=tm.OverTime,tb.[EarlyBy]=tm.EarlyBy,tb.[LateBy]=tm.LateBy,tb.[InTime]=tm.InTime
,tb.[OutTime]=tm.OutTime,tb.[Duration]=tm.Duration,tb.[PunchRecords]=tm.PunchRecords from TAB_BIOMETRIC as tb
inner join #tmp as tm on tm.EmployeeId=tb.BIOMETRIC_CODE and tm.AttendanceDate=tb.ATTENDANCE_DATE
where convert(date,ATTENDANCE_DATE) between CONVERT(date,'2022-05-1') and CONVERT(date,'2022-05-31') and
 BIOMETRIC_CODE=11016 


 ------------------------------------------------------------------------------------------------------------------------

 USE [DB29_2022_WFM]
GO

INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]--,[SHIFT_STARTTIME],[SHIFT_ENDTIME],[SHIFT_BREAK1STARTTIME],[SHIFT_BREAK1ENDTIME],[INSERTED_ON],[ATTENDANCELOG_ID]
--,[MDBFILEID],[IN_SEQID]
,[Present],[Absent],[Status]
,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords])
     
select [AttendanceLogId],isnull((select WF_ID from TAB_WORKFORCE_MASTER where BIOMETRIC_CODE=at1.[EmployeeId]),'FAEA125F-1987-4781-ABE6-823242D12486')
,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
,AttendanceDate,StatusCode,ShiftId,Present,Absent,Status
,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords
from [dbo].[AttendanceAtal1] as at1


----------------------------------------------------------------------------------------------------
          
INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
,[SHIFT_STARTTIME],[SHIFT_ENDTIME]--,[SHIFT_BREAK1STARTTIME],[SHIFT_BREAK1ENDTIME],[INSERTED_ON],[ATTENDANCELOG_ID]
--,[MDBFILEID],[IN_SEQID]
,[Present],[Absent],[Status]
,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])
     
select [AttendanceLogId],WF_ID

,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
,AttendanceDate,StatusCode,ShiftId
,convert(time,InTime),CONVERT(time,OutTime)
,Present,Absent,at1.Status
,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
from [dbo].[AttendanceAshutosh] as at1 
inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId



GO




CREATE TABLE [dbo].[TAB_EMPLOYEE_TRANSFER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WF_ID] [uniqueidentifier] NULL,
	[BUILDING_ID] [uniqueidentifier] NULL,
	[DEPT_ID] [uniqueidentifier] NULL,
	[SUB_DEPT_ID] [uniqueidentifier] NULL,
	[EMPLOYMENT_TYPE] int NULL,
	[REQUESTED_DATE] [datetime] NULL,
	[REQUESTED_BY] [nvarchar](50) NULL,
	[APPROVED_DATE] [datetime] NULL,
	[APPROVED_BY] [nvarchar](50) NULL,
	[IS_APPROVED] int NULL,
 CONSTRAINT [PK_TAB_EMPLOYEE_TRANSFER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


alter table TAB_EMPLOYEE_TRANSFER add Remark varchar(max)


CREATE FUNCTION [dbo].[fnSplitString] 
( 
  @string  varchar(MAX), 
  @delimiter CHAR(1) 
) 
RETURNS @output TABLE(splitdata  varchar(MAX) 
) 
BEGIN 
  DECLARE @start INT, @end INT 
  SELECT @start = 1, @end = CHARINDEX(@delimiter, @string) 
  WHILE @start < LEN(@string) + 1 BEGIN 
      IF @end = 0  
          SET @end = LEN(@string) + 1
     
      INSERT INTO @output (splitdata)  
      VALUES(SUBSTRING(@string, @start, @end - @start)) 
      SET @start = @end + 1 
      SET @end = CHARINDEX(@delimiter, @string, @start)
      
  END 
  RETURN 
  END

GO

set identity_insert [dbo].[TAB_ALL_MAIL] on

ALTER TABLE TAB_USER_DEPARTMENT_MAPPING add BUILDING_ID [uniqueidentifier]

ALTER TABLE TAB_LOGIN_MASTER add BUILDING_ID [uniqueidentifier]

ALTER TABLE [dbo].[TAB_WORKFORCE_SALARY] ADD PR_AMOUNT DECIMAL(18,2)

alter table [dbo].[TAB_WORKFORCE_SALARY] add NoofPresentDays int


alter table [dbo].[TAB_WORKFORCE_SALARY] add LunchAllowance decimal(18,2),DinnerAllowance decimal(18,2)

alter table [dbo].[TAB_WORKFORCE_SALARY] add  Permission int