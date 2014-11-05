<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="Infragistics4.Web.v14.2, Version=14.2.20142.1028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.2, Version=14.2.20142.1028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ig:WebScriptManager ID="WebScriptManager1" runat="server"></ig:WebScriptManager>
            <ig:WebHierarchicalDataGrid ID="hdgEnterExit"
                runat="server"
                EnableRelativeLayout="True"
                ShowFooter="False"
                HeaderCaptionCssClass="HeaderCaptionClass"
                ItemCssClass="ItemCssClass"
                AltItemCssClass="AltItemCssClass"
                FooterCaptionCssClass="HeaderCaptionClass"
                AutoGenerateColumns="False"
                AutoGenerateBands="True"
                DataMember="Years"
                DataKeyFields="Year"
                InitialDataBindDepth="-1">

                <Columns>
                    <ig:BoundDataField 
                        DataFieldName="Year" 
                        Key="Year" 
                        Footer-Text="Year" 
                        Header-Text="Year" 
                        CssClass="CenterAlign" 
                        Width="70">
                    </ig:BoundDataField>
                </Columns>
                <Bands>
                    <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="Months" DataKeyFields="ID">
                        <Columns>
                            <ig:BoundDataField DataFieldName="Month" Key="ID" Header-Text="Month" CssClass="CenterAlign" Width="70">
                            </ig:BoundDataField>
                        </Columns>

                        <Bands>
                            <ig:Band Key="ID" AutoGenerateColumns="false" DataMember="Days" DataKeyFields="ID">
                                <Columns>
                                    <ig:BoundDataField DataFieldName="Day" Key="ID" Header-Text="Day" CssClass="CenterAlign" Width="70">
                                    </ig:BoundDataField>
                                </Columns>
                                <Bands>
                                </Bands>
                            </ig:Band>
                        </Bands>
                    </ig:Band>
                </Bands>
            </ig:WebHierarchicalDataGrid>
            <asp:Button runat="server" ID="button" Text="DataBind" OnClick="button_Click_BindGrid" />

        </div>
    </form>
</body>
</html>
