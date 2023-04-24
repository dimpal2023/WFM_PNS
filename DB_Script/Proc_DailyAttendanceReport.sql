create or alter procedure Proc_DailyAttendanceReport
@BUILDING_ID UNIQUEIDENTIFIER=null,
@DEPT nvarchar(200)=null,
@SUB_DEPT nvarchar(200)=null,
@WF_EMP_TYPE int=null,
@EMP_NAME nvarchar(200)=null,
@SHIFT_ID nvarchar(20)=null,
@Date date=null,
@EmployeeId nvarchar(20)=null,
@OpCode int=null,
@EMP_TYPE varchar(10)=null,
@EMPLOYMENT_TYPE varchar(10)=null
as
begin
 if(@OpCode=41)
begin
		select bm.*,dm.DEPT_NAME,cm.COMPANY_NAME,sdm.SUBDEPT_NAME,wm.EMP_NAME,wm.EMP_ID
		,sm.SHIFT_NAME into #tmp 
		from TAB_BIOMETRIC as bm 
		inner join TAB_WORKFORCE_MASTER as wm on wm.WF_ID=bm.WORKFORCE_ID and bm.BIOMETRIC_CODE=wm.BIOMETRIC_CODE
		inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=wm.DEPT_ID
		inner join TAB_SUBDEPARTMENT_MASTER as sdm on sdm.SUBDEPT_ID=wm.SUBDEPT_ID
		left join TAB_COMPANY_MASTER as cm on cm.COMPANY_ID=wm.COMPANY_ID
		inner join TAB_SHIFT_MASTER as sm on sm.ID=bm.SHIFT_ID
		where  convert(date,ATTENDANCE_DATE)=convert(date,@Date) 
		and wm.BUILDING_ID=@BUILDING_ID
		and (isnull(@DEPT,'')='' or wm.DEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @DEPT))
		and (isnull(@SUB_DEPT,'')='' or sdm.SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @SUB_DEPT))
		and (isnull(@WF_EMP_TYPE,'0')='0' or wm.WF_EMP_TYPE=@WF_EMP_TYPE)
		and (isnull(@EMPLOYMENT_TYPE,'0')='0' or wm.EMP_TYPE_ID=@EMPLOYMENT_TYPE)
		and (ISNULL(@SHIFT_ID,'')='' or bm.SHIFT_ID=@SHIFT_ID)
		and (isnull(@EMP_NAME,'')='' or wm.WF_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @EMP_NAME))

		select(
		select top 1 DEPT_NAME,COMPANY_NAME,convert(varchar(20),ATTENDANCE_DATE,103) as ATTENDANCE_DATE,SUBDEPT_NAME
		,isnull((select EMP_ID,BIOMETRIC_CODE,EMP_NAME,SHIFT_NAME
		,case when STATUS_CODE='A' then '' else  CONVERT(VARCHAR(10), CAST(InTime AS TIME), 0) end as InTime
		,case when STATUS_CODE='A' then '' else  CONVERT(VARCHAR(10), CAST(OutTime AS TIME), 0) end as OutTime
		,isnull((select DATEDIFF(MINUTE,InTime,OutTime)),0) as Duration,
		OverTime,STATUS_CODE,Status
		from #tmp for json path ),'[]') as list
		from #tmp for json path 
		)as t

end
end