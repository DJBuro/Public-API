module MyAndromeda.Hr
{
    var app = angular.module("MyAndromeda.Hr.Config", [
        "MyAndromeda.Hr.Controllers",
        "MyAndromeda.Hr.Services",
        "MyAndromeda.Hr.Services.Scheduler", 
        "MyAndromeda.Hr.Directives",
        "MyAndromeda.Hr.Directives.Scheduler"
    ]);

    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {

        var hr: ng.ui.IState = {
            abstract: true,
            url: '/hr/:chainId',
            template: '<div id="masterUI" ui-view="main"></div>'
        };

        var hrStoreList: ng.ui.IState = {
 
            url: "/list/store/:andromedaSiteId",
            views: {
                "main": {
                    templateUrl: "employee-list.html",
                    controller: "employeeListController"
                },
            },
            onEnter: () => {
                Logger.Notify("Entering employee list");
            },
            cache: false
        };

        var hrEmployeeList: ng.ui.IState = {
            url: "/employees",
            views: {
                "store-employee-view": {
                    templateUrl: "store-employee-list.html"
                }
            }
        };

        var hrStoreScheduler: ng.ui.IState = {
            url: "/schedule",
            views: {
                "store-employee-view": {
                    templateUrl: "store-employee-scheduler.html"
                }
            }
        };


        var hrStoreEmployeeEdit: ng.ui.IState = {
            url: "/edit/:id",
            views: {
                //use the 'main' view area of the 'hr' state. 
                "main@hr": {
                    templateUrl: "employee-edit.html",
                    controller: "employeeEditController"
                }
            },
            onEnter: () => {
                Logger.Notify("Entering employee edit");
            },
            cache: false
        };

        var hrStoreEmployeEditDetails: ng.ui.IState = {
            url: "/details",
            views: {
                "editor-main": {
                    templateUrl: "employee-edit-details.html"
                }
            },
            cache: false
        };

        var hrStoreEmployeeDocuments: ng.ui.IState = {
            url: "/documents",
            views: {
                "editor-main": {
                    templateUrl: "hr.store-list.edit-employee.documents.html"
                }
            }
        };

        var hrStoreEmployeeEditScheduler: ng.ui.IState = {
            url: "/schedule",
            views: {
                "editor-main": {
                    templateUrl: "employee-edit-schedule.html",
                    controller: "employeeEditSchedulerController"
                }
            }, 
            onEnter: () => {
                Logger.Notify("Edit person's schedule.");
            },
            cache: false
        }

        var hrStoreEmployeeCreate: ng.ui.IState = {
            url: "/create/",
            views: {
                //use the 'main' view area of the 'hr' state. 
                "main@hr": {
                    templateUrl: "employee-edit.html",
                    controller: "employeeEditController"
                }
            },
            onEnter: () => {
                Logger.Notify("Entering employee create");
            },
            cache: false
        };

        Logger.Notify("set hr states");

        // route: /hr-store
        $stateProvider.state("hr", hr)
        $stateProvider.state("hr.store-list", hrStoreList);
        $stateProvider.state("hr.store-list.employee-list", hrEmployeeList);
        $stateProvider.state("hr.store-list.scheduler", hrStoreScheduler);


        $stateProvider.state("hr.store-list.edit-employee", hrStoreEmployeeEdit);
        $stateProvider.state("hr.store-list.edit-employee.details", hrStoreEmployeEditDetails);
        $stateProvider.state("hr.store-list.edit-employee.schedule", hrStoreEmployeeEditScheduler);
        $stateProvider.state("hr.store-list.edit-employee.documents", hrStoreEmployeeDocuments);
        $stateProvider.state("hr.store-list.create-employee", hrStoreEmployeeCreate);

    });

    app.run(($rootScope) => {
        $rootScope.$on('$stateChangeStart',
            function (event, toState, toParams, fromState, fromParams) {
                Logger.Notify("$stateChangeStart");
            });
        $rootScope.$on('$stateNotFound',
            function (event, unfoundState, fromState, fromParams) {
                Logger.Notify("$stateNotFound");
            });
        $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                Logger.Notify("$stateChangeSuccess");
            });
    });
}