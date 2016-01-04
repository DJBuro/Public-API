<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.IndexViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, ViewAllStores")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "Store")%> > <%=Html.Resource("Master, ViewAllStores")%></h2>
    <table>
        <tr class="separator">
            <td colspan="3"></td>
        </tr>
        <tr>
            <td><strong><%=Html.Resource("Master, StoreName")%></strong></td><td><strong><%=Html.Resource("Master, RamesesStoreId")%></strong></td><td style="width:80px"><strong><%=Html.Resource("Master, GpsEnabled")%></strong></td>
        </tr>
<%
    foreach (Account account in Model.Accounts)
    {%>
         <tr>
            <td><%= Html.ActionLink(account.Store.Name, "/FindByExternalId/" + account.Store.ExternalStoreId, "Store")%></td><td><%=account.Store.ExternalStoreId%></td><td class="<%=  account.GpsEnabled %>"></td>
        </tr> 
        <%
    }%>
</table>

</asp:Content>
