USE [WFM_PROD]
GO
/****** Object:  StoredProcedure [dbo].[Proc_GETPIB_Data]    Script Date: 12/8/2022 4:46:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   procedure [dbo].[Proc_GETPIB_Data]
as
begin
		DECLARE @DATE DATETIME,@STARTDATE datetime,@ENDDATE datetime
		SET @DATE=DateAdd(m,-1,getdate())
		SELECT  @STARTDATE = CONVERT(DATE,@DATE-DAY(@DATE)+1), @ENDDATE = EOMONTH(@DATE)
        --Delete from [dbo].[TAB_WORKFORCE_DAILYWORK] where convert(date,WORK_DATE)=convert(date,GETDATE())
		  Delete from [dbo].[TAB_WORKFORCE_DAILYWORK] where convert(date,WORK_DATE) between @STARTDATE and @ENDDATE


       INSERT INTO [dbo].[TAB_WORKFORCE_DAILYWORK]
           ([ID]
           ,[WF_ID]
           ,[RATE]
           ,[WORK_DATE]
           ,[UNIQUE_OPERATION_ID]
           ,[QTY]
           ,[CREATED_DATE]
           ,[CREATED_BY]
           ,[Operation]
		   ,[TotalPrice],
		   ITEM_ID)



       select newid(),
        wm.WF_ID
        ,PieceRate
        ,Date,
        (select top 1 UNIQUE_OPERATION_ID from TAB_ITEM_OPERATION_MASTER as io where io.Operation=edw.Operation),
        ItemQuantity,
        getdate(),
        'System',
        edw.Operation,
		edw.Price,
		'00000000-0000-0000-0000-000000000000'
        from [hrms_karam].[dbo].[Empdailywork] as edw
        inner join TAB_WORKFORCE_MASTER as wm on wm.EMP_ID=edw.Empcode
        inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_NAME=edw.Department
        where convert(date,Date) between @STARTDATE and @ENDDATE--convert(date,Date)=convert(date,GETDATE())
end