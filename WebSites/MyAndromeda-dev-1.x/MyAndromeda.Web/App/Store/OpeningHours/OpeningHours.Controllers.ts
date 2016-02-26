module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Controllers", []);
    
    app.controller("OpeningHoursController", ($scope, storeOccasionSchedulerService: Services.StoreOccasionSchedulerService) => {

        let schedulerOptions = storeOccasionSchedulerService.CreateScheduler();
        $scope.schedulerOptions = schedulerOptions

    });
} 