<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.IndexViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Logs")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (Model.Account != null)
   {%>
       <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "Store")%> > <%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/", "Store")%> >  <%= Html.ActionLink(Model.Account.Store.Name, "/FindByExternalId/" + Model.Account.Store.ExternalStoreId, "Store")%> > <%=Html.Resource("Master, Logs")%></h2>
<%}
   else
   {%>
      <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "Store")%> > <%= Html.Resource("Master, GlobalLogs")%> </h2>
<%} %>

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
