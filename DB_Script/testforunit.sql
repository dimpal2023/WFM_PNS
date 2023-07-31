
insert into [dbo].[TAB_USER_DEPARTMENT_MAPPING]
([USER_ID],[DEPT_ID],[created_date],[Created_by],[SUBDEPT_ID],[BUILDING_ID])
select [USER_ID],[DEPT_ID],[created_date],[Created_by],[SUBDEPT_ID]
,TRY_CONVERT(UNIQUEIDENTIFIER, 'b31e2dc8-9a41-eb11-9471-8cdcd4d2c4ef')
from TAB_USER_DEPARTMENT_MAPPING  where building_id is not null and building_id='B31E2DC8-9A41-EB11-9471-8CDCD4D2C4CD'