/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

module MyAndromeda.Menu {
    "use strict";

    interface IAppCallbacks {
        (app: ng.IModule): void;
    }

    export class MenuApp {
        public static ApplicationName: string = "MenuApplication";
    }

    export class Angular {
        public static ServicesInitilizations: IAppCallbacks[] = [];
        public static ControllersInitilizations: IAppCallbacks[] = [];

        public static Init() {
            Logger.Notify("bootstrap");

            var element = document.getElementById("MenuApp");
            var app = angular.module(MenuApp.ApplicationName, [
                "kendo.directives",
                "ngRoute",
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

            app.config(['$routeProvider', function ($routeProvider: ng.route.IRouteProvider) {

                /* route: / */
                $routeProvider.when(Menu.Controllers.ToppingsController.Route, {
                    template: Controllers.ToppingsController.Template(),
                    controller: Controllers.ToppingsController.Name
                });

                $routeProvider.otherwise({ redirectTo: "/" });
            }]);

            angular.bootstrap(element, [MenuApp.ApplicationName]);

            Logger.Notify("bootstrap-complete");
        }
    }
}