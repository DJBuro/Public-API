<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.StoreViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Orders")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <h2><%= Html.ActionLink(Html.Resource("Master, Stores"), "/", "Store")%> > <%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/", "Store")%> >  <%= Html.ActionLink(Model.Account.Store.Name, "/FindByExternalId/" + Model.Account.Store.ExternalStoreId, "Store")%> > <%=Html.Resource("Master, Orders")%></h2>

<table >
    <tr class="separator">
       <td colspan="7"></td>
    </tr> 
    <tr>
        <td colspan="3"><strong><%= Model.OrderStatuses.Count %> <%=Html.Resource("Master, Orders")%></strong></td><td colspan="3"><%= String.Format("{0:D}", DateTime.Now)%></td><td><%= Html.ActionLink(Html.Resource("Master, Refresh"), "Orders/" + Model.Account.Store.ExternalStoreId, "Store")%></td>
    </tr>
    <tr class="separator">
       <td colspan="7"></td>
    </tr>         
     <%
        foreach (var orderStatus in Model.OrderStatuses)
        {
            %>
            <tr class="orderStatus<%= orderStatus.Status.Id.Value %>">
                <td><strong><%= orderStatus.Order.ExternalOrderId%></strong></td><td colspan="2"><strong><%=orderStatus.Status.Name %></strong></td><td colspan="2"><strong><%= orderStatus.Processor%></strong></td><td colspan="2"><strong><%= String.Format("{0:t}", orderStatus.Time)%></strong></td>
            </tr>
            <%
                foreach (CustomerOrder customerOrder in orderStatus.Order.CustomerOrder)
            {%>
           <tr class="orderStatus<%= orderStatus.Status.Id.Value %>">
                <td></td><td colspan="2"><%= customerOrder.Customer.Name%></td><td><%=Html.Resource("Master, Credentials")%>:</td><td><%= customerOrder.Customer.Credentials%></td><td></td><td></td>
            </tr>   
            <%} %>         
            <tr class="orderStatus<%= orderStatus.Status.Id.Value %>">
                <td></td><td colspan="2"><%=Html.Resource("Master, ProximityDelivered")%>:<%= orderStatus.Order.ProximityDelivered.HasValue ? orderStatus.Order.ProximityDelivered.ToString() : " " + Html.Resource("Master, No")%></td><td colspan="2"><%=Html.Resource("Master, Ticket")%> # <%= orderStatus.Order.TicketNumber %></td><td colspan="2"><%= orderStatus.Order.ExtraInformation %></td>
            </tr>
            <tr class="orderStatus<%= orderStatus.Status.Id.Value %>">
                <td></td><td colspan="2"><%=Html.Resource("Master, OrderItems")%>:</td><td colspan="4"></td>
            </tr>
            <tr class="orderStatus<%= orderStatus.Status.Id.Value %>">
             <td colspan="2"></td><td colspan="5">
                <ul>
                <%
                    foreach (Item item in orderStatus.Order.Items)
                    {%>
                    
                    <li><%=item.Quantity%> - <%= item.Name %></li>
               <%} %>  
                </ul>
            </td>
            </tr>
            
    <tr class="separator">
       <td colspan="7"></td>
    </tr>
    <% 
        } 
   %> 
    
    
    </table>


</asp:Content>
