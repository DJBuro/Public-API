module MyAndromeda.Stores.OpeningHours.Services {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Services", []);

    class StoreOccasionAvailabilityService {
        constructor(private scheduler: kendo.ui.Scheduler) {

        }

        private GetTasksInRange(start: Date, end: Date) {
            let occurences: Models.IOccasionTask[] = this.scheduler.occurrencesInRange(start, end);

            let allDay = this.scheduler.dataSource.data().filter(e => {
                let task: Models.IOccasionTask = <any>e;
                if (!task.isAllDay) { return false; }

                let startOnSameDay = task.start.toDateString() == start.toDateString();
                if (startOnSameDay) {
                    return true;
                }
                let endsOnSameDay = task.end.toDateString() == end.toDateString();
                if (endsOnSameDay) {
                    return true;
                }

                return false;
            });

            let all = occurences.concat(allDay);

            return all;
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

            //Logger.Notify("startCheck : " + startCheck + " | endCheck: " + endCheck);
            //Logger.Notify(context);
            //Logger.Notify("Tasks in range: " + currentTasks.length);

            //Logger.Notify(currentTasks);

            //which occasion(s) are causing a problem.
            let matchedOccurences: Models.IOccasionTask[] = [];
            
            let taskResources = task.Occasions
                ? task.Occasions
                : [];

            //Logger.Notify("check resources: ");
            //Logger.Notify(task);
            //Logger.Notify(taskResources);

            let map = currentTasks.map(e=> {
                return {
                    task: e,
                    occasion: e.Occasions
                }
            });

            Rx.Observable.fromArray(map).where(e=> {

                for (let i = 0; i < e.occasion.length; i++) {
                    let compareOccasion = e.occasion[i];

                    for (let k = 0; k < taskResources.length; k++) {
                        let occasion = taskResources[k];

                        if (occasion.indexOf(compareOccasion) > -1) {
                            //Logger.Notify("task objection: " + e.task.title);
                            //Logger.Notify(e.task);
                            return true;
                        }
                    }
                }

                return false;
            }).subscribe((overlaped) => {
                matchedOccurences.push(overlaped.task);
            });

            //Logger.Notify("occurrences: " + matchedOccurences.length);

            return matchedOccurences;
        }

        public IsOccasionAvailable(start, end, task): Models.IOccasionTask[]  {
            return this.GetTasksByResource(start, end, task);
        }

        public IsValid(start, end) {
            let hours = Math.abs(end.getTime() - start.getTime()) / 36e5;
            Logger.Notify("Task length: " + hours);
            return (hours < 24)
        }
    }

    export class StoreOccasionSchedulerService {
        constructor(private $http: ng.IHttpService, private uuidService: MyAndromeda.Services.UUIdService, private SweetAlert) {

        }

        public ClearAllTasks() {
            let route = "/api/chain/{0}/store/{1}/remove-occasions";
            route = kendo.format(route, settings.chainId, settings.andromedaSiteId);

            let promise = this.$http.post(route, {});

            return promise;
        }

        private CreateDataSource() {
            let schema: any = {
                data: "Data",
                total: "Total",
                model: Models.getSchedulerDataSourceSchema(),
                timezone: "Etc/UTC",
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
                timezone: "Etc/UTC",
                //timezone: "Etc/UTC",
                //timezone: "Etc/Universal",
                showWorkHours: false,
                currentTimeMarker: {
                    useLocalTimezone: true
                },
                editable: {
                    template: "<occasion-task-editor task='dataItem'></occasion-task-editor>",
                    editRecurringMode: "series"
                },
                pdf: {
                    fileName: "Opening hours",
                    title: "Opening hours"
                },
                eventTemplate: "<occasion-task task='dataItem'></occasion-task>",
                allDayEventTemplate: `<occasion-task task='dataItem' all-day='"true"'></occasion-task>`,
                //toolbar: ["pdf"],
                
                resources: this.CreateResources(),
                views: [
                    { type: "week", selected: true, showWorkHours: false }
                    //{ type: "month" }
                ],

                resize: function (e: any) {
                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsValid(e.start, e.end)) {
                        Logger.Notify("too long");

                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }

                    let occasionsInSpace = tester.IsOccasionAvailable(e.start, e.end, e.event); 
                    if (occasionsInSpace.length > 0) {
                        Logger.Notify("cancel resize");

                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }
                },
                resizeEnd: (e: any) => {
                    Logger.Notify("resize-end");

                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsValid(e.start, e.end)) {
                        Logger.Notify("cancel resize");
                        this.SweetAlert.swal("Sorry", "The maximum length of the occasion is 24 hours", "error");

                        e.preventDefault();
                        return;
                    }

                    let occasionsInSpace = tester.IsOccasionAvailable(e.start, e.end, e.event);
                    if (occasionsInSpace.length > 0) {
                        Logger.Notify("cancel resize");
                        this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }
                },
                move: function (e: any) {
                    Logger.Notify("move-start");
                    Logger.Notify(e);
                    let tester = new StoreOccasionAvailabilityService(e.sender);

                    let occasionsInSpace = tester.IsOccasionAvailable(e.start, e.end, e.event);
                    if (occasionsInSpace.length > 0) {
                        this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                    }
                },
                moveEnd: (e) => {
                    Logger.Notify("move-end");

                    let tester = new StoreOccasionAvailabilityService(e.sender);
                    let occasionsInSpace = tester.IsOccasionAvailable(e.start, e.end, e.event);

                    if (occasionsInSpace.length > 0) {
                        Logger.Notify("cancel move");
                        this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }
                },
                add: (e) => {
                    Logger.Notify("add"); Logger.Notify(e);

                    var tester = new StoreOccasionAvailabilityService(e.sender);
                    if (!tester.IsValid(e.event.start, e.event.end)) {
                        Logger.Notify("cancel add");
                        this.SweetAlert.swal("Sorry", "The maximum length of the occasion is 24 hours", "error");

                        e.preventDefault();
                        return;
                    }

                    let occasionsInSpace = tester.IsOccasionAvailable(e.event.start, e.event.end, e.event);
                    if (occasionsInSpace.length > 0) {
                        Logger.Notify("cancel add");
                        //SweetAlert.swal("Sorry!", name + " has been saved.", "success");
                        this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");

                        e.preventDefault();
                    }
                },
                save: (e) => {
                    Logger.Notify("save");
                    Logger.Notify(e.event);

                    Logger.Notify("start time");
                    Logger.Notify(e.event.start);
                    Logger.Notify("end time");
                    Logger.Notify(e.event.end);

                    let ev: any = e.event;
                    if (ev.Occasions)
                    {
                        let o = ev.Occasions.length;
                        if (o.length === 0) {
                            this.SweetAlert.swal("occasions", "Please add at least one occasion", "information");
                        }
                    }

                    var tester = new StoreOccasionAvailabilityService(e.sender);

                    if (!tester.IsValid(e.event.start, e.event.end)) {
                        Logger.Notify("cancel save");
                        this.SweetAlert.swal("Sorry", "The maximum length of the occasion is 24 hours", "error");

                        e.preventDefault();
                        return;
                    }

                    let occasionsInSpace = tester.IsOccasionAvailable(e.event.start, e.event.end, e.event);
                    if (occasionsInSpace.length > 0) {
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