module MyAndromeda.Hr.Services {
    var app = angular.module("MyAndromeda.Hr.Services", []);

    export class EmployeeService
    {
        public EmployeeDataSource: kendo.data.DataSource; 

        constructor(private $http: ng.IHttpService)
        {
            this.EmployeeDataSource = new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "Id"
                    }
                },
                transport: {
                    read: (options) => {
                        var testPerson: Models.IEmployee = {
                            Id: "1",

                            Store: "Somewhere",
                            Code: "0001",
                            Name: "Bob the happy one",
                            ProfilePic: null,
                            Email: "bob@someplace.com",
                            Phone: "012345678",
                            PrimaryRole: "Driver",
                            Roles: ["1", "2"],

                            ShiftStatus: {
                                OnShift: true,
                                OnCall: false,
                                Available : false
                            }
                        };
                        var testPerson2 = {
                            Id: 2,
                            Store: "Somewhere else", 
                            Code: "0001",
                            Name: "Sadness",
                            ProfilePic: null,
                            Email: "notbob@someplace.com",
                            Phone: "012345678",
                            PrimaryRole: "Driver",
                            Roles: ["1", "2"],

                            ShiftStatus: {
                                OnShift: false,
                                OnCall: false,
                                Available: false
                            }
                        }; 

                        options.success([testPerson, testPerson2]);

                    }
                }
                //data: [
                //    testPerson
                //]
            }); 

            this.EmployeeDataSource.read();
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
}