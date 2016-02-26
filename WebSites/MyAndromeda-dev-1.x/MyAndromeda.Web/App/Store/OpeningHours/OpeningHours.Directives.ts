module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Directives", []);

    app.directive("occasionTask", () => {

        return {

            name: "occasionTaskController",
            controller: "occasionTaskController"

        };

    });

    app.controller("occasionTaskController", () => { });
} 