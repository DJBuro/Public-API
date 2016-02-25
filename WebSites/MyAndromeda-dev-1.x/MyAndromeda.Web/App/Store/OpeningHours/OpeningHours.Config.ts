module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Config", [
        "MyAndromeda.Store.OpeningHours.Controllers",
        "MyAndromeda.Store.OpeningHours.Services",
        "MyAndromeda.Store.OpeningHours.Directives"
    ]);


    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {
        var start: ng.ui.IState = {
            url: '/:andromedaSiteId',
            controller: "OpeningHoursController",
            template: '<div id="masterUI" ui-view="main"></div>'
        };


        $stateProvider.state("opening-hours", start);
    }

    
} 