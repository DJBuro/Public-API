<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink("Global Administration", "Index", "Global")%> > Global Logs</h2>
<table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <%
            foreach (var log in Model.Logs)
            {%>
        <tr class="<%=log.Severity %>">
           <td><strong>Store Id: <%= log.StoreId %></strong></td><td>Occurences: <%= log.Count%></td><td><%= log.Source%></td>
        </tr>
        <tr class="<%=log.Severity %>">
           <td colspan="3"><%= log.Message %></td>
        </tr>
        <tr class="separator">
           <td colspan="3"></td>
        </tr>
            <%    
            } %>
</table>
</asp:Content>
