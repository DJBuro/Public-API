/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
module MyAndromeda.Hr.Directives {
    var app = angular.module("MyAndromeda.Hr.Directives", []);

    app.directive("employeeDocs", () => {
        return {
            name: "employeeDocs",
            templateUrl: "employee-documents.html",
            restrict: "EA",
            scope: {
                dataItem: '=employee',
                save: "=save"
            },
            controller: (
                $element,
                $scope,
                $timeout,
                SweetAlert,
                progressService: MyAndromeda.Services.ProgressService,
                employeeService: Services.EmployeeService,
                employeeServiceState: Services.EmployeeServiceState,
                uuidService: MyAndromeda.Services.UUIdService) => {

                var dataItem: Models.IEmployee = $scope.dataItem;
                var save: () => void = () => {
                    Logger.Notify("save - from docs");
                    let employee: Models.IEmployee = $scope.dataItem;

                    var promise = employeeService.Save(employee);
                    progressService.ShowProgress($element);

                    promise.then(() => {
                        SweetAlert.swal("Saved!", name + " has been saved.", "success");
                        progressService.HideProgress($element);
                    });
                };
                $scope.save = save;

                Logger.Notify("dataItem: " + dataItem);
                Logger.Notify($scope);

                var status = {
                    uploading: false,
                    random: uuidService.GenerateUUID()
                };

                if (!dataItem.Documents) {
                    dataItem.Documents = [];
                } else {
                    dataItem.Documents = dataItem.Documents.filter(() => true);
                }

                $scope.status = status;

                $scope.uploadRoute = () => {
                    let chainId = employeeServiceState.CurrentAndromedaSiteId,
                        andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId,
                        document: Models.IEmployeeDocument = $scope.document;

                    let route = employeeService.GetDocumentUploadRoute(chainId, andromedaSiteId, dataItem.Id, document.Id);

                    return route;
                };

                $scope.onUploading = (e) => {
                    Logger.Notify("upload started");
                    status.uploading = true;
                    status.random = undefined;
                };

                $scope.onUploadSuccess = (e) => {
                    Logger.Notify("upload success");
                    Logger.Notify(e);

                    var document: Models.IEmployeeDocument = $scope.document;
                    
                    e.files.forEach((item: { name: string }) => {
                        document.Files.push({
                            FileName : item.name
                        });
                    });

                    //save();

                    $timeout(() => {
                        status.uploading = false;
                        status.random = uuidService.GenerateUUID();
                    });
                };
                $scope.onUploadComplete = (e) => {
                    Logger.Notify("upload complete:");
                    Logger.Notify(e);

                    $timeout(() => {
                        status.uploading = false;
                        status.random = uuidService.GenerateUUID();
                    });
                };

                $scope.removeAll = () => {
                    var r = () => {
                        dataItem.Documents = [];
                        save();
                    }

                    SweetAlert.swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover these files!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    }, (isConfirm: boolean) => {
                        Logger.Notify("confirm:" + isConfirm);
                        if (isConfirm) {
                            let editing: Models.IEmployeeDocument = $scope.document;
                            r();
                            Logger.Notify("alert removed");
                            SweetAlert.swal("Deleted!", "Your file has been deleted.", "success");
                        }
                        else {
                            Logger.Notify("alert cancel");
                        }
                    });

                };
                $scope.new = () => {
                    let doc: Models.IEmployeeDocument = {
                        Id: uuidService.GenerateUUID(),
                        Name: null,
                        Files: []
                        //DocumentUrl: ""
                    };

                    Logger.Notify("Add to documents");
                    dataItem.Documents.push(doc);
                    Logger.Notify("set $scope.document");
                    $scope.document = doc;
                };

                $scope.select = (doc: Models.IEmployeeDocument) => {
                    Logger.Notify(doc);

                    if (!doc.Files) { doc.Files = []; }

                    $timeout(() => {
                        $scope.document = undefined;
                    }).then(() => {
                        $timeout(() => {
                            $scope.document = doc;
                        }, 100);
                    });

                    
                };

                $scope.clear = () => {
                    $scope.document = undefined;
                };

                $scope.remove = (document: Models.IEmployeeDocument) => {
                    var removeItem = () => {
                        let editing: Models.IEmployeeDocument = $scope.document;

                        if (editing && editing.Id == document.Id) {
                            $scope.document = undefined;
                        }

                        dataItem.Documents = dataItem.Documents.filter((item) => {
                            return item.Id !== document.Id;
                        });

                        //save();
                    };

                    SweetAlert.swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this file!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    }, (isConfirm: boolean) => {
                        Logger.Notify("confirm:" + isConfirm);
                        if (isConfirm) {
                            let editing: Models.IEmployeeDocument = $scope.document;
                            removeItem();
                            Logger.Notify("alert removed");
                            SweetAlert.swal("Deleted!", "Your file has been deleted.", "success");
                        }
                        else {
                            Logger.Notify("alert cancel");
                        }
                    });


                };

