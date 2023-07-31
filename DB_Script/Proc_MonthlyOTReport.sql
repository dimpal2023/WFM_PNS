create or alter procedure Proc_MonthlyOTReport
@BUILDING_ID UNIQUEIDENTIFIER=null,
@DEPT nvarchar(200)=null,
@SUB_DEPT nvarchar(200)=null,
@WF_EMP_TYPE int=null,
@EMP_NAME nvarchar(200)=null,
@FROM_DATE date=null,
@TO_DATE date=null,
@EmployeeId nvarchar(20)=null,
@OpCode int=null,
@EMP_TYPE varchar(10)=null,
@EMPLOYMENT_TYPE varchar(10)=null
as
begin
 if(@OpCode=41)
begin
		select bm.*,dm.DEPT_NAME,cm.COMPANY_NAME,sdm.SUBDEPT_NAME
		,wm.EMP_NAME,wm.EMP_ID
		,Month(bm.ATTENDANCE_DATE) as MonthNum
		,(Select DateName(month,DateAdd(month,Month(bm.ATTENDANCE_DATE),-1))) as MonthNam
		into #tmp 
		from TAB_BIOMETRIC as bm 
		inner join TAB_WORKFORCE_MASTER as wm on wm.WF_ID=bm.WORKFORCE_ID and bm.BIOMETRIC_CODE=wm.BIOMETRIC_CODE
		inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=wm.DEPT_ID
		inner join TAB_SUBDEPARTMENT_MASTER as sdm on sdm.SUBDEPT_ID=wm.SUBDEPT_ID
		left join TAB_COMPANY_MASTER as cm on cm.COMPANY_ID=wm.COMPANY_ID
		where   wm.BUILDING_ID=@BUILDING_ID
		and (isnull(@DEPT,'')='' or wm.DEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @DEPT))
		and (isnull(@SUB_DEPT,'')='' or sdm.SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @SUB_DEPT))
		and (isnull(@WF_EMP_TYPE,'0')='0' or wm.WF_EMP_TYPE=@WF_EMP_TYPE)
		and (isnull(@EMPLOYMENT_TYPE,'0')='0' or wm.EMP_TYPE_ID=@EMPLOYMENT_TYPE)
		and (isnull(@EMP_NAME,'')='' or wm.WF_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @EMP_NAME))
		--and month(bm.ATTENDANCE_DATE)=11
		and bm.ATTENDANCE_DATE between @FROM_DATE and @TO_DATE
		
		select(
		 select top 1 DEPT_NAME,COMPANY_NAME,SUBDEPT_NAME,
		  isnull((select EMP_ID,BIOMETRIC_CODE,EMP_NAME
		     ,isnull((select convert(varchar(20),ATTENDANCE_DATE,103) as ATTENDANCE_DATE
			 ,Day(ATTENDANCE_DATE) as Dates,
			 CAST(OverTime / 60 AS VARCHAR(8)) + ':' + FORMAT(OverTime % 60, 'D2') as OT
			 from #tmp as dt where dt.BIOMETRIC_CODE=emp.BIOMETRIC_CODE 
			 order by Day(ATTENDANCE_DATE) asc
			 for json path
			 ),'[]') as AttendanceList
		   from #tmp as emp group by EMP_ID,BIOMETRIC_CODE,EMP_NAME for json path),'[]') as EmpList
		 from #tmp for json path
		) as t

end
end