alter table [dbo].[TAB_ITEM_OPERATION_MASTER] add [DEPT_ID] uniqueidentifier,
[SUBDEPT_ID] uniqueidentifier,
[ITEM_CODE] uniqueidentifier,
[ITEM_NAME] uniqueidentifier


alter table [dbo].[TAB_WORKFORCE_DAILYWORK] add [Operation] nvarchar(500)

INSERT INTO [dbo].[TAB_WORKFORCE_DAILYWORK]
           ([ID]
           ,[WF_ID]
           ,[RATE]
           ,[WORK_DATE]
           ,[UNIQUE_OPERATION_ID]
           ,[QTY]
           ,[CREATED_DATE]
           ,[CREATED_BY]
           ,[Operation])

		select newid(),
		wm.WF_ID
		,PieceRate
		,Date,
		(select top 1 UNIQUE_OPERATION_ID from TAB_ITEM_OPERATION_MASTER as io where io.Operation=edw.Operation),
		ItemQuantity,
		getdate(),
		'System',
		edw.Operation
		from Empdailywork as edw
		inner join TAB_WORKFORCE_MASTER as wm on wm.EMP_ID=edw.Empcode
		inner join TAB_DEPARTMENT_MASTER as dm on dm.DEPT_NAME=edw.Department

		where MONTH(Date)>8 and year(date)=2022

GO


