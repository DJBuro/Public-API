/// <reference path="Typings/google.maps.d.ts" />
/// <reference path="Typings/jquery.d.ts" />
module AndroWeb.OrderTracking 
{
    export interface IMapOrder
    {
        order: IOrder;
        orderLocation: IOrderLocation;
        eta: number;
        storeMarker: google.maps.Marker;
        storeInfoWindow: google.maps.InfoWindow;
        customerMarker: google.maps.Marker;
        customerInfoWindow: google.maps.InfoWindow;
        driverMarker: google.maps.Marker;
        driverInfoWindow: google.maps.InfoWindow;
        status: number;
        
    }

    export interface IOrder
    {
        id: number;
        status: number; // Order status
        storeLat: number; // GPS latitude of the store
        storeLon: number; // GPS longitude of the store
        custLat: number;  // GPS latitude of the customer
        custLon: number;  // GPS longitude of the customer
        personProcessing: string; // Name of the store employee currently processing the order
    }

    export interface IOrderLocation
    {
        id: number;
        status: number; // Order status
        lat: number; // GPS latitude of the store
        lon: number; // GPS longitude of the store
        active: boolean;
    }

    export interface IOrders
    {
        orders: IOrder[]; // A list of orders associated with the customer
    }

    export interface IOrderLocations
    {
        orders: IOrderLocation[]; // A list of orders associated with the customer
    }

    export interface IError
    {
        error: IErrorDetails;
    }
    export interface IErrorDetails
    {
        errorCode: number;
        errorMessage: string;
    }

    export class MapOrders
    {
        public list: IMapOrder[] = [];
        public lookup: any = {};
    }

    export class App 
    {
        private static sessionId: string = null;
        private static mapOrders: MapOrders = new MapOrders();
        private static trackingOrderLocations: boolean = false;

        public static start(): void
        {
            // Extract the session id from the url    
            App.sessionId = App.getSessionId();

            if (App.sessionId == null)
            {
                // Display some kind of error
                App.showError();
                return;
            }

            // Start polling orders
            App.pollOrders();
        }

        public static showError(errorCode?: number): void
        {
            var errorText = "Sorry, your order cannot be tracked at this time";

            if (errorCode == 1)
            {
                // Unhandled or unknown error
                errorText = "There was an unexpected problem tracking your order";
            }

            $("#pleaseWait").css("display", "none");
            $("#map").css("display", "none");
            $("#error").css("display", "block");
        }

        public static showMap(): void
        {
            $("#pleaseWait").css("display", "none");
            $("#map").css("display", "block");
            $("#error").css("display", "none");
        }

        private static pollOrders(): void
        {
            // Get a list of orders for this customer
            Services.getOrders
            (
                App.sessionId,
                function (errorCode: number, allOrders: AndroWeb.OrderTracking.IOrders)
                {
                    if (errorCode == 0)
                    {
                        // Make sure the map is visible
                        App.showMap();

                        // Keep hold of the orders for later
                        var shouldTrackOrderLocations: boolean = false;
                        for (var orderIndex: number = 0; orderIndex < allOrders.orders.length; orderIndex++)
                        {
                            var order: IOrder = allOrders.orders[orderIndex];

                            // Try and get the exsiting map order
                            var mapOrder: IMapOrder = App.mapOrders.lookup[order.id];
                                
                            // If this is the first time we've encountered the order we need to add it to the map
                            if (mapOrder == undefined)
                            {
                                // Create a new ma order
                                mapOrder =
                                    {
                                        order: order,
                                        orderLocation: null,
                                        eta: null,
                                        customerMarker: null,
                                        customerInfoWindow: null,
                                        driverMarker: null,
                                        driverInfoWindow: null,
                                        storeMarker: null,
                                        storeInfoWindow: null,
                                        status: null
                                    };

                                // Add it to the list
                                App.mapOrders.list.push(mapOrder);

                                // Also, make sure the order is a property so we can look it up by order id efficently later
                                App.mapOrders.lookup[mapOrder.order.id] = mapOrder;
                            }

                            // Update the map order with the order from the server
                            mapOrder.order = order;

                            // Do we need to track order locations on the map?
                            if (mapOrder.order.status == 4)
                            {
                                // This order is out for delivery - make sure we're polling the server for order locations
                                shouldTrackOrderLocations = true;
                            }
                        }

                        // Initialise the map
                        MapServices.initialiseGoogleMap(App.mapOrders);

                        if (shouldTrackOrderLocations && !App.trackingOrderLocations)
                        {
                            // We need to start tracking driver locations
                            App.pollOrderLocations();
                        }

                        // Wait for 10 seconds and then poll for orders again
                        setTimeout(App.pollOrders, 6000);
                    }
                    else
                    {
                        // There was a problem calling the web service
                        App.showError(errorCode);
                    }
                }
            );
        }

