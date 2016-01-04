/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

module MyAndromeda.WebOrdering {
    "use strict";

    interface IAppCallbacks {
        (app: ng.IModule): void;
    }

    export class WebOrderingApp {
        public static ApplicationName: string = "WebOrderingApplication";
    }

    export class Angular {
        public static ServicesInitilizations: IAppCallbacks[] = [];
        public static ControllersInitilizations: IAppCallbacks[] = [];


        public static Init() {
            Logger.Notify("bootstrap-WebOrdering");

            var element = document.getElementById("WebOrdering");
            var app = angular.module(WebOrderingApp.ApplicationName, [
                "kendo.directives",
                //"ngRoute",
                "ngAnimate",
                "ngSanitize"
            ]);


            Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
            Angular.ServicesInitilizations.forEach((value) => {
                value(app);
            });

            Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
            Angular.ControllersInitilizations.forEach((value) => {
                value(app);
            });

            //app.config([
            //    Services.ContextService.Name, 
            //    function(contextService){
            //        //stuff
            //    }
            //]);

            //app.config(['$routeProvider', function ($routeProvider: ng.route.IRouteProvider) {
            //    $routeProvider.when(WebOrdering.Controllers.PickThemeController.Route, {
            //        controller: Controllers.PickThemeController.Name
            //    });

            //    $routeProvider.otherwise({ redirectTo: "/" });
            //}]);

            angular.bootstrap(element, [WebOrderingApp.ApplicationName]);

            Logger.Notify("bootstrap-WebOrdering-complete");
        }
    }
} 