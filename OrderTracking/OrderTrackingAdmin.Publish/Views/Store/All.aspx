<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.StoreViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, ViewAllStores")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Stores"), "/", "Store")%> > <%=Html.Resource("Master, ViewAllStores")%></h2>
    <table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td><strong><%=Html.Resource("Master, StoreName")%> </strong></td><td><strong><%=Html.Resource("Master, RamesesStoreId")%></strong></td><td><strong>Trackers</strong></td><td style="width:80px"><strong><%=Html.Resource("Master, GpsEnabled")%></strong></td>
        </tr>
<%
    foreach (var account in Model.Accounts)
    {%>
         <tr>
            <td><%= Html.ActionLink(account.Store.Name, "/FindByExternalId/" + account.Store.ExternalStoreId, "Store")%></td><td><%=account.Store.ExternalStoreId%></td><td><%=  account.GpsEnabled ? Html.ActionLink(account.Store.Trackers.Count.ToString(), "Trackers/" + account.Store.ExternalStoreId, "Store").ToHtmlString() : ""%></td><td class="<%=  account.GpsEnabled %>"></td>
        </tr> 
        <%
    }%>
</table>

</asp:Content>
