<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.IndexViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Account.Store.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "Store")%> > <%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/", "Store")%> > <%=Model.Account.Store.Name %> </h2>

<%
    using (Html.BeginForm("UpdateStore", "Store", FormMethod.Post))
    {
%>    
<%=Html.Hidden("ExternalStoreId", Model.Account.Store.ExternalStoreId)%>
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
    <tr>
        <td colspan="3"><strong><%=Html.Resource("Master, StoreDetails")%></strong></td><td><%= Html.ActionLink(Html.Resource("Master, Refresh"), "/FindByExternalId/" + Model.Account.Store.ExternalStoreId, "Store")%></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, StoreName")%></td><td><%=Html.Resource("Master, RamesesStoreId")%></td><td><%=Html.Resource("Master, DeliveryRadius")%> (km)</td><td></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Name", Model.Account.Store.Name)%></td><td><%=Html.TextBox("ExternalStoreId", Model.Account.Store.ExternalStoreId)%></td><td><%=Html.TextBox("DeliveryRadius", Model.Account.Store.DeliveryRadius)%></td><td><input type="submit" value="<%= Html.Resource("Master, Update")%>" /></td>
    </tr>
    <tr>
        <td colspan="3"><%= Html.ActionLink(Html.Resource("Master, ViewLocation"), "/Map/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td></td>
    </tr>
<%
    }
%>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
        using (Html.BeginForm("ClearMonitor", "Store", FormMethod.Post))
        {
    %>     
<%=Html.Hidden("ExternalStoreId", Model.Account.Store.ExternalStoreId)%>
    <tr>
        <td colspan="3"><strong><%= Html.Resource("Master, CurrentDetails")%></strong></td><td><input type="submit" value="<%= Html.Resource("Master, ClearMonitor")%>" /></td>
    </tr>
    <%} %>
    <%if (Model.Account.Store.Trackers != null && Model.Account.GpsEnabled)
      {%>

    <tr>
        <td><%= Html.ActionLink(Model.Account.Store.Trackers.Count + " " + Html.Resource("Master, Trackers"), "Trackers/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td><%= Html.ActionLink(Model.Account.Store.Orders.Count + " " + Html.Resource("Master, OrdersTaken"), "Orders/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td><%= Html.ActionLink(Model.Account.Store.Drivers.Count + " " + Html.Resource("Master, Drivers"), "Drivers/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td></td>
    </tr>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 

    <%}
      else
      {%>   
    <tr>
        <td  class="error"><%= Html.Resource("Master, GPSDisabled")%></td><td><%= Html.ActionLink(Model.Account.Store.Orders.Count + " " + Html.Resource("Master, OrdersTaken"), "Orders/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td><%= Html.ActionLink(Model.Account.Store.Drivers.Count + " " +Html.Resource("Master, Drivers"), "Drivers/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td></td>
    </tr>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
    <%} %>
    
<%
    using (Html.BeginForm("UpdateAccount", "Store", FormMethod.Post))
      {
%>     
<%=Html.Hidden("Id", Model.Account.Store.Id)%>
    <tr>
        <td colspan="4"><strong><%= Html.Resource("Master, AccountDetails")%></strong></td>
    </tr>
    <tr>
        <td><%= Html.Resource("Master, Username")%></td><td><%= Html.Resource("Master, Password")%></td><td></td><td></td>
    </tr>
    <tr>
         <td><%=Html.TextBox("UserName", Model.Account.UserName)%></td><td><%=Html.TextBox("Password", Model.Account.Password)%></td><td>GPS Enabled <%=Html.CheckBox("GpsEnabled", Model.Account.GpsEnabled)%> </td><td><input type="submit" value="<%= Html.Resource("Master, Update")%>" /></td>
    </tr>
<%
    }
%>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
   <%
       using (Html.BeginForm("ClearAccountLogs", "Store", FormMethod.Post))
       {
    %>     
<%=Html.Hidden("ExternalStoreId", Model.Account.Store.ExternalStoreId)%>

    <tr>
        <td colspan="4"><strong>Site Logs</strong></td>
    </tr>
    <tr>
        <td><%= Html.ActionLink(Html.Resource("Master, TodaysLogs"), "TodaysLogs/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td><%= Html.ActionLink(Html.Resource("Master, PastWeeksLogs"), "WeeksLogs/" + Model.Account.Store.ExternalStoreId, "Store")%></td><td></td><td><input type="submit" value="<%= Html.Resource("Master, ClearAllLogs")%>" /></td>
    </tr>
    <%} %>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 

</table>


</asp:Content>
