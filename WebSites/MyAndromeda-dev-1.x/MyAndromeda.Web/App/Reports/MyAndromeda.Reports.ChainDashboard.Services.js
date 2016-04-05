var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.Services", []);
            var DashboardQueryContext = (function () {
                function DashboardQueryContext() {
                    this.Changed = new Rx.BehaviorSubject(false);
                    var today = new Date();
                    var thirtyDaysAgo = new Date();
                    this.Query = {
                        FromObj: new Date(thirtyDaysAgo.setDate(-30)),
                        ToObj: today
                    };
                    this.Changed.onNext(true);
                }
                return DashboardQueryContext;
            }());
            ChainDashboard.DashboardQueryContext = DashboardQueryContext;
            var GroupedDataWarehouseStoreResultsService = (function () {
                function GroupedDataWarehouseStoreResultsService($http) {
                    this.ChainData = null;
                    this.ChainDataObservable = new Rx.BehaviorSubject(null);
                    this.$http = $http;
                }
                GroupedDataWarehouseStoreResultsService.prototype.LoadChain = function (chainId, query) {
                    var _this = this;
                    var route = kendo.format("/chain-data-warehouse/{0}", chainId);
                    var data = query !== null ? this.CreateQueryObj(query) : {};
                    var promise = this.$http.post(route, data);
                    promise.then(function (result) {
                        _this.ChainData = result.data;
                        _this.ChainDataObservable.onNext(_this.ChainData);
                    });
                    return this.ChainDataObservable.asObservable();
                };
                GroupedDataWarehouseStoreResultsService.prototype.CreateQueryObj = function (query) {
                    var f = kendo.toString(query.FromObj, "u");
                    var t = kendo.toString(query.ToObj, "u");
                    MyAndromeda.Logger.Notify(f);
                    MyAndromeda.Logger.Notify(t);
                    return {
                        From: f,
                        To: t
                    };
                };
                GroupedDataWarehouseStoreResultsService.prototype.CreaateTotals = function (name, allOrders) {
                    var totals = {
                        Cancelled: 0,
                        CancelledValue: 0,
                        Completed: 0,
                        CompletedValue: 0,
                        OutForDelivery: 0,
                        OutForDeliveryValue: 0,
                        Oven: 0,
                        OvenValue: 0,
                        ReadyToDispatch: 0,
                        ReadyToDispatchValue: 0,
                        Received: 0,
                        ReceivedValue: 0,
                        Total: 0,
                        TotalValue: 0,
                        FutureOrder: 0,
                        FutureOrderValue: 0,
                        /* pie chart values */
                        Collection: 0,
                        CollectionValue: 0,
                        Delivery: 0,
                        DeliveryValue: 0,
                        InStore: 0,
                        InStoreValue: 0,
                        /* */
                        Name: "",
                        Data: new kendo.data.DataSource(),
                        AcsApplicationData: new kendo.data.DataSource({
                            group: [
                                {
                                    field: "ApplicationId"
                                }
                            ]
                        }),
                        OccasionData: new kendo.data.DataSource()
                    };
                    var k = [];
                    allOrders.subscribe(function (order) {
                        k.push(order);
                        totals.Total++;
                        totals.TotalValue += order.FinalPrice;
                        totals.Name = name;
                        switch (order.OrderType.toLocaleLowerCase()) {
                            case "collection": {
                                totals.Collection++;
                                totals.CollectionValue += order.FinalPrice;
                                break;
                            }
                            case "delivery": {
                                totals.Delivery++;
                                totals.DeliveryValue += order.FinalPrice;
                                break;
                            }
                            default: {
                                totals.InStore++;
                                totals.InStoreValue += order.FinalPrice;
                            }
                        }
                        switch (order.Status) {
                            //Order has been received by the store
                            case 1: {
                                totals.Received++;
                                totals.ReceivedValue += order.FinalPrice;
                                break;
                            }
                            //Order is in oven
                            case 2: {
                                totals.Oven++;
                                totals.OvenValue += order.FinalPrice;
                                break;
                            }
                            //Order is ready for dispatch
                            case 3: {
                                totals.ReadyToDispatch++;
                                totals.ReadyToDispatchValue += order.FinalPrice;
                                break;
                            }
                            //Order is out for delivery
                            case 4: {
                                totals.OutForDelivery++;
                                totals.OutForDeliveryValue += order.FinalPrice;
                                break;
                            }
                            //Order has been completed
                            case 5: {
                                totals.Completed++;
                                totals.CompletedValue += order.FinalPrice;
                                break;
                            }
                            //Order has been canceled
                            case 6: {
                                totals.Cancelled++;
                                totals.CancelledValue += order.FinalPrice;
                                break;
                            }
                            //Future Order
                            case 8: {
                                totals.FutureOrder++;
                                totals.FutureOrderValue += order.FinalPrice;
                                break;
                            }
                        }
                    });
                    totals.AcsApplicationData.data(k);
                    totals.Data.data(k);
                    totals.OccasionData.data([
                        { OrderType: "Collection", Count: totals.Collection, Sum: totals.CollectionValue },
                        { OrderType: "Delivery", Count: totals.Delivery, Sum: totals.DeliveryValue },
                        { OrderType: "Dine in", Count: totals.InStore, Sum: totals.InStoreValue }
                    ]);
                    return totals;
                };
                return GroupedDataWarehouseStoreResultsService;
            }());
            ChainDashboard.GroupedDataWarehouseStoreResultsService = GroupedDataWarehouseStoreResultsService;
            var GroupedStoreResultsService = (function () {
                function GroupedStoreResultsService(groupedStoreResultsDataService, dashboardQueryContext) {
                    var _this = this;
                    this.ChainData = null;
                    this.StoreData = null;
                    this.dashboardQueryContext = dashboardQueryContext;
                    this.groupedStoreResultsDataService = groupedStoreResultsDataService;
                    this.ChainDataObservable = new Rx.BehaviorSubject(null);
                    this.StoreObservable = new Rx.BehaviorSubject(null);
                    var throttled = dashboardQueryContext.Changed.where(function (e) { return e; }).throttle(700);
                    throttled.select(function () {
                        var promise = _this.groupedStoreResultsDataService.GetDailyAllStoreData(ChainDashboard.settings.chainId, _this.dashboardQueryContext.Query);
                        return promise;
                    }).subscribe(function (e) {
                        //load stuff; 
                        e.then(function (dataResult) {
                            _this.ChainData = dataResult.data;
                            _this.ChainDataObservable.onNext(dataResult.data);
                        });
                    });
                    this.ChainDataObservable.subscribe(function (data) {
                    });
                    throttled.where(function () { return _this.StoreData !== null; }).select(function () {
                        var promise = _this.groupedStoreResultsDataService.GetDailySingleStoreData(ChainDashboard.settings.chainId, _this.StoreData.StoreId, _this.dashboardQueryContext.Query);
                        return promise;
                    }).subscribe(function (e) {
                        e.then(function (dataResult) {
                            _this.StoreData = dataResult.data;
                            _this.StoreObservable.onNext(_this.StoreData);
                        });
                    });
                }
                GroupedStoreResultsService.prototype.LoadStore = function (andromedaSiteId) {
                    var _this = this;
                    if (this.ChainData == null) {
                        var promise = this.groupedStoreResultsDataService
                            .GetDailySingleStoreData(ChainDashboard.settings.chainId, andromedaSiteId, this.dashboardQueryContext.Query);
                        promise.then(function (dataResult) {
                            _this.StoreData = dataResult.data;
                            _this.StoreObservable.onNext(_this.StoreData);
                        });
                    }
                    else {
                        var store = this.ChainData.Data.filter(function (e) { return e.StoreId == andromedaSiteId; });
                        this.StoreData = store[0];
                        this.StoreObservable.onNext(this.StoreData);
                    }
                };
                return GroupedStoreResultsService;
            }());
            ChainDashboard.GroupedStoreResultsService = GroupedStoreResultsService;
            var GroupedStoreResultsDataService = (function () {
                function GroupedStoreResultsDataService($http) {
                    this.$http = $http;
                }
                GroupedStoreResultsDataService.prototype.CreateQueryObj = function (query) {
                    var f = kendo.toString(query.FromObj, "u");
                    var t = kendo.toString(query.ToObj, "u");
                    MyAndromeda.Logger.Notify(f);
                    MyAndromeda.Logger.Notify(t);
                    return {
                        From: f,
                        To: t
                    };
                };
                GroupedStoreResultsDataService.prototype.GetDailyAllStoreData = function (chainId, query) {
                    var route = kendo.format("/chain-data/{0}", chainId);
                    var queryBody = this.CreateQueryObj(query);
                    var promise = this.$http.post(route, queryBody);
                    return promise;
                };
                GroupedStoreResultsDataService.prototype.GetDailySingleStoreData = function (chainId, andromedaSiteId, query) {
                    var route = kendo.format("/chain-data/{0}/store/{1}", chainId, andromedaSiteId);
                    var queryBody = this.CreateQueryObj(query);
                    var promise = this.$http.post(route, queryBody);
                    return promise;
                };
                return GroupedStoreResultsDataService;
            }());
            ChainDashboard.GroupedStoreResultsDataService = GroupedStoreResultsDataService;
            var ChartOptions = (function () {
                function ChartOptions() {
                }
                ChartOptions.prototype.DataWareHouseOccasionChart = function () {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        seriesDefaults: {
                            labels: {
                                template: "#= category # - #= kendo.format('{0:P}', percentage)#",
                                //position: "outsideEnd",
                                visible: true,
                                background: "transparent"
                            },
                            type: "pie"
                        },
                        series: [
                            {
                                name: "Count", categoryField: "OrderType", field: "Count",
                                labels: {
                                    template: "#=category# - #= dataItem.Count# order(s)\n#= kendo.toString(dataItem.Sum, 'c') #"
                                }
                            }
                        ]
                    };
                    return options;
                };
                ChartOptions.prototype.DataWareHouseAcsApplicationChart = function () {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        categoryAxis: [
                            {
                                field: "WantedTime",
                                type: "date",
                                baseUnit: "months"
                            }
                        ],
                        valueAxis: [
                            {
                                title: "Sales",
                                name: "Sales",
                                labels: {
                                    template: "#= kendo.toString(value, 'c') #"
                                }
                            },
                        ],
                        series: [
                            { name: "Sales", field: "FinalPrice", type: "column", axis: "Sales", aggregate: "sum", stack: { group: "Sales" } },
                        ]
                    };
                    return options;
                };
                ChartOptions.prototype.WeekChart = function () {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        categoryAxis: [
                            {
                                field: "WeekOfYear",
                                baseUnitStep: "auto"
                            }
                        ],
                        valueAxis: [
                            {
                                title: "Sales",
                                name: "Sales",
                                labels: {
                                    template: "#= kendo.toString(value / 100, 'c') #"
                                }
                            },
                            { title: "Orders", name: "Orders" }
                        ],
                        //seriesDefaults: { type: "radarLine" },
                        series: [
                            { name: "Sales", field: "Total.NetSales", type: "area", axis: "Sales" },
                            { name: "Order Count", field: "Total.OrderCount", type: "column", axis: "Orders" },
                            { name: "Delivery", field: "Delivery.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Collection", field: "Collection.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Carry out", field: "CarryOut.NetSales", type: "area", axis: "Sales", aggregate: "sum" }
                        ]
                    };
                    return options;
                };
                ChartOptions.prototype.DayChart = function (baseUnit) {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        categoryAxis: [
                            {
                                field: "CreateTimeStamp",
                                //baseUnitStep: "auto",
                                type: "date",
                                baseUnit: baseUnit,
                                baseUnitStep: 1,
                                autoBaseUnitSteps: {
                                    days: [1],
                                    weeks: [],
                                    months: []
                                },
                                weekStartDay: 1
                            }
                        ],
                        valueAxis: [
                            {
                                title: "Sales", name: "Sales",
                                labels: {
                                    template: "#= kendo.toString(value / 100, 'c') #"
                                }
                            },
                            { title: "Orders", name: "Orders" }
                        ],
                        //seriesDefaults: { type: "radarLine" },
                        series: [
                            { name: "Sales", field: "Total.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Order Count", field: "Total.OrderCount", type: "column", axis: "Orders", aggregate: "sum" },
                            { name: "Delivery", field: "Delivery.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Collection", field: "Collection.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Carry out", field: "CarryOut.NetSales", type: "area", axis: "Sales", aggregate: "sum" }
                        ]
                    };
                    if (baseUnit == "days") {
                        var category = options.categoryAxis[0];
                        category.labels = {
                            step: 3,
                            format: "d/M"
                        };
                    }
                    if (baseUnit == "months") {
                        var category = options.categoryAxis[0];
                        category.labels = {
                            dateFormats: {
                                days: "d/M"
                            }
                        };
                    }
                    return options;
                };
                return ChartOptions;
            }());
            ChainDashboard.ChartOptions = ChartOptions;
            var ValueFormater = (function () {
                function ValueFormater() {
                }
                ValueFormater.prototype.Currency = function (value) {
                    var x = value / 100;
                    return kendo.toString(x, "c");
                };
                ValueFormater.prototype.CurrencyDecimal = function (value) {
                    return kendo.toString(value, "c");
                };
                ValueFormater.prototype.DateFormat = function (value) {
                    return kendo.toString(value, "g");
                };
                return ValueFormater;
            }());
            ChainDashboard.ValueFormater = ValueFormater;
            app.service("dashboardQueryContext", DashboardQueryContext);
            app.service("groupedStoreResultsDataService", GroupedStoreResultsDataService);
            app.service("groupedStoreResultsService", GroupedStoreResultsService);
            app.service("valueFormater", ValueFormater);
            app.service("chartOptions", ChartOptions);
            app.service("groupedDataWarehouseStoreResultsService", GroupedDataWarehouseStoreResultsService);
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