        private static pollOrderLocations(): void
        {
            // We're now polling for order locations
            App.trackingOrderLocations = true;

            // Go get the order locations
            Services.getOrderLocations
                (
                    App.sessionId,
                    function (errorCode: number, orderLocations: AndroWeb.OrderTracking.IOrderLocations)
                    {
                        var shouldKeepTrackingOrders: boolean = false;

                        if (errorCode == 0)
                        {
                            // Move the drivers around the map
                            shouldKeepTrackingOrders = MapServices.updateOrderLocations(App.mapOrders, orderLocations);
                        }
                        else
                        {
                            // There was a problem calling the web service
                            App.showError(errorCode);
                        }

                        if (shouldKeepTrackingOrders)
                        {
                            // Poll for driver locations again in 5 seconds
                            setTimeout(App.pollOrderLocations, 3000);
                        }
                        else
                        {
                            // We're no longer tracking any drivers
                            App.trackingOrderLocations = false;
                        }
                    }
                );
        }

        private static getSessionId(): string
        {
            var chunks = location.search.substr(1).split("&")
            for (var chunkIndex: number = 0; chunkIndex < chunks.length; chunkIndex++)
            {
                var chunk = chunks[chunkIndex];

                var chunkParts = chunk.split("=");
                if (chunkParts.length == 2)
                {
                    if (chunkParts[0].toUpperCase() == "SESSIONID")
                    {
                        return chunkParts[1];
                    }
                }
            }

            return null;
        }
    }

    export class Services 
    {
        public static getOrders(sessionId: string, callback: (success: number, data: AndroWeb.OrderTracking.IOrders) => void): void
        {
            var jqxhr = $.ajax
                (
                    {
                        type: "GET",
                        url: "http://localhost/services/ordertracking/TrackOrder/Order/" + sessionId,
                        timeout: 60000,  // 60 seconds
                        contentType: "application/json",
                        accepts: "application/json",
                        dataType: 'json',
                        cache: false,
                        data: undefined
                    }
                )
                .done
                (
                    function (data, textStatus, jqXHR)
                    {
                        try
                        {
                            if (textStatus != 'success')
                            {
                                // Got an error
                                callback(Services.getErrorCode(data), null);
                            }
                            else
                            {
                                // All good!
                                callback(0, <AndroWeb.OrderTracking.IOrders>data);
                            }
                        }
                        catch (exception)
                        {
                            // Got an error
                            callback(1, null);
                        }
                    }
                )
                .fail
                (
                    function (jqXHR, textStatus, errorThrown)
                    {
                        if (jqXHR != undefined &&
                            jqXHR.responseText != undefined)
                        {
                            callback(Services.getErrorCode(jqXHR.responseText), null);
                        }
                        else
                        {
                            callback(1, null);
                        }
                    }
            );
        }

        public static getOrderLocations(sessionId: string, callback: (success: number, data: AndroWeb.OrderTracking.IOrderLocations) => void): void
        {
            var jqxhr = $.ajax
                (
                {
                    type: "GET",
                    url: "http://localhost/services/ordertracking/TrackOrder/OrderLocation/" + sessionId,
                    timeout: 60000,  // 60 seconds
                    contentType: "application/json",
                    accepts: "application/json",
                    dataType: 'json',
                    cache: false,
                    data: undefined
                }
                )
                .done
                (
                function (data, textStatus, jqXHR)
                {
                    try
                    {
                        if (textStatus != 'success')
                        {
                            // Got an error
                            callback(Services.getErrorCode(data), null);
                        }
                        else
                        {
                            // All good!
                            callback(0, <AndroWeb.OrderTracking.IOrderLocations>data);
                        }
                    }
                    catch (exception)
                    {
                        // Got an error
                        callback(1, null);
                    }
                }
                )
                .fail
                (
                function (jqXHR, textStatus, errorThrown)
                {
                    if (jqXHR != undefined &&
                        jqXHR.responseText != undefined)
                    {
                        callback(Services.getErrorCode(jqXHR.responseText), null);
                    }
                    else
                    {
                        callback(1, null);
                    }
                }
                );

        }

