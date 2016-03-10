module MyAndromeda.Hr.Services {
    var app = angular.module("MyAndromeda.Hr.Services", []);

    export class EmployeeServiceState
    {
        public AndromedaSiteId: Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);
        public ChainId : Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);


        public CurrentChainId: number;
        public CurrentAndromedaSiteId: number;

        public EditEmployee: Rx.BehaviorSubject<Models.IEmployee> = new Rx.BehaviorSubject<Models.IEmployee>(null);
        public EmployeeUpdated: Rx.Subject<Models.IEmployee> = new Rx.Subject<Models.IEmployee>();

        constructor() {

            this.ChainId.where(e=> e !== null).subscribe((e) => {
                Logger.Notify("new chain id: " + e);
                this.CurrentChainId = e;

            });

            this.AndromedaSiteId.where(e=> e !== null).subscribe((e) => {
                Logger.Notify("new Andromeda site id: " + e);
                this.CurrentAndromedaSiteId = e;
            });

        }
    }

    export class EmployeeService
    {
        public ChainEmployeeDataSource: kendo.data.DataSource;
        public StoreEmployeeDataSource: kendo.data.DataSource;
        
        private chainId: number;
        private andromedaSiteId: number; 

        public Loading: Rx.Subject<boolean> = new Rx.Subject<boolean>();
        public IsLoading: boolean = false;
        public SavingSchedule: Rx.Subject<boolean> = new Rx.Subject<boolean>();
        public Saved: Rx.Subject<boolean> = new Rx.Subject<boolean>(); 
        public Error: Rx.Subject<string> = new Rx.Subject<string>();

        constructor(private $http: ng.IHttpService,
            private employeeServiceState: EmployeeServiceState,
            private uuidService: MyAndromeda.Services.UUIdService)
        {
            this.ChainEmployeeDataSource = new kendo.data.DataSource({
                schema: {
                    model: Models.employeeDataSourceSchema
                }
            });

            this.StoreEmployeeDataSource = new kendo.data.DataSource({
                batch: false,
                schema: {
                    model: Models.employeeDataSourceSchema,
                },
                transport: {
                    read: (options) => {
                        let route = "hr/{0}/employees/{1}/list";
                        route = kendo.format(route, this.chainId, this.andromedaSiteId);

                        let promise = this.$http.get(route);

                        this.Loading.onNext(true);
                        
                        Rx.Observable.fromPromise(promise).subscribe((callback) => {
                            options.success(callback.data);

                            this.Loading.onNext(false);
                        });
                    },
                    update: (e) => {
                        Logger.Notify("Update employee records");

                        let model = e.data;
                        this.SavingSchedule.onNext(true);
                        this.Update(model, (data) => {
                            this.SavingSchedule.onNext(false);
                            e.success(data);
                        }, (data) => {
                            this.SavingSchedule.onNext(false);
                            e.error(data);
                        });
                    },
                    create: (e) => {
                        Logger.Notify("Create employee record");
                        let data = e.data;
                        this.SavingSchedule.onNext(true);
                        this.Create(data, (model) => {
                            this.SavingSchedule.onNext(false);
                            e.success(model);
                        }, (data) => {
                            this.SavingSchedule.onNext(false);
                            e.error(data);
                        });
                        
                    }
                },
                sort: { field: "ShortName", dir: "desc" }
            }); 

            this.employeeServiceState.AndromedaSiteId.where(e=> e !== null).distinctUntilChanged(e=> e).subscribe((id) => {
                Logger.Notify("new Andromeda site id : " + id);
                this.andromedaSiteId = id;
                this.StoreEmployeeDataSource.read();
            });

            this.employeeServiceState.ChainId.where(e=> e !== null).distinctUntilChanged(e=> e).subscribe((id) => {
                Logger.Notify("new chain id : " + id);
                this.chainId = id;
                this.ChainEmployeeDataSource.read();
            });

            this.Loading.subscribe((e) => {
                this.IsLoading = e;
            });
        }

        public List(chainId: number, andromedaSiteId: number): ng.IHttpPromise<Models.IEmployee[]>
        {
            var route = "";

            var pomise = this.$http.get(route);

            return pomise;
        }

        public Save(employee): JQueryPromise<any> {
            employee.set("DirtyHack", true);

            let exists = this.StoreEmployeeDataSource.data().filter((item: Models.IEmployee) => {
                return item.Id == employee.id;
            });

            if (exists.length == 0) {
                Logger.Notify("Add employee");
                this.StoreEmployeeDataSource.add(employee);
            }

            Logger.Notify("sync");

            let sync = this.StoreEmployeeDataSource.sync();

            return sync;
        }

        private Update(model, onSuccess: (data) => void, onError: (data) => void)
        {
            let route = "hr/{0}/employees/{1}/update";
            route = kendo.format(route, this.chainId, this.andromedaSiteId);

            let promise = this.$http.post(route, model);

            this.Saved.onNext(false);

            Rx.Observable.fromPromise(promise).subscribe((callback) => {
                var callBackData = callback.data;

                Logger.Notify("result: ");
                Logger.Notify(callBackData);

                onSuccess(callBackData);

                this.Saved.onNext(true);
            }, (error) => {
                Logger.Error(error);
                this.Error.onNext("Updating Failed");
            });
        }

        private Create(model, onSuccess: (data) => void, onError : (data) => void)
        {
            let route = "hr/{0}/employees/{1}/create";
            route = kendo.format(route, this.chainId, this.andromedaSiteId);

            var promise = this.$http.post(route, model);

            this.Saved.onNext(false);

            Rx.Observable.fromPromise(promise).subscribe((callback) => {
                var callBackData = callback.data;

                Logger.Notify("result: ");
                Logger.Notify(callBackData);

                onSuccess(model);

                this.Saved.onNext(true);
            }, (error) => {
                Logger.Error(error);

                this.Error.onNext("Creating Failed");
                onError(error);
            });
        }


        public GetStore(chainId: number, andromedaSiteId: number): Rx.Observable<Models.IStore[]> {
            let route = "hr/{0}/employees/{1}/get-store";

            route = kendo.format(route, chainId, andromedaSiteId);

            let promise = this.$http.get(route);
            let map = Rx.Observable.fromPromise(promise).map(s => <Models.IStore[]>s.data);

            return map;
        }


        public GetStoreListByEmployee(chainId: number, andromedaSiteId: number, employeeId: string): Rx.Observable<Models.IStore[]> {
            let route = "hr/{0}/employees/{1}/list-stores/{2}"; 

            route = kendo.format(route, chainId, andromedaSiteId, employeeId);

            let promise = this.$http.get(route);
            let map = Rx.Observable.fromPromise(promise).map(s => <Models.IStore[]>s.data);

            return map;
        }

        public GetEmployeePictureUrl(chainId: number, andromedaSiteId: number, employeeId: string) : string
        {
            //"hr/{{$stateParams.chainId}}/employees/{{$stateParams.andromedaSiteId}}/resources/{{employee.Id}}"
            let path = "hr/{0}/employees/{1}/resources/{2}/profile-pic";
            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            //let r = this.uuidService.GenerateUUID();

            //path = path + "?r=" + r;

            return path;
        }

        public GetUploadRouteUrl(chainId: number, andromedaSiteId: number, employeeId: string) : string
        {
            let path = "hr/{0}/employees/{1}/resources/{2}/update-profile-pic";
            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path; 
        }

        public GetDocumentUploadRoute(chainId: number, andromedaSiteId, employeeId: string, documentId: string): string
        {
            let path = "hr/{0}/employees/{1}/resources/{2}/update-document/{3}";
            path = kendo.format(path, chainId, andromedaSiteId, employeeId, documentId);

            return path;  
        }

        public GetDocumentRouteUrl(chainId: number, andromedaSiteId, employeeId: string, documentId: string, fileName: string): string {
            let path = "hr/{0}/employees/{1}/resources/{2}/document/{3}/{4}";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId, documentId, fileName);

            return path;
        }

        public GetDocumentDownloadRouteUrl(chainId: number, andromedaSiteId, employeeId: string, documentId: string, fileName: string): string {
            let path = "hr/{0}/employees/{1}/resources/{2}/document/{3}/download/{4}";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId, documentId, fileName);

            return path;
        }

        public GetDataSourceForStoreScheduler(chainId: number, andromedaSiteId: number)
        {
            let schema: kendo.data.DataSourceSchema = {
                data: "Data",
                total: "Total",
                model: Models.getSchedulerDataSourceSchema(andromedaSiteId, this, undefined)
            };

            let dataSource = new kendo.data.SchedulerDataSource({
                batch: false,
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        let route = this.GetStoreEmployeeSchedulerReadRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success(callback.data);
                        });
                    },
                    update: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler update");
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success();
                        });
                    },
                    create: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler create");
                        Logger.Notify(options.data);
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("Create response:");
                            Logger.Notify(callback.data);
                            options.success(callback.data);
                        });
                    },
                    destroy: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("GetEmployeeSchedulerDestroyRoute");
                        Logger.Notify(options.data);

                        let route = this.GetEmployeeSchedulerDestroyRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("destroy response:");
                            Logger.Notify(callback.data);

                            options.success(callback.data);
                        });
                    }
                },
                //sort: [
                //    { field: "Department", dir: "asc" }
                //],
                schema: schema
            });

            dataSource.sort({
                field: "EmployeeId",
                dir : "desc"
            });

            return dataSource;
        }

        public GetDataSourceForEmployeeScheduler(chainId: number, andromedaSiteId: number, employeeId: string) {
            let schema: kendo.data.DataSourceSchema = {
                data: "Data",
                total: "Total",
                model: Models.getSchedulerDataSourceSchema(andromedaSiteId, this, employeeId)
            };

            let dataSource = new kendo.data.SchedulerDataSource({
                batch: false,
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        let route = this.GetEmployeeSchedulerReadRoute(chainId, andromedaSiteId, employeeId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success(callback.data);
                        });
                    },
                    update: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler update");
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success();
                        });
                    },
                    create: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler create");
                        Logger.Notify(options.data);
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("Create response:");
                            Logger.Notify(callback.data);
                            options.success(callback.data);
                        });
                    },
                    destroy: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("GetEmployeeSchedulerDestroyRoute");
                        Logger.Notify(options.data);

                        let route = this.GetEmployeeSchedulerDestroyRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("destroy response:");
                            Logger.Notify(callback.data);

                            options.success(callback.data);
                        });
                    }
                },
                schema: schema
            });

            return dataSource;

        }

        private GetStoreEmployeeSchedulerReadRoute(chainId: number, andromedaSiteId: number)
        {
            let path = "/hr/{0}/employees/{1}/schedule/store-list";

            path = kendo.format(path, chainId, andromedaSiteId);

            return path;
        }

        private GetEmployeeSchedulerReadRoute(chainId: number, andromedaSiteId, employeeId: string)
        {
            let path = "/hr/{0}/employees/{1}/schedule/list/{2}";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path;
        }

        private GetEmployeeSchedulerUpdateRoute(chainId: number, andromedaSiteId)
        {
            let path = "/hr/{0}/employees/{1}/schedule/update";

            path = kendo.format(path, chainId, andromedaSiteId);

            return path;
        }

        private GetEmployeeSchedulerDestroyRoute(chainId: number, andromedaSiteId)
        {
            let path = "/hr/{0}/employees/{1}/schedule/destroy";

            path = kendo.format(path, chainId, andromedaSiteId);

            return path;
        }
    }

    export class EmployeeSchedulerService {

        public saving: Rx.Subject<boolean> = new Rx.Subject<boolean>();

        constructor(private employeeServiceState: EmployeeServiceState, private employeeService: Services.EmployeeService, private SweetAlert)
        {

        }

        private GetResources(stores: Models.IStore[], employee? :Models.IEmployee): Array<{}>
        {
            let employeePart = () => {
                var part = {
                    field: "EmployeeId",
                    dataTextField: "ShortName",
                    dataValueField: "Id",
                    title: "Employee",
                    name: "Employee",
                    dataSource: <any>[]
                }

                if (employee)
                {
                    part.dataSource = [employee];
                    return part;
                }

                let employees = this.employeeService.StoreEmployeeDataSource;
                part.dataSource = employees;

                return part;
            };

            let departmentAvailable = false;
            if (employee) {
                if (employee.Department) {
                    Logger.Notify("department: " + employee.Department);
                    departmentAvailable = true;
                }
            }
            
            //= employee && employee.Department;
            let resources = [
                {
                    title: "Task",
                    field: "TaskType",
                    dataSource: [
                        {
                            text: "Normal Shift",
                            value: "Shift",
                            color: "#ffffff"
                        },
                        {
                            text: "Need cover",
                            value: "Need cover",
                            color: "#d9534f"
                        },
                        {
                            text: "Covering Shift",
                            value: "Covering Shift",
                            color: "#d9edf7"
                        },
                        {
                            text: "Unplanned leave",
                            value: "Unplanned",
                            color: "#f2dede"
                        },
                        {
                            text: "Planned leave",
                            value: "Planned leave",
                            color: "#fcf8e3"
                        }
                    ]
                },
                //{
                //    name: "Department",
                //    field: "Department",
                //    dataValueField: "text",
                //    dataTextField: "text",
                //    dataSource: Models.departments
                //    //dataSource: departmentAvailable
                //    //    ? [{ text: employee.Department }]
                //    //    : [{ text: "NA" }]
                //},
                employeePart(),
                
                {
                    name: "Store",
                    title: "Store",
                    field: "AndromedaSiteId",
                    dataSource: stores,
                    dataValueField: "AndromedaSiteId",
                    dataTextField: "Name"
                }
            ];



            return resources;
        }

        public GetStoreEmployeeScheduler(stores: Array<Models.IStore>)
        {
            let start = new Date();
            let end = new Date();

            start.setHours(0);
            end.setHours(24);

            let chainId = this.employeeServiceState.CurrentChainId,
                andromedaSiteId = this.employeeServiceState.CurrentAndromedaSiteId,
                currentStore = stores[0],
                dataSource = this.employeeService.GetDataSourceForStoreScheduler(chainId, andromedaSiteId);
                
            let employeeGroupTemplate = `
                <div>
                    #=text#
                </div>
                <div>
                    <span class="label" style="background-color:#=majorColor#">#=employee.Department #</span>
                </div>
                <div>
                    <span class="label" style="background-color:#=minorColor#">#=employee.PrimaryRole #</span>
                </div>
            `;

            let schedulerOptions: kendo.ui.SchedulerOptions = {
                date: new Date(),
                workDayStart: start,
                workDayEnd: end,
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
                    fileName: currentStore.Name + " schedule",
                    title: "Schedule"
                },
                //groupHeaderTemplate: "<div>#=text#</div>",
                groupHeaderTemplate: employeeGroupTemplate,
                toolbar: ["pdf"],
                showWorkHours: false,
                resources: this.GetResources(stores),
                views: [
                    {
                        type: "day", showWorkHours: false,
                        eventTemplate: "<employee-task task='dataItem'></employee-task>"
                    },
                    //{ type: "week", selected: true, showWorkHours: false },
                    //{ type: "month", showWorkHours: false },
                    //{ type: "timeline", showWorkHours: false },
                    //{
                    //    title: "Week thing",
                    //    eventHeight: 100,
                    //    slotTemplate: "<div style='background-color:\\#FFF'; height: 100%;width: 100%;'></div>",
                    //    selected: true,
                    //    majorTick: 1440,
                    //    minorTickCount: 1,
                    //    type: "kendo.ui.SchedulerTimelineWeekView", showWorkHours: false,
                    //    group: {
                    //        orientation: "vertical",
                    //        resources: ["Employee"]
                    //    }
                    //},
                    <any>{
                        title: "Week Overview",
                        selected: true,
                        eventHeight: 20,
                        type: "kendo.ui.MonthTimeWeekView", 
                        eventTemplate: "<working-task task='dataItem'></working-task>",
                        //dayTemplate: "",
                        dayTemplate: '#:kendo.toString(date, "dd")#',
                        
                        group: <any>{
                            orientation: "vertical",
                            resources: ["Employee"]
                        }
                    }
                ],
                
                resize: function (e: any) {
                    Logger.Notify("resize"); Logger.Notify(e);
                    let checker = new EmployeeAvailabilityTestService(e.sender);

                    let valid = checker.IsWorkAvailable(e.start, e.end, e.event);
                    if (!valid) {
                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }
                },
                resizeEnd: (e: any) => {
                    Logger.Notify("resize-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        return;
                    }

                    Logger.Notify("cancel resize");

                    this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                    e.preventDefault();
                },
                move: function (e: any) {
                    Logger.Notify("move"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        return;
                    }

                    this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                },
                moveEnd: (e) => {
                    Logger.Notify("move-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        return;
                    }

                    Logger.Notify("cancel move");

                    this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                    e.preventDefault();
                },
                add: (e) => {
                    Logger.Notify("add"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel add");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                save: (e) => {
                    Logger.Notify("save"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel save");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                }


            }

            return schedulerOptions;
        }

        public GetSingleEmployeeScheduler(stores: Array<Models.IStore>, employee: Models.IEmployee)
        {
            let start = new Date();
            let end = new Date();

            start.setHours(0);
            end.setHours(24);      

            let chainId = this.employeeServiceState.CurrentChainId,
                andromedaSiteId = this.employeeServiceState.CurrentAndromedaSiteId,
                dataSource = this.employeeService.GetDataSourceForEmployeeScheduler(chainId, andromedaSiteId, employee.Id);
            
            let schedulerOptions: kendo.ui.SchedulerOptions = {
                date: new Date(),
                workDayStart: start,
                workDayEnd: end,
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
                    fileName: employee.ShortName + " schedule",
                    title: "Schedule"
                },
                eventTemplate: "<employee-task task='dataItem'></employee-task>",
                toolbar: ["pdf"],
                showWorkHours: false,
                resources: this.GetResources(stores, employee),
                views: [
                    { type: "day", showWorkHours: false },
                    { type: "week", selected: true, showWorkHours: false },
                    { type: "month", showWorkHours: false },
                    { type: "timeline", showWorkHours: false }
                ],
                resize: function (e: any) {
                    Logger.Notify("resize"); Logger.Notify(e);
                    let checker = new EmployeeAvailabilityTestService(e.sender);

                    let valid = checker.IsWorkAvailable(e.start, e.end, e.event);
                    if (!valid) {
                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }
                },
                resizeEnd: (e: any) => {
                    Logger.Notify("resize-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel resize");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                move: function (e: any) {
                    Logger.Notify("move"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                    }
                },
                moveEnd: (e) => {
                    Logger.Notify("move-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel move");

                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                add: (e) => {
                    Logger.Notify("add"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel add");
                        //SweetAlert.swal("Sorry!", name + " has been saved.", "success");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                save: (e) => {
                    Logger.Notify("save"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel save");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                }
            }

            return schedulerOptions;
        }

        public IsEmployeeFree()
        {
            //var occurrences = occurrencesInRangeByResource(start, end, "attendee", event, resources);
        }

        public OccurrencesInRangeByResource()
        {

        }

    }


    export class EmployeeAvailabilityTestService
    {
        constructor(private scheduler: kendo.ui.Scheduler) { }

        private GetTasksInRange(start: Date, end: Date) {
            let occurences: Models.IEmployeeTask[] = this.scheduler.occurrencesInRange(start, end);

            return occurences;
        }

        private CheckTasksByEmployee(start: Date, end: Date, task: Models.IEmployeeTask)
        {
            var context = {
                start: start,
                end: end,
                task: task
            };

            let startCheck = start.toLocaleTimeString();
            let endCheck = end.toLocaleTimeString();

            //only interested in current employee, which is not the current task
            var currentTasks = this.GetTasksInRange(start, end);
            Logger.Notify("Tasks in range: " + currentTasks.length);

            Logger.Notify(currentTasks);
            currentTasks = currentTasks.filter(e=> e.id !== task.id);
            Logger.Notify("Tasks in range after removing self: " + currentTasks.length);

            currentTasks = currentTasks.filter(e=> e.EmployeeId === task.EmployeeId);
            Logger.Notify("Tasks in range - by employee: " + currentTasks.length);

            Logger.Notify("startCheck : " + startCheck + " | endCheck: " + endCheck);
            Logger.Notify(context);
            Logger.Notify("Tasks in range: " + currentTasks.length);

            return currentTasks.length === 0;
        }

        public IsWorkAvailable(start, end, task)
        {
            return this.CheckTasksByEmployee(start, end, task);
        }
    }

    app.service("employeeService", EmployeeService);
    app.service("employeeServiceState", EmployeeServiceState);
    app.service("employeeSchedulerService", EmployeeSchedulerService);
}