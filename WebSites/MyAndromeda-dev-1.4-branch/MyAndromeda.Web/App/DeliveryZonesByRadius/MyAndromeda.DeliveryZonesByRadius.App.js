/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        "use strict";
        var DeliveryZonesByRadiusApp = (function () {
            function DeliveryZonesByRadiusApp() {
            }
            DeliveryZonesByRadiusApp.ApplicationName = "DeliveryZonesByRadius";
            return DeliveryZonesByRadiusApp;
        })();
        DeliveryZonesByRadius.DeliveryZonesByRadiusApp = DeliveryZonesByRadiusApp;
        var Angular = (function () {
            function Angular() {
            }
            Angular.Init = function () {
                DeliveryZonesByRadius.Logger.Notify("bootstrap-Deliveryzonesbyradius");
                var element = document.getElementById("DeliveryZonesByRadius");
                var app = angular.module(DeliveryZonesByRadiusApp.ApplicationName, [
                    "kendo.directives",
                    //"ngRoute",
                    "ngAnimate"
                ]);
                DeliveryZonesByRadius.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });
                DeliveryZonesByRadius.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
                    value(app);
                });
                angular.bootstrap(element, [DeliveryZonesByRadiusApp.ApplicationName]);
                DeliveryZonesByRadius.Logger.Notify("bootstrap-Deliveryzonesbyradius-complete");
            };
            Angular.ServicesInitilizations = [];
            Angular.ControllersInitilizations = [];
            return Angular;
        })();
        DeliveryZonesByRadius.Angular = Angular;
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