        private static getErrorCode(text: any): number
        {
            var response: IError = undefined;

            try
            {
                response = <IError>JSON.parse(text);
            } catch (e) { }

            var errorCode = 1;
            if (response && response.error && response.error.errorCode)
            {
                errorCode = response.error.errorCode;
            }

            return errorCode;
        }
    }

    export class MapServices
    {
        public static map: google.maps.Map = null;
        private static markers: google.maps.Marker[] = null;

        public static initialiseGoogleMap(mapOrders: MapOrders): boolean
        {
            // Get a list of ALL the markers on the map (both visible and not visible)
            MapServices.markers = MapServices.getMarkers(mapOrders);

            if (MapServices.markers.length == 0)
            {
                // Nothing to put on map!!
                return;
            }

            // Get an area that covers all the markers
            var mapBounds: google.maps.LatLngBounds = MapServices.getMapBounds(MapServices.markers);

            var mapOptions: google.maps.MapOptions =
                {
                    center: MapServices.markers[0].getPosition()
                };

            // Do we need to create the map?
            if (MapServices.map == null)
            {
                MapServices.map = new google.maps.Map
                    (
                        document.getElementById('map'),
                        mapOptions
                    );
            }

            // Do we need to add any markers to the map?
            for (var markerIndex: number = 0; markerIndex < MapServices.markers.length; markerIndex++)
            {
                var marker = MapServices.markers[markerIndex];
                if (marker.getMap() == undefined || marker.getMap() == null)
                {
                    marker.setMap(MapServices.map);
                }
            }

            // Make sure the map shows all the markers
            MapServices.map.setCenter(mapBounds.getCenter());
            MapServices.map.fitBounds(mapBounds);
        }

        public static updateDrivers(orderLocations: AndroWeb.OrderTracking.IOrderLocations)
        {
            orderLocations
        }

        private static getMarkers(mapOrders: MapOrders): google.maps.Marker[]
        {
            var markers: google.maps.Marker[] = [];

            if (mapOrders.list.length == 0) return markers;

            for (var orderIndex: number = 0; orderIndex < mapOrders.list.length; orderIndex++)
            {
                var mapOrder: IMapOrder = mapOrders.list[orderIndex];
                
                if (mapOrder.order.custLat != 0 && mapOrder.order.custLon != 0)
                {
                    // Customer marker
                    markers.push(MapServices.getCustomerMarker(mapOrder));
                }

                if (mapOrder.order.storeLat != 0 && mapOrder.order.storeLon != 0)
                {
                    // Store marker
                    markers.push(MapServices.getStoreMarker(mapOrder));
                }

                // Driver marker
                var driverMarker: google.maps.Marker = MapServices.getDriverMarker(mapOrder);
                if (driverMarker) markers.push(driverMarker);

                // Update the status or the order, as shown on the map
                mapOrder.status = mapOrder.order.status;
            }

            return markers;
        }

        private static getCustomerMarker(mapOrder: IMapOrder): google.maps.Marker
        {
            var customerLatLng: google.maps.LatLng = new google.maps.LatLng(mapOrder.order.custLat, mapOrder.order.custLon);

            if (mapOrder.customerMarker == null)
            {
                mapOrder.customerMarker = new google.maps.Marker();
                mapOrder.customerMarker.setTitle('CUSTOMER');
                mapOrder.customerMarker.setAnimation(google.maps.Animation.DROP);
                mapOrder.customerMarker.setIcon("Images/Flag.png");
            }
            mapOrder.customerMarker.setPosition(customerLatLng);

            // Make sure the marker isn't bouncing
            mapOrder.customerMarker.setAnimation(null);

            // If the order has been delivered then show the order status above the customer marker
            if (mapOrder.status != mapOrder.order.status)
            {
                if (mapOrder.order.status == 5)
                {
                    mapOrder.customerMarker.setAnimation(google.maps.Animation.BOUNCE);

                    // Figure out what text to display in the info bubble
                    var statusText = "Order delivered";

                    // We need to show an info bubble 
                    if (mapOrder.customerInfoWindow == null)
                    {
                        // Create the info bubble
                        mapOrder.customerInfoWindow = new google.maps.InfoWindow({ content: statusText });
                    }
                    else
                    {
                        // Update the info bubble
                        mapOrder.customerInfoWindow.setContent(statusText);
                    }
                    mapOrder.customerInfoWindow.open(MapServices.map, mapOrder.customerMarker);
                }
                else
                {
                    // Order not completed - make sure there is no info bubble
                    if (mapOrder.customerInfoWindow != null) mapOrder.customerInfoWindow.close();
                }
            }

            return mapOrder.customerMarker;
        }

