/// <reference path="../../scripts/typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="myandromeda.reports.chaindashboard.models.d.ts" />
module MyAndromeda.Reports.ChainDashboard {

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

    app.controller("chainSalesDashboardSummaryController", ($scope, $timeout,
        groupedStoreResultsService: GroupedStoreResultsService,
        valueFormater: ValueFormater,
        chartOptions: ChartOptions) => {
        $scope.hi = "hi ho";
        $scope.data = [];
        $scope.currency = valueFormater.Currency;

        var radarOptions: kendo.dataviz.ui.ChartOptions = {
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
                //{ name: "Order Types", field: 
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

        groupedStoreResultsService.ChainDataObservable.where(e=> e !== null).subscribe((chainResult) => {
            $timeout(() => {
                $scope.data = chainResult;
                $scope.storeCount = chainResult.Data.length;

                Logger.Notify("$scope.data :");
                Logger.Notify(chainResult);

                Rx.Observable.from(chainResult.WeekData).maxBy(e=> e.Total.NetSales).subscribe((max) => {
                    Logger.Notify("$scope.bestWeek");
                    Logger.Notify(max[0]);
                    $scope.bestWeek = max[0];
                });

                var dayData = [];
                var dayDataObservable = Rx.Observable.from(chainResult.Data).flatMap(e=> Rx.Observable.fromArray(e.DailyData));
                dayDataObservable.subscribe((o) => {
                    dayData.push(o);
                });

                Logger.Notify("day data");
                Logger.Notify(dayData);
                salesWeekData.data(chainResult.WeekData);
                salesMonthData.data(dayData);
            });
        });

    });

    app.controller("chainSalesDashboardDetailController", (
        $scope, $timeout,
        $state: ng.ui.IStateService,
        groupedStoreResultsService: GroupedStoreResultsService, valueFormater: ValueFormater) =>
    {
        $scope.data = [];
        $scope.currency = valueFormater.Currency;
        $scope.select = (store: Models.IGroupedStoreResults) => {
            groupedStoreResultsService.StoreData = store;
            $state.go("store-sales-dashboard", { storeId: store.StoreId });
        };

        groupedStoreResultsService.ChainDataObservable.subscribe((data) => {
            $scope.data = data;
        });
    });


    app.controller("storeSalesDashboardSummaryController", (
        $scope,
        $timeout,
        $stateParams,
        groupedStoreResultsService: GroupedStoreResultsService,
        groupedStoreResultsDataService: GroupedStoreResultsDataService,
        dashboardQueryContext: DashboardQueryContext,
        valueFormater: ValueFormater) => {

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

    app.controller("storeSalesDashboardDetailController", ($scope,
        $timeout,
        $stateParams,
        groupedStoreResultsService: GroupedStoreResultsService,
        groupedStoreResultsDataService: GroupedStoreResultsDataService,
        dashboardQueryContext: DashboardQueryContext,
        chartOptions: ChartOptions,
        valueFormater: ValueFormater) => {

        $scope.storeWeekOptions = chartOptions.WeekChart();
        $scope.storeDailyOptions = chartOptions.DayChart("days");
        $scope.storeWeeklyOptions = chartOptions.DayChart("weeks");
        $scope.storeMonthlyOptions = chartOptions.DayChart("months");

        var weekDataSource = new kendo.data.DataSource({

        });
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

        var storeObservable = groupedStoreResultsService.StoreObservable.where(e=> e !== null).subscribe((store) => {
            Logger.Notify("reload charts");
            $timeout(() => {
                $scope.store = store;

                weekDataSource.data(store.WeekData);
                dailyDataSource.data(store.DailyData);
            });
        });
    });

    

    app.directive("dashboardAppFilter", () => {
        return {
            controller: ($scope, dashboardQueryContext: DashboardQueryContext) => {
                $scope.query = dashboardQueryContext.Query;
                //dashboardQueryContext.Query.ToObj
                $scope.$watch('query.ToObj', () => {
                    Logger.Notify("changes");
                    dashboardQueryContext.Changed.onNext(true);
                });

                $scope.$watch('query.FromObj', () => {
                    Logger.Notify("changes");
                    dashboardQueryContext.Changed.onNext(true);
                });

                $scope.today = () => {
                    dashboardQueryContext.Query.FromObj = new Date();
                };

                $scope.createPdf = () => {
                    Logger.Notify("hi");
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
                            title :"Chain",
                            paperSize: "auto",
                            margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                        });
                    })
                    .done(function (data) {
                        // Save the PDF file
                        kendo.saveAs({
                            dataURI: data,
                            fileName: "Dashboard.pdf",
                            //proxyURL: "//demos.telerik.com/kendo-ui/service/export"
                        });
                    });
                };

            },
            restrict: "E",
            templateUrl: "dashboard-app-filter.html"
        }
    });

    

    export var settings = {
        chainId : 0
    };

    export function setupChainDashboard (id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, ["ChainDashboard"]);
    };
}