<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:TextBox ID="tbx" runat="server"></asp:TextBox><asp:TextBox ID="tby" runat="server"></asp:TextBox><br />
            <asp:Button ID="btcclick" runat="server" OnClick="btcclick_Click" Text="Calculate" /><br />
            <br />
            <asp:Label>Coords: </asp:Label>
            <asp:Label ID="lblout" runat="server"></asp:Label><br />
            <br />
            <asp:Label>Google: </asp:Label><asp:Label ID="lblout2" runat="server"></asp:Label><br />
            <br />
            <asp:Label>ArcGIS: </asp:Label><asp:Label ID="lbltest" runat="server"></asp:Label><br />
            <br />
            <div id="divout" runat="server"></div>
        </div>
    </form>
</body>
</html>
