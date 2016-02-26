module MyAndromeda.Stores.OpeningHours.Services {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Services", []);


    export class StoreOccasionSchedulerService
    {
        constructor(private $http: ng.IHttpService)
        {

        }

        private CreateDataSource()
        {
            let schema: kendo.data.DataSourceSchema = {
                data: "Data",
                total: "Total",
                model: Models.getSchedulerDataSourceSchema()
            };
            
            let dataSource = new kendo.data.SchedulerDataSource({
                batch: false,
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        //let route = "/api/chain/{chainId}/store/{andromedaSiteId}/Occasions";
                        let route = "/api/chain/{0}/store/{1}/Occasions";
                        route = kendo.format(route, settings.chainId, settings.andromedaSiteId);

                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success(callback.data);
                        });
                    },
                    update: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler update");
                        let route = "/api/chain/{0}/store/{1}/update-occasion";
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success();
                        });
                    },
                    create: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler create");
                        Logger.Notify(options.data);
                        let route = "/api/chain/{0}/store/{1}/add-occasion";
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("Create response:");
                            Logger.Notify(callback.data);
                            options.success(callback.data);
                        });
                    },
                    destroy: (options: kendo.data.DataSourceTransportOptions) => {
                        let route = "/api/chain/{0}/store/{1}/delete-occasion";
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success(callback.data);
                        });
                    }
                },
                schema: schema
            });

            return dataSource;     
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

            return resources;
        }

        public CreateScheduler()
        {
            let dataSource = this.CreateDataSource();
            let schedulerOptions: kendo.ui.SchedulerOptions = {
                date: new Date(),
                majorTick: 60,
                minorTickCount: 1,
                workWeekStart: 0,
                workWeekEnd: 6,
                allDaySlot: true,
                dataSource: dataSource,
                timezone: "Europe/London",
                currentTimeMarker: {
                    useLocalTimezone: false
                },
                editable: true,
                pdf: {
                    fileName: "Opening hours",
                    title: "Schedule"
                },
                eventTemplate: "<employee-task task='dataItem'></employee-task>",
                toolbar: ["pdf"],
                showWorkHours: false,
                resources: this.CreateResources(),
                views: [
                    { type: "week", selected: true, showWorkHours: false },
                ],
                resize: (e) => { Logger.Notify("resize"); Logger.Notify(e); },
                resizeEnd: (e) => { Logger.Notify("resize-end"); Logger.Notify(e); },
                move: (e) => { Logger.Notify("move"); Logger.Notify(e); },
                moveEnd: (e) => { Logger.Notify("move-end"); Logger.Notify(e); },
                add: (e) => { Logger.Notify("add"); Logger.Notify(e); },
                save: (e) => { Logger.Notify("save"); Logger.Notify(e); }

            }

            return schedulerOptions;
        }
    }

    app.service("storeOccasionSchedulerService", StoreOccasionSchedulerService);
}