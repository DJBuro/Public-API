<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.TrackerViewData>" %>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add New Tracker
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h2><%= Html.ActionLink(Html.Resource("Master, Trackers"), "/", "Trackers")%> > Add New Tracker</h2>
    
    <%
        using (Html.BeginForm("AddTracker", "Trackers", FormMethod.Post))
        {
%>
    <table>
        <tr class="separator">
            <td colspan="3"></td>
        </tr>
        <tr>
            <td colspan="3"><strong>Tracker Details</strong></td>
        </tr>
        <tr>
            <td>Name/Serial Number</td><td>SIM Phone Number</td><td>Assign to GPS Enabled Store</td>
        </tr>
        <tr>
            <td><%=Html.TextBox("Name")%></td><td><%=Html.TextBox("PhoneNumber")%></td><td><%=Html.DropDownList("Store.Id", Model.StoreListItems)%></td>
        </tr>
        <tr>
            <td>APN</td><td>Tracker Type</td><td></td>
        </tr>
        <tr>
            <td><%=Html.DropDownList("Apn.Id", Model.ApnListItems)%></td><td><%=Html.DropDownList("Type.Id", Model.TrackerTypeListItems)%></td><td></td>
        </tr>
        <%
            if (Model.LicenseCount > 0)
            {
            %>
        <tr>
            <td></td><td></td><td><input type="submit" value="Add New Tracker" /></td>
        </tr>
        <%
            }
            else
            {
           %>
        <tr>
            <td></td><td></td><td class="error">There are no licenses available</td>
        </tr>
           <%} %>
    </table>
    <%} %>
</asp:Content>
