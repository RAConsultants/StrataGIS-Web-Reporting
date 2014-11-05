<%@ Page Title="StrataGIS Reporting" Language="C#" MasterPageFile="Site.master"
    AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="Infragistics4.Web.v14.2, Version=14.2.20142.1028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls"
    TagPrefix="ig" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table align="center" cellpadding="4" cellspacing="4">
        <tr valign="middle" align="center">
            <td align="right">
                <font style="font-weight: bold; font-size: large">Start Date:</font>
            </td>
            <td align="left">
                <ig:WebDatePicker ID="wdpStart" runat="server"></ig:WebDatePicker>
            </td>
            <td align="right">
                <font style="font-weight: bold; font-size: large">End Date:</font>
            </td>
            <td align="left">
                <ig:WebDatePicker ID="wdpEnd" runat="server"></ig:WebDatePicker>
            </td>
        </tr>
        <tr valign="top">
            <td align="center" valign="middle">
                <font style="font-weight: bold; font-size: large">Vehicles:</font>
            </td>
            <td colspan="3" align="center">
                <asp:CheckBoxList ID="cklVehicles" runat="server" RepeatDirection="Horizontal" RepeatColumns="6">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button ID="btnDailySearch" runat="server" Text="Daily Summary" OnClick="btnSearchDaily_Click" />
                <asp:Button ID="btnWeeklySearch" runat="server" Text="Weekly Summary" OnClick="btnSearchWeekly_Click" />
                <asp:Button ID="btnmonthlySearch" runat="server" Text="Monthly Summary" OnClick="btnSearchMonthly_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button ID="btnSearchEnterExit" runat="server" Text="Enter Exit Report" OnClick="btnSearchEnterExit_Click" />
            </td>
        </tr>
    </table>


    <table align="center">
        <tr align="center">
            <td align="center">
                <ig:WebDataGrid ID="wdgDaily" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Date" Key="Date" Footer-Text="Date" Header-Text="Date"
                            CssClass="CenterAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Vehicle" Key="Vehicle" Footer-Text="Vehicle" Header-Text="Vehicle"
                            CssClass="CenterAlign" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsRun" Key="HrsRun" Footer-Text="HrsRun" Header-Text="HrsRun"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsIdle" Key="HrsIdle" Footer-Text="HrsIdle" Header-Text="HrsIdle"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsOff" Key="HrsOff" Footer-Text="HrsOff" Header-Text="HrsOff"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="DIHrs" Key="DIHrs" Footer-Text="DIHrs" Header-Text="DIHrs"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Miles" Key="Miles" Footer-Text="Miles" Header-Text="Miles"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AvgSpeed" Key="AvgSpeed" Footer-Text="AvgSpeed"
                            Header-Text="AvgSpeed" CssClass="RightAlign" DataFormatString="{0:F}"
                            Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MaxSpeed" Key="MaxSpeed" Footer-Text="MaxSpeed"
                            Header-Text="MaxSpeed" CssClass="RightAlign" DataFormatString="{0:F}"
                            Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="OnTime" Key="OnTime" Footer-Text="OnTime" Header-Text="OnTime"
                            CssClass="RightAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="OffTime" Key="OffTime" Footer-Text="OffTime" Header-Text="OffTime"
                            CssClass="RightAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MILCodes" Key="MILCodes" Footer-Text="MILCodes"
                            Header-Text="MILCodes" CssClass="LeftAlign" Width="200">
                        </ig:BoundDataField>
                    </Columns>
                </ig:WebDataGrid>
            </td>
        </tr>
    </table>

    <table align="center">
        <tr>
            <td>
                <ig:WebDataGrid ID="wdgWeekly" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Year" Key="Year" Footer-Text="Year" Header-Text="Year"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Month" Key="Month" Footer-Text="Month" Header-Text="Month"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Week" Key="Week" Footer-Text="Week" Header-Text="Week"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Vehicle" Key="Vehicle" Footer-Text="Vehicle" Header-Text="Vehicle"
                            CssClass="CenterAlign" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsRun" Key="HrsRun" Footer-Text="HrsRun" Header-Text="HrsRun"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsIdle" Key="HrsIdle" Footer-Text="HrsIdle" Header-Text="HrsIdle"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsOff" Key="HrsOff" Footer-Text="HrsOff" Header-Text="HrsOff"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="DIHrs" Key="DIHrs" Footer-Text="DIHrs" Header-Text="DIHrs"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Miles" Key="Miles" Footer-Text="Miles" Header-Text="Miles"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AvgSpeed" Key="AvgSpeed" Footer-Text="AvgSpeed"
                            Header-Text="AvgSpeed" CssClass="RightAlign" DataFormatString="{0:F}"
                            Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MaxSpeed" Key="MaxSpeed" Footer-Text="MaxSpeed"
                            Header-Text="MaxSpeed" CssClass="RightAlign" DataFormatString="{0:F}"
                            Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="OnTime" Key="OnTime" Footer-Text="OnTime" Header-Text="OnTime"
                            CssClass="RightAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="OffTime" Key="OffTime" Footer-Text="OffTime" Header-Text="OffTime"
                            CssClass="RightAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MILCodes" Key="MILCodes" Footer-Text="MILCodes"
                            Header-Text="MILCodes" CssClass="LeftAlign" Width="200">
                        </ig:BoundDataField>
                    </Columns>
                </ig:WebDataGrid>
            </td>
        </tr>
    </table>

    <table align="center">
        <tr>
            <td>
                <ig:WebDataGrid ID="wdgMonthly" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Year" Key="Year" Footer-Text="Year" Header-Text="Year"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Month" Key="Month" Footer-Text="Month" Header-Text="Month"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="30">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Vehicle" Key="Vehicle" Footer-Text="Vehicle" Header-Text="Vehicle"
                            CssClass="CenterAlign" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsRun" Key="HrsRun" Footer-Text="HrsRun" Header-Text="HrsRun"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsIdle" Key="HrsIdle" Footer-Text="HrsIdle" Header-Text="HrsIdle"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HrsOff" Key="HrsOff" Footer-Text="HrsOff" Header-Text="HrsOff"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="DIHrs" Key="DIHrs" Footer-Text="DIHrs" Header-Text="DIHrs"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Miles" Key="Miles" Footer-Text="Miles" Header-Text="Miles"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AvgSpeed" Key="AvgSpeed" Footer-Text="AvgSpeed"
                            Header-Text="AvgSpeed" CssClass="RightAlign" DataFormatString="{0:F}"
                            Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MaxSpeed" Key="MaxSpeed" Footer-Text="MaxSpeed"
                            Header-Text="MaxSpeed" CssClass="RightAlign" DataFormatString="{0:F}"
                            Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="OnTime" Key="OnTime" Footer-Text="OnTime" Header-Text="OnTime"
                            CssClass="RightAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="OffTime" Key="OffTime" Footer-Text="OffTime" Header-Text="OffTime"
                            CssClass="RightAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MILCodes" Key="MILCodes" Footer-Text="MILCodes"
                            Header-Text="MILCodes" CssClass="LeftAlign" Width="200">
                        </ig:BoundDataField>
                    </Columns>
                </ig:WebDataGrid>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr align="center">
            <td align="center">
                <ig:WebHierarchicalDataGrid ID="hdgEnterExit" runat="server" EnableRelativeLayout="True"
                    ShowFooter="True" HeaderCaptionCssClass="HeaderCaptionClass"
                    ItemCssClass="ItemCssClass" AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" AutoGenerateBands="True"
                    DataMember="Vehicles" DataKeyFields="ID" InitialDataBindDepth="-1" Width="500px">
                    <Columns>
                        <ig:BoundDataField DataFieldName="VehicleId" Key="ID" Footer-Text="Vehicle" Header-Text="Vehicle"
                            CssClass="LeftAlign">
                        </ig:BoundDataField>
                    </Columns>
                    <Bands>
                        <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="Years" DataKeyFields="ID">
                            <Columns>
                                <ig:BoundDataField DataFieldName="Year" Key="Year" Footer-Text="Year" Header-Text="Year"
                                    CssClass="LeftAlign">
                                </ig:BoundDataField>
                            </Columns>
                            <Bands>
                                <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="Months" DataKeyFields="ID">
                                    <Columns>
                                        <ig:BoundDataField DataFieldName="Month" Key="ID" Footer-Text="Month" Header-Text="Month"
                                            CssClass="LeftAlign">
                                        </ig:BoundDataField>
                                    </Columns>
                                    <Bands>
                                        <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="Days" DataKeyFields="ID">
                                            <Columns>
                                                <ig:BoundDataField DataFieldName="Day" Key="ID" Footer-Text="Day"
                                                    Header-Text="Day"
                                                    CssClass="LeftAlign">
                                                </ig:BoundDataField>
                                            </Columns>

                                            <Bands>
                                                <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="EnterExits"
                                                    DataKeyFields="ID">
                                                    <Columns>
                                                        <ig:BoundDataField DataFieldName="Time" Key="ID"
                                                            Footer-Text="Time" Header-Text="Time"
                                                            Width="70" CssClass="RightAlign">
                                                        </ig:BoundDataField>
                                                        <ig:BoundDataField DataFieldName="EnterExit" Key="EnterExit"
                                                            Footer-Text="Event"
                                                            Header-Text="Event" Width="70" CssClass="LeftAlign">
                                                        </ig:BoundDataField>
                                                    </Columns>
                                                </ig:Band>
                                            </Bands>
                                        </ig:Band>
                                    </Bands>
                                </ig:Band>
                            </Bands>
                        </ig:Band>
                    </Bands>
                </ig:WebHierarchicalDataGrid>
                <asp:Label ID="lblTest" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
