module MyAndromeda.Stores.OpeningHours.Services {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Services", []);

    class StoreOccasionAvailabilityService {
        constructor(private scheduler: kendo.ui.Scheduler) {

        }

        private GetTasksInRange(start: Date, end: Date) {
            let occurences: Models.IOccasionTask[] = this.scheduler.occurrencesInRange(start, end);

            return occurences;
        }

        private GetTasksByResource(start: Date, end: Date, task: Models.IOccasionTask) {
            var context = {
                start: start,
                end: end,
                task: task
            };

            var currentTasks = this.GetTasksInRange(start, end)
                .filter(e=> e.Id !== task.Id && e.RecurrenceId !== task.Id);


            let startCheck = start.toLocaleTimeString();
            let endCheck = end.toLocaleTimeString();

            Logger.Notify("startCheck : " + startCheck + " | endCheck: " + endCheck);
            Logger.Notify(context);
            Logger.Notify("Tasks in range: " + currentTasks.length);

            Logger.Notify(currentTasks);

            let matchedOccurences: Models.IOccasionTask[] = [];
            let flagResource: string[] = [];

            //let allResources = [
            //    Models.occasionDefinitions.Delivery,
            //    Models.occasionDefinitions.Collection,
            //    Models.occasionDefinitions.DineIn
            //];
            let taskResources = task.Occasions ? task.Occasions.split(',') : [];

            Logger.Notify("check resources: ");
            Logger.Notify(taskResources);

            let map = currentTasks.map(e=> {
                return {
                    task: e,
                    occasion: e.Occasions.split(',')
                }
            });

            Rx.Observable.fromArray(map).where(e=> {

                for (let i = 0; i < e.occasion.length; i++) {
                    let compareOccasion = e.occasion[i];

                    for (let k = 0; k < taskResources.length; k++) {
                        let occasion = taskResources[k];

                        if (occasion.indexOf(compareOccasion) > -1) {
                            Logger.Notify("task objection: " + e.task.title);
                            Logger.Notify(e.task);
                            return true;
                        }
                    }
                }

                return false;
            }).subscribe((overlaped) => {
                matchedOccurences.push(overlaped.task);
            });

            Logger.Notify("occurrences: " + matchedOccurences.length);

            return matchedOccurences.length === 0;
        }

        public IsOccasionAvailable(start, end, task) {
            return this.GetTasksByResource(start, end, task);
        }
    }

    export class StoreOccasionSchedulerService {
        constructor(private $http: ng.IHttpService, private uuidService: MyAndromeda.Services.UUIdService, private SweetAlert) {

        }

        private CreateDataSource() {
            let schema: kendo.data.DataSourceSchema = {
                data: "Data",
                total: "Total",
                model: Models.getSchedulerDataSourceSchema()
            };

            let dataSource = new kendo.data.SchedulerDataSource({
                batch: false,
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        Logger.Notify("Scheduler read");

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
                        route = kendo.format(route, settings.chainId, settings.andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success();
                        });
                    },
                    create: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler create");
                        Logger.Notify(options.data);

                        let route = "/api/chain/{0}/store/{1}/update-occasion";
                        route = kendo.format(route, settings.chainId, settings.andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("Create response:");
                            Logger.Notify(callback.data);
                            options.success(callback.data);
                        });
                    },
                    destroy: (options: kendo.data.DataSourceTransportOptions) => {

                        let route = "/api/chain/{0}/store/{1}/delete-occasion";
                        route = kendo.format(route, settings.chainId, settings.andromedaSiteId);
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

        private CreateResources() {
            let resources = [
                {
                    title: "Occasion",
                    field: "Occasions",
                    multiple: true,
                    dataSource: [
                        {
                            text: Models.occasionDefinitions.Delivery.Name,
                            value: Models.occasionDefinitions.Delivery.Name,
                            color: Models.occasionDefinitions.Delivery.Colour
                        },
                        {
                            text: Models.occasionDefinitions.Collection.Name,
                            value: Models.occasionDefinitions.Collection.Name,
                            color: Models.occasionDefinitions.Collection.Colour
                        },
                        {
                            text: Models.occasionDefinitions.DineIn.Name,
                            value: Models.occasionDefinitions.DineIn.Name,
                            color: Models.occasionDefinitions.DineIn.Colour
                        }
                    ]
                },
            ];

            return resources;
        }

        public CreateScheduler() {
            var uuidService = this.uuidService;
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
                editable: {
                    template: "<occasion-task-editor task='dataItem'></occasion-task-editor>"
                },
                pdf: {
                    fileName: "Opening hours",
                    title: "Schedule"
                },
                eventTemplate: "<occasion-task task='dataItem'></occasion-task>",
                toolbar: ["pdf"],
                showWorkHours: false,
                resources: this.CreateResources(),
                views: [
                    { type: "week", selected: true, showWorkHours: false },
                    { type: "month" }
                ],

                resize: function (e: any) {
                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel resize");

                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }
                },
                resizeEnd: (e: any) => {
                    Logger.Notify("resize-end");

                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel resize");
                        this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }
                },
                move: function (e: any) {
                    Logger.Notify("move-start");
                    Logger.Notify(e);
                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                        this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                    }
                },
                moveEnd: (e) => {
                    Logger.Notify("move-end");

                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel move");
                        this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }
                },
                add: (e) => {
                    Logger.Notify("add"); Logger.Notify(e);

                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsOccasionAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel add");
                        //SweetAlert.swal("Sorry!", name + " has been saved.", "success");
                        this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }
                },
                save: (e) => {
                    Logger.Notify("save"); Logger.Notify(e);

                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsOccasionAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel save");
                        this.SweetAlert.swal("Sorry", "A task already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }

                }
            }

            return schedulerOptions;
        }
    }

    app.service("storeOccasionSchedulerService", StoreOccasionSchedulerService);
}