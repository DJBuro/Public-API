/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        "use strict";

        var WebOrderingApp = (function () {
            function WebOrderingApp() {
            }
            WebOrderingApp.ApplicationName = "WebOrderingApplication";
            return WebOrderingApp;
        })();
        WebOrdering.WebOrderingApp = WebOrderingApp;

        var Angular = (function () {
            function Angular() {
            }
            Angular.Init = function () {
                WebOrdering.Logger.Notify("bootstrap-WebOrdering");

                var element = document.getElementById("WebOrdering");
                var app = angular.module(WebOrderingApp.ApplicationName, [
                    "kendo.directives",
                    "ngAnimate"
                ]);

                WebOrdering.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });

                WebOrdering.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
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

                WebOrdering.Logger.Notify("bootstrap-WebOrdering-complete");
            };
            Angular.ServicesInitilizations = [];
            Angular.ControllersInitilizations = [];
            return Angular;
        })();
        WebOrdering.Angular = Angular;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
