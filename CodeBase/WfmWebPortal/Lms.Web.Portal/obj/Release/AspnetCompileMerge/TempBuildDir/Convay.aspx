<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Convay.aspx.cs" Inherits="Lms.Web.Portal.Convay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-3.6.1.js" integrity="sha256-3zlB5s2uwoUzrXK3BT7AX3FyvojsraNFxCc2vC/7pNI=" crossorigin="anonymous"></script>
</head>
<body>
    <label style="font-weight:bold">Password - </label><input type="text"  id="TxtPassword"/>
    <button type="button" id="btnEncrypt" onclick="Encrypt_Decrypt(1)">Encrypt</button>
    <button type="button" id="btnDecrypt" onclick="Encrypt_Decrypt(2)">Decrypt</button>
    <button type="button" id="btnReset" onclick="Reset()">Reset</button>
    <br /><br />
     <label style="font-weight:bold">Result - </label><label id="txtResult" style="padding-left:20px"></label>

    <script>
        function Encrypt_Decrypt(e) {
            debugger
            $.ajax({
                type: "POST",
                url: "/Convay.aspx/ChangeFormat",
                data: "{'Value':'" + e + "','Password':'" + $("#TxtPassword").val() + "'}",
                contentType: "application/json;charset=utf-8",
                datatype: "json",
                success: function (response) {
                    $("#txtResult").html(response.d);
                },
                error: function (response) {
                    console.log(response.d)
                }
            })
        }

        function Reset() {
            $("#TxtPassword").val('');
            $("#txtResult").html('');
        }
    </script>
</body>
</html>
