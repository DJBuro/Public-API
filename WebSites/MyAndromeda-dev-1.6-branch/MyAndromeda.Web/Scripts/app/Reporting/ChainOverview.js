var MyAndromeda;
(function (MyAndromeda) {
    (function (Services) {
        (function (Reporting) {
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
            Reporting.chainGridDetailSumarryService = chainGridDetailSumarryService;

            var chainDailySummaryService = (function () {
                function chainDailySummaryService(options) {
                    this.loadDelay = 250;
                    this.options = options;
                    var internal = this;
                    this.graphs = [];
                }
                chainDailySummaryService.prototype.generateGridChartActions = function () {
                    var internal = this, grid = $(this.options.gridElement), dataSource = this.getDataSource(), selectedTabItem = internal.getSelctedTab();

                    if (!this.isSelectedTab() || this.loaded)
                        return;

                    internal.graphs.length = 0;
                    $(internal.options.gridElement).find(".chain-sparkline").each(function (index, element) {
                        var e = $(element);
                        var uid = e.closest("tr").data("uid");
                        var dataRow = dataSource.getByUid(uid);
                        var dataSet = dataRow;
                        var data = dataRow.Data;

                        if (data.length == 0 || data.length == 1) {
                            return;
                        }

                        var descriptiveData = new kendo.data.DataSource({
                            data: data,
                            schema: {
                                model: {
                                    fields: {
                                        "Date": { "type": "date" }
                                    }
                                }
                            }
                        });
                        var action = null;
                        var showType = e.data("rep");
                        if (showType === "sales") {
                            action = function () {
                                internal.generateGridCharts(e, "Total Sales", "Combined.Sales", "area", descriptiveData, "#: kendo.toString(value/100, 'c') #");
                            };
                        }
                        if (showType === "orders") {
                            action = function () {
                                internal.generateGridCharts(e, "Total Orders", "Combined.OrderCount", "column", descriptiveData, "#: value #");
                            };
                        }
                        if (showType === "avgSpend") {
                            action = function () {
                                internal.generateGridCharts(e, "Avg Spend", "Combined.AvgSale", "area", descriptiveData, "#: kendo.toString(value/100, 'c') #");
                            };
                        }
                        if (showType === "otd") {
                            action = function () {
                                internal.generateGridCharts(e, "OTD", "Performance.AvgOutTheDoor", "line", descriptiveData, "#: value #");
                            };
                        }
                        if (showType === "ttd") {
                            action = function () {
                                internal.generateGridCharts(e, "TTD", "Performance.AvgDoorTime", "line", descriptiveData, "#: value #");
                            };
                        }

                        internal.graphs.push(action);
                    });

                    if (internal.graphs.length > 0) {
                        if (internal.graphs.length > 25) {
                            internal.graphs.forEach(function (act) {
                                setTimeout(function () {
                                    act();
                                }, internal.loadDelay);
                            });
                        } else {
                            internal.graphs.forEach(function (act) {
                                act();
                            });
                        }
                    }

                    this.loaded = true;
                };

                chainDailySummaryService.prototype.generateGridCharts = function (element, name, field, type, data, format) {
                    var e = $(element);
                    format || (format = "#: value #");
                    e.kendoSparkline({
                        theme: "bootstrap",
                        renderAs: kendo.support.mobileOS ? "canvas" : "svg",
                        series: [{
                                name: name, field: field, type: type
                            }],
                        dataSource: data,
                        tooltip: {
                            template: "<div>#: kendo.toString(dataItem.Date, 'd')# (#: kendo.toString(dataItem.Date, 'ddd') #)</div><div>" + format + "</div>"
                        },
                        chartArea: {
                            background: "transparent"
                        }
                    });
                };

                chainDailySummaryService.prototype.getGrid = function () {
                    return $(this.options.gridElement).data("kendoGrid");
                };

                chainDailySummaryService.prototype.getDataSource = function () {
                    var grid = this.getGrid(), dataSource = grid.dataSource;
                    return dataSource;
                };

                chainDailySummaryService.prototype.getTabStrip = function () {
                    var tabStrip = $(this.options.tabStripElement).data("kendoTabStrip");
                    return tabStrip;
                };

                chainDailySummaryService.prototype.getSelctedTab = function () {
                    var selectedElement = $(this.options.tabStripElement).find(".k-tabstrip-items").find(".k-state-active");
                    var name = selectedElement.data("tabName");
                    return name;
                };

                chainDailySummaryService.prototype.isSelectedTab = function () {
                    return this.getSelctedTab() === this.options.tabStripItemName;
                };

                chainDailySummaryService.prototype.runJob = function () {
                    this.graphs.forEach(function (value) {
                        value();
                    });
                };

                chainDailySummaryService.prototype.init = function () {
                    $(this.options.elementId).data("Reporting.chainDailySummaryService", this);

                    var internal = this;
                    var grid = this.getGrid();

                    grid.bind("detailInit", function (e) {
                        var actions = e.detailRow.find(".actions");
                        var content = e.detailRow.find(".content");
                        var rowData = e.data;
                        var detailService = new chainGridDetailSumarryService(internal, content, actions, rowData);
                    });

                    grid.dataSource.bind("change", function () {
                        console.log("datasource change");
                        if (internal.loaded) {
                            internal.loadDelay = 0;
                        }
                        internal.loaded = false;
                    });
                    grid.bind("dataBound", $.proxy(internal.generateGridChartActions, internal));

                    var tabStrip = this.getTabStrip();
                    tabStrip.bind("activate", function (e) {
                        if (internal.isSelectedTab()) {
                            //$.proxy(internal.runJob, internal);
                            //console.log("run: " + internal.getSelctedTab());
                            internal.generateGridChartActions();
                        }
                    });
                };

                chainDailySummaryService.prototype.fetch = function () {
                };
                return chainDailySummaryService;
            })();
            Reporting.chainDailySummaryService = chainDailySummaryService;
        })(Services.Reporting || (Services.Reporting = {}));
        var Reporting = Services.Reporting;
    })(MyAndromeda.Services || (MyAndromeda.Services = {}));
    var Services = MyAndromeda.Services;
})(MyAndromeda || (MyAndromeda = {}));
