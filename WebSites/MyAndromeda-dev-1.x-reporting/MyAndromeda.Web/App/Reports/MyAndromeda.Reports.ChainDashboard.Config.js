var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.Config", [
                "ui.router",
                "ngAnimate"
            ]);
            app.config(function ($stateProvider, $urlRouterProvider) {
                var app = {
                    abstract: true,
                    url: "/",
                    templateUrl: "dashboard-app-template.html"
                };
                var appChainSalesDashboard = {
                    url: "/sales-dashboard",
                    views: {
                        "summary": {
                            templateUrl: "chain-sales-dashboard-summary.html",
                            controller: "chainSalesDashboardSummaryController"
                        },
                        "detail": {
                            templateUrl: "chain-sales-dashboard-detail.html",
                            controller: "chainSalesDashboardDetailController"
                        }
                    }
                };
                var appStoreSalesDashboard = {
                    url: "/store-sales-dashboard/:storeId",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-dashboard-summary.html",
                            controller: "storeSalesDashboardSummaryController"
                        },
                        "detail": {
                            templateUrl: "store-sales-dashboard-detail.html",
                            controller: "storeSalesDashboardDetailController"
                        }
                    },
                    onEnter: function ($stateParams, groupedStoreResultsService) {
                        groupedStoreResultsService.LoadStore($stateParams.storeId);
                    },
                    cache: false
                };
                var appChainSalesDataWarehouse = {
                    url: "/chain-sales-live",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-day-dashboard-summary.html",
                            controller: "chainTodaysSalesSummaryController"
                        },
                        "detail": {
                            templateUrl: "store-sales-day-dashboard-detail.html",
                            controller: "chainTodaysSalesDetailController"
                        }
                    }
                };
                var appStoreSalesLive = {
                    url: "/store-sales-live/:storeId",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-day-dashboard-summary.html",
                            controller: "storeSalesDayDashboardSummaryController"
                        },
                    },
                    onEnter: function ($stateParams, groupedStoreResultsService) {
                        groupedStoreResultsService.LoadStore($stateParams.storeId);
                    },
                    cache: false
                };
                var storeMapDashboard = {
                    url: "/store-map-live/:storeId",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-map.summary.html",
                            controller: "storeMapDashboardSummaryController"
                        },
                        "detail": {
                            templateUrl: "store-sales-map.detail.html",
                            controller: "storeMapDashboardDetailController"
                        }
                    },
                    onEnter: function ($stateParams, groupedStoreResultsService) {
                        groupedStoreResultsService.LoadStore($stateParams.storeId);
                    },
                    cache: false
                };
                //$stateProvider.state("app", app);
                $stateProvider.state("chain-sales-dashboard", appChainSalesDashboard);
                $stateProvider.state("chain-sales-data-warehouse", appChainSalesDataWarehouse);
                $stateProvider.state("store-sales-dashboard", appStoreSalesDashboard);
                $stateProvider.state("store-sales-live", appStoreSalesLive);
                $stateProvider.state("store-map-live", storeMapDashboard);
                $urlRouterProvider.otherwise("/sales-dashboard");
            });
            app.run(function ($templateCache) {
                MyAndromeda.Logger.Notify("WebHooks Started");
                angular
                    .element('script[type="text/ng-template"]')
                    .each(function (i, element) {
                    $templateCache.put(element.id, element.innerHTML);
                });
            });
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
