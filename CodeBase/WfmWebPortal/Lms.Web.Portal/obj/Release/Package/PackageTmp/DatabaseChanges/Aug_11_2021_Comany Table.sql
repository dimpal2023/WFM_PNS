ALTER TABLE TAB_COMPANY_MASTER ADD ADDRESS1 varchar(500) null;
ALTER TABLE TAB_COMPANY_MASTER ADD ADDRESS2 varchar(500) null;

update TAB_COMPANY_MASTER Set ADDRESS1='C-12 Amaual Industrial Area' where COMPANY_ID='A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C'
update TAB_COMPANY_MASTER Set ADDRESS2='Nadarganj, Lucknow' where COMPANY_ID='A8E171FA-AC3F-EB11-9092-A0A8CDB0F79C'


/****** Object:  Table [dbo].[TAB_MAIL_TEMPLATE]    Script Date: 11/08/2021 18:16:52 ******/
DROP TABLE [dbo].[TAB_MAIL_TEMPLATE]
GO
/****** Object:  Table [dbo].[TAB_MAIL_TEMPLATE]    Script Date: 11/08/2021 18:16:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAB_MAIL_TEMPLATE](
	[TAMPLATE_ID] [int] IDENTITY(1,1) NOT NULL,
	[TEMPLATE_FOR] [nvarchar](250) NOT NULL,
	[TEMPLATE_CONTANT] [nvarchar](max) NOT NULL,
	[CC_MAIL] [nvarchar](350) NULL,
 CONSTRAINT [PK_TAB_MAIL_TEMPLATE] PRIMARY KEY CLUSTERED 
(
	[TAMPLATE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[TAB_MAIL_TEMPLATE] ON 

INSERT [dbo].[TAB_MAIL_TEMPLATE] ([TAMPLATE_ID], [TEMPLATE_FOR], [TEMPLATE_CONTANT], [CC_MAIL]) VALUES (1, N'GenerateOnRoleCard', N'<table width="80%" style="margin: 1% 1% 1% 1%;">
        <tr>
            <td>
                <table style="border: 1px solid black;border-collapse: collapse">
                    <tr>
                        <td>
                            <table width="450px" style="border: 1px solid black;border-collapse: collapse">
                                <tr>
                                    <td style="text-align:center">
                                        <img style="width:50px;" src="/Content/IdCardImages/karamIcon.png"  />
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <th><h4 style="margin:0;font-weight:bold;text-align:left;">[CMP_NAME]</h4></th>
                                                <td style="text-align:right">[ADDRESS1]</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">[ADDRESS2]</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="7" style="border: 1px solid black;"></td>
                                </tr>
                                <tr class="cardTr">
                                    <td style="width:36px;">
                                        <img src="/Content/IdCardImages/karam.JPG" />
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr style="vertical-align: top;">
                                                <th width="25%" style="text-align:left">Name</th>
                                                <td width="40%">[EMP_NAME]</td>
                                                <td rowspan="5" style="text-align:left">
                                                    <img style="width:120px" src="[WF_SRC_URL]" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="text-align:left">Emp_Code</th>
                                                <td>[EMP_CODE]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th style="text-align:left">Bio_Code</th>
                                                <td>[BIO_CODE]</td>
                                            </tr>
                                            <tr>
                                                <th style="text-align:left">Department</th>
                                                <td>[DEPARTMENT]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th style="text-align:left">Designation</th>
                                                <td>[DESIGNATION]</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="3">
                            <img style="width:23px;" src="/Content/IdCardImages/needIcon.JPG" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="border: 1px solid black;border-collapse: collapse">
                    <tr>
                        <td>
                            <table width="450px" style="border: 1px solid black;border-collapse: collapse;font-size: small;">
                                <tr>
                                    <td style="text-align:center">
                                        <img style="width:50px;" src="/Content/IdCardImages/karamIcon.png" />
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <th><h4 style="margin:0;font-weight:bold;text-align:left;">[CMP_NAME]</h4></th>
                                                <td style="text-align:right">[ADDRESS1]</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">[ADDRESS2]</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="7" style="border: 1px solid black;margin-top:2px"></td>
                                </tr>
                                <tr>
                                    <td style="width:36px;">
                                        <img src="/Content/IdCardImages/karam.JPG" />
                                    </td>
                                    <td>
                                        <table style="text-align:left;font-size: small;" width="100%">
                                            <tr style="vertical-align: top;">
                                                <th width="70px">Local Address</th>
                                                <td width="210px">[LOC_ADDR]</td>
                                                
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th>Permanent Address</th>
                                                <td>[PERM_ADDR]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th>Mobile No.</th>
                                                <td>[EMP_MOB]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th>Emergency No.</th>
                                                <td>[EMG_MOB]</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="3">
                            <img style="width:23px;" src="/Content/IdCardImages/needIcon.JPG" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
    </table>', NULL)
INSERT [dbo].[TAB_MAIL_TEMPLATE] ([TAMPLATE_ID], [TEMPLATE_FOR], [TEMPLATE_CONTANT], [CC_MAIL]) VALUES (2, N'GenerateOnAgentCard', N'<table width="80%" style="margin: 1% 1% 1% 1%;">
        <tr>
            <td>
                <table style="border: 1px solid black;border-collapse: collapse">
                    <tr>
                        <td>
                            <table width="475px" style="border: 1px solid black;border-collapse: separate;border-radius:10px;">
                                <tr style="text-align:center;background-color: #009e9e;color: white;">
                                    <td colspan="2">
                                        <table width="100%" style="color:white;">
                                            <tr>
                                                <th><h4 style="margin:0;font-weight:bold;text-align:center;">[CMP_NAME]</h4></th>
                                                <td ></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align:center">[ADDRESS1] [ADDRESS2]</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="7" style="border: 1px solid black;"></td>
                                </tr>
                                <tr class="cardTr">
                                    <td style="width:36px;height:150px;">
                                       
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr style="vertical-align: top;">
                                                <th width="25%" style="text-align:left">Name</th>
                                                <td width="40%">[EMP_NAME]</td>
                                                <td rowspan="5" style="text-align:left">
                                                    <img style="width:120px" src="[WF_SRC_URL]" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="text-align:left">Emp_Code</th>
                                                <td>[EMP_CODE]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th style="text-align:left">Bio_Code</th>
                                                <td>[BIO_CODE]</td>
                                            </tr>
                                            <tr>
                                                <th style="text-align:left">Department</th>
                                                <td>[DEPARTMENT]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th style="text-align:left">Designation</th>
                                                <td>[DESIGNATION]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th style="text-align:left">DOJ</th>
                                                <td>[DOJ]</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                       
                    </tr>
                </table>
            </td>
            <td>
                <table style="border: 1px solid black;border-collapse: collapse">
                    <tr>
                        <td>
                           <table width="475px" style="border: 1px solid black;border-collapse: separate;border-radius:10px;">
                                <tr style="text-align:center;background-color: #009e9e;color: white;">
                                    <td colspan="2">
                                        <table width="100%" style="color:white;">
                                            <tr>
                                                <th><h4 style="margin:0;font-weight:bold;text-align:center;">[CMP_NAME]</h4></th>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align:center">[ADDRESS1] [ADDRESS2]</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="7" style="border: 1px solid black;margin-top:2px"></td>
                                </tr>
                                <tr>
                                    <td style="width:36px;height:150px;">
                                    </td>
                                    <td>
                                        <table style="text-align:left;font-size: small;" width="100%">
                                            <tr style="vertical-align: top;">
                                                <th width="70px">Local Address</th>
                                                <td width="210px">[LOC_ADDR]</td>
                                                
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th>Permanent Address</th>
                                                <td>[PERM_ADDR]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th>Mobile No.</th>
                                                <td>[EMP_MOB]</td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <th>Emergency No.</th>
                                                <td>[EMG_MOB]</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
        
    </table>', NULL)
INSERT [dbo].[TAB_MAIL_TEMPLATE] ([TAMPLATE_ID], [TEMPLATE_FOR], [TEMPLATE_CONTANT], [CC_MAIL]) values(3, 'NewUserSmsMsg', 'You have been added as a application user in WFM. Your username is [USER_LOGIN_ID] and password is [CURRENT_PASSWORD].', null)
SET IDENTITY_INSERT [dbo].[TAB_MAIL_TEMPLATE] OFF
