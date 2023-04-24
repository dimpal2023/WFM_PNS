use DB29_2022_WFM
go
-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\Admin\AppData\Local\Temp\~vs26FA.sql

 alter PROCEDURE [dbo].[USP_SalaryCalculation]
  (		  
          @STARTDATE_DTE date='2022-04-01',
		  @ENDDATE_DTE date='2022-04-30',
		  @BIOMETRIC_CODE_CD varchar(50)='11016'

) AS
BEGIN
declare @WF_ID uniqueidentifier
declare @EMP_ID varchar(50)
declare @BIOMETRIC_CODE varchar(50)
declare @BASIC_DA int
declare @HRA int
declare @SPECIAL_ALLOWANCES int
declare @GROSS int
declare @STARTDATE date 
declare @ENDDATE date 
declare @SalaryMonth int
declare @NumOfSundays as int
declare @NumOfdays as int
declare @NumOfworkingdays as decimal(10,2)
declare @TotalNumberofDaysInMonth as int 
declare @FinancialYearStartDate as date
declare @WorkignDaysAdjustment as int = 0

DECLARE @DATE DATETIME
SET @DATE=DateAdd(m,-1,getdate())
SELECT  @STARTDATE = CONVERT(DATE,@DATE-DAY(@DATE)+1), @ENDDATE = EOMONTH(@DATE)

Select @STARTDATE ='04/01/2022'
Select @ENDDATE = '04/30/2022'

-- to update the biometric date as first date of current month, after salary calculation it will start taking biometric of current month
update tab_bioletric_start_end_date 
set start_date=  cast('2022-04-01' as Date),
end_date= cast('2022-04-30' as Date);

--IF DATEPART(MM,@STARTDATE) >= 4
	SELECT @FinancialYearStartDate = CAST ('01/01/' + CAST(DATEPART(YYYY,@STARTDATE) as varchar) as date)
--ELSE
	--SELECT @FinancialYearStartDate = CAST ('01/01/' + CAST((DATEPART(YYYY,@STARTDATE)-1) as varchar) as date)

SELECT @SalaryMonth = DATEPART(MM,@STARTDATE)
SELECT @NumOfSundays  = DATEDIFF(ww, CAST(@STARTDATE AS datetime)-1, @ENDDATE) 
SELECT @NumOfdays  = DATEDIFF(DAY, CAST(@STARTDATE AS datetime)-1, @ENDDATE) 
SELECT @NumOfworkingdays = 26
DECLARE @WORKINGDAYS as int = @NumOfdays - @NumOfSundays

--- Condition to adjust Working days in case of 31 days in a month or 28,29 days in a month
IF (@WORKINGDAYS>26 or @WORKINGDAYS<26)
	SELECT @WorkignDaysAdjustment = @WORKINGDAYS-26

DECLARE db_cursor CURSOR FOR 
Select wm.WF_ID,BIOMETRIC_CODE,BASIC_DA,HRA,SPECIAL_ALLOWANCES,GROSS,EMP_ID from TAB_WORKFORCE_MASTER(NOLOCK) wm join TAB_WORKFORCE_SALARY_MASTER(NOLOCK) ws on wm.WF_ID = ws.WF_ID
where wm.WF_ID in (Select distinct WORKFORCE_ID from TAB_BIOMETRIC where ATTENDANCE_DATE between @STARTDATE and @ENDDATE) and EMP_TYPE_ID=1
--and wm.WF_ID='E63255F2-74AA-EB11-BF79-A45D3669901F'

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @WF_ID, @BIOMETRIC_CODE, @BASIC_DA, @HRA, @SPECIAL_ALLOWANCES, @GROSS,@EMP_ID

WHILE @@FETCH_STATUS = 0  
BEGIN

declare @dbcount as int

SELECT @dbcount = Count(1) from (SELECT DISTINCT MDBFILEID from TAB_BIOMETRIC where ATTENDANCE_DATE between @STARTDATE and @ENDDATE and BIOMETRIC_CODE=@BIOMETRIC_CODE AND ATTENDANCE_DATE between @STARTDATE and @ENDDATE) as dbcount
 
