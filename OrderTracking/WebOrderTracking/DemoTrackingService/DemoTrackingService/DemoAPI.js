var serviceUrl = 'http://localhost/DemoTrackingService/DemoTrackingService.svc';
// 'DemoTrackingService.svc';
var map = null;
var storeMarker = null;
var customerMarker = null;
var driverMarker = null;

$().ready(function () {
    $.ajaxSetup({
        error: DisplayError
    });

    initializeMap();
});

function DisplayError(x, e) {
    $('span#error').text(e);
}

function DisplayDebugMessage(e) {
//    $('span#debugMessage').text(e);
}

function initializeMap() {

    var latlng = new google.maps.LatLng(-34.397, 150.644);
    var myOptions = {
        zoom: 8,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
}

function DisplayCommsError(x, e) {

    if (x.status == 0) {
        // You are offline! Please check your network connection.
        DisplayError(null, 'Désolé. Nous sommes incapables de trouver votre commande.');
    } else if (x.status == 404) {
        // Requested URL not found
        DisplayError(null, 'URL demandée ne se trouve pas');
    } else if (x.status == 500) {
        // Internel Server Error
        DisplayError(null, 'Internal Server Error');
    } else if (e == 'parsererror') {
        // Error.  Parsing JSON Request failed.
        DisplayError(null, 'Erreur. Parsing JSON demande a échoué.');
    } else if (e == 'timeout') {
        // Request Time out
        DisplayError(null, 'Demande Time out.');
    } else {
        // Unknown Error
        DisplayError(null, 'Erreur inconnue');
    }

    $.ajaxSetup({
        error: DisplayError
    });
}

function Submit() {

    $.ajaxSetup({
        error: DisplayCommsError
    });

    DisplayDebugMessage("Calling server");

    $.getJSON(
        serviceUrl + '/TrackOrder',
        { customerCredentials: document.getElementById('credentials').value },
        GotOrders);
}

function GotOrders(json) {

    DisplayDebugMessage("Got response from server");

    $.ajaxSetup({
        error: DisplayError
    });

    RefreshLastUpdated();

    var gotOrder = false;

    var latLonBounds = new google.maps.LatLngBounds();

    if (json != null && json.orders != null && json.orders.length > 0) {

        var order = json.orders[0];

        //                alert(json.Status);
        $('span#orderStatus').text(GetOrderStatusText(order.Status));

        //               alert(json.StoreLat);
        $('span#storeLat').text(order.StoreLat);

        //               alert(json.StoreLon);
        $('span#storeLon').text(order.StoreLon);

        if (order.StoreLat.length > 0 && order.StoreLon.length > 0) {

            var storeLatLon = new google.maps.LatLng(order.StoreLat, order.StoreLon);

            if (storeMarker == null) {
                // Store
                storeMarker = new google.maps.Marker({
                    position: storeLatLon,
                    title: "Boutique"
                });
                storeMarker.setMap(map);
            }
            else {
                storeMarker.position = storeLatLon;
            }

            latLonBounds.extend(storeLatLon);
        }

        //               alert(json.CustLat);
        $('span#custLat').text(order.CustLat);

        //               alert(json.CustLon);
        $('span#custLon').text(order.CustLon);

        if (order.CustLat.length > 0 && order.CustLon.length > 0) {

            var customerLatLon = new google.maps.LatLng(order.CustLat, order.CustLon);

            if (customerMarker == null) {
                // Customer
                customerMarker = new google.maps.Marker({
                    position: customerLatLon,
                    title: "client"
                });
                customerMarker.setMap(map);
            }
            else {
                customerMarker.position = customerLatLon;
            }

            latLonBounds.extend(customerLatLon);
        }

        //               alert(json.PersonProcessing);
        $('span#personProcessing').text(order.PersonProcessing);

        //               alert(json.Lat);
        $('span#lat').text(order.Lat);

        //               alert(json.Lon);
        $('span#lon').text(order.Lon);

        if (order.Lat.length > 0 && order.Lon.length > 0) {

            var driverLatLon = new google.maps.LatLng(order.Lat, order.Lon);

            if (driverMarker == null) {
                // Driver
                driverMarker = new google.maps.Marker({
                    position: driverLatLon,
                    title: "pilote"
                });
                driverMarker.setMap(map);
            }
            else {
                driverMarker.position = driverLatLon;
            }

            latLonBounds.extend(driverLatLon);
        }

        map.fitBounds(latLonBounds);
    }
    else if (json != null && json.error != null) {
        DisplayError(null, json.Message);
    }
    else {
        // Sorry. We are unable to find your order.
        DisplayError(null, "Désolé. Nous sommes incapables de trouver votre commande."); 
    }

    //               alert("Starting polling timer");

    if (gotOrder) {
        window.setTimeout(DoPoll, 2000);
    }

    //               alert("Got Order Finished");
}

function DoPoll() {

    //               alert("Poll");

    $.getJSON(
                    serviceUrl + '/OrderLocation',
                    { customerCredentials: document.getElementById('credentials').value },
                    GotLocation);

    window.setTimeout(DoPoll, 2000);
}

function GotLocation(json) {

    //                alert("Got Location");

    RefreshLastUpdated();

    if (json.orders.length > 0) {

        var order = json.orders[0];

        $('span#orderStatus').text(GetOrderStatusText(order.Status));
        $('span#personProcessing').text(order.PersonProcessing);
        $('span#lat').text(order.Lat);
        $('span#lon').text(order.Lon);

        if (order.Status == 1) {
            $('#status1').attr('src', 'Images/1Active.jpg');
            $('#status2').attr('src', 'Images/2.jpg');
            $('#status3').attr('src', 'Images/3.jpg');
            $('#status4').attr('src', 'Images/4.jpg');
            $('#status5').attr('src', 'Images/5.jpg');
        }
        else if (order.Status == 2) {
            $('#status1').attr('src', 'Images/1Complete.jpg');
            $('#status2').attr('src', 'Images/2Active.jpg');
            $('#status3').attr('src', 'Images/3.jpg');
            $('#status4').attr('src', 'Images/4.jpg');
            $('#status5').attr('src', 'Images/5.jpg');
        }
        else if (order.Status == 3) {
            $('#status1').attr('src', 'Images/1Complete.jpg');
            $('#status2').attr('src', 'Images/2Complete.jpg');
            $('#status3').attr('src', 'Images/3Active.jpg');
            $('#status4').attr('src', 'Images/4.jpg');
            $('#status5').attr('src', 'Images/5.jpg');
        }
        else if (order.Status == 4) {
            $('#status1').attr('src', 'Images/1Complete.jpg');
            $('#status2').attr('src', 'Images/2Complete.jpg');
            $('#status3').attr('src', 'Images/3Complete.jpg');
            $('#status4').attr('src', 'Images/4Active.jpg');
            $('#status5').attr('src', 'Images/5.jpg');
        }
        else if (order.Status == 5) {
            $('#status1').attr('src', 'Images/1Complete.jpg');
            $('#status2').attr('src', 'Images/2Complete.jpg');
            $('#status3').attr('src', 'Images/3Complete.jpg');
            $('#status4').attr('src', 'Images/4Complete.jpg');
            $('#status5').attr('src', 'Images/5Active.jpg');
        }
    }

    //               alert("Got Location Finished");
}

function RefreshLastUpdated() {
    var now = new Date();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();

    $('span#lastUpdated').text(((hours < 10) ? "0" : "") + hours + ":" + ((minutes < 10) ? ":0" : ":") + minutes + ":" + ((seconds < 10) ? ":0" : ":") + seconds);
}

function GetOrderStatusText(orderStatus) {
    var statusText;

    switch (orderStatus) {
        case "1":
            statusText = "Order taken";
            break;
        case "2":
            statusText = "Order made";
            break;
        case "3":
            statusText = "Ready for dispatch";
            break;
        case "4":
            statusText = "Out for delivery";
            break;
        case "5":
            statusText = "Delivered";
            break;
        default:
            statusText = "Unknown status";
    }

    return statusText;
}