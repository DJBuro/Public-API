module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Services", []);


    export class StoreOccasionSchedulerService
    {
        constructor()
        {

        }

        private CreateResources()
        {
            let resources = [
                {
                    title: "Occasion",
                    field: "TaskType",
                    dataSource: [
                        {
                            text: "All",
                            value: "All",
                            color: "#ffffff"
                        },
                        {
                            text: "Delivery",
                            value: "Delivery",
                            color: "#d9534f"
                        },
                        {
                            text: "Collection",
                            value: "Collection",
                            color: "#d9edf7"
                        },
                        {
                            text: "Dine in",
                            value: "Dine in",
                            color: "#f2dede"
                        }
                    ]
                },
            ];
        }

        public CreateScheduler()
        {
            
        }
    }

    app.service("storeOccasionSchedulerService", StoreOccasionSchedulerService);
}