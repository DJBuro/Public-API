<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.TrackerViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Map
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Trackers"), "/", "Trackers")%> > <%= Html.ActionLink("Tracker " + Model.Tracker.Name, "/Tracker/" + Model.Tracker.Name, "Trackers")%>  > Map</h2>
        <script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAObgT0Ow5uhBTml1-xciFUhQN_vHESFx-dmG8YEypkitcQy7zwhQrjZPYZUTjumCb6e7vOa2YQ9EA7Q"></script>
		<script type="text/javascript" charset="utf-8">
		    google.load("maps", "2.x", { "other_params": "sensor=false" });
		</script>
		<script type="text/javascript" charset="utf-8">
		    $(function() {

		        var map = new GMap2(document.getElementById('map'));
		        map.addControl(new GSmallMapControl());
		        map.enableScrollWheelZoom();
		        map.disableDoubleClickZoom();
		        var bounds = new GLatLngBounds();

		        var carLoc = new GIcon(G_DEFAULT_ICON);

		        var storeIcon = new GIcon(carLoc);
		        markerOptions = { icon: storeIcon };
		        var premises = new GLatLng(<%=Model.Tracker.Coordinates.Latitude %>, <%=Model.Tracker.Coordinates.Longitude %>)
		        PremisesMarker = new GMarker(premises, markerOptions);

		        map.addOverlay(PremisesMarker);

		        bounds.extend(premises);

		        map.setCenter(bounds.getCenter(), 14);
		    });
    </script>
        <table>        
        <tr>
            <td colspan="3"></td><td><%=Html.ActionLink(Html.Resource("Master, Refresh"), "/Map/" + Model.Tracker.Name,"Trackers")%></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="3">                        
                <div id="map" style="height:512px;width:512px;">No details</div><td></td>
            </td>
        </tr>
     </table>
</asp:Content>
