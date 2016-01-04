var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var dailySummaryDataService = (function () {
                function dailySummaryDataService(options) {
                    this.options = options;
                    this.initDataSource();
                }
                dailySummaryDataService.prototype.initDataSource = function () {
                    var internal = this;
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: internal.options.url,
                                data: internal.options.data,
                                type: "POST",
                                dataType: "json"
                            }
                        },
                        schema: {
                            model: {
                                fields: {
                                    "Date": { "type": "date" }
                                }
                            },
                            data: function (result) {
                                return $.map(result.Data, function (element, index) {
                                    var p = element.Performance.NumOrdersLT30Mins;
                                    var t = element.Combined.OrderCount;
                                    element.Performance.NumOrdersLT30MinsPercentage = p / t;
                                    return element;
                                });
                            },
                            total: function (result) { return result.Data.length; }
                        },
                        aggregate: [
                            { field: "Combined.OrderCount", aggregate: "sum" },
                            { field: "Combined.OrderCount", aggregate: "average" },
                            { field: "Combined.OrderCount", aggregate: "max" },
                            { field: "Combined.OrderCount", aggregate: "min" },
                            { field: "Performance.AvgDoorTime", aggregate: "average" },
                            { field: "Performance.AvgDoorTime", aggregate: "max" },
                            { field: "Performance.AvgDoorTime", aggregate: "min" },
                            { field: "Performance.AvgOutTheDoor", aggregate: "average" },
                            { field: "Performance.AvgOutTheDoor", aggregate: "max" },
                            { field: "Performance.AvgOutTheDoor", aggregate: "min" },
                            { field: "Performance.AvgMakeTime", aggregate: "average" },
                            { field: "Performance.AvgMakeTime", aggregate: "max" },
                            { field: "Performance.AvgMakeTime", aggregate: "min" },
                            { field: "Performance.AvgRackTime", aggregate: "average" },
                            { field: "Performance.AvgRackTime", aggregate: "max" },
                            { field: "Performance.AvgRackTime", aggregate: "min" },
                            { field: "Performance.NumOrdersLT30Mins", aggregate: "sum" },
                            { field: "Performance.NumOrdersLT30Mins", aggregate: "average" }
                        ]
                    });
                };
                dailySummaryDataService.prototype.bind = function (eventName, handler) {
                    this.dataSource.bind(eventName, handler);
                };
                return dailySummaryDataService;
            })();
            Services.dailySummaryDataService = dailySummaryDataService;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var dailyPerformanceService = (function () {
                function dailyPerformanceService(options) {
                    //console.log("new dailyPerformanceService");
                    this.options = options;
                    this.dashboardSalesService = new Services.dailySummaryDataService(options);
                    this.bound = false;
                }
                dailyPerformanceService.prototype.bindViewModel = function () {
                    //console.log("try bind");
                    if (this.bound) {
                        return;
                    }
                    var element = $(this.options.elementId);
                    //console.log("bind");
                    kendo.bind(element, this.viewModel);
                    this.bound = true;
                };
                dailyPerformanceService.prototype.setupResults = function () {
                    var dataSource = this.dashboardSalesService.dataSource, result = this.dashboardSalesService.result;
                    var orderCount = null, doorTime = null, outDoorTime = null, makeTime = null, rackTime = null, less30Mins = null;
                    try {
                        var aggs = this.dashboardSalesService.dataSource.aggregates();
                        orderCount = aggs["Combined.OrderCount"];
                        doorTime = aggs["Performance.AvgDoorTime"];
                        outDoorTime = aggs["Performance.AvgOutTheDoor"];
                        makeTime = aggs["Performance.AvgMakeTime"];
                        rackTime = aggs["Performance.AvgRackTime"];
                        less30Mins = aggs["Performance.NumOrdersLT30Mins"];
                        less30Mins.percent = function () {
                            var sum = less30Mins.sum;
                            var total = orderCount.sum;
                            return sum / total;
                        };
                    }
                    catch (e) {
                        console.log(e);
                    }
                    this.viewModel = kendo.observable({
                        dataSource: dataSource,
                        orderCount: orderCount,
                        doorTime: doorTime,
                        outDoorTime: outDoorTime,
                        makeTime: makeTime,
                        rackTime: rackTime,
                        less30Mins: less30Mins,
                        showCharts: function () {
                            return dataSource.total() > 1;
                        }
                    });
                    this.bindViewModel();
                    kendo.ui.progress($(this.options.elementId), false);
                };
                dailyPerformanceService.prototype.init = function () {
                    var internal = this;
                    var element = $(this.options.elementId);
                    kendo.ui.progress(element, true);
                    element.data("Reporting.DailySummaryService", this);
                    internal.dashboardSalesService.dataSource.fetch($.proxy(this.setupResults, internal));
                };
                return dailyPerformanceService;
            })();
            Services.dailyPerformanceService = dailyPerformanceService;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
