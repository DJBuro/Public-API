<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrackingViewData.TrackViewData>" %>
<%@ Import Namespace="OrderTracking.ClientMap.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.SiteName %> Order Tracking
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <%--<link href="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/<%= Model.SiteName %>.css" rel="stylesheet" type="text/css" />--%>
    <link href="http://localhost/OrderTrackingClientMap/Content/<%= Model.SiteName %>/<%= Model.SiteName %>.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td class="content_header"><%= Model.SiteName %> Order Tracking</td>
        </tr>
        <tr>
            <td id="logo"></td>
        </tr>
        <%
        using (Html.BeginForm("Order", "Track", FormMethod.Post))
        {
    %>
        <tr>
            <td>
                <table class="content_tables">
                    <tr>
                        <td  class="content_header">Find Your Order</td>
                    </tr>
                        <tr>
                            <td class="content_tablesPad">Order Tracking Number(usually the phone number you ordered with)</td>
                        </tr>
                        <tr>
                            <td class="content_tablesPad"><%= Html.TextBox("credentials")%> <input type="submit" value="Find My Order" /></td>
                        </tr>    
                </table>
            </td>
        </tr>
    <%
        }
    %>
    </table>
</asp:Content>
