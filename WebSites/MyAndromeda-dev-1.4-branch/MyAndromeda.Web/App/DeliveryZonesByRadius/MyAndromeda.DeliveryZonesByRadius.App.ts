/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

module MyAndromeda.DeliveryZonesByRadius {
    "use strict";

    interface IAppCallbacks {
        (app: ng.IModule): void;
    }

    export class DeliveryZonesByRadiusApp {
        public static ApplicationName: string = "DeliveryZonesByRadius";
    }

    export class Angular {
        public static ServicesInitilizations: IAppCallbacks[] = [];
        public static ControllersInitilizations: IAppCallbacks[] = [];


        public static Init() {
            Logger.Notify("bootstrap-Deliveryzonesbyradius");

            var element = document.getElementById("DeliveryZonesByRadius");
            var app = angular.module(DeliveryZonesByRadiusApp.ApplicationName, [
                "kendo.directives",
            //"ngRoute",
                "ngAnimate"
            ]);


            Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
            Angular.ServicesInitilizations.forEach((value) => {
                value(app);
            });

            Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
            Angular.ControllersInitilizations.forEach((value) => {
                value(app);
            });
         
            angular.bootstrap(element, [DeliveryZonesByRadiusApp.ApplicationName]);

            Logger.Notify("bootstrap-Deliveryzonesbyradius-complete");
        }
    }
}  