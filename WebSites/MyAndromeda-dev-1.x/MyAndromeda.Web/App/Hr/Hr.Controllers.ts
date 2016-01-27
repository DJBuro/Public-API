module MyAndromeda.Hr.Controllers {
    var app = angular.module("MyAndromeda.Hr.Controllers", ["kendo.directives", "oitozero.ngSweetAlert"]);

    app.controller("employeeListController", ($scope, $stateParams: Models.IEmployeeStoreListState,
        SweetAlert: any,
        employeeService: Services.EmployeeService,
        employeeServiceState: Services.EmployeeServiceState) => {

        Logger.Notify("stateParams");
        Logger.Notify($stateParams);

        employeeServiceState.ChainId.onNext($stateParams.chainId);
        employeeServiceState.AndromedaSiteId.onNext($stateParams.andromedaSiteId);

        employeeService.Loading.subscribe((isLoading) => {
            if (isLoading) {
                var message =
                    SweetAlert.swal({
                        title: "Loading",
                    });

            } else {
                SweetAlert.hide();
            }
        });

        employeeService.Saved.subscribe((saved) => {
            if (saved)
            {
                SweetAlert.swal("Saved!", "", "success");
            }
        });

        var employeeGridDataSource = employeeService.StoreEmployeeDataSource;

        var headerTemplate = $("#employee-list-header-template").html();
        var actionsTemplate = $("#employee-list-row-template").html();



        var employeeGridOptions = { //kendo.ui.GridOptions = {
            dataSource: employeeGridDataSource,
            autoBind: false,
            filterable: true,
            sortable: true,
            toolbar: kendo.template(headerTemplate),
            columns: [
                { field: "Store", title: "Store", width: 100, filterable: {checkAll: true, multi: false} },
                //{ field: "Code", title: "Code", width: 100 },
                { field: "PrimaryRole", title: "Primary Role", width: 100 },
                {
                    title: "Contact",
                    columns: [
                        {
                            field: "Name",
                            title: "Name",
                            //width: 200,
                            template: "<employee-pic employee='dataItem'></employee-pic>"
                        },
                        { field: "Phone", title: "Phone", width: 100 },
                        { field: "Email", title: "Email", width: 200 }
                    ]
                },
                {
                    title: "Actions",
                    width: 100,
                    template: actionsTemplate,
                }
        
                
                //{ title: "Contact Details", minScreenWidth: 400, template: "<employee-contact-details></employee-contact-details>" }
            ]
        };

        $scope.employeeGridOptions = employeeGridOptions;

    });

    app.controller("employeeEditController", ($scope, $stateParams, employeeService: Services.EmployeeService, employeeServiceState: Services.EmployeeServiceState) => {
        Logger.Notify("stateParams");
        Logger.Notify($stateParams);

        employeeServiceState.ChainId.onNext($stateParams.chainId);
        employeeServiceState.AndromedaSiteId.onNext($stateParams.andromedaSiteId);

        var getEmployee = (): Models.IEmployee => {
            let employeeId: string = $stateParams.id;
            if (!employeeId)
            {
                //create new employee
                return {
                    Name: "",
                    PrimaryRole: "",
                    Roles: [],
                    ShiftStatus: {}
                };
            }

            Logger.Notify("employee id:" + employeeId);

            let employee: Models.IEmployee = <any>employeeService.StoreEmployeeDataSource.get(employeeId);

            return employee;
        };

        
        let employee = getEmployee();
        let save = (employee: Models.IEmployee) => {
            if (!employee.Id) {
                employeeService.StoreEmployeeDataSource.add(employee);
            }
            //else { }
            employeeService.StoreEmployeeDataSource.sync();
        };

        Logger.Notify(employee);

        $scope.employee = employee;
        $scope.save = save;
    });
}