module MyAndromeda.Hr.Controllers {
    var app = angular.module("MyAndromeda.Hr.Controllers", ["kendo.directives", "oitozero.ngSweetAlert"]);
    
    app.controller("employeeListController", ($scope, $stateParams: Models.IEmployeeStoreListState,
        SweetAlert: any,
        employeeService: Services.EmployeeService,
        employeeServiceState: Services.EmployeeServiceState) => {

        $scope.$stateParams = $stateParams;
        

        employeeServiceState.ChainId.onNext($stateParams.chainId);
        employeeServiceState.AndromedaSiteId.onNext($stateParams.andromedaSiteId);

        employeeService.Loading.subscribe((isLoading) => {
            //var message = null;
            //if (isLoading) {
            //    message =
            //        SweetAlert.swal({
            //            title: "Loading",
            //        });

            //} else {
            //    message.hide();
            //}
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

        var chainId = $stateParams.chainId;
        var andromedaSiteId = $stateParams.andromedaSiteId;

        var employeePicTemplate = "<employee-pic employee='dataItem'></employee-pic>";
        employeePicTemplate = kendo.format(employeePicTemplate, chainId, andromedaSiteId);

        var employeeGridOptions = {
            dataSource: employeeGridDataSource,
            autoBind: true,
            filterable: true,
            sortable: true,
            groupable: true,
            toolbar: kendo.template(headerTemplate),
            columns: [
                { field: "Department", title: "Department", width: 100 },
                {
                    title: "Contact",
                    columns: [
                        {
                            field: "Name",
                            title: "Name",
                            //width: 200,
                            template: employeePicTemplate
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

    app.controller("employeeEditController", (
        $scope, $stateParams, $timeout,
        SweetAlert,
        employeeService: Services.EmployeeService,
        employeeServiceState: Services.EmployeeServiceState,
        uuidService: Services.UUIDService) => {

        Logger.Notify("stateParams");
        Logger.Notify($stateParams);

        let chainId: number = $stateParams.chainId;
        let andromedaSiteId: number = $stateParams.andromedaSiteId;

        employeeServiceState.ChainId.onNext(chainId);
        employeeServiceState.AndromedaSiteId.onNext(andromedaSiteId);

        var status = {
            uploading: false,
            random: uuidService.GenerateUUID()
        };

        let getOrCreateEmployee = (): any => {
            let employeeId: string = $stateParams.id;
            if (!employeeId)
            {
                let modelCreator = kendo.data.Model.define(Models.employeeDataSourceSchema);

                let newEmployee = new modelCreator({
                    Name: "",

                    PrimaryRole: "",
                    Roles: [],
                    ShiftStatus: {}
                });
                //create new employee

                return newEmployee;
            }

            Logger.Notify("employee id:" + employeeId);

            let employee = employeeService.StoreEmployeeDataSource.get(employeeId);

            Logger.Notify(employee);
            Logger.Notify("uid: " + employee.uid); 

            return employee;
        };

        let dataSource = employeeService.StoreEmployeeDataSource;

        Logger.Notify("data-source length: " + dataSource.data().length);

        let noData = dataSource.data().length === 0;

        var setEmployee = () => {
            let employee = getOrCreateEmployee();
            Logger.Notify(employee);
            $scope.employee = employee;
        };

        if (noData && !employeeService.IsLoading) {
            Logger.Notify("Load employees");
            let promise = employeeService.StoreEmployeeDataSource.read();

            promise.then(() => {
                setEmployee();
            });
        }
        else if (noData && employeeService.IsLoading)
        {
            var loadingSubscription = employeeService.Loading.where(e=> !e).subscribe(() => {
                setEmployee();
                loadingSubscription.dispose();
            });
        }
        else
        {
            setEmployee();
        }

        let save = (employee: kendo.data.Model) => {
            Logger.Notify("saved called");
            
            var gridEmployee = employeeService.StoreEmployeeDataSource.get(employee.id);

            //do i need these anymore? 
            //employee.dirty = true;
            employee.set("DirtyHack", true);

            let validator : kendo.ui.Validator = $scope.validator; 
            let valid = validator.validate();
            let id = employee.get("Id"); 

            if (!valid) {
                Logger.Notify("validation failed.");
                return;
            }

            if (!id) {
                employeeService.StoreEmployeeDataSource.add(employee);
            } else {
                Logger.Notify("Edit view uid: " + employee.uid);
                Logger.Notify("Grid view uid: " + gridEmployee.uid);
            }

            Logger.Notify("sync");
           
            let sync = employeeService.StoreEmployeeDataSource.sync();

            sync.then(() => {
                Logger.Notify("Sync done");
                var name = employee.get("ShortName");
                SweetAlert.swal("Saved!", name + " has been saved.", "success");
            });
        };
        let getProfilePic = (employee: Models.IEmployee) => {
            let route = "/content/profile-picture.jpg";

            if (employee) {
                route = employeeService.GetEmployeePictureUrl($stateParams.chainId, $stateParams.andromedaSiteId, employee.Id);
                route += "?r=" + status.random;
            }

            return route;
        };

        $scope.status = status;
        $scope.saveRoute = (employee: Models.IEmployee) => {
            Logger.Notify("save route");
            let id = ""; 
            if (employee) {
                id = employee.Id;
            }

            let route = employeeService.GetUploadRouteUrl($stateParams.chainId, $stateParams.andromedaSiteId, $scope.employee.Id);
            return route; 
        };
        //$scope.onUploadComplete = (args) => {
        //    Logger.Notify("upload complete");
        //    Logger.Notify(args);

        //    let employee: Models.IEmployee = $scope.employee;
        //    employee.ProfilePic = employeeService.GetEmployeePictureUrl($stateParams.chainId, $stateParams.andromedaSiteId, employee.Id);
        //};
        //#k - upload="onUploading"
        //k - success="onUploadSuccess"
                                   
        $scope.onUploading = () => {
            Logger.Notify("uploading profile pic");
            status.uploading = true;
            status.random = undefined;
        };

        $scope.onUploadSuccess = (e) => {
            Logger.Notify("uploaded profile pic");
            
            $timeout(() => {
                status.uploading = false;
                status.random = uuidService.GenerateUUID();
            });
        };


        $scope.onSelect = function (e) {
            var message = $.map(e.files, function (file) { return file.name; }).join(", ");
            console.log(message);
        };
        //uploadSettings

        $scope.save = save;
        $scope.profilePicture = getProfilePic;
    });
}