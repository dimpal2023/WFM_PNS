

	ALTER PROCEDURE [dbo].[USP_SalaryCalculation_OP]
	(		  
		@STARTDATE_DTE date=null,
		@ENDDATE_DTE date=null,
		@BIOMETRIC_CODE_CD varchar(50)= null
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

	Select @STARTDATE ='10/01/2022'
	Select @ENDDATE = '10/31/2022'

	-- to update the biometric date as first date of current month, after salary calculation it will start taking biometric of current month
	update tab_bioletric_start_end_date 
	set start_date=  cast(DATEADD(DD,-(DAY(GETDATE() -1)), GETDATE()) as Date),
	end_date=cast(DATEADD(DD,-(DAY(GETDATE())), DATEADD(MM, 1, GETDATE())) as Date);

	SELECT @FinancialYearStartDate = CAST ('01/01/' + CAST(DATEPART(YYYY,@STARTDATE) as varchar) as date)
	

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
	where wm.WF_ID in (Select distinct WORKFORCE_ID from TAB_BIOMETRIC where ATTENDANCE_DATE between @STARTDATE and @ENDDATE)
	and wm.STATUS='Y' --and WF_EMP_TYPE=2 
	 and WF_EMP_TYPE=1 and EMP_TYPE_ID=1 and DEPT_ID='6f0877e4-658a-eb11-bf75-a45d3669901f'
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

		Select @TotalNoofMinuteWorked = WorkInMinutes,@NoOfDaysPresent = NoOfDaysPresent 
		from dbo.CalculateWorkingAndOverTimeMinutes(@BIOMETRIC_CODE,@STARTDATE,@ENDDATE)

		    
		select @TotalNoofOvertime=(select sum(CAST(OverTime as int)) from TAB_BIOMETRIC where BIOMETRIC_CODE= @BIOMETRIC_CODE 
		AND ATTENDANCE_DATE between @STARTDATE and @ENDDATE AND STATUS_CODE in ('P','폩','HP','WOP','WO폩','H폩'))

		

		Select @WeekDayLunchDinnerAllowance = Allowance,@WeekDayLunchDinnerCount = DayCount from dbo.CalculateWeekdaysAllowance(@BIOMETRIC_CODE,@STARTDATE,@ENDDATE)

		Select @WeekendLunchDinnerAllowance = CASE WHEN sum(Lunch) is NULL THEN 0 ELSE sum(Lunch) END,
		@WeekendLunchDinnerCount = count(Lunch) 
		from (Select CASE WHEN ROUND(CAST((CASE WHEN MinuteWorked IS NULL THEN 0 ELSE MinuteWorked END) as decimal (10,0))/60,2) >=7.5 THEN 105 ELSE 0 END as Lunch from (Select DATEDIFF(MINUTE,START_DATE,END_DATE) as MinuteWorked from TAB_BIOMETRIC(NOLOCK) 
		where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE in ('HP','WOP','WO폩','H폩')) as WHOvertime) as abc

		---- No. of Holidays

		Select @TotalHolidayInAMonth = count(1) from(select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and STATUS_CODE in ('H','HP','H폩')) as holidays
			
		--- Leaves Calculation -
		
		Select @TotalLeavesInAMonth = count(1) from (Select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and ATTENDANCE_DATE between @STARTDATE and @ENDDATE and Absent in ('1')) as LeavesInAMonth
		
		--  Add Logic to pull Leave Balance data provided by Vaibhav for June Year 2021

		IF DATEPART(YYYY,@FinancialYearStartDate) > 2021
		BEGIN
			Select @TotalLeavesInAYear = count(1) from (Select Distinct ATTENDANCE_DATE from TAB_BIOMETRIC(NOLOCK) where WORKFORCE_ID = @WF_ID and Absent in ('1') and ATTENDANCE_DATE>=@FinancialYearStartDate and ATTENDANCE_DATE < @STARTDATE) as whoeyearleaves
			--Select @LeaveCarryForward = @TotalLeavesInAYear-(DATEDIFF(MM,@FinancialYearStartDate,@STARTDATE))
			--Select @LeaveCarryForward =LEAVE_BALANCE from TAB_WORKFORCE_SALARY where SALARY_MONTH=(MONTH(@STARTDATE)-1) and WF_ID= @WF_ID --and YEAR(STARTDATE)=(YEAR(@STARTDATE))
			Select @LeaveCarryForward =LeaveBalance from [dbo].[Test_CalLeave] where [Month]=(MONTH(@STARTDATE)-1) and WF_ID = @WF_ID --and YEAR(STARTDATE)=(YEAR(@STARTDATE))
			select @TotalLeavesInAYear,@LeaveCarryForward,@TotalLeavesInAMonth,(DATEDIFF(MM,@FinancialYearStartDate,@STARTDATE)),(MONTH(@STARTDATE)-1)

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
			SELECT @TotalNoofOvertime = @TotalNoofOvertime + @DinnerBreak + @TotalGatePassOutMinutesOfficial

			

	-----------------DIMPAL----CALCULATE TOTAL GATEPASS MINUTE---------------------------------
	         DECLARE @PERMISSION_HOUR DECIMAL(18,2),@PERMISSION DECIMAL(18,2)
			DECLARE @PUNCH_RECORD NVARCHAR(MAX)='',@IN DATETIME,@OUT DATETIME

			DECLARE @PERMISSION_MINUTE DECIMAL(18,2)=0
		 

			select @PERMISSION_MINUTE=sum(p1) from(select  
			(select Permission from [dbo].[fn_CalPermission]
			(InTime,OutTime,BreakStartTime,BreakEndTime,OverTime,EarlyBy,LateBy,PunchRecords)) as p1
			from TAB_BIOMETRIC as bm
			inner join TAB_SHIFT_MASTER as sm on sm.ID=bm.SHIFT_ID
			where BIOMETRIC_CODE= @BIOMETRIC_CODE
			AND ATTENDANCE_DATE between @STARTDATE and @ENDDATE AND STATUS_CODE in ('P','폩','HP','WOP','WO폩','H폩')) as d3

			

			SELECT @PERMISSION_HOUR=(@PERMISSION_MINUTE % 1440) / 60
		    SELECT @PERMISSION=@PERMISSION_HOUR / 8

			SELECT @TotalNumberOfDaysWorked = @NoOfDaysPresent-@PERMISSION
		
--------------------------------------------------------------------------------------------
			IF @NoOfDaysPresent>0 
			BEGIN
				--- Condition to not to adjust Working days in case of 31 days in a month or 28,29 days in a month
				IF (@TotalNumberOfDaysWorked<15)
					SELECT @TempWorkignDaysAdjustment = 0


				SELECT @TotalNumberOfDaysOvertime = (@TotalNoofOvertime / 60) / @PerDayHour
				SELECT @TotalNumberOfHoursOvertime  = @TotalNoofOvertime / 60

				-- Adding Leave if Leave is from Leave Balance -------------------------------Dimpal
				IF @LeaveDeduction = 0 
					SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked + @TotalLeavesInAMonth
				ELSE
					SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked + @LeaveAvailable

				--- Adding Holiday
					SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked + @TotalHolidayInAMonth
				--- WorkingDays Adjustment
				
					SELECT @TotalNumberOfDaysWorked = @TotalNumberOfDaysWorked - @TempWorkignDaysAdjustment 
				
				declare @GetPresentNoOfDays decimal(18,2)=@TotalNumberOfDaysWorked
				select @TotalHolidayInAMonth,@TotalNumberOfDaysWorked
				--select @TotalNumberOfDaysWorked=@TotalNumberOfDaysWorked-1  ------------------------new line added
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
					declare @PerDaySPECIAL_ALLOWANCES as decimal (10,2),
					@PR_AMOUNT as decimal (10,2)

					SELECT @PR_AMOUNT=sum(RATE*QTY) from TAB_WORKFORCE_DAILYWORK where 
					WF_ID=@WF_ID and WORK_DATE between @STARTDATE and @ENDDATE
					--SELECT @PR_AMOUNT='14638.441'
					--SELECT @PR_AMOUNT='4515.554'

					


					SELECT @PerDayBASIC_DA = @BASIC_DA / @NumOfworkingdays
					SELECT @PerDayHRA =  @HRA / @NumOfworkingdays
					SELECT @PerDaySPECIAL_ALLOWANCES =   @SPECIAL_ALLOWANCES / @NumOfworkingdays

					declare @CalEarn decimal(18,2) = isnull(@WeekendLunchDinnerAllowance,0) + isnull(@WeekDayLunchDinnerAllowance,0) + isnull(@PR_AMOUNT,0)
					declare @PerdaySalary decimal(18,2)=@PerDayBASIC_DA + @PerDayHRA + @PerDaySPECIAL_ALLOWANCES
					declare @ExtraEarnSalary decimal(18,2)=isnull(@CalEarn,0) - isnull(@GROSS,0)
					if (@ExtraEarnSalary>0 and (select EMP_TYPE_ID from TAB_WORKFORCE_MASTER where WF_ID=@WF_ID)=2)
					begin
					   select @TotalNumberOfDaysWorked=26
					end

					SELECT @Actual_BASIC_DA = ROUND(@PerDayBASIC_DA * @TotalNumberOfDaysWorked,0)
					SELECT @Actual_HRA = ROUND(@PerDayHRA * @TotalNumberOfDaysWorked,0)
					SELECT @Actual_SPECIAL_ALLOWANCE = ROUND(@PerDaySPECIAL_ALLOWANCES * @TotalNumberOfDaysWorked,0)


					SELECT @WorkingDaySalary = @Actual_BASIC_DA + @Actual_HRA + @Actual_SPECIAL_ALLOWANCE
			
					SELECT @OvertimeSalary = ROUND(((@PerDayBASIC_DA + @PerDayHRA + @PerDaySPECIAL_ALLOWANCES)/8) * @TotalNumberOfHoursOvertime,0)
					SELECT @TotalSalary = @WorkingDaySalary


	
					-------- Source is not define for the below mentioned Perticulars --------
					declare @PRODUCTION_INCENTIVE_BONUS as decimal (10,2)
					declare @TDS as decimal (10,2)
					declare @Advance as decimal (10,2)
					declare @ShopFloorFine as decimal (10,2)
					declare @OtherDeduction as decimal (10,2)

					if(select EMP_TYPE_ID from TAB_WORKFORCE_MASTER where WF_ID=@WF_ID)=1
					begin
					  SELECT @PRODUCTION_INCENTIVE_BONUS = isnull(@WeekendLunchDinnerAllowance,0) + isnull(@WeekDayLunchDinnerAllowance,0) + isnull(@OvertimeSalary,0)+isnull(@WorkingDaySalary,0)
					  
					end
					else
					begin
					  SELECT @PRODUCTION_INCENTIVE_BONUS = isnull(@WeekendLunchDinnerAllowance,0) + isnull(@WeekDayLunchDinnerAllowance,0) + isnull(@PR_AMOUNT,0)
					 
					  
					  select @TotalNumberOfHoursOvertime=case when ((@ExtraEarnSalary/@PerdaySalary)*8)>0 then ((@ExtraEarnSalary/@PerdaySalary)*8) else 0 end
					 
					end
					
					SELECT @TDS = 0
					SELECT @Advance = 0
					SELECT @ShopFloorFine = 0 
					SELECT @OtherDeduction = 0
					------------------------------------------------
					

					SELECT @EarnedSalaryForEPF = CASE WHEN (@BASIC_DA + @SPECIAL_ALLOWANCES) > 15000 THEN 15000 ELSE ((@BASIC_DA + @SPECIAL_ALLOWANCES)/@NumOfworkingdays)*@TotalNumberOfDaysWorked END
					SELECT @EarnedSalaryForESI = CASE WHEN @PRODUCTION_INCENTIVE_BONUS > 21000 THEN 21000 ELSE @PRODUCTION_INCENTIVE_BONUS END

					SELECT @PF = Round((ROUND(@EarnedSalaryForEPF,0) * 12)/100,0)
					SELECT @EmployeeESI = ROUND((ROUND(@EarnedSalaryForESI,0) * 0.75)/100,0)

					SELECT @TOTAL_WAGES_AFTER_DEDUCTION= (@PRODUCTION_INCENTIVE_BONUS) - (@PF + @EmployeeESI + @TDS + @Advance + @ShopFloorFine)

					SELECT @EPF = @PF
					SELECT @EmployerESI = (ROUND(@EarnedSalaryForESI,0) * 3.25) / 100
					SELECT @AdminCharges = (ROUND(@WorkingDaySalary,0) * 0.5)/100
					SELECT @EDLICharges = (ROUND(@WorkingDaySalary,0) * 0.5)/100

					--Print 'WF ID ' + CAST(@WF_ID as varchar(50)) + '   Actual Wage  : ' + CAST(@TOTAL_WAGES_AFTER_DEDUCTION as varchar)
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
						   CREATED_ON,
						   [PR_AMOUNT],
						   [NoofPresentDays],
						   [LunchAllowance]
                           ,[DinnerAllowance],
                           [Permission],
						   Status
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
						   GETDATE(),
						   @PR_AMOUNT,
						   @GetPresentNoOfDays,
						   @WeekendLunchDinnerAllowance,
						   @WeekDayLunchDinnerAllowance,
						   @PERMISSION,
						   11
						   )
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