<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Global Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > Global Administration</h2>
<table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4">Site Types</td>
        </tr>
        <tr>
            <td colspan="4">Value Types <%//= Html.ActionLink("Value Types", "ValueTypes", "Global")%></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
           <td colspan="4"><strong><%= Html.ActionLink("Global Logs", "Logs", "Global")%></strong></td>
        </tr>
</table>
</asp:Content>
