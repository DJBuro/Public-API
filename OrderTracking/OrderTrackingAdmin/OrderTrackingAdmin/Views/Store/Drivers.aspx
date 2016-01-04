<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.StoreViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Drivers")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Stores"), "/", "Store")%> > <%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/", "Store")%> >  <%= Html.ActionLink(Model.Account.Store.Name, "/FindByExternalId/" + Model.Account.Store.ExternalStoreId, "Store")%> > <%=Html.Resource("Master, Drivers")%></h2>

    <table>
    
 <tr class="separator">
       <td colspan="5"></td>
    </tr> 
    <tr>
        <td colspan="4"><strong><%= Model.Drivers.Count %> <%=Html.Resource("Master, Drivers")%></strong></td><td><%= Html.ActionLink(Html.Resource("Master, Refresh"), "Drivers/" + Model.Account.Store.ExternalStoreId, "Store")%></td>
    </tr>
    <tr class="separator">
       <td colspan="5"></td>
    </tr>         
        <tr>
            <td><%=Html.Resource("Master, Drivers")%></td><td><%=Html.Resource("Master, Vehicle")%></td><td><%=Html.Resource("Master, ExternalDriverId")%></td><td><%=Html.Resource("Master, Trackers")%></td><td><%=Html.Resource("Master, Orders")%></td>
        </tr>
        <%
            foreach (Driver driver in Model.Drivers)
            {
                var tracker = new Tracker();

                if(driver.Trackers.Count ==1)
                {
                    tracker = (Tracker)driver.Trackers[0];
                }
%>        
        
        <tr>
            <td><strong><%=driver.Name %></strong></td><td><%= driver.Vehicle %></td><td><%=driver.ExternalDriverId %></td><td><%= driver.Trackers.Count == 0 ? "none" : Html.ActionLink(tracker.Name,"Tracker/" + tracker.Name, "Trackers").ToHtmlString() %>  </td><td><%=driver.DriverOrder.Count %></td>
        </tr> 
            
            <%
                foreach (DriverOrder driverOrder in driver.DriverOrder)
                {%>
        <tr>
           <td></td><td>Order: <%= driverOrder.Order.ExternalOrderId %></td><td colspan="2">Proximity delivered:<%= driverOrder.Order.ProximityDelivered.HasValue ? driverOrder.Order.ProximityDelivered.ToString() : " no"%></td><td><%= driverOrder.Order.ExtraInformation %></td>
        </tr>
                <%
                }%> 
    <tr class="separator">
       <td colspan="5"></td>
    </tr>
        <%
            } 
            %> 
        
    </table>

</asp:Content>
