/// <reference path="Loyalty.Services.ts" />
/// <reference path="Loyalty.Controllers.ts" />

module MyAndromeda.Loyalty {
    export var LoyaltyName = "MyAndromedaLoyalty"
    export var Settings = {
        AndromedaSiteId : ""
    };

    var loyaltyModule = angular.module(LoyaltyName, [
        'ngRoute',
        'ngAnimate',
        "kendo.directives",
        Loyalty.ServicesName,
        Loyalty.ControllersName
    ]);

    loyaltyModule.config(($routeProvider: ng.route.IRouteProvider) => {
        
        $routeProvider.when('/', {
            templateUrl : "start.html",
            controller: StartController
        });

        $routeProvider.when("/edit/:providerName", {
            templateUrl : "edit.html",
            controller: EditLoyaltyController
        });

    });

    loyaltyModule.run(($templateCache: ng.ITemplateCacheService) => {
        console.log("Loyalty Started");

        angular.element('script[type="text/template"]').each((i, element: HTMLElement) => {
            $templateCache.put(element.id, element.innerHTML);
        });
    });

    export var Start = (element : HTMLElement) => {
        angular.bootstrap(element, [LoyaltyName]);
    };
}
 