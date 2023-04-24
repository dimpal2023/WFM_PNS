create or alter function fn_GetGatePasshour  
(  
   @attendanceLogId nvarchar(max)
)  
returns nvarchar(100)  
as  
begin
declare @count int=0,@Out datetime='',@In datetime='',@dt varchar(max)=''

set @count=(SELECT count(1)
		FROM STRING_SPLIT((SELECT top 1 reverse(stuff(reverse(PunchRecords), 1, 1, ''))
		FROM  [dbo].[New_TAB_BIOMETRIC]
		where AttendanceLogId=@attendanceLogId
		), ','))
   if(@count>3)
   begin
		set @Out=(select Strings from FN_FindPermissionList(@attendanceLogId) where RowNumber=2)
		set @In=(select Strings from FN_FindPermissionList(@attendanceLogId) where RowNumber=3)
		set @dt=(DATEDIFF(MINUTE, @Out, @In))
	end
	else
	begin
	 set @dt=''
	end

   return @dt
end  

