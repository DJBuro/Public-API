var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.Today", ["ChainDashboard.Services"]);
            app.controller("chainTodaysSalesSummaryController", function ($scope, $stateParams, $timeout, dashboardQueryContext, chartOptions, groupedDataWarehouseStoreResultsService, valueFormater) {
                //only work with 5;
                //var observable = groupedDataWarehouseStoreResultsService
                //    .LoadChain(settings.chainId)
                //    .where(e => e !== null);
                var observable = groupedDataWarehouseStoreResultsService
                    .LoadChain(ChainDashboard.settings.chainId, null)
                    .where(function (e) { return e !== null; });
                var changeSubscription = dashboardQueryContext.Changed.throttle(700).subscribe(function (b) {
                    groupedDataWarehouseStoreResultsService
                        .LoadChain(ChainDashboard.settings.chainId, dashboardQueryContext.Query);
                });
                observable.subscribe(function (data) {
                    var allOrders = Rx.Observable.from(data.Stores).selectMany(function (store) { return Rx.Observable.from(store.Orders); });
                    var r = groupedDataWarehouseStoreResultsService.CreaateTotals(data.Name, allOrders);
                    $timeout(function () {
                        $scope.summary = r;
                    });
                });
                $scope.occasionChartOptions = chartOptions.DataWareHouseOccasionChart();
                $scope.acsChartOptions = chartOptions.DataWareHouseAcsApplicationChart();
                $scope.currency = valueFormater.CurrencyDecimal;
            });
            app.controller("chainTodaysSalesDetailController", function ($scope, $stateParams, $timeout, dashboardQueryContext, chartOptions, groupedDataWarehouseStoreResultsService, valueFormater) {
                var observable = groupedDataWarehouseStoreResultsService
                    .LoadChain(5, null)
                    .where(function (e) { return e !== null; });
                observable.subscribe(function (data) {
                    var summaries = [];
                    //var allOrders = Rx.Observable.from(data.Stores).selectMany((store) => Rx.Observable.from(store.Orders));
                    var stores = Rx.Observable.from(data.Stores);
                    stores.subscribe(function (store) {
                        var allOrders = Rx.Observable.from(store.Orders);
                        var r = groupedDataWarehouseStoreResultsService.CreaateTotals(store.Name, allOrders);
                        summaries.push(r);
                    });
                    $timeout(function () {
                        $scope.summaries = summaries;
                    });
                });
                $scope.currency = valueFormater.CurrencyDecimal;
            });
            app.controller("storeSalesDayDashboardSummaryController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, chartOptions, valueFormater) {
                MyAndromeda.Logger.Notify("Starting storeSalesDayDashboardSummaryController");
                $scope.currency = valueFormater.Currency;
                $scope.store = groupedStoreResultsService.StoreData;
                groupedStoreResultsService.StoreObservable.subscribe(function (store) {
                    MyAndromeda.Logger.Notify("set store");
                    $timeout(function () {
                        $scope.store = store;
                    });
                });
                if (groupedStoreResultsService.StoreData == null) {
                    MyAndromeda.Logger.Notify("load store data");
                    //ignore store 
                    groupedStoreResultsService.LoadStore($stateParams.storeId);
                }
            });
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
