create or alter procedure Proc_AssetAllocation_List
@BUILDING_ID varchar(100)=null,
@DEPT varchar(100)=null,
@SUB_DEPT varchar(100)=null,
@WF_EMP_TYPE int=null,
@OpCode  int=null,
@EMP_NAME nvarchar(100)=null
as 
begin
	if @OpCode=41
	begin

	 select (
			select wm.EMP_NAME,wm.EMP_ID,wt.EMP_TYPE as WF_Type,etm.EMP_TYPE,
			ASSET_NAME,ASSET_LIFE,wm.MOBILE_NO
			,CONVERT(varchar(20),al.ASSET_ASSIGN_DATE,103) as ASSET_ASSIGN_DATE
			,PURPOSE,ASSET_TYPE,QUANTITY,isnull(IS_REFOUND,'N') as IsReturn,ASSET_ASSIGN_BY,
			bm.BUILDING_NAME,dm.DEPT_NAME,sd.SUBDEPT_NAME
			from TAB_ASSET_ALLOCATION as al
			inner join TAB_ASSET_MASTER as am on am.ASSET_ID=al.ASSET_ID
			inner join TAB_WORKFORCE_MASTER as wm on wm.WF_ID=al.WF_ID
			inner join TAB_WORFORCE_TYPE as wt on wt.WF_EMP_TYPE=wm.WF_EMP_TYPE
			inner join TAB_EMPL_TYPE_MASTER  as etm on etm.EMP_TYPE_ID=wm.EMP_TYPE_ID
			inner join TAB_BUILDING_MASTER as bm on bm.BUILDING_ID=wm.BUILDING_ID
			inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=al.DEPT_ID
			inner join TAB_SUBDEPARTMENT_MASTER as sd on sd.SUBDEPT_ID=wm.SUBDEPT_ID
			where (wm.BUILDING_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@BUILDING_ID))
			and (isnull(@DEPT,'')='' or al.DEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@DEPT))
			and (isnull(@SUB_DEPT,'')='' or al.SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@SUB_DEPT))
			and (ISNULL(@WF_EMP_TYPE,'')='' or wm.WF_EMP_TYPE=@WF_EMP_TYPE)
			and (isnull(@EMP_NAME,'')='' or al.WF_ID=TRY_CONVERT(UNIQUEIDENTIFIER,@EMP_NAME))
			order by wm.EMP_NAME asc
	 for json path ) as list
	end

end