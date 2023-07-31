alter procedure Proc_MasterSalarySheet
@BUILDING_ID UNIQUEIDENTIFIER=null,
@DEPT nvarchar(500)=null,
@SUB_DEPT nvarchar(500)=null,
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
if(@OpCode=41)
begin
		select(select COMPANY_NAME,ADDRESS1,
			isnull((select distinct EMP_NAME,EMP_ID,dm.DEPT_NAME as Department,BIOMETRIC_CODE,COMPANY_NAME,ADDRESS1
			,EMP_NAME+' ('+EMP_ID+') ' as ForSupplierEntry
			,scm.CITY_NAME as SupplierSite,SUBDEPT_NAME as SubDepartment
			,isnull(CONVERT(varchar,sal.ESIC_NO),'N/A') as ESIC_NO
			,isnull(sal.UAN_NO,'N/A') as UAN_NO
			,isnull(sal.BANK_ACCOUNT_NO,'N/A') as AccountNo
			,isnull(CONVERT(decimal(18,0),wsm.BASIC_DA+wsm.HRA+wsm.SPECIAL_ALLOWANCES),'0') as GROSS
			,isnull(CONVERT(decimal(18,0),wsm.BASIC_DA),'0') as BASIC_DA
			,isnull(CONVERT(decimal(18,0),wsm.HRA),'0') as HRA,
			isnull(CONVERT(decimal(18,0),wsm.SPECIAL_ALLOWANCES),'0') as spclAllow,
			isnull(CONVERT(decimal(18,0),wsm.BASIC_DA+wsm.HRA+wsm.SPECIAL_ALLOWANCES),'0') as Total,
			isnull(CONVERT(decimal(18,0),wsm.PRODUCTION_INCENTIVE_BONUS),'0') as ExtraEarnSalary,
			isnull(CONVERT(decimal(18,0),wsm.PRODUCTION_INCENTIVE_BONUS),'0') as PRODUCTION_INCENTIVE_BONUS,
			isnull(CONVERT(varchar,wsm.OVERTIME_HOURS),'0') as OTHours,
			ISNULL(CONVERT(decimal(18,0),wsm.PR_AMOUNT),0) as PRPayableAmount,
			
			isnull(wsm.NoofPresentDays,'0') as PaidDays,
			isnull(wsm.PAID_DAYS,'0') as PaidDay,
			'0' as ActualPresentdaysforPRWorkers
			,et.EMP_TYPE as NEType,
			--isnull(CONVERT(varchar,wsm.OVERTIME_HOURS),'0') as OTHour,
			isnull(CONVERT(varchar,wsm.TOTAL_LEAVE_TAKEN_CURRENT_MONTH),'0') as TotalLeaveTakenMonthWise,
			isnull(CONVERT(varchar,wsm.LEAVE_CARRY_FORWARD),'0') as LeaveCarryforwarded,
			'1' as LeaveAllowed,
			isnull(CONVERT(varchar,wsm.TOTAL_LEAVE_AVAILABLE),'0') as TotalAvailableLeavetillMonth,
			isnull(CONVERT(varchar,wsm.LEAVE_ADJUSTED),'0') as LeaveAdjustedinMonth,
			isnull(CONVERT(varchar,wsm.LEAVE_BALANCE),'0') as EW_TotalLeavebalanceMonth,
			isnull(CONVERT(decimal(18,0),(wsm.ACTUAL_BASIC_DA)),'0') as EW_BasicDA,
			isnull(CONVERT(decimal(18,0),(wsm.ACTUAL_HRA)),'0') as EW_HRA,
			isnull(CONVERT(decimal(18,0),(wsm.ACTUAL_SPECIAL_ALLOWANCES)),'0') as EW_SplAllow,
			isnull(CONVERT(decimal(18,0),wsm.PRODUCTION_INCENTIVE_BONUS),'0') as EW_ProductionIncentiveBonus,
			isnull(CONVERT(decimal(18,0),wsm.OTHER_DEDUCTION),'0') as OtherAllowance,
			isnull(CONVERT(decimal(18,0),wsm.DinnerAllowance),'0') as DinnerAllowance,
			isnull(CONVERT(decimal(18,0),wsm.LunchAllowance),'0') as LunchAllowance
			,isnull(CONVERT(decimal(18,0),wsm.SHOP_FLOOR_FINE),'0') as SHOPFLOORFINE
			,ISNULL(CONVERT(decimal(18,0),wsm.PRODUCTION_INCENTIVE_BONUS),0) as DeductionTotal1
			,ISNULL(CONVERT(decimal(18,0),wsm.EarnedTotal),0) as DeductionTotal2
			,isnull(CONVERT(decimal(18,0),wsm.PF),'0') as PF
			,isnull(CONVERT(decimal(18,0),wsm.ESI),'0') as ESI,
			isnull(CONVERT(decimal(18,0),wsm.TDS),'0') as TDS
			,isnull(CONVERT(decimal(18,0),wsm.TOTAL_WAGES_AFTER_DEDUCTION),'0') as ActualWagesPaid,
			isnull(CONVERT(decimal(18,0),wsm.ADVANCE),'0') as ADVANCE,
			isnull(CONVERT(decimal(18,0),wsm.EMPLOYER_EPF),'0')  as EmpEPF
			,isnull(CONVERT(decimal(18,0),wsm.EMPLOYER_ESI),'0') as EmpESI
			,isnull(CONVERT(decimal(18,2),ADMIN_CHARGES),'0') as AdminCharges
			,isnull(CONVERT(decimal(18,2),EDLI_CHARGES),'0') as EDLICharges
			,isnull([Permission],'0') as Permission
			from TAB_WORKFORCE_MASTER as wf
			inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_ID=wf.DEPT_ID
			inner join TAB_SUBDEPARTMENT_MASTER as sm on sm.SUBDEPT_ID=wf.SUBDEPT_ID
			inner join TAB_STATE_CITY_MASTER as scm on scm.CITY_ID=wf.PERMANENT_ADDRESS_CITY
			inner join TAB_EMPL_TYPE_MASTER as et on et.EMP_TYPE_ID=wf.EMP_TYPE_ID
			inner join [dbo].[TAB_WORKFORCE_SALARY_MASTER] as sal on sal.WF_ID=wf.WF_ID
			inner join [dbo].[TAB_WORKFORCE_SALARY] as wsm on wsm.WF_ID=wf.WF_ID 
			where wf.BUILDING_ID=@BUILDING_ID 
			and SALARY_MONTH=@MONTH_ID
			and YEAR(EndDate)=@YEAR_ID
			and (isnull(@DEPT,'')='' or wf.DEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @DEPT))
			and (isnull(@SUB_DEPT,'')='' or wf.SUBDEPT_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @SUB_DEPT))
			and wf.WF_EMP_TYPE=@WF_EMP_TYPE
			AND (WF.EMP_TYPE_ID=@EMPLOYMENT_TYPE OR @EMPLOYMENT_TYPE=3)
			and (isnull(@EMP_NAME,'')='' or wf.WF_ID=TRY_CONVERT(UNIQUEIDENTIFIER, @EMP_NAME))
			and isnull(PAID_DAYS,'0')>=0
			--and wf.EMP_ID='PNIW0000055'
			group by EMP_NAME,dm.DEPT_NAME,wf.BIOMETRIC_CODE,EMP_ID,CITY_NAME,SUBDEPT_NAME
			,ESI,UAN_NO,BANK_ACCOUNT_NO,GROSS, wsm.BASIC_DA,wsm.HRA,wsm.SPECIAL_ALLOWANCES,et.EMP_TYPE,
			wsm.ADMIN_CHARGES,wsm.EDLI_CHARGES,EDLI_CHARGES,EMPLOYER_ESI,EMPLOYER_EPF,ADVANCE,sal.ESIC_NO,
			wsm.TDS,PF,wsm.SHOP_FLOOR_FINE,wsm.PAID_DAYS,wsm.OVERTIME_HOURS,wsm.TOTAL_LEAVE_TAKEN_CURRENT_MONTH
			,wsm.LEAVE_CARRY_FORWARD,wsm.TOTAL_LEAVE_AVAILABLE,wsm.LEAVE_ADJUSTED,wsm.LEAVE_BALANCE
			,wsm.PRODUCTION_INCENTIVE_BONUS,OVERTIME_WAGES,wsm.ACTUAL_BASIC_DA,wsm.ACTUAL_HRA,wsm.ACTUAL_SPECIAL_ALLOWANCES
			,sal.BASIC_DA,sal.HRA,sal.SPECIAL_ALLOWANCES,wsm.TOTAL_WAGES_AFTER_DEDUCTION,wsm.NoofPresentDays--,wsm.PR_AMOUNT
			,wsm.LunchAllowance,Permission,PR_AMOUNT,wsm.EarnedTotal,wsm.OTHER_DEDUCTION,wsm.DinnerAllowance
			order by wf.EMP_ID asc
			for json path
			),'[]')as list
		from TAB_COMPANY_MASTER where COMPANY_NAME='PNI'
		for json path)as Details

end
end