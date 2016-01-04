/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        "use strict";
        var MenuApp = (function () {
            function MenuApp() {
            }
            MenuApp.ApplicationName = "MenuApplication";
            return MenuApp;
        })();
        Menu.MenuApp = MenuApp;
        var Angular = (function () {
            function Angular() {
            }
            Angular.Init = function () {
                Menu.Logger.Notify("bootstrap");
                var element = document.getElementById("MenuApp");
                var app = angular.module(MenuApp.ApplicationName, [
                    "kendo.directives",
                    "ngRoute",
                    "ngAnimate"
                ]);
                Menu.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });
                Menu.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
                    value(app);
                });
                app.config(['$routeProvider', function ($routeProvider) {
                        /* route: / */
                        $routeProvider.when(Menu.Controllers.ToppingsController.Route, {
                            template: Menu.Controllers.ToppingsController.Template(),
                            controller: Menu.Controllers.ToppingsController.Name
                        });
                        $routeProvider.otherwise({ redirectTo: "/" });
                    }]);
                angular.bootstrap(element, [MenuApp.ApplicationName]);
                Menu.Logger.Notify("bootstrap-complete");
            };
            Angular.ServicesInitilizations = [];
            Angular.ControllersInitilizations = [];
            return Angular;
        })();
        Menu.Angular = Angular;
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
