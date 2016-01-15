module MyAndromeda.Hr.Controllers {
    var app = angular.module("MyAndromeda.Hr.Controllers", ["kendo.directives"]);

    app.controller("employeeListController", ($scope, $stateParams, employeeService: Services.EmployeeService) => {
        Logger.Notify("Route params: ");
        Logger.Notify($stateParams);

        var employeeGridDataSource = employeeService.EmployeeDataSource;

        var actionsTemplate = $("#employee-list-row-template").html();
        var employeeGridOptions: kendo.ui.GridOptions = {
            dataSource: employeeGridDataSource,
            autoBind: true,
            filterable: true,
            sortable: true,
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

    app.controller("employeeEditController", ($scope, $stateParams, employeeService: Services.EmployeeService) => {
        var employeeId: string = $stateParams.id;
        
        var employee: Models.IEmployee = <any>employeeService.EmployeeDataSource.get(employeeId);

        Logger.Notify("employee id:" + employeeId);
        Logger.Notify(employee);

        $scope.employee = employee;
    });
}