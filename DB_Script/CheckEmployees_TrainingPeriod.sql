create or alter proc CheckEmployees_TrainingPeriod

as
begin
   update TAB_WORKFORCE_MASTER set EMP_STATUS_ID=6 where EMP_STATUS_ID in (1,3) and  REFERENCE_ID is not null and DATEDIFF(month,DOJ, GETDATE())>REFERENCE_ID

end

--update TAB_WORKFORCE_MASTER set ALTERNATE_NO='' where ALTERNATE_NO='NA' 

select * from TAB_WORKFORCE_MASTER  where ALTERNATE_NO='NA' 