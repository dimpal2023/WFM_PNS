alter table dbo.tab_menu add [ORDER] int default(0)
alter table dbo.tab_submenu add [ORDER] int default(0)

update dbo.TAB_MENU set [ORDER] = 1 where ID = 'C1CD3A17-0682-4EEF-8049-310097C6CA3F' --Home
update dbo.TAB_MENU set [ORDER] = 2 where ID = '88B9E226-4239-4407-A494-3B16DAB4B649' --New hiring
update dbo.TAB_MENU set [ORDER] = 3 where ID = '5186024B-A537-4A11-B135-7AC5AC0318E9' --Exit management
update dbo.TAB_MENU set [ORDER] = 4 where ID = '55514AD4-6D66-EB11-BF71-A45D3669901F' --workforce
update dbo.TAB_MENU set [ORDER] = 5 where ID = '1DB2094C-43FC-4254-B90C-488E0AAE772B' --Attendance
update dbo.TAB_MENU set [ORDER] = 6 where ID = 'FDD13B14-6620-47B3-B719-6FEA1A86665A' --Workforce training
update dbo.TAB_MENU set [ORDER] = 7 where ID = 'ABD98032-4089-4607-AFD5-93C84689E974' --card generation
update dbo.TAB_MENU set [ORDER] = 8 where ID = 'E93ADD4C-EBB0-48F5-999C-F8B39593228C' --Asset manegement
update dbo.TAB_MENU set [ORDER] = 9 where ID = '88CC250A-D681-458D-BFE9-5B9A60515FB5' --Tool Talk
update dbo.TAB_MENU set [ORDER] = 10 where ID = '71E8E569-429A-485D-97FA-D2C58CD7F431' --Gate pass
update dbo.TAB_MENU set [ORDER] = 11 where ID = '862834F6-9BF2-4849-BC4A-EC903AE79566' --Shift
update dbo.TAB_MENU set [ORDER] = 12 where ID = '5C3DD11C-9C3C-46F3-979B-B8EC8BD5547A' --User management

--Dashboard
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '7A6A1413-44F4-4768-8B0A-0A91A3D17920'

--New Hiring
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '74BA0CF7-336C-4AA0-8557-8A4E6244180B'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = '574A47B6-4488-4EF1-B529-189C99BDC3CD'
update dbo.TAB_SUBMENU set [ORDER] = 3 where ID = '2ADC6A5E-B1A1-43AE-B6AC-C0627E07772C'
update dbo.TAB_SUBMENU set [ORDER] = 4 where ID = 'C92391AF-9297-4071-936D-5F5F2CF7C61E'

--Exit mangement
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = 'F0BF21BD-7D54-4761-A5DC-2C155DB58F68'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = '554B5A6E-48B8-4541-AC13-971A4BD3D65E'

--Workforce
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '56514AD4-6D66-EB11-BF71-A45D3669901F'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = '700ACF8B-8B6D-EB11-BF73-A45D3669901F'
update dbo.TAB_SUBMENU set [ORDER] = 3 where ID = '0068F2BC-498D-EB11-BF77-A45D3669901F'
update dbo.TAB_SUBMENU set [ORDER] = 4 where ID = '9BCD58C9-A082-EB11-BF75-A45D3669901F'
update dbo.TAB_SUBMENU set [ORDER] = 5 where ID = '4D629F39-8399-EB11-BF77-A45D3669901F'

--Attendance
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = 'C0B02DC3-CA5D-4697-BB99-C54A55720AA0'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = '66CC0B03-71C8-44C4-9F48-35D3360966E0'

--Workforce training
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '8DE951D4-E2DA-40A4-8C07-5F0C3A919E08'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = 'BDE61589-6395-4D17-8B5E-C1479E212203'
update dbo.TAB_SUBMENU set [ORDER] = 3 where ID = '2470BD0F-0848-4B53-AB55-CF9538BB1DC8'

--ID card generation
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = 'F0AAC190-F551-4518-B42F-671392479E8E'

--Asset management
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = 'B6507166-7F54-431C-ACB6-663A9E641198'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = 'E682DDC9-8CFA-4FA1-9281-B24B15AAB5B7'
update dbo.TAB_SUBMENU set [ORDER] = 3 where ID = '76C54825-B450-4BE8-A274-4AF9CD5A1453'

--Tool Talk
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '21B9A4C6-38BD-4607-8EB0-5556854D0BDF'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = '054BB491-CF7C-40D7-AA60-B093AB330097'
update dbo.TAB_SUBMENU set [ORDER] = 3 where ID = '458B0631-9B70-4E98-96AE-DF543128BF84'
update dbo.TAB_SUBMENU set [ORDER] = 4 where ID = 'FB6A6964-C7B2-44C4-A685-98C359188E8F'

--Gate Pass
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '34620B09-E49E-4F42-ACAF-2CE7BA16F109'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = 'B52ABE9A-87F0-40EA-BC5C-48A492467345'

--Shift
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = '69489331-6D9A-4093-9D90-07DAEFF45A74'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = 'A0E07C4B-5D7B-44FC-A27D-B4E34C762D0A'

--User mangement
update dbo.TAB_SUBMENU set [ORDER] = 1 where ID = 'DA8C7919-1A97-4B99-83C9-5D470304CEFB'
update dbo.TAB_SUBMENU set [ORDER] = 2 where ID = '8A7355B3-CDFF-4798-ACCB-6C4BC0763CA6'

select * from dbo.TAB_SUBMENU 