alter procedure Proc_MonthlyAttendanceReport
@BUILDING_ID UNIQUEIDENTIFIER=null,
@DEPT UNIQUEIDENTIFIER=null,
@SUB_DEPT nvarchar(200)=null,
@WF_EMP_TYPE int=null,
@EMP_NAME nvarchar(200)=null,
@MONTH_ID int=null,
@YEAR_ID int=null,
@EmployeeId nvarchar(20)=null,
@OpCode int=null,
@EMP_TYPE int=null,
@EMPLOYMENT_TYPE int=null
as
begin
 if(@OpCode=42)
begin
		select bm.* into #tmp 
		from TAB_BIOMETRIC as bm 
		inner join TAB_WORKFORCE_MASTER as wm on wm.WF_ID=bm.WORKFORCE_ID and bm.BIOMETRIC_CODE=wm.BIOMETRIC_CODE
		inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=wm.DEPT_ID
		where  MONTH(ATTENDANCE_DATE)=@MONTH_ID and YEAR(ATTENDANCE_DATE)=@YEAR_ID
		and wm.BUILDING_ID=@BUILDING_ID
		and wm.DEPT_ID=@DEPT
		and (isnull(@SUB_DEPT,'')='' or SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @SUB_DEPT))
		and wm.WF_EMP_TYPE=@WF_EMP_TYPE

		select(
		select dm.DEPT_NAME,BIOMETRIC_CODE,COMPANY_NAME,EMP_NAME+' - '+EMP_ID as EMP_NAME
		,(select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and Present in ('1')
		AND STATUS_CODE in ('P')) as Present
		,(select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and Absent in ('1')) as Absent

		,(select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and STATUS_CODE in ('WO','WOP','WO폩')
		) as WeeklyOff

		,(select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and STATUS_CODE in ('H','HP','H폩')
		) as Holiday

		,(select sum(cast(OverTime  as int))
		from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE
		 AND STATUS_CODE in ('P','폩','HP','WOP','WO폩','H폩')) as OTHours,

		--(Select CASE WHEN sum(MinuteWorked) IS NULL THEN 0 ELSE sum(MinuteWorked) END from (Select DATEDIFF(MINUTE,START_DATE,END_DATE) as MinuteWorked
		--from TAB_BIOMETRIC(NOLOCK) 
		--where BIOMETRIC_CODE = wf.BIOMETRIC_CODE and MONTH(ATTENDANCE_DATE)=@MONTH_ID and YEAR(ATTENDANCE_DATE)=@YEAR_ID
		--and STATUS_CODE in ('HP','WOP','WO폩','H폩')) as WHOvertime
  --      ) as WHOvertime,

       (select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and Present in ('1')
		 and EarlyBy>0) as EarlyByDay

		,(select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and Present in ('1')
		and LateBy>0) as LateByDay

		,(select count(1) from #tmp where BIOMETRIC_CODE=wf.BIOMETRIC_CODE and Present in ('1')
		and LateBy>0) as sd

		
		,isnull((select STATUS_CODE as StatusCode,ATTENDANCE_DATE as AttendanceDate,InTime,OutTime,BIOMETRIC_CODE as EmployeeId
		   ,Day(ATTENDANCE_DATE) as Date, YEAR(ATTENDANCE_DATE) as Year,MONTH(ATTENDANCE_DATE) AS Month,
			CONVERT(varchar(5),CONVERT(datetime,InTime),108) as SHIFT_STARTTIME,
			CONVERT(varchar(5),CONVERT(datetime,OutTime),108) as SHIFT_ENDTIME,
			CAST(Duration / 60 AS VARCHAR(8)) + ':' + FORMAT(Duration % 60, 'D2') AS Duration,
			CAST(EarlyBy / 60 AS VARCHAR(8)) + ':' + FORMAT(EarlyBy % 60, 'D2') as EarlyBy,
			CAST(LateBy / 60 AS VARCHAR(8)) + ':' + FORMAT(LateBy % 60, 'D2') as LateBy,
			CAST(OverTime / 60 AS VARCHAR(8)) + ':' + FORMAT(OverTime % 60, 'D2') as OT,
			sm.SHIFT_CODE as shifts
			,PunchRecords as Permit
			,(select Permission from [dbo].[fn_CalPermission](InTime,OutTime,BreakStartTime,BreakEndTime,OverTime,EarlyBy,LateBy,PunchRecords)) as Permission
			,CONVERT(varchar(5),sm.SHIFT_START_TIME) as ShiftStart,
	        CONVERT(varchar(5),sm.SHIFT_END_TIME) as ShiftEnd
			,(cast(EarlyBy as int)+cast(LateBy as int)+cast(DATEDIFF(MINUTE, InTime, OutTime) as int)) as diffminute,
			(DATEDIFF(MINUTE, InTime, OutTime)) as WorkDuration,
			isnull((select * from splitPuchRecords(PunchRecords,InTime,OutTime) order by SquenceId asc for json path),'[]') as punchrecord
			
			from #tmp as bm 
			inner join TAB_SHIFT_MASTER as sm on sm.ID=bm.SHIFT_ID
			where BIOMETRIC_CODE=wf.BIOMETRIC_CODE  
			order by ATTENDANCE_DATE asc
		for json path),'[]') as AttendanceSheet

		from TAB_WORKFORCE_MASTER as wf
		inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=wf.DEPT_ID
		inner join TAB_COMPANY_MASTER as cm on cm.COMPANY_ID=wf.COMPANY_ID
		where wf.BUILDING_ID=@BUILDING_ID
		and wf.DEPT_ID=@DEPT
		and (isnull(@SUB_DEPT,'')='' or SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @SUB_DEPT))
		and wf.WF_EMP_TYPE=@WF_EMP_TYPE
		AND (WF.EMP_TYPE_ID=@EMPLOYMENT_TYPE OR @EMPLOYMENT_TYPE=3)
		and (isnull(@EMP_NAME,'')='' or wf.WF_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @EMP_NAME))
		group by EMP_NAME,dm.DEPT_NAME,wf.BIOMETRIC_CODE,EMP_ID,COMPANY_NAME
		for json path)as list

end
end