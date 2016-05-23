module MyAndromeda.Reports.ChainDashboard {
    var app = angular.module("ChainDashboard.StoreMap", ["ChainDashboard.Services"]);
    
    app.controller("storeMapDashboardSummaryController", ($scope,
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

            groupedStoreResultsService.LoadStore($stateParams.storeId);
        }
    });

    app.controller("storeMapDashboardDetailController", ($scope,
        $timeout,
        $stateParams, orderService: Data.Services.OrderService,
        dashboardQueryContext: DashboardQueryContext,
        valueFormater: ValueFormater) => {

        $scope.currency = valueFormater.CurrencyDecimal;
        //var dataSource = orderService.ListOrders($stateParams.storeId);
        var dataSource = orderService.ListOrdersForMap(1885, dashboardQueryContext.Query.FromObj, dashboardQueryContext.Query.ToObj);

        dashboardQueryContext.Changed.throttle(300).subscribe(() => {
            dataSource.read();
        });

        var getMap = () => {
            return $scope.myMap;
        };

        var selectMarker = (e: kendo.dataviz.ui.MapMarkerClickEvent) => {
            var marker: any = e.marker;
            var dataItem: MyAndromeda.Data.Services.Models.IOrderHeader = marker.dataItem;

            Logger.Notify(dataItem);

            $timeout(() => {
                $scope.highlightedOrder = dataItem;
            });
        };

        var mapOptions: kendo.dataviz.ui.MapOptions = {
            markerClick: selectMarker,
            center: [0, 0],
            zoom: 2,
            layers: [
                {
                    type: "tile",
                    urlTemplate: "http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png",
                    subdomains: ["a", "b", "c"],
                    attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap contributors</a>"
                },
                {
                    type: "marker",
                    dataSource: dataSource,
                    locationField: "Customer.GeoLocation"
                }
            ],
            //markers: [{
            //    location: [30.268107, -97.744821],
            //    shape: "pin",
            //}]
        };

        $scope.mapOptions = mapOptions;
    });

    


}