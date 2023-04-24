create or alter function fn_Count_TotalPunch  
(  
   @attendanceLogId nvarchar(max)
)  
returns int  
as  
begin
declare @count int=0

set @count=(SELECT count(1)
		FROM STRING_SPLIT((SELECT top 1 reverse(stuff(reverse(PunchRecords), 1, 1, ''))
		FROM  [dbo].[New_TAB_BIOMETRIC]
		where AttendanceLogId=@attendanceLogId
		), ','))

  

   return @count
end  

