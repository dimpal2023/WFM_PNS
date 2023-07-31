create or alter procedure Proc_InsertError_Log(
@ErrorMessage nvarchar(max)=null,
@Method nvarchar(max)=null,
@Path nvarchar(max)=null,
@UserId varchar(200)=null
)
as 
begin
    insert into [dbo].[TAB_ERROR_LOGS]([User_ID],[ErrorProcedure],[ErrorMessage],[ErrorDateTime],[Db_User])
	values(@UserId,@Path,@ErrorMessage,GETDATE(),@Method)
end