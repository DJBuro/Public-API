/// <reference path="Loyalty.Services.ts" />
/// <reference path="Loyalty.Controllers.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Loyalty;
    (function (Loyalty) {
        Loyalty.LoyaltyName = "MyAndromedaLoyalty";
        Loyalty.Settings = {
            AndromedaSiteId: ""
        };
        var loyaltyModule = angular.module(Loyalty.LoyaltyName, [
            'ngRoute',
            'ngAnimate',
            "kendo.directives",
            Loyalty.ServicesName,
            Loyalty.ControllersName
        ]);
        loyaltyModule.config(function ($routeProvider) {
            $routeProvider.when('/', {
                templateUrl: "start.html",
                controller: Loyalty.StartController
            });
            $routeProvider.when("/edit/:providerName", {
                templateUrl: "edit.html",
                controller: Loyalty.EditLoyaltyController
            });
        });
        loyaltyModule.run(function ($templateCache) {
            console.log("Loyalty Started");
            angular.element('script[type="text/template"]').each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        Loyalty.Start = function (element) {
            angular.bootstrap(element, [Loyalty.LoyaltyName]);
        };
    })(Loyalty = MyAndromeda.Loyalty || (MyAndromeda.Loyalty = {}));
})(MyAndromeda || (MyAndromeda = {}));