        private static getStoreMarker(mapOrder: IMapOrder): google.maps.Marker
        {
            var storeLatLng: google.maps.LatLng = new google.maps.LatLng(mapOrder.order.storeLat, mapOrder.order.storeLon);

            if (mapOrder.storeMarker == null)
            {
                mapOrder.storeMarker = new google.maps.Marker();
                mapOrder.storeMarker.setTitle('STORE');
                mapOrder.storeMarker.setAnimation(google.maps.Animation.DROP);
                mapOrder.storeMarker.setIcon("Images/ic_place_black_24dp_2x.png");
            }
            mapOrder.storeMarker.setPosition(storeLatLng);

            // Make sure the marker isn't bouncing
            mapOrder.storeMarker.setAnimation(null);

            // Show the order status in an info bubble above the store marker
            if (mapOrder.status != mapOrder.order.status)
            {
                var statusText = null;

                // Figure out what (if any) text to display in the info bubble
                switch (mapOrder.order.status)
                {
                    case 0: statusText = "Order received"; break;
                    case 1: statusText = "Order being prepared"; break;
                    case 2: statusText = "Order in oven"; break;
                    case 3: statusText = "Order ready for dispatch"; break;
                    case 4: statusText = "Order out for delivery"; break;
                    case 6: statusText = "Order cancelled"; break;
                }

                if (statusText == null)
                {
                    // Order no longer at the store - make sure there is no info bubble
                    if (mapOrder.storeInfoWindow != null) mapOrder.storeInfoWindow.close();
                }
                else
                {
                    // The order status has changed
                    mapOrder.storeMarker.setAnimation(google.maps.Animation.BOUNCE);

                    // Show the info bubble
                    if (mapOrder.storeInfoWindow == null)
                    {
                        // Create the info bubble
                        mapOrder.storeInfoWindow = new google.maps.InfoWindow({ content: statusText });

                    }
                    else
                    {
                        // Update the info bubble
                        mapOrder.storeInfoWindow.setContent(statusText);
                    }
                    mapOrder.storeInfoWindow.open(MapServices.map, mapOrder.storeMarker);
                }
            }

            return mapOrder.storeMarker;
        }

        private static getDriverMarker(mapOrder: IMapOrder): google.maps.Marker
        {
            // Create a driver marker - it won't be visible initially
            if (mapOrder.driverMarker == null)
            {
                mapOrder.driverMarker = new google.maps.Marker();
                mapOrder.driverMarker.setTitle('DRIVER');
                mapOrder.driverMarker.setAnimation(google.maps.Animation.DROP);
                mapOrder.driverMarker.setIcon("Images/ic_local_shipping_black_24dp_2x.png");
            }

            if (mapOrder.order.status != 4)
            {
                // Hide the map marker
                mapOrder.driverMarker.setVisible(false);

                // Order not out for delivery - make sure there is no info bubble
                if (mapOrder.driverInfoWindow != null) mapOrder.driverInfoWindow.close();
            }

            return mapOrder.driverMarker;
        }

        private static refreshMapBounds()
        {
            var mapBounds: google.maps.LatLngBounds = MapServices.getMapBounds(MapServices.markers);

            MapServices.map.fitBounds(mapBounds);
        }

        private static getMapBounds(markers: google.maps.Marker[]): google.maps.LatLngBounds
        {
            var bounds: google.maps.LatLngBounds = new google.maps.LatLngBounds();

            for (var markerIndex: number = 0; markerIndex < markers.length; markerIndex++)
            {
                var marker = markers[markerIndex];
                if (marker.getVisible())
                {
                    var position: google.maps.LatLng = marker.getPosition()
                    if (position && position.lat() && position.lng())
                    {
                        bounds.extend(marker.getPosition());
                    }
                }
            }

            return bounds;
        }

