module MyAndromeda.Hr.Services {
    var app = angular.module("MyAndromeda.Hr.Services", []);

    export class EmployeeServiceState
    {
        public ChainId : Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);
        public AndromedaSiteId: Rx.BehaviorSubject<number> = new Rx.BehaviorSubject(null);

        constructor() {

        }
    }

    export class EmployeeService
    {
        public ChainEmployeeDataSource: kendo.data.DataSource;
        public StoreEmployeeDataSource: kendo.data.DataSource;
        
        private chainId: number;
        private andromedaSiteId: number; 

        public Loading: Rx.Subject<boolean> = new Rx.Subject<boolean>();
        public Saved: Rx.Subject<boolean> = new Rx.Subject<boolean>(); 
        public Error: Rx.Subject<string> = new Rx.Subject<string>();

        constructor(private $http: ng.IHttpService, private employeeServiceState: EmployeeServiceState)
        {
            this.ChainEmployeeDataSource = new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "Id"
                    }
                }
            });

            this.StoreEmployeeDataSource = new kendo.data.DataSource({
                
                schema: {
                    model: {
                        id: "Id"
                    }
                },
                transport: {
                    read: (options) => {
                        //var testPerson: Models.IEmployee = {
                        //    Id: "1",

                        //    Store: "Somewhere",
                        //    Code: "0001",
                        //    Name: "Bob the happy one",
                        //    ProfilePic: null,
                        //    Email: "bob@someplace.com",
                        //    Phone: "012345678",
                        //    PrimaryRole: "Driver",
                        //    Roles: ["1", "2"],

                        //    ShiftStatus: {
                        //        OnShift: true,
                        //        OnCall: false,
                        //        Available : false
                        //    }
                        //};

                        //var testPerson2 = {
                        //    Id: 2,
                        //    Store: "Somewhere else", 
                        //    Code: "0001",
                        //    Name: "Sadness",
                        //    ProfilePic: null,
                        //    Email: "notbob@someplace.com",
                        //    Phone: "012345678",
                        //    PrimaryRole: "Driver",
                        //    Roles: ["1", "2"],

                        //    ShiftStatus: {
                        //        OnShift: false,
                        //        OnCall: false,
                        //        Available: false
                        //    }
                        //}; 

                        //options.success([testPerson, testPerson2]);

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

                        let data = e.data;
                        let route = "hr/{0}/employees/{1}/update"; 
                        route = kendo.format(route, this.chainId, this.andromedaSiteId);

                        let promise = this.$http.post(route, data);
                        this.Saved.onNext(false);
                        Rx.Observable.fromPromise(promise).subscribe((callback) => {
                            var callBackData = callback.data;
                            Logger.Notify("result: ");
                            Logger.Notify(callBackData);

                            e.success();
                            this.Saved.onNext(true);
                        }, (error) => {
                            Logger.Error(error);
                            this.Error.onNext("Updating Failed");
                        });
                    },
                    create: (e) => {
                        Logger.Notify("Create employee record");
                        let data = e.data;
                        
                        let route = "hr/{0}/employees/{1}/create";
                        route = kendo.format(route, this.chainId, this.andromedaSiteId);

                        var promise = this.$http.post(route, data); 

                        this.Saved.onNext(false);

                        Rx.Observable.fromPromise(promise).subscribe((callback) => {
                            var callBackData = callback.data;
                            Logger.Notify("result: ");
                            Logger.Notify(callBackData);

                            e.success(callback.data);
                            this.Saved.onNext(true);
                        }, (error) => {
                            Logger.Error(error);
                            this.Error.onNext("Creating Failed");
                        });
                    }
                },
                batch: false
                //data: [
                //    testPerson
                //]
            }); 

            this.employeeServiceState.AndromedaSiteId.where(e=> e !== null).subscribe((id) => {
                Logger.Notify("new andromeda site id : " + id);
                this.andromedaSiteId = id;
                this.StoreEmployeeDataSource.read();
            });
            this.employeeServiceState.ChainId.where(e=> e !== null).subscribe((id) => {
                Logger.Notify("new chain id : " + id);
                this.chainId = id;
                this.ChainEmployeeDataSource.read();
            });
        }

        public List(chainId: number, andromedaSiteId: number): ng.IHttpPromise<Models.IEmployee[]>
        {
            var route = "";

            var pomise = this.$http.get(route);

            return pomise;
        }

        //public Update(employee: Models.IEmployee): ng.IHttpPromise<Models.IEmployee>
        //{
            
        //}
    }

    app.service("employeeService", EmployeeService);
    app.service("employeeServiceState", EmployeeServiceState);
}