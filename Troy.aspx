<%@ Page Title="StrataGIS Reporting" Language="C#" MasterPageFile="Site.master"
    AutoEventWireup="true" CodeFile="Troy.aspx.cs" Inherits="Troy" %>

<%@ Register Assembly="Infragistics4.Web.v14.2, Version=14.2.20142.1028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls"
    TagPrefix="ig" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table align="center" cellpadding="4" cellspacing="4"
        style="background-color: #CEF0FF">
        <tr valign="middle" align="center">
            <td align="right">
                <table align="center" cellpadding="4" cellspacing="4">
                    <tr valign="middle" align="center">
                        <td align="right">
                            <font style="font-weight: bold; font-size: large">Start Date:</font>
                        </td>
                        <td align="left">
                            <ig:WebDatePicker ID="wdpStart" runat="server"></ig:WebDatePicker>
                        </td>
                    </tr>
                    <tr valign="middle" align="center">
                        <td align="right">
                            <font style="font-weight: bold; font-size: large">End Date:</font>
                        </td>
                        <td align="left">
                            <ig:WebDatePicker ID="wdpEnd" runat="server"></ig:WebDatePicker>
                        </td>
                    </tr>
                    <tr valign="middle" align="center">
                        <td align="right">
                            <font style="font-weight: bold; font-size: large">Report Type:</font>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddReportType" runat="server">
                                <asp:ListItem Text="Daily Usage" Value="Daily"></asp:ListItem>
                                <asp:ListItem Text="Daily Usage (Grouped)" Value="DailyG"></asp:ListItem>
                                <asp:ListItem Text="Total Usage" Value="Total"></asp:ListItem>
                                <asp:ListItem Text="Total Usage (Grouped)" Value="TotalG"></asp:ListItem>
                                <%-- <asp:ListItem Text="Geofence Report" Value="EnterExit"></asp:ListItem>--%>
                                <asp:ListItem Text="Trip Report" Value="Trip"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="right" valign="middle">
                            <font style="font-weight: bold; font-size: large">Department:</font>
                        </td>
                        <td align="left" valign="middle">
                            <asp:DropDownList ID="lbDept" runat="server"
                                OnSelectedIndexChanged="lbDept_SelectedIndexChanged" AutoPostBack="True" RepeatColumns="2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table align="center" cellpadding="4" cellspacing="4">
                    <tr valign="middle" align="center">

                        <td align="right" valign="middle">
                            <font style="font-weight: bold; font-size: large">Vehicles:</font>
                        </td>
                        <td align="left" valign="middle">
                            <asp:ListBox ID="lbVehicles" runat="server" SelectionMode="Multiple" Rows="8"></asp:ListBox>
                        </td>

                    </tr>
                </table>

            </td>
        </tr>

        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnRun" runat="server" Text="Run Report" OnClick="btnRun_Click" />

            </td>
        </tr>
    </table>

    <table align="center" width="100%">
        <tr align="center">
            <td align="center">
                <ig:WebDataGrid ID="wdgTrip" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" OnInitializeRow="wdgTrip_InitializeRow" EnableDataViewState="True">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Date" Key="Date" Footer-Text="Date" Header-Text="Date"
                            CssClass="CenterAlign" Width="70">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Vehicle" Key="Vehicle" Footer-Text="Vehicle" Header-Text="Vehicle"
                            CssClass="CenterAlign" Width="50">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="StartLocalTime" Key="StartLocalTime" Footer-Text="StartTime"
                            Header-Text="StartTime"
                            CssClass="RightAlign" Width="70">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="StopLocalTime" Key="StopLocalTime" Footer-Text="StopTime"
                            Header-Text="StopTime"
                            CssClass="RightAlign" Width="70">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="TripDuration" Key="TripDuration" Footer-Text="Duration"
                            Header-Text="Duration"
                            CssClass="RightAlign" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="IdleTime" Key="IdleTime" Footer-Text="IdleTime"
                            Header-Text="IdleTime"
                            CssClass="RightAlign" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="TripMiles" Key="TripMiles" Footer-Text="Miles"
                            Header-Text="Miles"
                            CssClass="RightAlign" DataFormatString="{0:F}" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="StopLength" Key="StopLength" Footer-Text="StopLength"
                            Header-Text="StopLength" CssClass="RightAlign" Width="70">
                        </ig:BoundDataField>
                        <ig:UnboundField Footer-Text="Stop Address" Key="Address"
                            Header-Text="Stop Address" CssClass="LeftAlign" Width="150">
                        </ig:UnboundField>
                        <ig:BoundDataField DataFieldName="STX" Key="STX" Footer-Text="STX"
                            Header-Text="STX" CssClass="LeftAlign" Width="100" Hidden="True">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="STY" Key="STY" Footer-Text="STY"
                            Header-Text="STY" CssClass="LeftAlign" Width="100" Hidden="True">
                        </ig:BoundDataField>
                    </Columns>
                </ig:WebDataGrid>
            </td>
        </tr>
    </table>
    <table align="center">
        <tr align="center">
            <td align="center">
                <ig:WebDataGrid ID="wdgDaily" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" EnableViewState="True" EnableDataViewState="True">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Date" Key="Date" Footer-Text="Date" Header-Text="Date"
                            CssClass="CenterAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
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
                <ig:WebDataGrid ID="wdgTotal" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" EnableViewState="True" EnableDataViewState="True">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
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
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Month" Key="Month" Footer-Text="Month" Header-Text="Month"
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Week" Key="Week" Footer-Text="Week" Header-Text="Week"
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
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
                    AutoGenerateColumns="False" EnableDataViewState="True">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Year" Key="Year" Footer-Text="Year" Header-Text="Year"
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Month" Key="Month" Footer-Text="Month" Header-Text="Month"
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
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
                <ig:WebDataGrid ID="hdgEnterExit" runat="server" EnableRelativeLayout="True" ShowFooter="True"
                    HeaderCaptionCssClass="HeaderCaptionClass" ItemCssClass="ItemCssClass"
                    AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" EnableDataViewState="True">
                    <Columns>
                        <ig:BoundDataField DataFieldName="Dpt" Key="Department" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Vehicle" Key="Vehicle" Footer-Text="Vehicle" Header-Text="Vehicle"
                            CssClass="CenterAlign" Width="60">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Date" Key="Date" Footer-Text="Date" Header-Text="Date"
                            CssClass="CenterAlign" Width="80">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Time" Key="ID" Footer-Text="Time" Header-Text="Time"
                            Width="70" CssClass="RightAlign">
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="EnterExit" Key="EnterExit" Footer-Text="Event"
                            Header-Text="Event" Width="70" CssClass="LeftAlign">
                        </ig:BoundDataField>
                    </Columns>
                </ig:WebDataGrid>

            </td>
        </tr>
    </table>

    <table width="100%">
        <tr align="center">
            <td align="center">
                <ig:WebHierarchicalDataGrid ID="hdgTotal" runat="server" EnableRelativeLayout="True"
                    ShowFooter="False" HeaderCaptionCssClass="HeaderCaptionClass"
                    ItemCssClass="ItemCssClass" AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" AutoGenerateBands="True"
                    DataMember="Dpts" DataKeyFields="ID" InitialDataBindDepth="-1" EnableDataViewState="True"
                    Width="1100" ExpandableAreaCssClass="FixPadding">
                    <Columns>
                        <ig:BoundDataField DataFieldName="ID" Key="ID" Footer-Text="Dpt" Header-Text="Dpt"
                            CssClass="CenterAlign" Width="57">
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
                            Width="80" Hidden="True">
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
                    </Columns>
                    <Bands>
                        <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="Vehicles" DataKeyFields="ID"
                            ExpandableAreaCssClass="FixPadding">
                            <Columns>
                                <ig:BoundDataField DataFieldName="Dpt" Key="Dpt" Footer-Text="Dpt" Header-Text="Dpt"
                                    CssClass="CenterAlign" Width="57" Hidden="true">
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ID" Key="ID" Footer-Text="Vehicle" Header-Text="Vehicle"
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
                                    Width="80" Hidden="True">
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
                        </ig:Band>
                    </Bands>
                </ig:WebHierarchicalDataGrid>
                <asp:Label ID="lblTest" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <div id="divCharts" runat="server">
        <table width="100%">
            <%--<tr align="center">
            <td align="right">
                <igchart:UltraChart ID="ucDeptMiles" runat="server" ChartType="PieChart" TitleTop-Text="Miles by Dept"
                    TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                    Height="400px" Width="666px" Legend-Font="Microsoft Sans Serif, 8.9pt"
                    Legend-SpanPercentage="20" PieChart-BreakAlternatingSlices="True" ColorModel-ColorBegin="Blue"
                    ColorModel-ColorEnd="Red">
                </igchart:UltraChart>
            </td>
            <td align="left">
                <igchart:UltraChart ID="ucVMiles" runat="server" ChartType="PieChart" TitleTop-Text="Miles by Vehicle"
                    TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                    Height="400px" Width="666px" Legend-Font="Microsoft Sans Serif, 8.9pt"
                    Legend-SpanPercentage="20" PieChart-BreakAlternatingSlices="True" ColorModel-ColorBegin="Blue"
                    ColorModel-ColorEnd="Red">
                </igchart:UltraChart>
            </td>
        </tr>--%>
            <tr align="center">
                <td align="right">
                    <igchart:UltraChart ID="ucDeptMilesB" runat="server" ChartType="ColumnChart" TitleTop-Text="Miles by Dept"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Data-UseMinMax="True"
                        Tooltips-Format="Custom" Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#>"
                        ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
                <td align="left">
                    <igchart:UltraChart ID="ucVMilesB" runat="server" ChartType="ColumnChart" TitleTop-Text="Miles by Vehicle"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Tooltips-Format="Custom"
                        Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#>" ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
            </tr>
            <tr align="center">
                <td align="right">
                    <igchart:UltraChart ID="ucDeptRun" runat="server" ChartType="ColumnChart" TitleTop-Text="Hours Running by Dept"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Data-UseMinMax="True"
                        Tooltips-Format="Custom" Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#.#>"
                        ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
                <td align="left">
                    <igchart:UltraChart ID="ucVRun" runat="server" ChartType="ColumnChart" TitleTop-Text="Hours Running by Vehicle"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Tooltips-Format="Custom"
                        Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#.#>" ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
            </tr>
            <tr align="center">
                <td align="right">
                    <igchart:UltraChart ID="ucDeptIdle" runat="server" ChartType="ColumnChart" TitleTop-Text="Hours Idle by Dept"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Data-UseMinMax="True"
                        Tooltips-Format="Custom" Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#.#>"
                        ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
                <td align="left">
                    <igchart:UltraChart ID="ucVIdle" runat="server" ChartType="ColumnChart" TitleTop-Text="Hours Idle by Vehicle"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Tooltips-Format="Custom"
                        Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#.#>" ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
            </tr>
            <tr align="center">
                <td align="right">
                    <igchart:UltraChart ID="ucDeptMax" runat="server" ChartType="ColumnChart" TitleTop-Text="Max Speed by Dept"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Data-UseMinMax="True"
                        Tooltips-Format="Custom" Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#.#>"
                        ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
                <td align="left">
                    <igchart:UltraChart ID="ucVMax" runat="server" ChartType="ColumnChart" TitleTop-Text="Max Speed by Vehicle"
                        TitleTop-HorizontalAlign="Center" TitleTop-FontSizeBestFit="True" Legend-Visible="False"
                        Height="400px" Width="400px" Data-ZeroAligned="True" Tooltips-Format="Custom"
                        Tooltips-FormatString="<ITEM_LABEL>: <DATA_VALUE:#.#>" ColumnChart-ColumnSpacing="1">
                    </igchart:UltraChart>
                </td>
            </tr>
        </table>
    </div>

    <table width="100%">
        <tr align="center">
            <td align="center">
                <ig:WebHierarchicalDataGrid ID="hdgDailyG" runat="server" EnableRelativeLayout="True"
                    ShowFooter="False" HeaderCaptionCssClass="HeaderCaptionClass"
                    ItemCssClass="ItemCssClass" AltItemCssClass="AltItemCssClass" FooterCaptionCssClass="HeaderCaptionClass"
                    AutoGenerateColumns="False" AutoGenerateBands="True"
                    DataMember="Dates" DataKeyFields="ID" InitialDataBindDepth="-1" EnableDataViewState="True"
                    Width="1100" ExpandableAreaCssClass="FixPadding">
                    <Columns>
                        <ig:BoundDataField DataFieldName="ID" Key="ID" Footer-Text="Date" Header-Text="Date"
                            CssClass="CenterAlign" Width="90">
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
                            Width="80" Hidden="True">
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
                    </Columns>
                    <Bands>
                        <ig:Band Key="Dpts" AutoGenerateColumns="false" DataMember="Dpts" DataKeyFields="ID"
                            ExpandableAreaCssClass="FixPadding" Width="1000">
                            <Columns>
                                <ig:BoundDataField DataFieldName="Date" Key="Date" Footer-Text="Date" Header-Text="Date"
                                    CssClass="CenterAlign" Width="57" Hidden="true">
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ID" Key="ID" Footer-Text="Dpt" Header-Text="Dpt"
                                    CssClass="CenterAlign" Width="57" Hidden="true">
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Dpt" Key="Dpt" Footer-Text="Dpt" Header-Text="Dpt"
                                    CssClass="CenterAlign" Width="57">
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
                                    Width="80" Hidden="True">
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
                            </Columns>
                            <Bands>
                                <ig:Band Key="Vehicles" AutoGenerateColumns="false" DataMember="Vehicles" DataKeyFields="ID"
                                    ExpandableAreaCssClass="FixPadding">
                                    <Columns>
                                        <ig:BoundDataField DataFieldName="Dpt" Key="Dpt" Footer-Text="Dpt" Header-Text="Dpt"
                                            CssClass="CenterAlign" Width="57" Hidden="true">
                                        </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="VehicleId" Key="VehicleId" Footer-Text="Vehicle"
                                            Header-Text="Vehicle"
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
                                            Width="80" Hidden="True">
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
                                </ig:Band>
                            </Bands>
                        </ig:Band>
                    </Bands>
                </ig:WebHierarchicalDataGrid>
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
