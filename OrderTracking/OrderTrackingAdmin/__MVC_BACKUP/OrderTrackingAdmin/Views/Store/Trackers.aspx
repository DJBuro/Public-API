<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.IndexViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Trackers")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "Store")%> > <%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/", "Store")%> > <%= Html.ActionLink(Model.Account.Store.Name, "/FindByExternalId/" + Model.Account.Store.ExternalStoreId, "Store")%> > <%=Html.Resource("Master, Trackers")%></h2>
<table>
    <tr class="separator">
       <td colspan="5"></td>
    </tr> 
    <tr>
        <td colspan="4"><strong><%= Model.Account.Store.Trackers.Count%> <%=Html.Resource("Master, Trackers")%></strong></td><td><%= Html.ActionLink(Html.Resource("Master, Refresh"), "Trackers/" + Model.Account.Store.ExternalStoreId, "Store")%></td>
    </tr>
    <tr class="separator">
       <td colspan="5"></td>
    </tr>         
    <tr>
        <td><%=Html.Resource("Master, Name")%></td><td><%=Html.Resource("Master, Drivers")%></td><td><%=Html.Resource("Master, TrackerStatus")%></td><td><%=Html.Resource("Master, BatteryLevel")%></td>
    </tr> 
    <%
        foreach (Tracker tracker in Model.Account.Store.Trackers)
        {%>
    <tr>
        <td><%= tracker.Name %></td><td><%= tracker.Driver != null ? tracker.Driver.Name : "" %></td><td><%= tracker.Status.Name %></td><td><%= tracker.BatteryLevel != -1 ? tracker.BatteryLevel.ToString() + " %" : Html.Resource("Master, Unknown")%></td>
    </tr>
    <% 
        } 
   %> 
    
    
    </table>

</asp:Content>
