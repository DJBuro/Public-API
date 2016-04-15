var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.StoreMap", ["ChainDashboard.Services"]);
            app.controller("storeMapDashboardSummaryController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, chartOptions, valueFormater) {
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
                    groupedStoreResultsService.LoadStore($stateParams.storeId);
                }
            });
            app.controller("storeMapDashboardDetailController", function ($scope, $timeout, $stateParams, orderService, dashboardQueryContext, valueFormater) {
                $scope.currency = valueFormater.CurrencyDecimal;
                //var dataSource = orderService.ListOrders($stateParams.storeId);
                var dataSource = orderService.ListOrdersForMap(1885, dashboardQueryContext.Query.FromObj, dashboardQueryContext.Query.ToObj);
                dashboardQueryContext.Changed.throttle(300).subscribe(function () {
                    dataSource.read();
                });
                var getMap = function () {
                    return $scope.myMap;
                };
                var selectMarker = function (e) {
                    var marker = e.marker;
                    var dataItem = marker.dataItem;
                    MyAndromeda.Logger.Notify(dataItem);
                    $timeout(function () {
                        $scope.highlightedOrder = dataItem;
                    });
                };
                var mapOptions = {
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
                };
                $scope.mapOptions = mapOptions;
            });
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
