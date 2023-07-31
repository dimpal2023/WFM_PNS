create or alter procedure Proc_SalaryRevision(
@SKILL_ID varchar(100)=null,
@WF_EMP_TYPE varchar(100)=null,
@BASIC_SALARY varchar(100)=null,
@PERCENTAGE varchar(100)=null,
@WEF datetime=null,
@CalNewBasic decimal(18,2)=null,
@NewGross decimal(18,2)=null,
@OPcode int=null,
@Salary decimal(18,2)=null
)
as 
begin
if @OPcode=1
	begin
		select * from TAB_SKILL_MASTER where SKILL_ID=@SKILL_ID
	end
if @OPcode=2
	begin
	   
	    select @CalNewBasic=isnull(BASIC_SALARY,0)+ (isnull(BASIC_SALARY,0) * (CONVERT(decimal(18,2),isnull(@PERCENTAGE,0)))/100)
		from TAB_SKILL_MASTER where SKILL_ID=@SKILL_ID

		--update  TAB_SKILL_MASTER set BASIC_SALARY=@CalNewBasic where SKILL_ID=@SKILL_ID

		--insert into [dbo].[TAB_SALARY_REVISION_LOG]([OLD_SALARY],[NEW_SALARY],Percentage,[UPDATED_DATE])
		--values(@BASIC_SALARY,@CalNewBasic,@PERCENTAGE,GETDATE())

		--update TAB_WORKFORCE_SALARY_MASTER set OldSalary=BASIC_DA

		--update sm set BASIC_DA=@CalNewBasic,
		--GROSS=@CalNewBasic+ISNULL(HRA,0)+ISNULL(SPECIAL_ALLOWANCES,0)
		--,UPDATED_DATE=@WEF
		--from TAB_WORKFORCE_SALARY_MASTER as sm
		--inner join TAB_WORKFORCE_MASTER as wm on wm.WF_ID=sm.WF_ID
		--where wm.SKILL_ID=@SKILL_ID and WF_EMP_TYPE=@WF_EMP_TYPE 
	end
end