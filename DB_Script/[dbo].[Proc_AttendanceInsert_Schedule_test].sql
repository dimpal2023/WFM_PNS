USE [WFM_PROD]
GO
/****** Object:  StoredProcedure [dbo].[Proc_AttendanceInsert_Schedule_test]    Script Date: 12/9/2022 12:33:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec Proc_AttendanceInsert_Schedule

ALTER procedure [dbo].[Proc_AttendanceInsert_Schedule_test]
as 
begin
        DECLARE @Month int,@Year int
		SELECT @Month=MONTH(DATEADD(MONTH, -1, CURRENT_TIMESTAMP));
		SELECT @Year=DATENAME (YEAR, GETDATE())

		delete from TAB_BIOMETRIC where Month(ATTENDANCE_DATE)=@Month and YEAR(ATTENDANCE_DATE)=@Year

		
		------------------------------------[AttendanceAsheesh] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceAsheesh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------[AttendanceAshutosh] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceAshutosh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------ [AttendanceAtal]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceAtal] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------ [AttendanceJitendra]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceJitendra] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------ [AttendanceManas]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceManas] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------ [AttendancePrakash]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendancePrakash] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------[AttendanceRajesh] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceRajesh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------[AttendanceRY] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceRY] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------ [AttendanceSandila]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceSandila] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		------------------------------------ [AttendanceShailesh]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceShailesh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year


		------------------------------------ [AttendanceVaibhav]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceVaibhav] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year


		------------------------------------ [AttendanceAnand]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [hrms_karam].[dbo].[AttendanceAnand] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where month(AttendanceDate)=@Month and YEAR(AttendanceDate)=@Year

		---------------------------------------delete duplicate data--------------------------------------
		select id,tp.ATTENDANCE_DATE,tb.BIOMETRIC_CODE,STATUS_CODE,
		ROW_NUMBER() OVER
		(PARTITION BY  tp.ATTENDANCE_DATE, tp.BIOMETRIC_CODE
		ORDER BY STATUS_CODE,tp.BIOMETRIC_CODE,tp.ATTENDANCE_DATE)
		AS 'Row Number'
		into #tmp1  from (select count(1) as counts,ATTENDANCE_DATE,BIOMETRIC_CODE
		from TAB_BIOMETRIC
		where month(ATTENDANCE_DATE)=@Month and YEAR(ATTENDANCE_DATE)=@Year 
		group by BIOMETRIC_CODE,ATTENDANCE_DATE
		HAVING COUNT(1) > 1
		--order by BIOMETRIC_CODE,ATTENDANCE_DATE asc
		)
		as tp
		inner join TAB_BIOMETRIC as tb on tb.BIOMETRIC_CODE=tp.BIOMETRIC_CODE and tp.ATTENDANCE_DATE=tb.ATTENDANCE_DATE
		order by BIOMETRIC_CODE,ATTENDANCE_DATE,STATUS_CODE asc

		delete from TAB_BIOMETRIC where id in (select id from #tmp1 where [Row Number]='1')
end