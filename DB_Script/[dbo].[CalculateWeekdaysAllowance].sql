CREATE or alter FUNCTION [dbo].[CalculateWeekdaysAllowance] (@bcode VARCHAR(10),@startdate datetime,@enddate datetime)        
RETURNS        
 @returnAllowance TABLE (Allowance int, DayCount int)        
AS        
BEGIN        
	--DECLARE @bcode varchar(10)
	--DECLARE @startdate datetime,@enddate datetime


	--SELECT @bcode = '1515'
	--SELECT @startdate = '07/01/2021'
	--SELECT @enddate = '07/31/2021'

 DECLARE @BIOMETRIC_CODE VARCHAR(10)        
 DECLARE @ATTENDANCE_DATE date        
 DECLARE @WorkInMinutes int = 0        
 DECLARE @OverTimeInMinutes int = 0        
 DECLARE @TempList TABLE (ATTENDANCE_DATE Datetime,WORKINGHOURS decimal(10,2),OVERTIEMHOURS decimal(10,2))        
         
         
 DECLARE db_cursorattdates CURSOR FOR          
 Select DISTINCT BIOMETRIC_CODE,ATTENDANCE_DATE from TAB_BIOMETRIC WHERE BIOMETRIC_CODE=@bcode AND ATTENDANCE_DATE between @startdate and @enddate AND STATUS_CODE in ('P','½P') order by ATTENDANCE_DATE         
 OPEN db_cursorattdates        
 FETCH NEXT FROM db_cursorattdates INTO @BIOMETRIC_CODE, @ATTENDANCE_DATE        
         
 WHILE @@FETCH_STATUS = 0          
 BEGIN         
          
         
  DECLARE @WORKFORCE_ID NVARCHAR(255)        
  DECLARE @START_DATE datetime        
  DECLARE @END_DATE datetime        
  DECLARE @ATT_DATE date        
  DECLARE @SHIFT_STARTTIME sql_variant        
  DECLARE @SHIFT_ENDTIME sql_variant        
  DECLARE @PREV_START_DATE datetime          
  DECLARE @PREV_END_DATE datetime          
  DECLARE @PREV_ATTENDANCE_DATE datetime    
  DECLARE @PREV_DAY_WORKMinutes int = 0           
  DECLARE @NoOfWrokingDays int = 0          
           
  SELECT @PREV_START_DATE = NULL          
  SELECT @PREV_END_DATE = NULL          
  SELECT @PREV_ATTENDANCE_DATE = NULL        
         
  DECLARE db_cursorWorkinMinutes CURSOR FOR         
  Select DISTINCT WORKFORCE_ID,START_DATE,END_DATE,ATTENDANCE_DATE,SHIFT_STARTTIME,SHIFT_ENDTIME from TAB_BIOMETRIC 
  WHERE BIOMETRIC_CODE=@BIOMETRIC_CODE AND ATTENDANCE_DATE=@ATTENDANCE_DATE AND STATUS_CODE in ('P','½P') order by WORKFORCE_ID,ATTENDANCE_DATE        
  
  OPEN db_cursorWorkinMinutes          
  FETCH NEXT FROM db_cursorWorkinMinutes INTO @WORKFORCE_ID, @START_DATE, @END_DATE, @ATT_DATE, @SHIFT_STARTTIME, @SHIFT_ENDTIME        
         
   WHILE @@FETCH_STATUS = 0          
   BEGIN         
         
   DECLARE @SHIFTINDATETIME datetime        
   DECLARE @SHIFTOUTDATETIME datetime        
   DECLARE @SHIFTWORK int        
   DECLARE @OVERTIME int        
   DECLARE @5MINUTESGRACEDEDUCTIONFROMOT int          
   DECLARE @INTIME_FOR_WORKCALCULATION datetime        
   DECLARE @OUTTIME_FOR_WORKCALCULATION datetime        
         
   DECLARE @INTIME_FOR_OVERTIMECALCULATION datetime        
   DECLARE @OUTTIME_FOR_OVERTIMECALCULATION datetime        
         
   SELECT @SHIFTINDATETIME = CAST(CONCAT(@ATT_DATE, ' ', CONVERT(time,@SHIFT_STARTTIME)) AS datetime2(0))        
        
   IF (@SHIFT_STARTTIME > @SHIFT_ENDTIME)        
    SELECT @SHIFTOUTDATETIME = CAST(CONCAT(DATEADD(d,1,@ATT_DATE), ' ', CONVERT(time,@SHIFT_ENDTIME)) AS datetime2(0))        
   ELSE        
    SELECT @SHIFTOUTDATETIME = CAST(CONCAT(@ATT_DATE, ' ', CONVERT(time,@SHIFT_ENDTIME)) AS datetime2(0))        
         
          
  ----------------- In and Out before Shift Start Time Check ------------        
  IF @END_DATE<@SHIFTINDATETIME        
  BEGIN        
   FETCH NEXT FROM db_cursorWorkinMinutes INTO @WORKFORCE_ID, @START_DATE, @END_DATE, @ATT_DATE, @SHIFT_STARTTIME, @SHIFT_ENDTIME        
   CONTINUE        
  END        
        
        
      --------------- In Time for work Calculation ---------------          
   SELECT @5MINUTESGRACEDEDUCTIONFROMOT = 0           
   IF @START_DATE > @SHIFTINDATETIME          
    IF convert(date,@START_DATE) <= @ATT_DATE AND @START_DATE <= @SHIFTOUTDATETIME          
     IF DATEDIFF(MINUTE,@SHIFTINDATETIME,@START_DATE) > 5          
  BEGIN        
      SELECT @INTIME_FOR_WORKCALCULATION = @START_DATE          
  END        
     ELSE          
   BEGIN        
  SELECT @INTIME_FOR_WORKCALCULATION = @SHIFTINDATETIME        
   IF @START_DATE> @SHIFTINDATETIME        
    SELECT @5MINUTESGRACEDEDUCTIONFROMOT = DATEDIFF(MINUTE,@SHIFTINDATETIME,@START_DATE)        
   END        
    ELSE          
     SELECT @INTIME_FOR_WORKCALCULATION = NULL          
                      
   ELSE          
     SELECT @INTIME_FOR_WORKCALCULATION = @SHIFTINDATETIME          
           
   --------------- Out Time for work Calculation ---------------             
   IF @END_DATE > @SHIFTOUTDATETIME          
     IF DATEDIFF(MINUTE,@SHIFTOUTDATETIME,@END_DATE) > 30            
   SELECT @OUTTIME_FOR_WORKCALCULATION = @SHIFTOUTDATETIME           
  ELSE        
   BEGIN        
    --IF DATEDIFF(MINUTE,@INTIME_FOR_WORKCALCULATION,@SHIFTOUTDATETIME)<510        
    --    SELECT @OUTTIME_FOR_WORKCALCULATION = @END_DATE                
    --ELSE        
     SELECT @OUTTIME_FOR_WORKCALCULATION = @SHIFTOUTDATETIME           
   END        
   ELSE          
      SELECT @OUTTIME_FOR_WORKCALCULATION = @END_DATE           
               
              
   --------------- In Time for Overtime Calculation ---------------           
   IF @INTIME_FOR_WORKCALCULATION IS NOT NULL          
    IF @END_DATE > @SHIFTOUTDATETIME          
        IF convert(date,@END_DATE) <= @ATT_DATE          
         IF DATEDIFF(MINUTE,@SHIFTOUTDATETIME,@END_DATE) > 30          
          SELECT @INTIME_FOR_OVERTIMECALCULATION = @SHIFTOUTDATETIME            
         ELSE        
   IF DATEDIFF(MINUTE,@SHIFTOUTDATETIME,@END_DATE) > 0          
    SELECT @INTIME_FOR_OVERTIMECALCULATION = @SHIFTOUTDATETIME            
   ELSE        
    SELECT @INTIME_FOR_OVERTIMECALCULATION = @END_DATE            
        ELSE          
         SELECT @INTIME_FOR_OVERTIMECALCULATION = @SHIFTOUTDATETIME                       
     ELSE          
      SELECT @INTIME_FOR_OVERTIMECALCULATION = NULL                    
   ELSE          
      SELECT @INTIME_FOR_OVERTIMECALCULATION = @START_DATE                    
           
   --------------- Out Time for Overtime Calculation ---------------             
           
   IF @INTIME_FOR_OVERTIMECALCULATION IS NOT NULL          
    SELECT @OUTTIME_FOR_OVERTIMECALCULATION = @END_DATE          
   ELSE          
    SELECT @OUTTIME_FOR_OVERTIMECALCULATION = NULL          
           
             
   IF @INTIME_FOR_WORKCALCULATION IS NOT NULL          
    SELECT @SHIFTWORK = DATEDIFF(MINUTE,@INTIME_FOR_WORKCALCULATION,@OUTTIME_FOR_WORKCALCULATION)          
   ELSE          
    SELECT @SHIFTWORK = 0          
           
   IF @INTIME_FOR_OVERTIMECALCULATION IS NOT NULL          
    SELECT @OVERTIME = DATEDIFF(MINUTE,@INTIME_FOR_OVERTIMECALCULATION,@OUTTIME_FOR_OVERTIMECALCULATION)          
   ELSE          
    SELECT @OVERTIME = 0          
     
     ------- Lunch or Dinner Break Minutes Calculation ---------------          
 DECLARE @LuchBreakMinutes as int = 0           
 DECLARE @DinnerBreakMinutes as int = 0           
           
 ------------ Lunch and Dinner Break together --------------          
 IF Convert(smalldatetime,@PREV_END_DATE) < DateAdd(Minute,30,(DateAdd(HOUR,4,@SHIFTINDATETIME))) AND @START_DATE>@SHIFTOUTDATETIME AND @PREV_ATTENDANCE_DATE = @ATT_DATE      
 BEGIN           
  ----- Lunch Break ------          
  SELECT @LuchBreakMinutes = 30          
  ----- Dinner Break -----          
   IF (@OVERTIME/60) >=7.5          
    SELECT @DinnerBreakMinutes = 30          
              
 END          
 ------------ Dinner Break --------------          
 ELSE IF convert(date,@END_DATE) > @ATT_DATE  AND @PREV_START_DATE IS NOT NULL  AND @PREV_ATTENDANCE_DATE = @ATT_DATE          
  BEGIN          
   SELECT @DinnerBreakMinutes = DateDiff (Minute,@PREV_END_DATE,@INTIME_FOR_OVERTIMECALCULATION)          
  END          
 ---------------- Lunch Break ------------          
 ELSE          
  BEGIN          
    IF @PREV_END_DATE IS NOT NULL          
    BEGIN          
     IF Convert(smalldatetime,@PREV_END_DATE) < DateAdd(Minute,30,(DateAdd(HOUR,4,@SHIFTINDATETIME))) AND @START_DATE>DateAdd(HOUR,4,@SHIFTINDATETIME)  AND @PREV_ATTENDANCE_DATE = @ATT_DATE    
      BEGIN          
       IF Convert(smalldatetime,@PREV_END_DATE) > DateAdd(Minute,30,(DateAdd(HOUR,4,@SHIFTINDATETIME)))          
 SELECT @LuchBreakMinutes = DateDiff (Minute,@PREV_END_DATE,DateAdd(HOUR,4.5,@SHIFTINDATETIME))          
       ELSE          
        SELECT @LuchBreakMinutes = DateDiff (Minute,@PREV_END_DATE,@START_DATE)          
      END  
   ELSE  
  BEGIN  
   ----------- Handle case when there is no same day in and out  
   declare @NextInOnSameDay INT = 0   
   SELECT @NextInOnSameDay = count(1) from TAB_BIOMETRIC(NOLOCK) where BIOMETRIC_CODE= @BIOMETRIC_CODE and ATTENDANCE_DATE = @ATT_DATE and START_DATE>@END_DATE  
   IF @NextInOnSameDay=0 AND  @END_DATE < DateAdd(HOUR,4.5,@SHIFTINDATETIME)  
   BEGIN  
    SELECT @LuchBreakMinutes = 30  
   END      
  END          
    END          
  END           
          
 IF @LuchBreakMinutes>30          
   SELECT @LuchBreakMinutes = 30          
     
          
 IF @DinnerBreakMinutes>30          
   SELECT @DinnerBreakMinutes = 30          
     
     
 SELECT @SHIFTWORK = @SHIFTWORK + @LuchBreakMinutes    
 SELECT @OVERTIME = @OVERTIME + @DinnerBreakMinutes       
 ------------------------------------------------------------------------------------------------------------------      
    
     
           
   declare @diffmin as int = 0         
           
   IF @INTIME_FOR_WORKCALCULATION IS NOT NULL OR @OUTTIME_FOR_WORKCALCULATION<@START_DATE        
   BEGIN        
     IF @PREV_START_DATE IS NOT NULL AND @ATT_DATE = CAST(@PREV_START_DATE as date)   ---- !!!!!!!!!! IMPORTANT !!!!!!! This Change Needs to be updated in all Functions        
  BEGIN        
  IF(@PREV_DAY_WORKMinutes + @SHIFTWORK)<510           
   IF @START_DATE< @SHIFTOUTDATETIME OR @SHIFTOUTDATETIME>@PREV_END_DATE ---- !!!!!!!!!! IMPORTANT !!!!!!! This Change Needs to be updated in all Functions          
    SELECT  @diffmin= 510 - (@PREV_DAY_WORKMinutes + @SHIFTWORK)        
   ELSE        
    SELECT  @diffmin= 510 - @SHIFTWORK         
  END      
 ELSE      
  SELECT  @diffmin= 510 - @SHIFTWORK      
        
   IF @diffmin>0        
   BEGIN        
    IF @diffmin < @OVERTIME        
    BEGIN        
     SELECT @SHIFTWORK = @SHIFTWORK + @diffmin        
     SELECT @OVERTIME = @OVERTIME - @diffmin        
    END        
    ELSE        
    BEGIN        
     SELECT @SHIFTWORK = @SHIFTWORK + @OVERTIME        
     SELECT @OVERTIME = 0        
    END        
   END        
   END        
        
   -------------- OvetTime 30 Mins Check -------------        
   IF @OVERTIME < 30        
  SELECT @OVERTIME=0         
        
   IF @OVERTIME>=@5MINUTESGRACEDEDUCTIONFROMOT        
  SELECT @OVERTIME = @OVERTIME - @5MINUTESGRACEDEDUCTIONFROMOT        
         
        
           
   SELECT @WorkInMinutes = @WorkInMinutes + @SHIFTWORK        
   SELECT @OverTimeInMinutes = @OverTimeInMinutes + @OVERTIME        
        
   SELECT @PREV_START_DATE = @START_DATE        
   SELECT @PREV_END_DATE = @END_DATE        
       
   IF @PREV_ATTENDANCE_DATE = @ATT_DATE    
 SELECT @PREV_DAY_WORKMinutes = @PREV_DAY_WORKMinutes + @SHIFTWORK    
   ELSE    
 BEGIN    
  SELECT @PREV_DAY_WORKMinutes = @SHIFTWORK    
  SELECT @NoOfWrokingDays = @NoOfWrokingDays + 1    
 END    
    
   SELECT @PREV_ATTENDANCE_DATE = @ATT_DATE     
       
  -- PRINT CAST(@ATT_DATE as varchar(15)) +'  WORK : ' + CAST((@SHIFTWORK/60) as varchar(10))  +'  Overtime : ' + CAST((@OVERTIME/60) as varchar(10))         
   
   Insert into @TempList values (@ATT_DATE,ROUND((CAST(@SHIFTWORK as decimal(10,2))/60),2),ROUND((CAST(@OVERTIME as decimal(10,2))/60),2))        
           
   FETCH NEXT FROM db_cursorWorkinMinutes INTO @WORKFORCE_ID, @START_DATE, @END_DATE, @ATT_DATE, @SHIFT_STARTTIME, @SHIFT_ENDTIME        
   END         
         
  CLOSE db_cursorWorkinMinutes          
  DEALLOCATE db_cursorWorkinMinutes        
         
 FETCH NEXT FROM db_cursorattdates INTO @BIOMETRIC_CODE, @ATTENDANCE_DATE        
 END         
 CLOSE db_cursorattdates          
 DEALLOCATE db_cursorattdates         
         
 declare @Lunch as int = 0        
 declare @LunchCount as int = 0        

        
Insert into @returnAllowance
Select sum(Allowance),sum(AllowanceCount) from
(Select ATTENDANCE_DATE,(CASE WHEN sum(OVERTIEMHOURS)>7.5 THEN 95 ELSE 0 END) as Allowance, 1 as AllowanceCount from @TempList group by ATTENDANCE_DATE having sum(OVERTIEMHOURS)>7.5) as abc        
        
	RETURN        
END
