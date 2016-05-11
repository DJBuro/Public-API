module MyAndromeda.Components {
    var storeAdminComponent = angular.module("MyAndromeda.Components.StoreAdminControls", ["MyAndromeda.Store.Services"]);

    storeAdminComponent.controller("storeAdminLinkGroupController", (
        $scope,
        $timeout,
        storeService: MyAndromeda.Store.Services.StoreService) => {

        Logger.Notify("Store assigned? ");
        Logger.Notify($scope.store);

        var context = {
            Store: "",
        };
        
    });


    storeAdminComponent.directive("storeAdminLinks", () => {
        return {
            name: "storeAdminLinks",
            scope: {
                store: "=store"
            },
            template: `
                
                    <div class="panel panel-danger">
                        <div class="panel-heading">
                            Admin
                        </div>
                        <div class="panel-body">
                            <div class="btn-group">
                                <a class="btn btn-default btn-sm" href="/OrderManagement/Chain/{{store.ChainId}}/{{store.ExternalSiteId}}/Orders">
                                    Orders
                                </a>
                                <a class="btn btn-default btn-sm" href="/AndroWebOrdering/Chain/{{store.ChainId}}/{{store.ExternalSiteId}}/AndroWebOrdering/List">
                                    Websites
                                </a>
                                <a class="btn btn-default btn-sm" ui-sref="hr.store-list.employee-list({ chainId : store.ChainId, andromedaSiteId: store.AndromedaSiteId})">
                                    Employees
                                </a>
                            </div>
                        </div>
                    </div>
                `
        };
    });

    storeAdminComponent.directive("storeDetailLinks", () => {
        return {
            name: "storeDetailLinks",
            scope: {
                store: "=store"
            },
            template: `
                    <div class="panel panel-warning">
                        <div class="panel-heading">
                            Store
                        </div>
                        <div class="panel-body">
                            <div class="btn-group">
                                <a class="btn btn-default btn-sm" href="/Chain/{{store.ChainId}}/Store/{{store.ExternalSiteId}}">
                                    Details
                                </a>

                                <a class="btn btn-default btn-sm" href="/Menus/Chain/{{store.ChainId}}/{{store.ExternalSiteId}}/MenuNavigation">
                                    Menu
                                </a>
                            </div>
                        </div>
                `
        };
    });

    storeAdminComponent.directive("storeReportingLinks", () => {
        return {
            name: "storeReportingLinks",
            scope: {
                store: "=store"
            },
            controller: ($scope) => {
                $scope.authorizeEnrolement = (store: any, enrolement: string[]) => {
                    Logger.Notify("authorize store?");

                    let enrolements: Array<{ Name: string }> = store.StoreEnrollments;
                    var found = false;
                    var show = enrolements.filter(e => enrolement.filter(k => k === e.Name).length > 0).length > 0;

                    return show;
                };
            },
            template: `
     
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <i class="fa fa-area-chart"></i>
                            Reporting
                        </div>
                        <div class="panel-body">
                            <div class="btn-group">

                                <a ng-if="authorizeEnrolement(store, ['ACS Reports','GPRS store'])" class="btn btn-default btn-sm" href="/Reporting/Chain/{{store.ChainId}}/{{store.ExternalSiteId}}/Store">
                                    Web
                                </a>

                                <a ng-if="authorizeEnrolement(store, ['Rameses (AMS) Reports'])" class="btn btn-default btn-sm" href="/Reporting/Chain/{{store.ChainId}}/{{store.ExternalSiteId}}/DailyReporting">
                                    Rameses
                                </a>

                            </div>

                        </div>
                    </div>

                
            `
        };
    });
    
    storeAdminComponent.directive("storeAdminGroup", () => {

        return {
            name: "storeAdminLinkGroup",
            scope: {
                store: "=store"
            },
            controller: "storeAdminLinkGroupController",
            template: 
                `
                <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-4">
                        <store-admin-links store="store"></store-admin-links>
                    </div>
                    <div class="col-sm-4">
                        <store-detail-links store="store"></store-detail-links>    
                    </div>
                    <div class="col-sm-4">
                        <store-reporting-links store="store"></store-reporting-links>
                    </div>
                </div>
                </div>
            
            `
        };

    });

}