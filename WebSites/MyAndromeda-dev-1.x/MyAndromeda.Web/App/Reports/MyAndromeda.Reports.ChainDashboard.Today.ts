module MyAndromeda.Reports.ChainDashboard {

    var app = angular.module("ChainDashboard.Today", ["ChainDashboard.Services"]);

    

    app.controller("chainTodaysSalesSummaryController", (
        $scope, $stateParams,
        $timeout,
        dashboardQueryContext: DashboardQueryContext,
        chartOptions: ChartOptions,
        groupedDataWarehouseStoreResultsService: GroupedDataWarehouseStoreResultsService,
        valueFormater: ValueFormater) => {

        //only work with 5;
        //var observable = groupedDataWarehouseStoreResultsService
        //    .LoadChain(settings.chainId)
        //    .where(e => e !== null);

        var observable = groupedDataWarehouseStoreResultsService
            .LoadChain(settings.chainId, null)
            .where(e => e !== null);

        var changeSubscription = dashboardQueryContext.Changed.throttle(700).subscribe(b => {
            groupedDataWarehouseStoreResultsService
                .LoadChain(settings.chainId, dashboardQueryContext.Query)

        });

        observable.subscribe((data) => {

            var allOrders = Rx.Observable.from(data.Stores).selectMany((store) => Rx.Observable.from(store.Orders));
            
            var r = groupedDataWarehouseStoreResultsService.CreaateTotals(data.Name, allOrders);

            $timeout(() => {
                $scope.summary = r; 
            });

        });
        
        $scope.occasionChartOptions = chartOptions.DataWareHouseOccasionChart();
        $scope.acsChartOptions = chartOptions.DataWareHouseAcsApplicationChart();
        $scope.currency = valueFormater.CurrencyDecimal;
    });

    app.controller("chainTodaysSalesDetailController", ($scope, $stateParams,
        $timeout,
        dashboardQueryContext: DashboardQueryContext,
        chartOptions: ChartOptions,
        groupedDataWarehouseStoreResultsService: GroupedDataWarehouseStoreResultsService,
        valueFormater: ValueFormater) => {

        var observable = groupedDataWarehouseStoreResultsService
            .LoadChain(5, null)
            .where(e => e !== null);

        observable.subscribe((data) => {
            var summaries = [];
            //var allOrders = Rx.Observable.from(data.Stores).selectMany((store) => Rx.Observable.from(store.Orders));

            var stores = Rx.Observable.from(data.Stores);
            stores.subscribe((store) => {

                var allOrders = Rx.Observable.from(store.Orders);
                var r = groupedDataWarehouseStoreResultsService.CreaateTotals(store.Name, allOrders);
                
                summaries.push(r);
            });

            $timeout(() => {
                $scope.summaries = summaries;
            });

        });

        $scope.currency = valueFormater.CurrencyDecimal;
    });


    app.controller("storeSalesDayDashboardSummaryController", ($scope,
        $timeout,
        $stateParams,
        groupedStoreResultsService: GroupedStoreResultsService,
        groupedStoreResultsDataService: GroupedStoreResultsDataService,
        dashboardQueryContext: DashboardQueryContext,
        chartOptions: ChartOptions,
        valueFormater: ValueFormater) => {

        Logger.Notify("Starting storeSalesDayDashboardSummaryController");

        $scope.currency = valueFormater.Currency;
        $scope.store = groupedStoreResultsService.StoreData;

        groupedStoreResultsService.StoreObservable.subscribe((store) => {
            Logger.Notify("set store");

            $timeout(() => {
                $scope.store = store;
            });
        });

        if (groupedStoreResultsService.StoreData == null) {
            Logger.Notify("load store data");

            //ignore store 
            groupedStoreResultsService.LoadStore($stateParams.storeId);
        }
    });


}