module MyAndromeda.Hr.Services {
    var app = angular.module("MyAndromeda.Hr.Services", []);

    export class EmployeeServiceState
    {
        public ChainId : Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);

        public AndromedaSiteId: Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);

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
                Logger.Notify("new andromeda site id: " + e);
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
        public Saved: Rx.Subject<boolean> = new Rx.Subject<boolean>(); 
        public Error: Rx.Subject<string> = new Rx.Subject<string>();

        constructor(private $http: ng.IHttpService, private employeeServiceState: EmployeeServiceState, private uuidService: UUIDService)
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
                        this.Update(model, (data) => {
                            Logger.Notify("call datasource success");
                            e.success(data);
                        }, (data) => {
                            e.error(data);
                        });
                    },
                    create: (e) => {
                        Logger.Notify("Create employee record");
                        let data = e.data;
                        
                        this.Create(data, (model) => {
                            e.success(model);
                        }, (data) => {
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

        public GetDataSourceForEmployeeScheduler(chainId: number, andromedaSiteId: number, employeeId: string) {
            let schema: kendo.data.DataSourceSchema = {
                data: "Data",
                total: "Total",
                model: <kendo.data.DataSourceSchemaModel>{
                    id: "Id",
                    fields: {
                        Id: <kendo.data.DataSourceSchemaModelField>{
                            type: "string",
                            nullable: true
                        },
                        title: { from: "Title", defaultValue: "No title", validation: { required: true } },
                        start: { type: "date", from: "Start" },
                        end: { type: "date", from: "End" },
                        startTimezone: { from: "StartTimezone" },
                        endTimezone: { from: "EndTimezone" },
                        description: { from: "Description" },
                        recurrenceId: { from: "RecurrenceId" },
                        recurrenceRule: { from: "RecurrenceRule" },
                        recurrenceException: { from: "RecurrenceException" },
                        isAllDay: { type: "boolean", from: "IsAllDay" },
                        EmployeeId: <kendo.data.DataSourceSchemaModelField>{
                            type: "string",
                            defaultValue: employeeId,
                            nullable: false,
                            validation: {
                                required: true
                            }
                        },
                        AndromedaSiteId: <kendo.data.DataSourceSchemaModelField>{
                            type: "number",
                            defaultValue: andromedaSiteId,
                            nullable: false,
                            validation: {
                                required: true
                            }
                        },
                        TaskType: <kendo.data.DataSourceSchemaModelField>{
                            type: "string",
                            defaultValue: "Shift",
                            nullable: false,
                            validation: {
                                required: true
                            }
                        },
                    }
                }
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
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId, employeeId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            options.success();
                        });
                    },
                    create: (options: kendo.data.DataSourceTransportOptions) => {
                        Logger.Notify("Scheduler create");
                        Logger.Notify(options.data);
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId, employeeId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            Logger.Notify("Create response:");
                            Logger.Notify(callback.data);
                            options.success(callback.data);
                        });
                    },
                    destroy: (options: kendo.data.DataSourceTransportOptions) => {
                        throw "Matt- not implemented - scheduler destroy";
                    }
                },
                schema: schema
            });

            return dataSource;

        }

        private GetEmployeeSchedulerReadRoute(chainId: number, andromedaSiteId, employeeId: string)
        {
            let path = "/hr/{0}/employees/{1}/schedule/{2}/list";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path;
        }

        private GetEmployeeSchedulerUpdateRoute(chainId: number, andromedaSiteId, employeeId: string)
        {
            let path = "/hr/{0}/employees/{1}/schedule/{2}/update";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path;
        }

        private GetEmployeeSchedulerDestroyRoute(chainId: number, andromedaSiteId, employeeId: string)
        {
            let path = "/hr/{0}/employees/{1}/schedule/{2}/destroy";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path;
        }
    }

    export class UUIDService {
        constructor() {

        }

        public GenerateUUID() {
            var d = new Date().getTime();
            if (window.performance && typeof window.performance.now === "function") {
                d += performance.now(); //use high-precision timer if available
            }
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
            return uuid;

        }
    }

    app.service("employeeService", EmployeeService);
    app.service("employeeServiceState", EmployeeServiceState);
    app.service("uuidService", UUIDService);
}