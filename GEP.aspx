<%@ Page Title="StrataGIS Reporting" Language="C#" MasterPageFile="Site.master"
    AutoEventWireup="true" CodeFile="GEP.aspx.cs" Inherits="GEP" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table align="center" cellpadding="4" cellspacing="4">
        <tr valign="middle">
            <td align="center">
                <font style="font-weight: bold; font-size: large">Vehicles</font>
            </td>
            <td align="center">
                <font style="font-weight: bold; font-size: large">Years</font>
            </td>
            <td align="center">
                <font style="font-weight: bold; font-size: large">Months</font>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:CheckBoxList ID="cklVehicles" runat="server" RepeatColumns="2">
                </asp:CheckBoxList>
            </td>
            <td>
                <asp:CheckBoxList ID="cklYear" runat="server">
                </asp:CheckBoxList>
            </td>
            <td>
                <asp:CheckBoxList ID="cklMonth" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td colspan=3>
                <asp:Button ID="btnDailySearch" runat="server" Text="Daily Report" OnClick="btnSearchDaily_Click" />
                 <asp:Button ID="btnWeeklySearch" runat="server" Text="Weekly Report" OnClick="btnSearchWeekly_Click" />
                  <asp:Button ID="btnmonthlySearch" runat="server" Text="Monthly Report" OnClick="btnSearchMonthly_Click" />
            </td>
        </tr>
    </table>
    
    
    <asp:GridView ID="gvDaily" runat="server" CellPadding="5" CellSpacing="8" 
        onrowdatabound="gvDaily_RowDataBound" BorderColor="Black" 
        BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" 
        ViewStateMode="Enabled" >
        <AlternatingRowStyle BackColor="#CCFFFF" BorderColor="Black" 
            BorderStyle="Solid" />
        <EditRowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
            VerticalAlign="Middle" />
        <HeaderStyle BackColor="#99CCFF" BorderColor="Black" BorderStyle="Solid" 
            BorderWidth="1px" Font-Bold="True" HorizontalAlign="Center" />
        <RowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
    </asp:GridView>

    
    <asp:GridView ID="gvWeekly" runat="server" CellPadding="5" CellSpacing="8" 
        onrowdatabound="gvWeekly_RowDataBound" BorderColor="Black" 
        BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" 
        ViewStateMode="Enabled" >
        <AlternatingRowStyle BackColor="#CCFFFF" BorderColor="Black" 
            BorderStyle="Solid" />
        <EditRowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
            VerticalAlign="Middle" />
        <HeaderStyle BackColor="#99CCFF" BorderColor="Black" BorderStyle="Solid" 
            BorderWidth="1px" Font-Bold="True" HorizontalAlign="Center" />
        <RowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
    </asp:GridView>

    
    <asp:GridView ID="gvMonthly" runat="server" CellPadding="5" CellSpacing="8" 
        onrowdatabound="gvMonthly_RowDataBound" BorderColor="Black" 
        BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" 
        ViewStateMode="Enabled" >
        <AlternatingRowStyle BackColor="#CCFFFF" BorderColor="Black" 
            BorderStyle="Solid" />
        <EditRowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
            VerticalAlign="Middle" />
        <HeaderStyle BackColor="#99CCFF" BorderColor="Black" BorderStyle="Solid" 
            BorderWidth="1px" Font-Bold="True" HorizontalAlign="Center" />
        <RowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
    </asp:GridView>
</asp:Content>
