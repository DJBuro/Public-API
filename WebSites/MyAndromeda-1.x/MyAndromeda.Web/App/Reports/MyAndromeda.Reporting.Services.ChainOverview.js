var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
        (function (Services) {
            var chainGridDetailSumarryService = (function () {
                function chainGridDetailSumarryService(parent, contentElement, actionsElement, data) {
                    this.parent = parent;
                    this.contentElement = contentElement;
                    this.actionsElement = actionsElement;

                    this.data = data;

                    this.url = parent.options.hourlyUrl.replace("EXTERNALSITEID", this.data.ExternalSiteId);

                    this.contentViewModel = kendo.observable({
                        Data: null
                    });

                    this.setupActionsViewModel();
                    this.setupContentViewModel();
                }
                chainGridDetailSumarryService.prototype.setupActionsViewModel = function () {
                    var internal = this;
                    this.actionsViewModel = kendo.observable({
                        visibleSales: true,
                        visibleOrderCount: true,
                        visiblePerformance: true
                    });

                    this.actionsViewModel.bind("change", function (e) {
                        //console.log("ive changed");
                        internal.onActionsChange(e);
                        //$.proxy(internal.onActionsChange, internal)
                    });

                    kendo.bind(this.actionsElement, this.actionsViewModel);
                };
                chainGridDetailSumarryService.prototype.setupContentViewModel = function () {
                    var grid = this.parent.getGrid(), dataSource = this.parent.getDataSource();

                    if (this.data.Data && this.data.Data.length > 1) {
                        this.setupContentViewModelFromExisting();
                    } else {
                        this.LoadHourly();
                    }

                    kendo.bind(this.contentElement, this.contentViewModel);
                };

                chainGridDetailSumarryService.prototype.setupContentViewModelFromExisting = function () {
                    var chartData = this.data.Data;
                    var dataSource = new kendo.data.DataSource({
                        data: chartData,
                        schema: {
                            model: {
                                fields: {
                                    "Date": { "type": "date" }
                                }
                            }
                        }
                    });
                    this.contentViewModel.set("Data", dataSource);
                };

                chainGridDetailSumarryService.prototype.LoadHourly = function () {
                    var hourlyDataSource = new kendo.data.DataSource({
                        transport: {
                            read: {
                                data: {},
                                type: "POST",
                                dataType: "json",
                                url: this.url
                            }
                        },
                        schema: {
                            model: {
                                fields: {
                                    "Date": { type: "date" }
                                }
                            },
                            data: function (result) {
                                return result.Data;
                            },
                            total: function (result) {
                                return result.Data.length;
                            }
                        }
                    });

                    this.contentViewModel.set("Data", hourlyDataSource);
                    hourlyDataSource.read();
                };

                chainGridDetailSumarryService.prototype.onActionsChange = function (e) {
                    //console.log("ive changed");
                    var chart = this.getGridDetailChart(this.contentElement);
                    var series = chart.options.series;
                    var valueAxis = chart.options.valueAxis;

                    series.forEach(function (seriesSet) {
                        var a = seriesSet;
                        if (e.field === "visibleOrderCount" && seriesSet.axis === "orderCount") {
                            a.visible = !a.visible;
                        }
                        if (e.field === "visibleSales" && seriesSet.axis === "sales") {
                            a.visible = !a.visible;
                        }
                        if (e.field === "visiblePerformance" && (seriesSet.axis === "otd" || seriesSet.axis === "ttd")) {
                            a.visible = !a.visible;
                        }
                    });

                    valueAxis.forEach(function (axis) {
                        if (e.field === "visibleOrderCount" && axis.name === "orderCount") {
                            axis.visible = !axis.visible;
                        }
                        if (e.field === "visibleSales" && axis.name === "sales") {
                            axis.visible = !axis.visible;
                        }
                        if (e.field === "visiblePerformance" && (axis.name === "otd" || axis.name === "ttd")) {
                            axis.visible = !axis.visible;
                        }
                    });

                    chart.redraw();
                };

                chainGridDetailSumarryService.prototype.getGridDetailChart = function (content) {
                    var contentChart = $(content).find(".k-chart-detail").data("kendoChart");
                    return contentChart;
                };
                return chainGridDetailSumarryService;
            })();
            Services.chainGridDetailSumarryService = chainGridDetailSumarryService;
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