        public static updateOrderLocations(mapOrders: MapOrders, orderLocations: AndroWeb.OrderTracking.IOrderLocations): boolean
        {
            if (!orderLocations.orders) return false;

            var keepTrackingOrders: boolean = false;

            for (var orderLocationIndex: number = 0; orderLocationIndex < orderLocations.orders.length; orderLocationIndex++)
            {
                var orderLocation = orderLocations.orders[orderLocationIndex];

                // Is the order out for delivery?
                if (orderLocation.status == 4)
                {
                    keepTrackingOrders = true;
                }

                // Lookup the order
                var mapOrder: IMapOrder = mapOrders.lookup[orderLocation.id];

                if (mapOrder && orderLocation.lat && orderLocation.lat != 0 && orderLocation.lon && orderLocation.lon != 0)
                {
                    // Update the driver location
                    mapOrder.orderLocation = orderLocation;

                    // Show the driver on the map
                    var driverLatLng: google.maps.LatLng = new google.maps.LatLng(mapOrder.orderLocation.lat, mapOrder.orderLocation.lon);

                    mapOrder.driverMarker.setPosition(driverLatLng);

                    if (!mapOrder.driverMarker.getVisible())
                    {
                        mapOrder.driverMarker.setAnimation(google.maps.Animation.DROP);
                    }

                    mapOrder.driverMarker.setVisible(true);

                    // Make sure the ETA dispalyed in the info bubble is kept upto date
                    MapServices.refreshETA(mapOrder);
                }
                else
                {
                    // We don't know the drivers location so hide the marker
                    if (mapOrder && mapOrder.driverMarker) mapOrder.driverMarker.setVisible(false);
                }
            }

            // Zoom to fit the map markers
            MapServices.refreshMapBounds();

            return keepTrackingOrders;
        }

        private static refreshETA(mapOrder: IMapOrder): void
        {
            if (mapOrder.driverMarker == null || mapOrder.customerMarker == null)
            {
                // Can't show the eta
                if (mapOrder.driverInfoWindow != null) mapOrder.driverInfoWindow.close();
                return;
            }

            // Work out the start and end positions of the journey so that Google can figure out the route to take
            var origin = mapOrder.driverMarker.getPosition().lat() + "," + mapOrder.driverMarker.getPosition().lng();
            var destination = mapOrder.customerMarker.getPosition().lat() + "," + mapOrder.customerMarker.getPosition().lng();

            // Get Google to figure out the route and thus the journey time
            var directionsService = new google.maps.DirectionsService;
            directionsService.route
                (
                    {
                        origin: origin,
                        destination: destination,
                        optimizeWaypoints: true,
                        travelMode: google.maps.TravelMode.DRIVING
                    },
                    function (response, status)
                    {
                        // The duration of the journey, as calcualted by Google
                        var duration: number = null;

                        // Did we get any useful data back from Google?
                        if (status == google.maps.DirectionsStatus.OK)
                        {
                            var route = response.routes == null ? null : response.routes[0];
                            if (route)
                            {
                                var leg = route.legs == null ? null : route.legs[0];
                                if (leg)
                                {
                                    if (leg.duration)
                                    {
                                        duration = leg.duration.value;
                                    }
                                }
                            }
                        }

                        if (duration == null)
                        {
                            // We were unable to get the duration of the journey so no ETA to show
                            if (mapOrder.driverInfoWindow != null) mapOrder.driverInfoWindow.close();
                            return;
                        }

                        // Figure out what text to display in the info bubble
                        var etaText = "ETA " + Math.floor((duration / 60)).toString() + " minutes";

                        // The order status has changed
                        if (mapOrder.driverInfoWindow == null)
                        {
                            // Create the info bubble
                            mapOrder.driverInfoWindow = new google.maps.InfoWindow({ content: etaText });
                        }
                        else
                        {
                            // Update the info bubble
                            mapOrder.driverInfoWindow.setContent(etaText);
                        }
                        mapOrder.driverInfoWindow.open(MapServices.map, mapOrder.driverMarker);
                    }
                );
        }
    }
}