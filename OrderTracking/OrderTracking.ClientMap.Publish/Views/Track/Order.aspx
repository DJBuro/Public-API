<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrackingViewData.TrackViewData>" %>
<%@ Import Namespace="OrderTracking.ClientMap.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Order Tracking
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<link href="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/<%= Model.SiteName %>.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAZBe7uHI90ESk2XAmWRL3RxR6u04U0tImA3bfwZ3-HKdEno7z2xRk2YE6OkudtBX5qy0vLrgbf1DUCg"></script>
<%--<script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAObgT0Ow5uhBTml1-xciFUhQQk0e8vP37Pb4S_0yQP9EgvG2GvhT3TKlWQ0i-4IYb_0irb77TYl9sgg"></script>--%>
		<script type="text/javascript" charset="utf-8">
		    google.load("maps", "2.x", { "other_params": "sensor=true" });
		    google.load("jquery", "1.3");
		</script>

    <script src="http://localhost/OrderTrackingClientMap/Scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="http://localhost/OrderTrackingClientMap/Scripts/jquery.timer.js" type="text/javascript"></script>

		<script type="text/javascript" charset="utf-8">
		    $(document).ready(function() {

		        $("#mapTest").hide();
		        $(this).oneTime(10, function() {
		            $.ajaxSetup({ cache: false }); //IE - Cache FIX
		            $.ajax({
		                type: "GET",
		                url: "http://localhost/OrderTrackingClientMap/<%=Model.SiteName %>/GetOrder/<%=Model.Credentials %>",
		                contentType: "application/json; charset=utf-8",
		                dataType: "json",
		                success: function(msg) {
		                    
		                    GenerateMap(msg);
		                }
		            });
		        });

		        $(this).everyTime(10000, function() {
		            $.ajaxSetup({ cache: false }); //IE - Cache FIX

		            $.ajax({
		                type: "GET",
		                url: "http://localhost/OrderTrackingClientMap/<%=Model.SiteName %>/GetOrder/<%=Model.Credentials %>",
		                contentType: "application/json; charset=utf-8",
		                dataType: "json",
		                success: function(msg) {
		                    GenerateMap(msg);
		                }
		            });
		        }); //set to true == 'belaying' is on!
		    });

		    function GenerateMap(details) {

		        if (details.OrderStatus != null) {
		            $("#map").hide();
		            $("#mapTest").show();

		            /*                     
		            1 = Order taken
		            2 = In Oven
		            3 = Cut (ready for delivery)
		            4 = Out for delivery
		            5 = Cashed off (delivered)
		            6 = Cancelled
		            */

		            if (details.OrderStatus == "1") {
		                $("#orderInfo").text("Order taken by: " + details.OrderProcessor + " on " + details.OrderStatusTime);
		            }
		            else if (details.OrderStatus == "2") {
		                $("#orderInfo").text("Put in the oven by: " + details.OrderProcessor + " at " + details.OrderStatusTime);

		                $("#step1").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/1Complete.jpg");
		                $("#step2").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/2Active.jpg");
		            }
		            else if (details.OrderStatus == "3") {
		                $("#orderInfo").text("Being packed for delivery by: " + details.OrderProcessor + " at " + details.OrderStatusTime);

		                $("#step1").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/1Complete.jpg");
		                $("#step2").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/2Complete.jpg");
		                $("#step3").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/3Active.jpg");

		            }
		            else if (details.OrderStatus == "4") {
		                $("#orderInfo").text("On its way to you, your driver is: " + details.OrderProcessor);
		                
		                if (details.DeliveryDriver != null) {
		                    $("#mapTest").hide();
		                    $("#map").show();
		                    var map = new GMap2(document.getElementById('map'));

		                    var bounds = new GLatLngBounds();

		                    //Driver Icon
		                    var baseCustomerIcon = new GIcon(G_DEFAULT_ICON);
		                    baseCustomerIcon.iconSize = new GSize(34, 34);
		                    baseCustomerIcon.iconAnchor = new GPoint(10, 83);
		                    baseCustomerIcon.infoWindowAnchor = new GPoint(9, 2);

		                    var customerIcon = new GIcon(baseCustomerIcon);
		                    customerIcon.image = "http://ordertracking.androtechnology.co.uk/images/default/OrderOutForDelivery.gif";
		                    markerOptions = { icon: customerIcon };

		                    //if the order is outside the delivery area, ignore it.
		                    if (details.Customer.Latitude != 0 & details.Customer.Longitude != 0) {
		                        var customer = new GLatLng(details.Customer.Latitude, details.Customer.Longitude);
		                        CustomerMarker = new GMarker(customer, markerOptions);
		                        map.addOverlay(CustomerMarker);
		                        bounds.extend(customer);
		                    }
		                    if (details.DeliveryDriver.HasFix == true) {

		                        if (details.DeliveryOrder.ProximityDelivered == false) {
		                            //Driver Icon
		                            var baseDriverIcon = new GIcon(G_DEFAULT_ICON);
		                            baseDriverIcon.iconSize = new GSize(49, 37);
		                            baseDriverIcon.iconAnchor = new GPoint(19, 45);
		                            baseDriverIcon.infoWindowAnchor = new GPoint(9, 2);

		                            var driverIcon = new GIcon(baseDriverIcon);
		                            driverIcon.image = "http://ordertracking.androtechnology.co.uk/images/default/DriverCurrentPosition.gif";
		                            markerOptions = { icon: driverIcon };

		                            var driver = new GLatLng(details.DeliveryDriver.Latitude, details.DeliveryDriver.Longitude);
		                            DriverMarker = new GMarker(driver, markerOptions);

		                            map.addOverlay(DriverMarker);
		                        }
		                    }

		                    //store Icon
		                    var baseStoreIcon = new GIcon(G_DEFAULT_ICON);
		                    baseStoreIcon.iconSize = new GSize(36, 37);

		                    baseStoreIcon.iconAnchor = new GPoint(10, 83);
		                    baseStoreIcon.infoWindowAnchor = new GPoint(9, 2);

		                    var storeIcon = new GIcon(baseStoreIcon);
		                    storeIcon.image = "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/<%= Model.SiteName %>.gif";
		                    markerOptions = { icon: storeIcon };

		                    var premises = new GLatLng(details.Premises.Latitude, details.Premises.Longitude);
		                    PremisesMarker = new GMarker(premises, markerOptions);

		                    map.addOverlay(PremisesMarker);

		                    bounds.extend(premises);

		                    map.setCenter(bounds.getCenter(), map.getBoundsZoomLevel(bounds));
		                }
		                else {

		                    $("#step1").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/1Complete.jpg");
		                    $("#step2").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/2Complete.jpg");
		                    $("#step3").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/3Complete.jpg");
		                    $("#step4").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/4Active.jpg");
		                }
		            }
		            else if (details.OrderStatus == "5") {
		                $("#orderInfo").text("Your order has been delivered");
		                $("#mapTest").show();

		                $("#step1").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/1Complete.jpg");
		                $("#step2").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/2Complete.jpg");
		                $("#step3").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/3Complete.jpg");
		                $("#step4").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/4Complete.jpg");
		                $("#step5").attr("src", "http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/5Complete.jpg");
		            }
		            else if (details.OrderStatus == "6") {
		                $("#orderInfo").text("Your order was cancelled at:" + details.OrderStatusTime);
		            }
		        }
		        else {
		            $("#mapTest").hide();
		        }
		    }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td class="content_header"><%= Model.SiteName %> Order Tracking</td>
        </tr>
        <tr>
            <td id="logo"></td>
        </tr>
        <tr>
            <td>
                <table class="content_tables">
                    <tr>
                        <td class="content_header" id="orderInfo">Sorry, there is no information available</td>
                    </tr>
                    <tr>
                        <td>                        
                            <div id="map"><img alt="Sorry, there is no information available" src="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/GPSSorry.gif" /></div>
                            <div id="mapTest">
                                <img id="step5" src="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/5.jpg" />
                                <img id="step4" src="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/4.jpg" />
                                <img id="step3" src="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/3.jpg" />
                                <img id="step2" src="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/2.jpg" />
                                <img id="step1" src="http://ordertracking.androtechnology.co.uk/sites/<%= Model.SiteName %>/1Active.jpg" />
                            </div>
                        </td>
                    </tr>
                 </table>
            </td>
         </tr>
    </table>
</asp:Content>