IF @dbcount = 1 
BEGIN

		declare @PerDayHour as decimal (10,2)
		declare @TotalNumberOfDaysWorked as decimal (10,2)
		declare @TotalNumberOfDaysOvertime as decimal (10,2)
		declare @TotalNumberOfHoursOvertime as decimal (10,2)

		declare @TotalNoofMinuteWorked as decimal (10,2)
		declare @TotalNoofOvertime  as decimal (10,2)
		declare @TotalNoofMinuteWorkedonW_H as decimal (10,2)
		declare @TotalGatePassOutMinutesPersonal as decimal (10,2) = 0
		declare @TotalGatePassOutMinutesOfficial  as decimal (10,2)= 0
		declare @TotalNoOfWrokingMinutesInAMonth  as decimal (10,2)

		declare @TotalLeavesInAYear as int =0
		declare @TotalLeavesInAMonth as int =0
		declare @TotalHolidayInAMonth as int=0
		declare @LeaveBalance as int = 0
		declare @LeaveDeduction int = 0
		declare @LeaveAvailable int = 0
		declare @LeaveCarryForward int = 0
		declare @LeaveAdjusted int = 0
		declare @WeekendLunchDinnerAllowance as int = 0
		declare @WeekendLunchDinnerCount as int = 0
		declare @WeekDayLunchDinnerAllowance as int = 0
		declare @WeekDayLunchDinnerCount as int = 0

		declare @LunchBreak as int  = 0
		declare @DinnerBreak as int  = 0
		declare @NoOfDaysPresent as int = 0
		declare @TempWorkignDaysAdjustment as int = @WorkignDaysAdjustment

		SELECT @PerDayHour = 8 --- Add code to pull it from Table
		---- GatePass Time
		---- Need to put 5 minutes grace period check
		--Select @TotalGatePassOutMinutesPersonal = CASE WHEN Sum(GPMin) IS NULL THEN 0 ELSE Sum(GPMin) END from (Select CASE WHEN DATEDIFF(MINUTE,ACTUAL_OUTTIME,ACTUAL_INTIME) IS NULL then 0 ELSE DATEDIFF(MINUTE,ACTUAL_OUTTIME,ACTUAL_INTIME) END as GPMin from TAB_GATEPASS(NOLOCK) where WORKFORCE_ID = @WF_ID and PURPOSE='Personal') as GatePassTime
		--- Offical Gate Pass is being added in Overtime for Lunch and Dinner
		--Select @TotalGatePassOutMinutesOfficial = CASE WHEN Sum(GPMin) IS NULL THEN 0 ELSE Sum(GPMin) END from (Select CASE WHEN DATEDIFF(MINUTE,ACTUAL_OUTTIME,ACTUAL_INTIME) IS NULL then 0 ELSE DATEDIFF(MINUTE,ACTUAL_OUTTIME,ACTUAL_INTIME) END as GPMin from TAB_GATEPASS(NOLOCK) where WORKFORCE_ID = @WF_ID and PURPOSE='Official') as GatePassTime
		---- Daily Attendance
		--Select @TotalNoofMinuteWorked = CASE WHEN sum(MinuteWorked) IS NULL THEN 0 ELSE sum(MinuteWorked) END, @TotalNoofOvertime = CASE WHEN sum(Overtime) IS NULL THEN 0 ELSE sum(Overtime) END from (Select Case when DATEDIFF(MINUTE,START_DATE,END_DATE) > 510 then 510 else DATEDIFF(MINUTE,START_DATE,END_DATE) end as MinuteWorked,Case when (DATEDIFF(MINUTE,START_DATE,END_DATE) - 510)>30 then DATEDIFF(MINUTE,START_DATE,END_DATE) - 510 else 0 end as Overtime from TAB_BIOMETRIC where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE in ('P','폩')) as HoursWorked
		
		
		
		--Select @TotalNoofMinuteWorked = WorkInMinutes,@TotalNoofOvertime = OverTimeInMinutes,@NoOfDaysPresent = NoOfDaysPresent   from dbo.CalculateWorkingAndOverTimeMinutes(@BIOMETRIC_CODE,@STARTDATE,@ENDDATE)

		Select @TotalNoofMinuteWorked = 1,@TotalNoofOvertime = 1,@NoOfDaysPresent = 24






		----- Lunch and Dinner Break -------------

		------ Select @LunchBreak = CASE WHEN LunchBreakInMinutes is NULL THEN 0 ELSE LunchBreakInMinutes END ,@DinnerBreak = CASE WHEN DinnerBreakInMinutes is NULL THEN 0 ELSE DinnerBreakInMinutes END from dbo.CalculateWorkingAndOverTimeGatePassMinutes(@BIOMETRIC_CODE,@STARTDATE,@ENDDATE)

		---- Lunch/Dinner Allowance on Week days (Overtime)





		--Select @WeekDayLunchDinnerAllowance = Allowance,@WeekDayLunchDinnerCount = DayCount from dbo.CalculateWeekdaysAllowance(@BIOMETRIC_CODE,@STARTDATE,@ENDDATE)
		Select @WeekDayLunchDinnerAllowance = 1,@WeekDayLunchDinnerCount = 1





		---- Attendance on Holiday and Weekoff days (Overtime)
		Select @TotalNoofMinuteWorkedonW_H = CASE WHEN sum(MinuteWorked) IS NULL THEN 0 ELSE sum(MinuteWorked) END from (Select DATEDIFF(MINUTE,START_DATE,END_DATE) as MinuteWorked
		from TAB_BIOMETRIC(NOLOCK) 
		where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE in ('HP','WOP','WO폩','H폩')) as WHOvertime

		---- Lunch/Dinner Allowance on Holiday and Weekoff days (Overtime)
		Select @WeekendLunchDinnerAllowance = CASE WHEN sum(Lunch) is NULL THEN 0 ELSE sum(Lunch) END,@WeekendLunchDinnerCount = count(Lunch) from (Select CASE WHEN ROUND(CAST((CASE WHEN MinuteWorked IS NULL THEN 0 ELSE MinuteWorked END) as decimal (10,0))/60,2) >=7.5 THEN 85 ELSE 0 END as Lunch from (Select DATEDIFF(MINUTE,START_DATE,END_DATE) as MinuteWorked from TAB_BIOMETRIC(NOLOCK) 
		where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE in ('HP','WOP','WO폩','H폩')) as WHOvertime) as abc

		---- No. of Holidays
		Select @TotalHolidayInAMonth = count(1) from(select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE in ('H','HP','H폩')) as holidays
		--- Leaves Calculation -
		

		Select @TotalLeavesInAMonth = count(1) from (Select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE='A') as LeavesInAMonth
		
		--  Add Logic to pull Leave Balance data provided by Vaibhav for June Year 2021
		IF DATEPART(YYYY,@FinancialYearStartDate) > 2021
			BEGIN
				Select @TotalLeavesInAYear = count(1) from (Select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and STATUS_CODE='A' and ATTENDANCE_DATE>=@FinancialYearStartDate and ATTENDANCE_DATE < @STARTDATE) as whoeyearleaves
				Select @LeaveCarryForward = (DATEDIFF(MM,@FinancialYearStartDate,@STARTDATE)) - @TotalLeavesInAYear
			END
		ELSE
			BEGIN
			------- Conditional Query to pull Leaves from July 2021 onwards
			--Nishant,commented neeraj	Select @TotalLeavesInAYear = count(1) from (Select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and STATUS_CODE='A' and ATTENDANCE_DATE>=@FinancialYearStartDate and ATTENDANCE_DATE < @STARTDATE and ATTENDANCE_DATE > '06/30/2021') as whoeyearleaves
				Select @TotalLeavesInAYear = count(1) from (Select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and STATUS_CODE='A' and ATTENDANCE_DATE>=@FinancialYearStartDate and ATTENDANCE_DATE < @STARTDATE and ATTENDANCE_DATE > '08/31/2021') as whoeyearleaves
				Select @LeaveCarryForward = LeaveCarryForward from TAB_LEAVE_CARRY_FWD where EmpCode=@EMP_ID
			--Nishant,commented neeraj		Select @LeaveCarryForward = (@LeaveCarryForward + (DATEDIFF(MM,'07/01/2021',@STARTDATE))) - @TotalLeavesInAYear
				Select @LeaveCarryForward = (@LeaveCarryForward + (DATEDIFF(MM,'09/01/2021',@STARTDATE))) - @TotalLeavesInAYear
			END

			IF @LeaveCarryForward>=0
				SELECT @LeaveAvailable= @LeaveCarryForward + 1 --Current Month Leave
			ELSE
				BEGIN
					SELECT @LeaveAvailable = 1  --Current Month Leave
					SELECT @LeaveCarryForward = 0
				END
	
			IF (@LeaveAvailable - @TotalLeavesInAMonth) >= 0
				BEGIN
					SELECT @LeaveDeduction = 0
					SELECT @LeaveAdjusted = @TotalLeavesInAMonth
				END
			ELSE
				BEGIN
					SELECT @LeaveDeduction = @TotalLeavesInAMonth - @LeaveAvailable
					SELECT @LeaveAdjusted = @LeaveAvailable
				END

			IF @LeaveAvailable<@TotalLeavesInAMonth
				SELECT @LeaveBalance = 0
			ELSE
				SELECT @LeaveBalance = @LeaveAvailable - @TotalLeavesInAMonth

		--- Final Minutes Caluclation for Salary

		SELECT @TotalNoofMinuteWorked = @TotalNoofMinuteWorked + @LunchBreak - @TotalGatePassOutMinutesPersonal
		SELECT @TotalNoofOvertime = @TotalNoofOvertime + @DinnerBreak + @TotalNoofMinuteWorkedonW_H + @TotalGatePassOutMinutesOfficial

		DECLARE @DaysNeedsToBeDeducted decimal(10,2) = 0 

		SELECT @DaysNeedsToBeDeducted = ((@NoOfDaysPresent * 510.00 ) - @TotalNoofMinuteWorked )
		---SELECT @TotalNumberOfDaysWorked =(((@NoOfDaysPresent * 510.00) - @DaysNeedsToBeDeducted) / 60.00) / 8.5
		SELECT @TotalNumberOfDaysWorked = @NoOfDaysPresent - (@DaysNeedsToBeDeducted / 60.00 / @PerDayHour)

		IF @NoOfDaysPresent>0 
		BEGIN
			--- Condition to not to adjust Working days in case of 31 days in a month or 28,29 days in a month
			IF (@TotalNumberOfDaysWorked<15)
				SELECT @TempWorkignDaysAdjustment = 0


			SELECT @TotalNumberOfDaysOvertime = (@TotalNoofOvertime / 60) / @PerDayHour
			SELECT @TotalNumberOfHoursOvertime  = @TotalNoofOvertime / 60

			--- Adding Leave if Leave is from Leave Balance
			IF @LeaveDeduction = 0 
				SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked + @TotalLeavesInAMonth
			ELSE
				SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked + @LeaveAvailable

			--- Adding Holiday
				SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked + @TotalHolidayInAMonth
			--- WorkingDays Adjustment
				
				SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked - @TempWorkignDaysAdjustment 
				

			-----Salary Calculation

				declare @WorkingDaySalary as decimal (10,2)
				declare @OvertimeSalary as decimal (10,2)
				declare @TotalSalary as decimal (10,2)
				declare @Actual_BASIC_DA as decimal (10,2)
				declare @Actual_HRA as decimal (10,2)
				declare @Actual_SPECIAL_ALLOWANCE as decimal (10,2)
				declare @TOTAL_WAGES_AFTER_DEDUCTION as decimal (10,2)
				declare @EarnedSalaryForEPF as decimal (10,2)
				declare @EarnedSalaryForESI as decimal (10,2)
				declare @PF as decimal (10,2)
				declare @EPF as decimal (10,2)
				declare @EmployeeESI as decimal (10,2)
				declare @EmployerESI as decimal (10,2)
				declare @AdminCharges as decimal (10,2)
				declare @EDLICharges as decimal (10,2)
				declare @PerDayBASIC_DA as decimal (10,2)
				declare @PerDayHRA as decimal (10,2)
				declare @PerDaySPECIAL_ALLOWANCES as decimal (10,2)


				SELECT @PerDayBASIC_DA = @BASIC_DA / @NumOfworkingdays
				SELECT @PerDayHRA =  @HRA / @NumOfworkingdays
				SELECT @PerDaySPECIAL_ALLOWANCES =   @SPECIAL_ALLOWANCES / @NumOfworkingdays

				SELECT @Actual_BASIC_DA = ROUND(@PerDayBASIC_DA * @TotalNumberOfDaysWorked,0)
				SELECT @Actual_HRA = ROUND(@PerDayHRA * @TotalNumberOfDaysWorked,0)
				SELECT @Actual_SPECIAL_ALLOWANCE = ROUND(@PerDaySPECIAL_ALLOWANCES * @TotalNumberOfDaysWorked,0)

				SELECT @WorkingDaySalary = @Actual_BASIC_DA + @Actual_HRA + @Actual_SPECIAL_ALLOWANCE
				--SELECT @OvertimeSalary = ROUND((@PerDayBASIC_DA + @PerDayHRA + @PerDaySPECIAL_ALLOWANCES) * @TotalNumberOfDaysOvertime,0)
				SELECT @OvertimeSalary = ROUND(((@PerDayBASIC_DA + @PerDayHRA + @PerDaySPECIAL_ALLOWANCES)/8) * @TotalNumberOfHoursOvertime,0)
				SELECT @TotalSalary = @WorkingDaySalary


	
				-------- Source is not define for the below mentioned Perticulars --------
				declare @PRODUCTION_INCENTIVE_BONUS as decimal (10,2)
				declare @TDS as decimal (10,2)
				declare @Advance as decimal (10,2)
				declare @ShopFloorFine as decimal (10,2)
				declare @OtherDeduction as decimal (10,2)

				SELECT @PRODUCTION_INCENTIVE_BONUS = @WeekendLunchDinnerAllowance + @WeekDayLunchDinnerAllowance + @OvertimeSalary
				SELECT @TDS = 0
				SELECT @Advance = 0
				SELECT @ShopFloorFine = 0 
				SELECT @OtherDeduction = 0
				------------------------------------------------

				SELECT @EarnedSalaryForEPF = CASE WHEN (@BASIC_DA + @SPECIAL_ALLOWANCES) > 15000 THEN 15000 ELSE ((@BASIC_DA + @SPECIAL_ALLOWANCES)/@NumOfworkingdays)*@TotalNumberOfDaysWorked END
				SELECT @EarnedSalaryForESI = CASE WHEN @WorkingDaySalary+@PRODUCTION_INCENTIVE_BONUS > 21000 THEN 21000 ELSE @WorkingDaySalary+@PRODUCTION_INCENTIVE_BONUS END

				SELECT @PF = Round((ROUND(@EarnedSalaryForEPF,0) * 12)/100,0)
				SELECT @EmployeeESI = ROUND((ROUND(@EarnedSalaryForESI,0) * 0.75)/100,0)

				SELECT @TOTAL_WAGES_AFTER_DEDUCTION= (@TotalSalary + @PRODUCTION_INCENTIVE_BONUS) - (@PF + @EmployeeESI + @TDS + @Advance + @ShopFloorFine)

				SELECT @EPF = @PF
				SELECT @EmployerESI = (ROUND(@EarnedSalaryForESI,0) * 3.25) / 100
				SELECT @AdminCharges = (ROUND(@EarnedSalaryForEPF,0) * 0.5)/100
				SELECT @EDLICharges = (ROUND(@EarnedSalaryForEPF,0) * 0.5)/100

				Print 'WF ID ' + CAST(@WF_ID as varchar(50)) + '   Actual Wage  : ' + CAST(@TOTAL_WAGES_AFTER_DEDUCTION as varchar)
			--IF @NoOfDaysPresent> 0 
				BEGIN
					INSERT INTO [dbo].[TAB_WORKFORCE_SALARY]
					   (ID,
					   [WF_ID]
					   ,[SALARY_MONTH]
					   ,[STARTDATE]
					   ,[ENDDATE]
					   ,[PAID_DAYS]
					   ,[OVERTIME_HOURS]
					   ,TOTAL_LEAVE_TAKEN_CURRENT_MONTH
					   ,LEAVE_CARRY_FORWARD
					   ,TOTAL_LEAVE_AVAILABLE
					   ,LEAVE_ADJUSTED
					   ,LEAVE_BALANCE
					   ,LEAVE_DEDUCTION
					   ,[BASIC_DA]
					   ,[HRA]
					   ,[SPECIAL_ALLOWANCES]
					   ,[ACTUAL_BASIC_DA]	
					   ,[ACTUAL_HRA]
					   ,[ACTUAL_SPECIAL_ALLOWANCES]
					   ,[PRODUCTION_INCENTIVE_BONUS]
					   ,[PF]
					   ,[ESI]
					   ,[TDS]
					   ,[SHOP_FLOOR_FINE]
					   ,[OTHER_DEDUCTION]
					   ,[ADVANCE]
					   ,[OVERTIME_WAGES]
					   ,[WORKINGDAY_WAGES]
					   ,[TOTAL_WAGES]
					   ,[TOTAL_WAGES_AFTER_DEDUCTION]
					   ,[EMPLOYER_EPF]
					   ,[EMPLOYER_ESI]
					   ,[ADMIN_CHARGES]
					   ,[EDLI_CHARGES]
					   ,[MODE_OF_PAYMENT]
					   ,[PAID_STATUS],
					    CREATED_ON
					   )
				 VALUES
					   (NEWID(),
					   @WF_ID
					   ,@SalaryMonth
					   ,@STARTDATE
					   ,@ENDDATE
					   ,@TotalNumberOfDaysWorked
					   ,@TotalNumberOfHoursOvertime
					   ,@TotalLeavesInAMonth
					   ,@LeaveCarryForward
					   ,@LeaveAvailable
					   ,@LeaveAdjusted
					   ,@LeaveBalance
					   ,@LeaveDeduction
					   ,@BASIC_DA
					   ,@HRA
					   ,@SPECIAL_ALLOWANCES
					   ,@Actual_BASIC_DA
					   ,@Actual_HRA
					   ,@Actual_SPECIAL_ALLOWANCE
					   ,@PRODUCTION_INCENTIVE_BONUS
					   ,@PF
					   ,@EmployeeESI
					   ,@TDS
					   ,@ShopFloorFine
					   ,@OtherDeduction
					   ,@Advance
					   ,@OvertimeSalary
					   ,@WorkingDaySalary
					   ,@TotalSalary
					   ,@TOTAL_WAGES_AFTER_DEDUCTION
					   ,@PF
					   ,@EmployerESI
					   ,@AdminCharges
					   ,@EDLICharges
					   ,''
					   ,'UnPaid',
					   GETDATE())
				END
			END
	END
ELSE
	BEGIN
			declare @dbname varchar(1000)
			
			SELECT @dbname = COALESCE(@dbname + ',','') + MDBFILENAME FROM TAB_BIOMETRIC_DATABASE 
			where ID in (SELECT DISTINCT MDBFILEID from TAB_BIOMETRIC where ATTENDANCE_DATE between @STARTDATE and @ENDDATE and BIOMETRIC_CODE=@BIOMETRIC_CODE)
			----------- Add record with Multiple entry of a BioMetric code in Multiple DB files
			Insert into TAB_FAULTY_PUNCHES (BIOMETRIC_CODE,ATTENDANCE_DATE,PUNCH_RECORD,STATUS,REMARKS) values (@BIOMETRIC_CODE,getdate(),@dbname,'','BioMetric in multiple DB files.')
			SELECT @dbname = ''
	END
		FETCH NEXT FROM db_cursor INTO @WF_ID, @BIOMETRIC_CODE, @BASIC_DA, @HRA, @SPECIAL_ALLOWANCES, @GROSS,@EMP_ID
								
 
END
CLOSE db_cursor  
DEALLOCATE db_cursor 

END
GO


