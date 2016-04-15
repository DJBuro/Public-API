/// <reference path="../../scripts/typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="myandromeda.reports.chaindashboard.models.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard", [
                "MyAndromeda.Resize",
                "MyAndromeda.Progress",
                "MyAndromeda.Data.Orders",
                "ChainDashboard.Services",
                "ChainDashboard.StoreMap",
                "ChainDashboard.Today",
                "ChainDashboard.Config",
                "kendo.directives",
                "oitozero.ngSweetAlert",
                "ngAnimate"
            ]);
            app.controller("chainSalesDashboardSummaryController", function ($scope, $timeout, groupedStoreResultsService, valueFormater, chartOptions) {
                $scope.hi = "hi ho";
                $scope.data = [];
                $scope.currency = valueFormater.Currency;
                var radarOptions = {
                    legend: {
                        position: "top"
                    },
                    categoryAxis: [
                        {
                            field: "WeekOfYear",
                            baseUnitStep: "auto",
                        }
                    ],
                    valueAxis: [
                        { title: "Sales", name: "Sales" },
                        { title: "Orders", name: "Orders" }
                    ],
                    //seriesDefaults: { type: "radarLine" },
                    series: [
                        { name: "Sales", field: "Total.NetSales", type: "area", axis: "Sales" },
                        { name: "Order Count", field: "Total.OrderCount", type: "column", axis: "Orders" }
                    ]
                };
                $scope.weekChartOptions = chartOptions.WeekChart();
                $scope.dayChartOptions = chartOptions.DayChart("days");
                $scope.weekChartOptions = chartOptions.DayChart("weeks");
                $scope.monthChartOptions = chartOptions.DayChart("months");
                var salesWeekData = new kendo.data.DataSource({});
                var salesMonthData = new kendo.data.DataSource({
                    group: [
                        { field: "CreatedTimeStamp" }
                    ],
                    schema: {
                        model: {
                            fields: {
                                CreatedTimeStamp: {
                                    type: "date"
                                }
                            }
                        }
                    }
                });
                $scope.radarOptions = radarOptions;
                $scope.salesDayData = salesMonthData;
                $scope.salesWeekData = salesWeekData;
                groupedStoreResultsService.ChainDataObservable.where(function (e) { return e !== null; }).subscribe(function (chainResult) {
                    $timeout(function () {
                        $scope.data = chainResult;
                        $scope.storeCount = chainResult.Data.length;
                        MyAndromeda.Logger.Notify("$scope.data :");
                        MyAndromeda.Logger.Notify(chainResult);
                        Rx.Observable.from(chainResult.WeekData).maxBy(function (e) { return e.Total.NetSales; }).subscribe(function (max) {
                            MyAndromeda.Logger.Notify("$scope.bestWeek");
                            MyAndromeda.Logger.Notify(max[0]);
                            $scope.bestWeek = max[0];
                        });
                        var dayData = [];
                        var dayDataObservable = Rx.Observable.from(chainResult.Data).flatMap(function (e) { return Rx.Observable.fromArray(e.DailyData); });
                        dayDataObservable.subscribe(function (o) {
                            dayData.push(o);
                        });
                        MyAndromeda.Logger.Notify("day data");
                        MyAndromeda.Logger.Notify(dayData);
                        salesWeekData.data(chainResult.WeekData);
                        salesMonthData.data(dayData);
                    });
                });
            });
            app.controller("chainSalesDashboardDetailController", function ($scope, $timeout, $state, groupedStoreResultsService, valueFormater) {
                $scope.data = [];
                $scope.currency = valueFormater.Currency;
                $scope.select = function (store) {
                    groupedStoreResultsService.StoreData = store;
                    $state.go("store-sales-dashboard", { storeId: store.StoreId });
                };
                groupedStoreResultsService.ChainDataObservable.subscribe(function (data) {
                    $scope.data = data;
                });
            });
            app.controller("storeSalesDashboardSummaryController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, valueFormater) {
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
            app.controller("storeSalesDashboardDetailController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, chartOptions, valueFormater) {
                $scope.storeWeekOptions = chartOptions.WeekChart();
                $scope.storeDailyOptions = chartOptions.DayChart("days");
                $scope.storeWeeklyOptions = chartOptions.DayChart("weeks");
                $scope.storeMonthlyOptions = chartOptions.DayChart("months");
                var weekDataSource = new kendo.data.DataSource({});
                var dailyDataSource = new kendo.data.DataSource({
                    schema: {
                        model: {
                            fields: {
                                CreateTimeStamp: {
                                    type: "date"
                                }
                            }
                        }
                    }
                });
                $scope.storeWeekData = weekDataSource;
                $scope.dailyDataSource = dailyDataSource;
                var storeObservable = groupedStoreResultsService.StoreObservable.where(function (e) { return e !== null; }).subscribe(function (store) {
                    MyAndromeda.Logger.Notify("reload charts");
                    $timeout(function () {
                        $scope.store = store;
                        weekDataSource.data(store.WeekData);
                        dailyDataSource.data(store.DailyData);
                    });
                });
            });
            app.directive("dashboardAppFilter", function () {
                return {
                    controller: function ($scope, dashboardQueryContext) {
                        $scope.query = dashboardQueryContext.Query;
                        //dashboardQueryContext.Query.ToObj
                        $scope.$watch('query.ToObj', function () {
                            MyAndromeda.Logger.Notify("changes");
                            dashboardQueryContext.Changed.onNext(true);
                        });
                        $scope.$watch('query.FromObj', function () {
                            MyAndromeda.Logger.Notify("changes");
                            dashboardQueryContext.Changed.onNext(true);
                        });
                        $scope.today = function () {
                            dashboardQueryContext.Query.FromObj = new Date();
                        };
                        $scope.createPdf = function () {
                            MyAndromeda.Logger.Notify("hi");
                            var region = $(".pdfRegion");
                            kendo.drawing.drawDOM(region)
                                .then(function (group) {
                                // Render the result as a PDF file
                                return kendo.drawing.exportPDF(group, {
                                    creator: "Andromeda",
                                    keywords: "Chain,Report",
                                    date: new Date(),
                                    landscape: false,
                                    subject: "Report",
                                    title: "Chain",
                                    paperSize: "auto",
                                    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                                });
                            })
                                .done(function (data) {
                                // Save the PDF file
                                kendo.saveAs({
                                    dataURI: data,
                                    fileName: "Dashboard.pdf",
                                });
                            });
                        };
                    },
                    restrict: "E",
                    templateUrl: "dashboard-app-filter.html"
                };
            });
            ChainDashboard.settings = {
                chainId: 0
            };
            function setupChainDashboard(id) {
                var element = document.getElementById(id);
                angular.bootstrap(element, ["ChainDashboard"]);
            }
            ChainDashboard.setupChainDashboard = setupChainDashboard;
            ;
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
