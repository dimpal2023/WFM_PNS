
INSERT INTO [dbo].[TAB_WORKFORCE_DAILYWORK]
           ([ID]
           ,[WF_ID]
           ,[RATE]
           ,[WORK_DATE]
           ,[UNIQUE_OPERATION_ID]
           ,[ITEM_ID]
           ,[QTY]
           ,[CREATED_DATE]
           ,[CREATED_BY]
           ,[UPDATED_DATE]
           ,[UPDATED_BY])
     select 
            [ID]
           ,[WF_ID]
           ,[RATE]
           ,[WORK_DATE]
           ,[UNIQUE_OPERATION_ID]
           ,[ITEM_ID]
           ,[QTY]
           ,[CREATED_DATE]
           ,[CREATED_BY]
           ,[UPDATED_DATE]
           ,[UPDATED_BY] from [Test].[dbo].[TAB_WORKFORCE_DAILYWORK]
GO


