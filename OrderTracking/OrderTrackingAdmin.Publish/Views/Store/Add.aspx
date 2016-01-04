<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.StoreViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add New Store
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Stores"), "/", "Store")%> > <%=Html.Resource("Master, AddNewStore") %></h2>

<%
using (Html.BeginForm("AddAccount", "Store", FormMethod.Post))
{
%>
    <table>
    <tr class="separator">
            <td colspan="3"></td>
        </tr>
        <tr>
            <td colspan="3"><strong><%=Html.Resource("Master, AccountDetails")%></strong></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, Username")%></td><td><%=Html.Resource("Master, Password")%></td><td></td>
        </tr>
        <tr>
            <td><%=Html.TextBox("StoreDetails.AccountUserName")%></td><td><%=Html.TextBox("StoreDetails.AccountPassword")%></td><td><%=Html.Resource("Master, GPSEnabled")%> <%=Html.CheckBox("StoreDetails.GpsEnabled")%></td>
        </tr>
        <tr class="separator">
            <td colspan="3"></td>
        </tr> 
        <tr>
            <td colspan="3"><strong><%=Html.Resource("Master, StoreDetails")%></strong></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, StoreName")%></td><td><%=Html.Resource("Master, RamesesStoreId")%></td><td><%=Html.Resource("Master, DeliveryRadius")%> (km)</td>
        </tr>
        <tr>
            <td><%=Html.TextBox("StoreDetails.StoreName")%></td><td><%=Html.TextBox("StoreDetails.ExternalStoreId")%></td><td><%=Html.TextBox("StoreDetails.DeliveryRadius")%></td>
        </tr>
        <tr class="separator">
            <td colspan="3"></td>
        </tr> 
        <tr>
            <td colspan="3"><strong><%=Html.Resource("Master, Address")%></strong></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, Number")%></td><td colspan="3"><%=Html.TextBox("StoreDetails.HouseNumber")%></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, BuildingName")%></td><td  colspan="3"><%=Html.TextBox("StoreDetails.BuildingName")%></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, RoadName")%></td><td  colspan="3"><%=Html.TextBox("StoreDetails.RoadName")%></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, City")%></td><td  colspan="3"><%=Html.TextBox("StoreDetails.TownCity")%></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, PostCode")%></td><td  colspan="3"><%=Html.TextBox("StoreDetails.PostCode")%></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, Country")%></td><td  colspan="3"><%=Html.TextBox("StoreDetails.Country")%></td>
        </tr>
        <tr>
            <td></td><td></td><td><input type="submit" value="<%=Html.Resource("Master, AddNewStore") %>" /></td>
        </tr>
    </table>

<%}%>
</asp:Content>
