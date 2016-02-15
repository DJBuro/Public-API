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
        customerMarker: google.maps.Marker;
        driverMarker: google.maps.Marker;
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

    export class App 
    {
        private static sessionId: string = null;
        private static mapOrders: IMapOrder[] = [];

        public static start(): void
        {
            // Extract the session id from the url    
            App.sessionId = App.getSessionId();

            if (App.sessionId == null)
            {
                // Display some kind of error
                return;
            }

            // Start the session
            Services.getOrders
            (
                App.sessionId,
                function (errorCode: number, allOrders: AndroWeb.OrderTracking.IOrders)
                {
                    if (errorCode == 0)
                    {
                        // Keep hold of the orders for later
                        for (var orderIndex: number = 0; orderIndex < allOrders.orders.length; orderIndex++)
                        {
                            var order: IOrder = allOrders.orders[orderIndex];

                            var mapOrder: IMapOrder =
                                {
                                    order: order,
                                    orderLocation: null,
                                    eta: null,
                                    customerMarker: null,
                                    driverMarker: null,
                                    storeMarker: null
                                };

                            // Add to the list
                            App.mapOrders.push(mapOrder);
                            // Also, add as a property so we can look it up efficently later
                            App.mapOrders[mapOrder.order.id] = mapOrder;
                        }

                        // Initialise the map
                        var success: boolean = MapServices.initialiseGoogleMap(App.mapOrders);

                        if (success)
                        {
                            App.refreshETAs(App.mapOrders);

                            // Start tracking driver locations
                            App.pollOrderLocations();
                        }
                    }
                    else
                    {
                        // There was a problem calling the web service
                        App.showError(errorCode);
                    }
                }
            );
        }

        private static refreshETAs(mapOrders: IMapOrder[]): void
        {
            for (var orderIndex: number = 0; orderIndex < mapOrders.length; orderIndex++)
            {
                var mapOrder: IMapOrder = mapOrders[orderIndex];

                //if (mapOrder.customerMarker && mapOrder.driverMarker.getVisible())
                //{
                //    MapServices.getEta
                //        (
                //            mapOrder,
                //            function (mapOrder: IMapOrder, eta: number)
                //            {
                //                mapOrder.eta = eta;
                //                MapServices.map.controls[google.maps.ControlPosition.RIGHT_TOP].push(document.getElementById('legend'));

                //                var legend = document.getElementById('legend');

                //                var div = document.createElement('div');
                //                div.innerHTML = '<h1>ETA</h1><p>' + eta + '</p>';
                //                legend.appendChild(div);
                //            }
                //        );
                //}
                //else if (mapOrder.customerMarker && mapOrder.storeMarker)
                //{
                //    MapServices.getEta
                //        (
                //            mapOrder,
                //            function (mapOrder: IMapOrder, eta: number)
                //            {
                //                mapOrder.eta = eta;
                //                MapServices.map.controls[google.maps.ControlPosition.RIGHT_TOP].push(document.getElementById('legend'));

                //                var legend = document.getElementById('legend');

                //                var div = document.createElement('div');
                //                div.innerHTML = '<h1>ETA</h1><p>' + eta + '</p>';
                //                legend.appendChild(div);
                //            }
                //        );
                //}
            }
        }

        private static pollOrderLocations(): void
        {
            // Go get the order locations
            Services.getOrderLocations
                (
                    App.sessionId,
                    function (errorCode: number, orderLocations: AndroWeb.OrderTracking.IOrderLocations)
                    {
                        if (errorCode == 0)
                        {
                            // Move the drivers around the map
                            MapServices.updateOrderLocations(App.mapOrders, orderLocations);
                        }
                        else
                        {
                            // There was a problem calling the web service
                            App.showError(errorCode);
                        }

                        // Poll again in 10 seconds
                        setTimeout(App.pollOrderLocations, 6000); 
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

        private static showError(errorCode: number): void
        {
            if (errorCode == 20)
            {
                // Invalid session id
                alert("Sorry, your order cannot be tracked");
            }
            else
            {
                // Unhandled or unknown error
                alert("There was an unexpected problem tracking your order");
            }
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

        public static initialiseGoogleMap(orders: IMapOrder[]): boolean
        {
            var success: boolean = true;

            try
            {
                MapServices.markers = MapServices.getMarkers(orders);

                if (MapServices.markers.length == 0)
                {
                    // Nothing to put on map!!
                    return;
                }

                var mapBounds: google.maps.LatLngBounds = MapServices.getMapBounds(MapServices.markers);

                var mapOptions: google.maps.MapOptions =
                    {
                        center: MapServices.markers[0].getPosition()
                    };

                MapServices.map = new google.maps.Map
                    (
                        document.getElementById('map'),
                        mapOptions
                    );

                for (var markerIndex: number = 0; markerIndex < MapServices.markers.length; markerIndex++)
                {
                    var marker = MapServices.markers[markerIndex];
                    marker.setMap(MapServices.map);
                }

                MapServices.map.setCenter(mapBounds.getCenter());
                MapServices.map.fitBounds(mapBounds);
            }
            catch (exception)
            {
                success = false;
            }

            return success;
        }

        public static updateDrivers(orderLocations: AndroWeb.OrderTracking.IOrderLocations)
        {
            orderLocations
        }

        private static getMarkers(mapOrders: IMapOrder[]): google.maps.Marker[]
        {
            var storeAndCustomerMarkers: google.maps.Marker[] = [];

            if (mapOrders.length == 0) return storeAndCustomerMarkers;

            for (var orderIndex: number = 0; orderIndex < mapOrders.length; orderIndex++)
            {
                var mapOrder: IMapOrder = mapOrders[orderIndex];
                
                if (mapOrder.order.custLat != 0 && mapOrder.order.custLon != 0)
                {
                    var customerLatLng: google.maps.LatLng = new google.maps.LatLng(mapOrder.order.custLat, mapOrder.order.custLon);

                    mapOrder.customerMarker = new google.maps.Marker();
                    mapOrder.customerMarker.setPosition(customerLatLng);
                    mapOrder.customerMarker.setTitle('CUSTOMER');
                    mapOrder.customerMarker.setAnimation(google.maps.Animation.DROP);
                    mapOrder.customerMarker.setIcon("Images/Flag.png");
                    storeAndCustomerMarkers.push(mapOrder.customerMarker);
                }

                if (mapOrder.order.storeLat != 0 && mapOrder.order.storeLon != 0)
                {
                    var storeLatLng: google.maps.LatLng = new google.maps.LatLng(mapOrder.order.storeLat, mapOrder.order.storeLon);

                    mapOrder.storeMarker = new google.maps.Marker();                    
                    mapOrder.storeMarker.setPosition(storeLatLng);
                    mapOrder.storeMarker.setTitle('STORE');
                    mapOrder.storeMarker.setAnimation(google.maps.Animation.DROP);
                    mapOrder.storeMarker.setIcon("Images/ic_place_black_24dp_2x.png");
                    storeAndCustomerMarkers.push(mapOrder.storeMarker);
                }

                // Create a driver marker - it won't be visible initially
                mapOrder.driverMarker = new google.maps.Marker();
                mapOrder.driverMarker.setTitle('DRIVER');
                mapOrder.driverMarker.setAnimation(google.maps.Animation.DROP);
                mapOrder.driverMarker.setIcon("Images/ic_local_shipping_black_24dp_2x.png");
                mapOrder.driverMarker.setVisible(false);
                storeAndCustomerMarkers.push(mapOrder.driverMarker);
            }

            return storeAndCustomerMarkers;
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
                    bounds.extend(marker.getPosition());
                }
            }

            return bounds;
        }

        public static getEta(mapOrder: IMapOrder, callback: any): void
        {
            var origin = mapOrder.order.storeLat + "," + mapOrder.order.storeLon;
            var destination = mapOrder.order.custLat + "," + mapOrder.order.custLon;

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
                    if (status === google.maps.DirectionsStatus.OK)
                    {
                        var route = response.routes[0];
                        var leg = route.legs[0];

                        callback(mapOrder, leg.duration.value);
                    }
                    else
                    {
                        window.alert('Directions request failed due to ' + status);
                    }
                }
            );
        }

        public static updateOrderLocations(mapOrders: IMapOrder[], orderLocations: AndroWeb.OrderTracking.IOrderLocations)
        {
            if (!orderLocations.orders) return;

            for (var orderLocationIndex: number = 0; orderLocationIndex < orderLocations.orders.length; orderLocationIndex++)
            {
                var orderLocation = orderLocations.orders[orderLocationIndex];

                // Lookup the order
                var mapOrder: IMapOrder = mapOrders[orderLocation.id];

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
                }
            }

            // Zoom to fit the map markers
            MapServices.refreshMapBounds();
        }
    }
}