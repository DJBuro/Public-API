<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.GlobalViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink("Global Administration", "/", "Global")%> > Global logs for last 7 days</h2>

<table>
    <tr class="separator">
       <td colspan="3"></td>
    </tr> 
    <%
        foreach (var log in Model.Logs)
        {%>
        <tr class="<%=log.Severity %>">
           <td><strong><%= log.Severity %></strong></td><td><%= log.Source%></td><td><%= String.Format("{0:F}",log.Created)%></td>
        </tr>
        <tr class="<%=log.Severity %>">
           <td colspan="3"><%= log.Message %></td>
        </tr>
        <tr class="separator">
           <td colspan="3"></td>
        </tr>
       <% } %>      
</table>

</asp:Content>
