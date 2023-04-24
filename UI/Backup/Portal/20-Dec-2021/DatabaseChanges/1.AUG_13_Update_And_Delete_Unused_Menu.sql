--updating training spelling correction
update dbo.TAB_MENU set Name = 'Workforce Training' where ID = 'FDD13B14-6620-47B3-B719-6FEA1A86665A'
update dbo.TAB_SUBMENU set Name = 'Add Training' where ID = '8DE951D4-E2DA-40A4-8C07-5F0C3A919E08'
update dbo.TAB_SUBMENU set Name = 'Training Workforce' where ID = 'BDE61589-6395-4D17-8B5E-C1479E212203'
update dbo.TAB_SUBMENU set Name = 'Training Master' where ID = '2470BD0F-0848-4B53-AB55-CF9538BB1DC8'

--deleting holidays menu
delete from dbo.TAB_MENU_ROLE_MAPPING where MENU_ID = 'C53029F5-6FCB-4CE7-8095-ABFEE57BA7F1'
delete from dbo.TAB_SUBMENU where MENU_ID = 'C53029F5-6FCB-4CE7-8095-ABFEE57BA7F1'
delete from dbo.TAB_MENU where ID = 'C53029F5-6FCB-4CE7-8095-ABFEE57BA7F1'

--deleting leave management menu
delete from dbo.TAB_MENU_ROLE_MAPPING where MENU_ID = '0BD63C81-ABAF-404E-A1F0-96E7617697E9'
delete from dbo.TAB_SUBMENU where MENU_ID = '0BD63C81-ABAF-404E-A1F0-96E7617697E9'
delete from dbo.TAB_MENU where ID = '0BD63C81-ABAF-404E-A1F0-96E7617697E9'

--delete today attendeance
delete from dbo.TAB_MENU_ROLE_MAPPING where Id = '75036FDF-FF33-427B-8DD9-5A88BA0C4D4D'
delete from dbo.TAB_SUBMENU where ID = '1A592127-F164-4F10-9FD3-FA2B5DCFC704'
