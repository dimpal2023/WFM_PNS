INSERT [dbo].[TAB_MAIL_TEMPLATE] ([TEMPLATE_FOR], [TEMPLATE_CONTANT], [CC_MAIL]) VALUES (N'ApproveAndReject', N'<p>Dear [TONAME],</p>
<p>Please approve the [APPROVALTYPE] request from portal for [TASKNAME].</p>
<p>Please <a href="[REDIRECTURL]">Click hear<a/> to approve/reject.</p>
<p>Regards,</p>
<p>[FROMNAME]</p>', NULL)

INSERT [dbo].[TAB_MAIL_TEMPLATE] ([TEMPLATE_FOR], [TEMPLATE_CONTANT], [CC_MAIL]) VALUES (N'ApproveAndRejectStatus', N'<p>Dear [TONAME],</p>
<p>Your [APPROVALTYPE] request has been [TASKSTATUS].</p>
<p>Please <a href="[REDIRECTURL]">Click hear<a/> to view.</p>
<p>Regards,</p>
<p>[FROMNAME]</p>', NULL)