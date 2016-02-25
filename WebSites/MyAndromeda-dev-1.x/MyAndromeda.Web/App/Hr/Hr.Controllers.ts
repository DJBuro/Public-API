module MyAndromeda.Hr.Controllers {
    var app = angular.module("MyAndromeda.Hr.Controllers", ["kendo.directives", "oitozero.ngSweetAlert"]);
    
    app.controller("employeeListController", (
        $scope,
        $element,
        $timeout,
        $stateParams: Models.IEmployeeStoreListState,
        SweetAlert: any,
        employeeService: Services.EmployeeService,
        employeeServiceState: Services.EmployeeServiceState,
        employeeSchedulerService: Services.EmployeeSchedulerService,
        progressService: MyAndromeda.Services.ProgressService) => {

        $scope.$stateParams = $stateParams;
        

        employeeServiceState.ChainId.onNext($stateParams.chainId);
        employeeServiceState.AndromedaSiteId.onNext($stateParams.andromedaSiteId);

        employeeService.Saved.subscribe((saved) => {
            if (saved)
            {
                SweetAlert.swal("Saved!", "", "success");
            }
        });

        let employeeGridDataSource = employeeService.StoreEmployeeDataSource;

        let headerTemplate = $("#employee-list-header-template").html();
        let actionsTemplate = $("#employee-list-row-template").html();

        let chainId = $stateParams.chainId;
        let andromedaSiteId = $stateParams.andromedaSiteId;

        let employeePicTemplate = "<employee-pic employee='dataItem'></employee-pic>";
        employeePicTemplate = kendo.format(employeePicTemplate, chainId, andromedaSiteId);

        let employeeGridOptions = {
            dataSource: employeeGridDataSource,
            autoBind: true,
            filterable: true,
            sortable: true,
            groupable: true,
            resizable: true,
            toolbar: kendo.template(headerTemplate),
            columns: [
                { field: "Department", title: "Department", width: 100, minScreenWidth: 400 },
                {
                    title: "Contact",
                    columns: [
                        {
                            field: "Name",
                            title: "Name",
                            width: 400,
                            template: employeePicTemplate
                        },
                        { field: "Phone", title: "Phone", width: 100 },
                        { field: "Email", title: "Email", width: 200, minScreenWidth: 400 }
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

        var storeIdChanged = employeeServiceState.AndromedaSiteId.where(e=> e !== null).map(e => {
            let r = employeeService.GetStore(chainId, e);
            return r;
        }).flatMap(e => e);

        storeIdChanged.subscribe((stores) => {
            Logger.Notify("stores: ");
            Logger.Notify(stores);

            $timeout(() => {
                let schedulerOptions = employeeSchedulerService.GetStoreEmployeeScheduler(stores);
                $scope.schedulerOptions = schedulerOptions;
            });
            
        });

        var savingSubscription = employeeService.SavingSchedule.where(e=> e).subscribe(saving => {
            Logger.Notify("Scheduler saving.... show progress");

            progressService.ShowProgress($element);
        });

        var savedSubscription = employeeService.SavingSchedule.where(e=> !e).subscribe(notSaving => {
            Logger.Notify("Scheduler saved!.... hide progress");

            progressService.HideProgress($element);
        });

        $scope.$on('$destroy', () => {
            savingSubscription.dispose();
            savedSubscription.dispose();
        });

        $scope.employeeGridOptions = employeeGridOptions;
    });

    app.controller("employeeEditController", (
        $element,
        $scope, $stateParams, $timeout,
        SweetAlert,
        progressService: MyAndromeda.Services.ProgressService,
        employeeService: Services.EmployeeService,
        employeeServiceState: Services.EmployeeServiceState,
        uuidService: MyAndromeda.Services.UUIdService) => {

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
                    Id: uuidService.GenerateUUID(),
                    Name: "",
                    PrimaryRole: "",
                    Roles: [],
                    ShiftStatus: {}
                });

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

            employeeServiceState.EditEmployee.onNext(employee);
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

            //do i need these anymore? 
            //employee.dirty = true;
            employee.set("DirtyHack", true);

            let validator : kendo.ui.Validator = $scope.validator; 
            let valid = validator.validate();
            
            
            if (!valid) {
                Logger.Notify("validation failed.");
                return;
            }

            progressService.ShowProgress($element);

            var sync = employeeService.Save(employee);
            
            sync.then(() => {
                progressService.HideProgress($element);

                Logger.Notify("Sync done");
                var name = employee.get("ShortName");
                SweetAlert.swal("Saved!", name + " has been saved.", "success");

                employeeServiceState.EmployeeUpdated.onNext(<any>employee);
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

    app.controller("employeeEditSchedulerController", (
        $element,
        $scope,
        $timeout,
        employeeService: Services.EmployeeService,
        employeeServiceState: Services.EmployeeServiceState,
        employeeSchedulerService: Services.EmployeeSchedulerService) => {

        let loadRelatedStoresObservable = employeeServiceState.EditEmployee
            .where(e=> e !== null)
            .map((employee) => {
                var loadObservable = employeeService.GetStoreListByEmployee(employeeServiceState.CurrentChainId, employeeServiceState.CurrentAndromedaSiteId, employee.Id);

                return loadObservable;
            }).flatMap(e=> e);
        
        let editEmployeeObservable = employeeServiceState.EditEmployee
            .where(e=> e !== null);
        
        let merged = Rx.Observable.combineLatest(loadRelatedStoresObservable, editEmployeeObservable, (stores, employee) => {
            Logger.Notify("Stores: ");
            Logger.Notify(stores);
            Logger.Notify("Employee: " + employee.ShortName);

            return {
                stores: stores,
                employee: employee
            };
        });

        let editSubscription = merged.subscribe((data) => {
            Logger.Notify("stores+ employees available");

            $timeout(() => {
                let schedulerOptions = employeeSchedulerService.GetSingleEmployeeScheduler(data.stores, data.employee);
                $scope.schedulerOptions = schedulerOptions;
            });
        });

        $scope.$on('$destroy', function () {
            editSubscription.dispose();
        });

    });
}