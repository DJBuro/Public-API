<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.IndexViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Map")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "Store")%> > <%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/", "Store")%> >  <%= Html.ActionLink(Model.Account.Store.Name, "/FindByExternalId/" + Model.Account.Store.ExternalStoreId, "Store")%> > <%=Html.Resource("Master, ViewLocation")%></h2>
<%
      using (Html.BeginForm("UpdateCoordinates", "Store", FormMethod.Post))
    {
%>    
  <input id="Id" name="Id" type="hidden" value="<%=Model.Account.Store.Coordinates.Id %>" />
    <table>
        <tr class="separator">
           <td colspan="4"></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, Latitude")%></td><td><%=Html.Resource("Master, Longitude")%></td><td></td>
        </tr>
        <tr>
            <td><%=Html.TextBox("Latitude", Model.Account.Store.Coordinates.Latitude.ToString().Replace(",", "."))%></td><td><%=Html.TextBox("Longitude", Model.Account.Store.Coordinates.Longitude.ToString().Replace(",", "."))%></td><td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td>
        </tr>
        <tr class="separator">
           <td colspan="4"></td>
        </tr> 
        <tr>
            <td colspan="3"><div id="map"></div></td>
        </tr>
    </table>
<%
    }
%>
    <script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAZBe7uHI90ESk2XAmWRL3RxR6u04U0tImA3bfwZ3-HKdEno7z2xRk2YE6OkudtBX5qy0vLrgbf1DUCg"></script>
<%--<script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAObgT0Ow5uhBTml1-xciFUhQKpAZmmqYu8XSFYCL4Y1LNhJDbYxTPJAhRuuEPvvqXckq86JJqUWct-w"></script>--%>
		<script type="text/javascript" charset="utf-8">
		    google.load("maps", "2.x", { "other_params": "sensor=true" });
		    google.load("jquery", "1.3");
		</script>
		<script type="text/javascript" charset="utf-8">
		    $(document).ready(function() {
		        GenerateMap();
		    });

		    function GenerateMap() {

	                    var map = new GMap2(document.getElementById('map'));
	                    
	                    map.addControl(new GLargeMapControl());
                        map.addControl(new GScaleControl());
                        map.enableScrollWheelZoom();
                        map.disableDoubleClickZoom();

	                    var bounds = new GLatLngBounds();

	                    //store Icon
	                    var baseStoreIcon = new GIcon(G_DEFAULT_ICON);

	                    var storeIcon = new GIcon(baseStoreIcon);
	                    markerOptions = { icon: storeIcon };
	                    markerOptions = {draggable: true};

	                    var premises = new GLatLng(<%= Model.Account.Store.Coordinates.Latitude.ToString().Replace(",",".") %>, <%= Model.Account.Store.Coordinates.Longitude.ToString().Replace(",",".")%>);
	                    PremisesMarker = new GMarker(premises, markerOptions);

	                    map.addOverlay(PremisesMarker);

	                    bounds.extend(premises);

                        GEvent.addListener(PremisesMarker, 'dragend', function(overlay,point) 
                        {
                            var point = PremisesMarker.getLatLng();
                            $("#Latitude").val(point.y);
                            $("#Longitude").val(point.x);
                        });

	                    map.setCenter(bounds.getCenter(), map.getBoundsZoomLevel(bounds));
		    }
		</script>
    	<style type="text/css" media="screen">
			#map { float:left; width:650px; height:550px; margin-top:10px; }
		</style>
</asp:Content>
