<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SchoolErp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="js/JScript.js" ></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:TextBox ID="txtusername" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:RequiredFieldValidator ID="ReqFusernmae" runat="server" ControlToValidate="txtusername" ErrorMessage="Name Required" Font-Bold="True" ForeColor="#FF3300"></asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            <asp:TextBox ID="txtFNmae" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:RequiredFieldValidator ID="reqfName" runat="server" ControlToValidate="txtFNmae" ErrorMessage="First Name Required" Font-Bold="True" ForeColor="#FF3300"></asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Button ID="Button1" runat="server"  Text="Button" OnClick="Button1_Click" />
        </div>
        <div>
            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
            
        </div>
    </form>
</body>
</html>
