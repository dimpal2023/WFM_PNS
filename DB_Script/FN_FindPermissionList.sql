CREATE or alter FUNCTION FN_FindPermissionList
(
 @attendanceLogId nvarchar(max)
)
RETURNS @list TABLE (
    RowNumber int,
    Value varchar(100),
    Strings varchar(100)
)
AS
BEGIN
    INSERT INTO @list
	SELECT ROW_NUMBER() OVER(ORDER BY value ASC) AS Row#, 
	value,
	SUBSTRING(value, 1, 5) AS substring--,SUBSTRING(value, 7, 7) AS substring1
	FROM STRING_SPLIT((SELECT top 1 reverse(stuff(reverse(PunchRecords), 1, 1, ''))
	FROM  [dbo].[New_TAB_BIOMETRIC]
	where AttendanceLogId=@attendanceLogId
	), ',')
    RETURN;
END;
GO