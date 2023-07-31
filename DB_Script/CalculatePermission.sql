alter FUNCTION [dbo].[fn_CalPermission] 
( 
  @InTime datetime, 
  @OutTime datetime,
  @BreakStart varchar(20),
  @BreakEnd varchar(20),
  @OverTime int,
  @Early int,
  @Late int,
  @PunchRecord nvarchar(max)
) 
RETURNS @output TABLE(
   Permission  int
   ,GatePass nvarchar(max)
   --BreakStartTime datetime,
   --BreakEndTime datetime
) 
BEGIN 
  DECLARE @ActualBreakStartTime datetime,@ActualBreakEndTime datetime,@P1 int=0,@P2 int=0, @P3 int,@WorkDuration int=0,@LunchDeduction int=0,
  @AdjustMinute int=0,@PunchRecordOut datetime,@PunchRecordIn datetime,@LunchDeductforP2 int=0
      
	  select @WorkDuration=(DATEDIFF(MINUTE, @InTime, @OutTime))
	  ,@PunchRecordOut=(select PuchTime from splitPuchRecords(@PunchRecord,@InTime,@OutTime) WHERE SquenceId=2 AND Direction='OUT')
	  ,@PunchRecordIn=(select PuchTime from splitPuchRecords(@PunchRecord,@InTime,@OutTime) WHERE SquenceId=3 AND Direction='IN')

	   declare @GatePassDuration int=isnull((select DATEDIFF(MINUTE,@PunchRecordOut,@PunchRecordIn)),0)

	  if(@BreakStart in ('1:00','0:00')) ---- get break start time (condition mentioned because of next days date change)
	  begin
	      select @ActualBreakStartTime=CAST(CONCAT(CONVERT(Varchar(10), DATEADD(day,1,@InTime), 112), ' ', DATEADD(MINUTE, 0,CONVERT(time,@BreakStart))) AS datetime2(0))
	  end
	  else
	  begin
	      select @ActualBreakStartTime=CAST(CONCAT(CONVERT(Varchar(10),@InTime,112), ' ', DATEADD(MINUTE, 0,CONVERT(time,@BreakStart))) AS datetime2(0))
	  end

	  if(@BreakEnd in ('1:30','0:30')) ---- get break end time (condition mentioned because of next days date change)
	  begin
	      select @ActualBreakEndTime=CAST(CONCAT(CONVERT(Varchar(10), DATEADD(day,1,@InTime), 112), ' ', CONVERT(time,@BreakEnd)) AS datetime2(0))
	  end
	  else
	  begin
	      select @ActualBreakEndTime=CAST(CONCAT(CONVERT(Varchar(10),@InTime,112), ' ', CONVERT(time,@BreakEnd)) AS datetime2(0))
	  end

	  ----------------------------------------check normal permission---------------------------------------

	  if(DATEDIFF(MINUTE,@InTime,@PunchRecordOut)<=240 and @OutTime<=@ActualBreakStartTime)--- if duration is less or equal to 4 hour
	  begin
	     select @LunchDeduction=30
	  end

	  --if(@OutTime>=@ActualBreakStartTime and @OutTime<DATEADD(MINUTE, 15,@ActualBreakStartTime))----check lunch 15 minute gross
	  --begin
	  --    select @LunchDeduction=30
	  --end

	  if((isnull(@Early,0)+isnull(@Late,0))>0)----- first get (early By + Late By) and @WorkDuration<=510
	  begin
	   
	    if(@WorkDuration<=510)
		  select @P1=isnull(@Early,0)+isnull(@Late,0)-@LunchDeduction

		  if(@OverTime=0 and @Early=0 and @GatePassDuration=0 and @WorkDuration>=510 and @Late>0)
		  begin
		     select @P1=0
		  end

		  if(@OverTime=0 and @Early=0 and @GatePassDuration=0 and @WorkDuration<510 and @Late>0)
		  begin
		     select @P1=510-@WorkDuration
		  end
	  end

	  --------------------------------------------------------------------------------------------------
	 
	 
	  if(@GatePassDuration>0 and @OverTime=0 )--and @WorkDuration<=510
	  begin
	     
		  if(@PunchRecordOut>=@ActualBreakStartTime and @PunchRecordIn<=@ActualBreakEndTime)----between lunch
		  begin
			 select @P2=0
		  end

		  ELSE if(@PunchRecordOut<=@ActualBreakStartTime and @PunchRecordIn>=@ActualBreakEndTime)----
		  begin
			select @P2=@GatePassDuration-30
		  end

		  ELSE if((@PunchRecordOut>=DATEADD(MINUTE, 15,@ActualBreakStartTime) and @PunchRecordOut<@ActualBreakEndTime) and @PunchRecordIn>@ActualBreakEndTime)
		  begin
			 select @P2=@GatePassDuration-isnull((select DATEDIFF(MINUTE,@PunchRecordOut,@ActualBreakEndTime)),0)
		  end
		  --ELSE if(@PunchRecordOut>@ActualBreakEndTime and @PunchRecordIn>@ActualBreakEndTime)
		  --begin
			 --select @P2=@GatePassDuration
		  --end
		 
		  ELSE 
		  BEGIN
		      select @P2=@GatePassDuration
		  END
	  end
	  

	  ----------------------------------------------------------------------------------------------------
	  select @P3=(@P1+@P2)

	  declare @P4 int

	  if(@WorkDuration>510 and @P2>0)------need to remove 
	  begin
	     select @P4=@WorkDuration-510
		 select @P3=@P4-@P2

		if(@P3<=0)
		begin
			select @P3=0
		end
	  end

	  


      INSERT INTO @output (Permission,GatePass)  
      VALUES(@P3,CONVERT(varchar(20),@GatePassDuration)) 
	   

  RETURN 
  END

GO

