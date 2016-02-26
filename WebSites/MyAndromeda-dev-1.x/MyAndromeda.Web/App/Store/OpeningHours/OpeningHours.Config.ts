module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Config", [
        "ui.router",
        "kendo.directives", "oitozero.ngSweetAlert",
        "MyAndromeda.Store.OpeningHours.Controllers",
        "MyAndromeda.Store.OpeningHours.Services",
        "MyAndromeda.Store.OpeningHours.Directives"
    ]);

    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {
        var start: ng.ui.IState = {
            url: '/:andromedaSiteId',
            controller: "OpeningHoursController",
            templateUrl: "OpeningHours-template.html"
        };

        $stateProvider.state("opening-hours", start);

        $urlRouterProvider.otherwise("/" + settings.andromedaSiteId);
    });
} 