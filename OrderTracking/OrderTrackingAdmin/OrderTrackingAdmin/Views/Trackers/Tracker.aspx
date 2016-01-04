<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.TrackerViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tracker
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Trackers"), "/", "Trackers")%> > Tracker <%=Model.Tracker.Name %></h2>
        <%
            using (Html.BeginForm("UpdateTracker", "Trackers", FormMethod.Post))
            {
%>
<input id="Id" name="Id" type="hidden" value="<%=Model.Tracker.Id.Value%>" /> 
<input id="Name" name="Name" type="hidden" value="<%=Model.Tracker.Name%>" /> 
    <table>        
        <tr>
            <td colspan="2"><%=Html.ActionLink("Setup Tracker", "/Setup/" + Model.Tracker.Name, "Trackers")%></td><td></td><td><%=Html.ActionLink(Html.Resource("Master, Refresh"), "/Tracker/" + Model.Tracker.Name,"Trackers")%></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td><strong>Name/Serial Number</strong></td><td><strong>Phone Number</strong></td><td><strong></strong></td><td><strong></strong></td>
        </tr>
        <tr>
            <td><%=Model.Tracker.Name%></td><td><%=Html.TextBox("PhoneNumber", Model.Tracker.PhoneNumber)%></td><td>Active <%=Html.CheckBox("Active", Model.Tracker.Active)%></td><td>Status: <%=Model.Tracker.Status.Name%></td>
        </tr>
        <tr>
            <td><strong>Store</strong></td><td><strong>APN</strong></td><td><strong>Type</strong></td><td><strong></strong></td>
        </tr>
        <tr>
            <td><%=Html.DropDownList("Store.Id", Model.StoreListItems)%></td><td><%=Html.DropDownList("Apn.Id", Model.ApnListItems)%><%=Model.Tracker.Apn == null ? "ERROR on SERVER" : ""%></td><td><%=Html.DropDownList("Type.Id", Model.TrackerTypeListItems)%></td><td></td>
        </tr>
        <tr>
            <td><strong>Latitude</strong></td><td><strong>Longitude</strong></td><td><strong>Battery Level</strong></td><td><strong></strong></td>
        </tr>
        <tr>
            <td><%=Model.Tracker.Coordinates.Latitude%></td><td><%=Model.Tracker.Coordinates.Longitude%></td><td><%=Model.Tracker.BatteryLevel%></td><td></td>
        </tr>
        <tr>
            <td colspan="3"></td><td><input type="submit" value="Update Tracker" /></td>
        </tr>
        <%
                if (Model.Tracker.Coordinates.Latitude != 0)
                {
                %>
        <tr>
            <td colspan="4"><strong><%=Html.ActionLink("View last known location on map", "Map/" + Model.Tracker.Name, "Trackers")%></strong></td>
        </tr>
        <%
            }%>
    </table>
    <%} %>
</asp:Content>
