alter procedure Proc_GetEmployeeDetails_ShiftWise(
@DEPT varchar(50)=null,
@SUB_DEPT varchar(50)=null,
@BUILDING_ID varchar(100)=null,
@SHIFT_ID int=null
)
as 
begin

		select EMP_ID,EMP_NAME,em.EMP_TYPE as SalaryType,tp.EMP_TYPE from [dbo].[TAB_BIOMETRIC] as tb
		inner join TAB_WORKFORCE_MASTER as wm on wm.BIOMETRIC_CODE=tb.BIOMETRIC_CODE
		inner join [dbo].[TAB_EMPL_TYPE_MASTER] as em on em.EMP_TYPE_ID=wm.EMP_TYPE_ID
		inner join [dbo].[TAB_WORFORCE_TYPE] as tp on tp.[WF_EMP_TYPE]=wm.WF_EMP_TYPE
		where SHIFT_ID=@SHIFT_ID
		and convert(date, ATTENDANCE_DATE)=convert(date,GETDATE()) 
		and wm.BUILDING_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @BUILDING_ID)
		and wm.DEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @DEPT) 
		and wm.SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @SUB_DEPT)
		select @BUILDING_ID,@DEPT,@SUB_DEPT
end