                $scope.removeFile = (document: Models.IEmployeeDocument, file: Models.IFile) => {
                    var removeItem = () => {
                        document.Files = document.Files.filter((item) => item.FileName !== file.FileName);

                        //save();
                    };
                    SweetAlert.swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this file!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    }, (isConfirm: boolean) => {
                        Logger.Notify("confirm:" + isConfirm);
                        if (isConfirm) {
                            let editing: Models.IEmployeeDocument = $scope.document;
                            removeItem();
                            Logger.Notify("alert removed");
                            SweetAlert.swal("Deleted!", "Your file has been deleted.", "success");
                        }
                        else {
                            Logger.Notify("alert cancel");
                        }
                    });
                    
                };

                $scope.getEmployeeDocumentImage = (document: Models.IEmployeeDocument, file: Models.IFile) => {
                    let chainId = employeeServiceState.CurrentAndromedaSiteId,
                        andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId;

                    let route = employeeService.GetDocumentRouteUrl(chainId, andromedaSiteId, dataItem.Id, document.Id, file.FileName);

                    route = route + "?r=" + status.random;

                    return route;
                };

                $scope.downloadDocumentFile = (document, file) => {
                    let chainId = employeeServiceState.CurrentAndromedaSiteId,
                        andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId;

                    let route = employeeService.GetDocumentDownloadRouteUrl(chainId, andromedaSiteId, dataItem.Id, document.Id, file.FileName);

                    return route;
                };

                $scope.dataItem = dataItem;
            }
        }
    });

    app.directive("employeePic", () => {

        return {
            name: "employeePic",
            templateUrl: "employee-pic.html",
            restrict: "EA",
            transclude: true,
            scope: {
                employee: '=employee',
                showShortName: "=showShortName",
                showFullName: "=showFullName",
                showWorkStatus: "=showWorkStatus"

            },
            controller: ($scope, $timeout, 
                employeeService: Services.EmployeeService,
                employeeServiceState: Services.EmployeeServiceState,
                uuidService: MyAndromeda.Services.UUIdService) => {
                
                let dataItem: Models.IEmployee = $scope.employee;
                let getValueOrDefault = (source: any, defaultValue: any) => {
                    let v = source;
                    let k = typeof (v);
                    
                    if (k == "undefined") {
                        return defaultValue;
                    }
                    return v;
                };


                let options = {
                    showShortName: getValueOrDefault($scope.showShortName, false),
                    //typeof ($scope.showShortName) == "undefined" ? true : $scope.showShortName,
                    showFullName: getValueOrDefault($scope.showFullName, false),
                    //typeof($scope.showFullName) == "undefined" ? true : $scope.showFullName,
                    showWorkStatus: getValueOrDefault($scope.showWorkStatus, false)
                    //typeof ($scope.showWorkStatus) == "undefined" ? true : $scope.showWorkStatus
                };
                Logger.Notify(options);

                $scope.options = options;


                let state = {
                    random : uuidService.GenerateUUID()
                };

                $scope.$watch('showShortName', (newValue, old) => {
                    Logger.Notify("showShortName: " + newValue);

                    $timeout(() => { options.showShortName = getValueOrDefault(newValue, true); });
                });
                $scope.$watch('showFullName', (newValue, oldValue) => {
                    Logger.Notify("showFullName: " + newValue);

                    $timeout(() => { options.showFullName = getValueOrDefault(newValue, true); });
                });
                $scope.$watch('showWorkStatus', (newValue, oldValue) => {
                    Logger.Notify("showWorkStatus: " + newValue);
                    
                    $timeout(() => { options.showWorkStatus = getValueOrDefault(newValue, true); });
                });
               

                let updates = employeeServiceState.EmployeeUpdated.where(e=> e.Id == dataItem.Id).subscribe((change) => {
                    $timeout(() => {
                        Logger.Notify(dataItem.ShortName + " updated");
                        //just run ... not nothing to do. 
                        state.random = uuidService.GenerateUUID();
                    });
                });;
                

                $scope.state = state;
                $scope.profilePicture = () => {
                    //var profilePicture = "/content/profile-picture.jpg";
                    let chainId = employeeServiceState.CurrentChainId;
                    let andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId;

                    let route = employeeService.GetEmployeePictureUrl(chainId, andromedaSiteId, dataItem.Id);
                    route = route + "?r=" + state.random;

                    return {
                        'background-image': 'url(' + route + ')'
                    };
                };

                $scope.$on('$destroy', () => {
                    updates.dispose();
                });


            }
        };
    });

    app.directive("employeeTask", () => {
        return {
            name: "employeeTask",
            templateUrl: "employee-task.html",
            scope: {
                task: "=task",
                //employee: "=employee"
            },
            controller: ($element, $scope, employeeService: Services.EmployeeService) => {
                var task: Models.IEmployeeTask = $scope.task;
                var employee: Models.IEmployee = <any>employeeService.StoreEmployeeDataSource.get(task.EmployeeId);
                if (employee === null) { Logger.Notify("cant find the person"); }

                $scope.employee = employee;

                Logger.Notify("setup employee task");

                let top = $($element).closest(".k-event");
                let borderStyle = ""

                switch (employee.Department) {
                    case "Front of house": borderStyle = 'task-front-of-house'; break;
                    case "Kitchen": borderStyle = 'task-kitchen'; break; 
                    case "Management": borderStyle = 'task-management'; break;
                    case "Delivery": borderStyle = 'task-delivery'; break;
                }

                top.addClass("task-border");
                top.addClass(borderStyle);

                var status = {
                    clone : null 
                };

                var popover = top.popover({
                    title: "Task preview",
                    placement: "auto",
                    html: true,
                    content: "please wait",
                    trigger: "hover"
                }).on("show.bs.popover", () => {
                    let html = top.html();
                    popover.attr('data-content', html);
                });

                Logger.Notify("top");
                Logger.Notify(top);
                top.on("hover", function (e) {
                    Logger.Notify("animate .k-event");
                });

                
                

                var extra = {
                    hours: Math.abs(task.end.getTime() - task.start.getTime()) / 36e5,
                    startTime: kendo.toString(task.start, "HH:mm"),
                    endTime: kendo.toString(task.end, "HH:mm")
                };  
                
                $scope.extra = extra; 
            }
        };
    });
}