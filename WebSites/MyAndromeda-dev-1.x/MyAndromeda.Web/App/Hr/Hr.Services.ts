module MyAndromeda.Hr.Services {
    var app = angular.module("MyAndromeda.Hr.Services", []);

    export class EmployeeServiceState {
        public AndromedaSiteId: Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);
        public ChainId: Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);


        public CurrentChainId: number;
        public CurrentAndromedaSiteId: number;

        public EditEmployee: Rx.BehaviorSubject<Models.IEmployee> = new Rx.BehaviorSubject<Models.IEmployee>(null);
        public EmployeeUpdated: Rx.Subject<Models.IEmployee> = new Rx.Subject<Models.IEmployee>();

        constructor() {

            this.ChainId.where(e => e !== null).subscribe((e) => {
                Logger.Notify("new chain id: " + e);
                this.CurrentChainId = e;

            });

            this.AndromedaSiteId.where(e => e !== null).subscribe((e) => {
                Logger.Notify("new Andromeda site id: " + e);
                this.CurrentAndromedaSiteId = e;
            });

        }
    }

    export class EmployeeService {
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
            private uuidService: MyAndromeda.Services.UUIdService) {
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

            this.employeeServiceState.AndromedaSiteId.where(e => e !== null).distinctUntilChanged(e => e).subscribe((id) => {
                Logger.Notify("new Andromeda site id : " + id);
                this.andromedaSiteId = id;
                this.StoreEmployeeDataSource.read();
            });

            this.employeeServiceState.ChainId.where(e => e !== null).distinctUntilChanged(e => e).subscribe((id) => {
                Logger.Notify("new chain id : " + id);
                this.chainId = id;
                this.ChainEmployeeDataSource.read();
            });

            this.Loading.subscribe((e) => {
                this.IsLoading = e;
            });
        }

        public List(chainId: number, andromedaSiteId: number): ng.IHttpPromise<Models.IEmployee[]> {
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

        private Update(model: Models.IEmployee, onSuccess: (data) => void, onError: (data) => void) {
            let route = "hr/{0}/employees/{1}/update";
            route = kendo.format(route, this.chainId, this.andromedaSiteId);

            Logger.Notify("DOB - ");
            if (model.DateOfBirth) {
                var offsetMiliseconds = new Date().getTimezoneOffset() * 60000;
                var a = kendo.toString(model.DateOfBirth, "Y");

                Logger.Notify(model.DateOfBirth);
                Logger.Notify(a);
                //Logger.Notify(b);
                Logger.Notify(offsetMiliseconds);

                model.DateOfBirth = new Date(model.DateOfBirth.getTime() + offsetMiliseconds);

                Logger.Notify(model.DateOfBirth);
            }

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

        private Create(model, onSuccess: (data) => void, onError: (data) => void) {
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

        public GetEmployeePictureUrl(chainId: number, andromedaSiteId: number, employeeId: string): string {
            //"hr/{{$stateParams.chainId}}/employees/{{$stateParams.andromedaSiteId}}/resources/{{employee.Id}}"
            let path = "hr/{0}/employees/{1}/resources/{2}/profile-pic";
            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            //let r = this.uuidService.GenerateUUID();

            //path = path + "?r=" + r;

            return path;
        }

        public GetUploadRouteUrl(chainId: number, andromedaSiteId: number, employeeId: string): string {
            let path = "hr/{0}/employees/{1}/resources/{2}/update-profile-pic";
            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path;
        }

        public GetDocumentUploadRoute(chainId: number, andromedaSiteId, employeeId: string, documentId: string): string {
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

        public GetDataSourceForStoreScheduler(chainId: number, andromedaSiteId: number) {
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
                dir: "desc"
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
                        let route = this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                        let promise = this.$http.post(route, options.data);

                        promise.then((callback) => {
                            var callbackControllerError = callback.data["Errors"];

                            if (callbackControllerError) {
                                options.error(callback.data);
                            }
                            else {
                                options.success(callback.data);
                            }
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

        private GetStoreEmployeeSchedulerReadRoute(chainId: number, andromedaSiteId: number) {
            let path = "/hr/{0}/employees/{1}/schedule/store-list";

            path = kendo.format(path, chainId, andromedaSiteId);

            return path;
        }

        private GetEmployeeSchedulerReadRoute(chainId: number, andromedaSiteId, employeeId: string) {
            let path = "/hr/{0}/employees/{1}/schedule/list/{2}";

            path = kendo.format(path, chainId, andromedaSiteId, employeeId);

            return path;
        }

        private GetEmployeeSchedulerUpdateRoute(chainId: number, andromedaSiteId) {
            let path = "/hr/{0}/employees/{1}/schedule/update";

            path = kendo.format(path, chainId, andromedaSiteId);

            return path;
        }

        private GetEmployeeSchedulerDestroyRoute(chainId: number, andromedaSiteId) {
            let path = "/hr/{0}/employees/{1}/schedule/destroy";

            path = kendo.format(path, chainId, andromedaSiteId);

            return path;
        }
    }

    app.service("employeeService", EmployeeService);
    app.service("employeeServiceState", EmployeeServiceState);
}