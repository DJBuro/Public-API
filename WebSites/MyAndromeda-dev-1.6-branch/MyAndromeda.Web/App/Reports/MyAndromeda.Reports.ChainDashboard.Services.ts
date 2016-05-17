module MyAndromeda.Reports.ChainDashboard {

    var app = angular.module("ChainDashboard.Services", []);

    export class DashboardQueryContext {
        public Query: Models.IQuery;

        public Changed: Rx.BehaviorSubject<boolean>;

        constructor() {
            this.Changed = new Rx.BehaviorSubject<boolean>(false);

            var today = new Date();
            var thirtyDaysAgo = new Date();

            this.Query = {
                FromObj: new Date(thirtyDaysAgo.setDate(-30)),
                ToObj: today
            };

            this.Changed.onNext(true);
        }
    }

    export class GroupedDataWarehouseStoreResultsService {
        private $http: ng.IHttpService;

        public ChainData: Models.IDataWarehouseChain = null;
        public ChainDataObservable: Rx.BehaviorSubject<Models.IDataWarehouseChain> = new Rx.BehaviorSubject<Models.IDataWarehouseChain>(null);

        constructor($http) {
            this.$http = $http;
        }

        public LoadChain(chainId: number, query: Models.IQuery)
        {
            var route = kendo.format("/chain-data-warehouse/{0}", chainId);

            var data = query !== null ? this.CreateQueryObj(query) : {};
            var promise = this.$http.post<Models.IDataWarehouseChain>(route, data);

            promise.then((result) => {
                this.ChainData = result.data;
                this.ChainDataObservable.onNext(this.ChainData);
            });

            return this.ChainDataObservable.asObservable();
        }

        private CreateQueryObj(query: Models.IQuery)
        {
            var f = kendo.toString(query.FromObj, "u");
            var t = kendo.toString(query.ToObj, "u");

            Logger.Notify(f);
            Logger.Notify(t);

            return {
                From: f,
                To: t
            };
        }

        public CreaateTotals(name: string, allOrders: Rx.IObservable<Models.IDataWarehouseOrder>) {

            var totals: Models.IChainTodaysSalesSummaryControllerModel = {
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
            allOrders.subscribe(order => {
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
        }

    }

    export class GroupedStoreResultsService {
        private groupedStoreResultsDataService: GroupedStoreResultsDataService;
        private dashboardQueryContext: DashboardQueryContext;

        public ChainDataObservable: Rx.BehaviorSubject<Models.IChainResult>;
        public StoreObservable: Rx.BehaviorSubject<Models.IGroupedStoreResults>;

        public ChainData: Models.IChainResult = null;
        public StoreData: Models.IGroupedStoreResults = null;

        constructor(groupedStoreResultsDataService: GroupedStoreResultsDataService, dashboardQueryContext: DashboardQueryContext) {
            this.dashboardQueryContext = dashboardQueryContext;
            this.groupedStoreResultsDataService = groupedStoreResultsDataService;

            this.ChainDataObservable = new Rx.BehaviorSubject<Models.IChainResult>(null);
            this.StoreObservable = new Rx.BehaviorSubject<Models.IGroupedStoreResults>(null);

            var throttled = dashboardQueryContext.Changed.where(e=> e).throttle(700);

            throttled.select(() => {
                var promise = this.groupedStoreResultsDataService.GetDailyAllStoreData(settings.chainId, this.dashboardQueryContext.Query);

                return promise;
            }).subscribe((e) => {
                //load stuff; 
                e.then((dataResult) => {
                    this.ChainData = dataResult.data;
                    this.ChainDataObservable.onNext(dataResult.data);
                });
            });

            this.ChainDataObservable.subscribe((data) => {
                
            });

            throttled.where(() => this.StoreData !== null).select(() => {
                var promise = this.groupedStoreResultsDataService.GetDailySingleStoreData(settings.chainId, this.StoreData.StoreId, this.dashboardQueryContext.Query);

                return promise;
            }).subscribe((e) => {
                e.then(dataResult => {
                    this.StoreData = dataResult.data;
                    this.StoreObservable.onNext(this.StoreData);
                });
            });
        }

        public LoadStore(andromedaSiteId: number) {
            if (this.ChainData == null) {

                var promise = this.groupedStoreResultsDataService
                    .GetDailySingleStoreData(settings.chainId, andromedaSiteId, this.dashboardQueryContext.Query);

                promise.then((dataResult) => {
                    this.StoreData = dataResult.data;
                    this.StoreObservable.onNext(this.StoreData);
                });
            }
            else {
                var store = this.ChainData.Data.filter((e) => e.StoreId == andromedaSiteId);

                this.StoreData = store[0];
                this.StoreObservable.onNext(this.StoreData);
            }
        }
    }

    export class GroupedStoreResultsDataService {
        private $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        private CreateQueryObj(query: Models.IQuery) {
            var f = kendo.toString(query.FromObj, "u");
            var t = kendo.toString(query.ToObj, "u");

            Logger.Notify(f);
            Logger.Notify(t);

            return {
                From: f,
                To: t
            };
        }

        public GetDailyAllStoreData(chainId: number, query: Models.IQuery): ng.IHttpPromise<Models.IChainResult> {

            var route = kendo.format("/chain-data/{0}", chainId);

            var queryBody = this.CreateQueryObj(query);
            var promise = this.$http.post(route, queryBody);

            return promise;
        }

        public GetDailySingleStoreData(chainId: number, andromedaSiteId: number, query: Models.IQuery): ng.IHttpPromise<Models.IGroupedStoreResults> {

            var route = kendo.format("/chain-data/{0}/store/{1}", chainId, andromedaSiteId);

            var queryBody = this.CreateQueryObj(query);
            var promise = this.$http.post(route, queryBody);

            return promise;
        }

    }

    export class ChartOptions {

        public DataWareHouseOccasionChart(): kendo.dataviz.ui.ChartOptions {
            var options: kendo.dataviz.ui.ChartOptions = {
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
                    //{
                    //    name: "Sales", categoryField: "OrderType", field: "Sum", type: "donut",
                    //    labels: {
                    //        template: "#= category # #= kendo.toString(dataItem.Sum, 'c') #"
                    //    }
                    //}
                ]
            }

            return options;
        }

        public DataWareHouseAcsApplicationChart(): kendo.dataviz.ui.ChartOptions {
            var options: kendo.dataviz.ui.ChartOptions = {
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
                        },

                    },
                    //{ title: "Orders", name: "Orders" }
                ],
                series: [
                    { name: "Sales", field: "FinalPrice", type: "column", axis: "Sales", aggregate: "sum", stack: { group: "Sales" } },
                    //{ name: "Orders", field: "OrderType", type: "column", axis: "Orders", aggregate: "count", stack: { group: "Orders" } },
                    //{
                    //    name: "Collection", field: "OrderType", type: "column", stack: true, aggregate: (values, series, dataItems, category) => {
                    //        var items: Models.IDataWarehouseOrder[] = dataItems;
                    //        var total = 0;
                    //        if (!items) { return 0; }
                    //        Rx.Observable.from(items)
                    //            .count((e: Models.IDataWarehouseOrder) => e.OrderType.toLowerCase() == "collection")
                    //            .subscribe((count) => {
                    //                total = count
                    //            });
                    //        return total;
                    //    }
                    //}
                ]
            }
            

            return options;
        }

        

        public WeekChart(): kendo.dataviz.ui.ChartOptions {

            var options: kendo.dataviz.ui.ChartOptions = {
                theme: "bootstrap",
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
                    //{ name: "Order Types", field: 
                ]
            };

            return options;
        }

        public DayChart(baseUnit: string): kendo.dataviz.ui.ChartOptions {

            var options: kendo.dataviz.ui.ChartOptions = {
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
                        weekStartDay: 1,
                        
                        //startAngle: 90,
                        
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
                    //{ name: "Order Types", field: 
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
        }
    }

    export class ValueFormater {
        public Currency(value: number): string {
            var x = value / 100;
            return kendo.toString(x, "c");
        }

        public CurrencyDecimal(value: number): string {
            return kendo.toString(value, "c");
        }

        public DateFormat(value: Date): string {
            return kendo.toString(value, "g");
        }
    }

    app.service("dashboardQueryContext", DashboardQueryContext);
    app.service("groupedStoreResultsDataService", GroupedStoreResultsDataService);
    app.service("groupedStoreResultsService", GroupedStoreResultsService);
    app.service("valueFormater", ValueFormater);
    app.service("chartOptions", ChartOptions);

    app.service("groupedDataWarehouseStoreResultsService", GroupedDataWarehouseStoreResultsService);
} 