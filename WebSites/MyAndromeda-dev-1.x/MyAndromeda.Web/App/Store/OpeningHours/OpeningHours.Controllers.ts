module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Controllers", []);
    
    app.controller("OpeningHoursController", ($scope, SweetAlert, storeOccasionSchedulerService: Services.StoreOccasionSchedulerService) => {

        let schedulerOptions = storeOccasionSchedulerService.CreateScheduler();

        $scope.schedulerOptions = schedulerOptions;

        $scope.create = () => {
            let scheduler: kendo.ui.Scheduler = $scope.OccasionScheduler;
            let startDate = scheduler.date();
            scheduler.addEvent({});

            
        }
        $scope.clearAll = () => {
           

            SweetAlert.swal({
                title: "Are you sure?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            }, (isConfirm: boolean) => {
                Logger.Notify("confirm:" + isConfirm);
                if (isConfirm) {
                    storeOccasionSchedulerService.ClearAllTasks().then(() => {
                        let scheduler: kendo.ui.Scheduler = $scope.OccasionScheduler;
                        scheduler.dataSource.data([]);

                        SweetAlert.swal("Deleted!", "All times have been deleted", "success");
                    });
                }
                else
                {
                    Logger.Notify("alert cancel");
                }
            });

           
        };
    });
} 