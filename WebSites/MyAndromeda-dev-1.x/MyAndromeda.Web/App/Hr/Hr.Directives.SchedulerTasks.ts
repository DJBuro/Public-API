module MyAndromeda.Hr.Directives {
    var app = angular.module("MyAndromeda.Hr.Directives.Scheduler", []);

    app.directive("rotaTaskEditor", () => {
        return {
            name: "rotaTaskEditor",
            scope: {
                task: "=task",
            },
            templateUrl: "rotaTaskEditor.html",
            controller: ($scope, employeeService: Services.EmployeeService, employeeServiceState: Services.EmployeeServiceState) => {
                var task: Models.IEmployeeTask = $scope.task;

                Logger.Notify("rota task started");

                let storeEmployeeDataSource = employeeService.StoreEmployeeDataSource;
                let taskTypeDataSource = Models.taskTypes;//Models.

                let dataSources = {
                    storeEmployeeDataSource: storeEmployeeDataSource,
                    storeTaskTypeDataSource: taskTypeDataSource,
                };
                $scope.dataSources = dataSources;
            }
        };
    });
    app.directive("workingTask", () => {
        return {
            name: "workingTask",
            templateUrl: "working-task.html",
            scope: {
                task: "=task",
                timeLineMode: "=timeLineMode"
                //employee: "=employee"
            },
            controller: ($element, $scope, employeeService: Services.EmployeeService) => {
                var task: Models.IEmployeeTask = $scope.task;
                var employee: Models.IEmployee = <any>employeeService.StoreEmployeeDataSource.get(task.EmployeeId);
                if (employee === null) { Logger.Notify("cant find the person"); }

                $scope.employee = employee;

                let topElement = $($element).closest(".k-event");
                let borderStyle = ""

                switch (employee.Department) {
                    case "Front of house": borderStyle = 'task-front-of-house'; break;
                    case "Kitchen": borderStyle = 'task-kitchen'; break;
                    case "Management": borderStyle = 'task-management'; break;
                    case "Delivery": borderStyle = 'task-delivery'; break;
                }

                topElement.addClass("task-border");
                topElement.addClass(borderStyle);

                var status = {
                    clone: null
                };

                var popover = topElement.popover({
                    title: "Task preview",
                    placement: "auto",
                    html: true,
                    content: "please wait",
                    trigger: "click"
                }).on("show.bs.popover", function () {
                    let html = topElement.html();
                    popover.attr('data-content', html);
                    var current = $(this);
                    setTimeout(() => { current.popover('hide'); }, 5000);
                    $scope.$on('$destroy', function () {
                        //current.fadeOut();
                    });
                });

                $scope.$on('$destroy', function () {
                    popover.hide();
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

    app.directive("employeeTask", () => {
        return {
            name: "employeeTask",
            templateUrl: "employee-task.html",
            scope: {
                task: "=task",
                timeLineMode: "=timeLineMode"
                //employee: "=employee"
            },
            controller: ($element, $scope, employeeService: Services.EmployeeService) => {
                var task: Models.IEmployeeTask = $scope.task;
                var employee: Models.IEmployee = <any>employeeService.StoreEmployeeDataSource.get(task.EmployeeId);
                if (employee === null) { Logger.Notify("cant find the person"); }

                $scope.employee = employee;

                let topElement = $($element).closest(".k-event");
                let borderStyle = ""

                switch (employee.Department) {
                    case "Front of house": borderStyle = 'task-front-of-house'; break;
                    case "Kitchen": borderStyle = 'task-kitchen'; break;
                    case "Management": borderStyle = 'task-management'; break;
                    case "Delivery": borderStyle = 'task-delivery'; break;
                }

                topElement.addClass("task-border");
                topElement.addClass(borderStyle);

                var status = {
                    clone: null
                };

                //var popover = topElement.popover({
                //    title: "Task preview",
                //    placement: "auto",
                //    html: true,
                //    content: "please wait",
                //    trigger: "hover"
                //}).on("show.bs.popover", function() {
                //    let html = topElement.html();
                //    popover.attr('data-content', html);
                //    var current = $(this); 
                //    setTimeout(() => { current.popover('hide'); }, 5000)
                //});

                //topElement.on("hover", function (e) {
                //    Logger.Notify("animate .k-event");
                //});

                
                var popover = topElement.popover({
                    title: "Task preview",
                    placement: "auto",
                    html: true,
                    content: "please wait",
                    trigger: "click"
                }).on("show.bs.popover", function () {
                    let html = topElement.html();
                    popover.attr('data-content', html);
                    var current = $(this);
                    setTimeout(() => { current.popover('hide'); }, 5000);
                    $scope.$on('$destroy', function () {
                        //current.fadeOut();
                    });
                });

                $scope.$on('$destroy', function () {
                    popover.hide();
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