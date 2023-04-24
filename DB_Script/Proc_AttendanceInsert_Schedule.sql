create or alter procedure Proc_AttendanceInsert_Schedule
as 
begin
		delete from TAB_BIOMETRIC where convert(date,ATTENDANCE_DATE)=convert(date,GETDATE())

		------------------------------------ [AttendanceAnand] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceAnand] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------[AttendanceAsheesh] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceAsheesh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------[AttendanceAshutosh] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
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
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------ [AttendanceAtal]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceAtal] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------ [AttendanceJitendra]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceJitendra] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------ [AttendanceManas]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceManas] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------ [AttendancePrakash]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendancePrakash] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------[AttendanceRajesh] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceRajesh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------[AttendanceRY] ------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceRY] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------ [AttendanceSandila]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceSandila] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

		------------------------------------ [AttendanceShailesh]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceShailesh] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())


		------------------------------------ [AttendanceVaibhav]------------------------------------------
		INSERT INTO [dbo].[TAB_BIOMETRIC] ([ID],[WORKFORCE_ID]
		,[COMPANY_ID],[BIOMETRIC_CODE],[START_DATE],[END_DATE],[DEVICEID]
		,[ATTENDANCE_DATE],[STATUS_CODE],[SHIFT_ID]
		,[SHIFT_STARTTIME],[SHIFT_ENDTIME]
		,[Present],[Absent],[Status]
		,[OverTime],[EarlyBy],[LateBy],[InTime],[OutTime],[Duration],[PunchRecords],[INSERTED_ON])

		select [AttendanceLogId],WF_ID
		,'A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C',[EmployeeId],InTime,OutTime,InDeviceId
		,AttendanceDate,StatusCode,ShiftId
		,convert(time,InTime),CONVERT(time,OutTime)
		,Present,Absent,at1.Status
		,OverTime,EarlyBy,LateBy,InTime,OutTime,Duration,PunchRecords,GETDATE()
		from [dbo].[AttendanceVaibhav] as at1
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=at1.EmployeeId
		where convert(date,AttendanceDate)=convert(date,GETDATE())

end