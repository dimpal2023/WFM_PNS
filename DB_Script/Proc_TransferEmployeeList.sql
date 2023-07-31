alter procedure Proc_TransferEmployeeList
@BUILDING_ID varchar(100)=null,
@DEPT varchar(100)=null,
@SUB_DEPT varchar(100)=null,
@Status varchar(10)=null,
@OpCode  int=null,
@TransferID varchar(max)=null,
@ApprovedBy varchar(100)=null,
@Remark varchar(max)=null,
@CurrentUser nvarchar(100)=null
as 
begin
	if @OpCode=41
	begin
	 select ( select wm.EMP_NAME+' ('+wm.EMP_ID+') ' as Workforce,et.WF_ID,et.ID,
	 case when et.IS_APPROVED=1 then 'YES' else 'NO' end as IS_APPROVED,tm.EMP_TYPE
	 ,CONVERT(varchar(20),et.REQUESTED_DATE,103) as REQUESTED_DATE
	 ,isnull(REQUESTED_BY,'') as REQUESTED_BY,isnull(APPROVED_BY,'') as APPROVED_BY
	 ,ISNULL(CONVERT(varchar(20),APPROVED_DATE,103),'') as APPROVED_DATE
	  ,bm.BUILDING_NAME as CurrentBuilding,dm.DEPT_NAME as CurrentDept
	  ,sd.SUBDEPT_NAME as CurrentSubDept,emt.EMP_TYPE as CurrentEmploymenttype

	  ,newbm.BUILDING_NAME as NewBuilding,newdm.DEPT_NAME as NewDept
	  ,newsd.SUBDEPT_NAME as NewSubDept,emt1.EMP_TYPE as NewEmployementType
	  from [dbo].[TAB_EMPLOYEE_TRANSFER]  as et
	  inner join TAB_WORKFORCE_MASTER as wm on wm.WF_ID=et.WF_ID
	  inner join TAB_WORFORCE_TYPE as tm on tm.WF_EMP_TYPE=wm.WF_EMP_TYPE
	  inner join TAB_EMPL_TYPE_MASTER as emt on emt.EMP_TYPE_ID=wm.EMP_TYPE_ID
	  inner join TAB_BUILDING_MASTER as bm on bm.BUILDING_ID=wm.BUILDING_ID
	  inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=wm.DEPT_ID
	  inner join TAB_SUBDEPARTMENT_MASTER as sd on sd.SUBDEPT_ID=wm.SUBDEPT_ID

	  inner join TAB_BUILDING_MASTER as newbm on newbm.BUILDING_ID=et.BUILDING_ID
	  inner join TAB_DEPARTMENT_MASTER as newdm on newdm.DEPT_ID=et.DEPT_ID
	  inner join TAB_SUBDEPARTMENT_MASTER as newsd on newsd.SUBDEPT_ID=et.SUB_DEPT_ID
	  inner join TAB_EMPL_TYPE_MASTER as emt1 on emt1.EMP_TYPE_ID=et.EMPLOYMENT_TYPE

	  where (isnull(@BUILDING_ID,'')='' or wm.BUILDING_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@BUILDING_ID))
	  and (isnull(@DEPT,'')='' or wm.DEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@DEPT))
	  and (isnull(@SUB_DEPT,'')='' or wm.SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@SUB_DEPT))
	  and (isnull(@Status,'')='' or et.IS_APPROVED=CAST(@Status as int))
	  and (ISNULL(@CurrentUser,'')='Admin' or et.REQUESTED_BY=@CurrentUser)
	  order by et.IS_APPROVED,ID asc
	 for json path ) as list
	end
	else if @OpCode=42
	begin
	     update TAB_EMPLOYEE_TRANSFER set IS_APPROVED=1, APPROVED_BY=@ApprovedBy,APPROVED_DATE=GETDATE(),
		 Remark=@Remark where ID in (SELECT * FROM [dbo].[fnSplitString](@TransferID, ','))

	     update wm set wm.BUILDING_ID=ET.BUILDING_ID
		 ,wm.DEPT_ID=et.DEPT_ID
		 ,wm.SUBDEPT_ID=et.SUB_DEPT_ID
		 ,wm.EMP_TYPE_ID=ET.EMPLOYMENT_TYPE from TAB_WORKFORCE_MASTER as wm 
		 inner join TAB_EMPLOYEE_TRANSFER as ET on ET.WF_ID=wm.WF_ID
		 where ID in (SELECT * FROM [dbo].[fnSplitString](@TransferID, ','))
		
	end
end