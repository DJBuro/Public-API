<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ValueTypes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > Global Admin > ValueTypes</h2>
<table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4">Site Types</strong></td>
        </tr>
        <tr>
            <td colspan="4">Value Types</strong></td>
        </tr>
</table>
</asp:Content>
