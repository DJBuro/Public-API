module MyAndromeda.Hr
{
    var app = angular.module("MyAndromeda.Hr.Config", [
        "MyAndromeda.Hr.Controllers",
        "MyAndromeda.Hr.Services"]);

    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {

        var hr: ng.ui.IState = {
            abstract: true,
            url: '/hr',
            template: '<div ui-view="main"></div>'
        };

        var hrStoreList: ng.ui.IState = {
 
            url: "/list/:chainId/store/:andromedaSiteId",
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
        }

        Logger.Notify("set hr states");

        // route: /hr-store
        $stateProvider.state("hr", hr)
        $stateProvider.state("hr.store-list", hrStoreList);
        $stateProvider.state("hr.store-list.edit-employee", hrStoreEmployeeEdit);
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