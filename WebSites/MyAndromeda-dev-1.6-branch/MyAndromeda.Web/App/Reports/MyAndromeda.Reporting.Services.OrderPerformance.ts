module MyAndromeda.Reporting.Services {
    export class dailySummaryDataService {
        public options: IDailySummaryOptions;
        public dataSource: kendo.data.DataSource;
        public result: IStoreReportSet;

        constructor(options: IDailySummaryOptions) {
            this.options = options;

            this.initDataSource();
        }

        private initDataSource() {
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
                    //{ field: "Over15lessThan20", aggregate: "sum" },
                    //{ field: ""}
                ]
            });
        }

        public bind(eventName: string, handler: Function): void {
            this.dataSource.bind(eventName, handler);
        }
    }
}

 module MyAndromeda.Reporting.Services {
    export class dailyPerformanceService {
        private dashboardSalesService: Services.dailySummaryDataService;
        public bound: boolean;
        public options: IDailySummaryOptions;
        public viewModel: kendo.data.ObservableObject;

        constructor(options: IDailySummaryOptions) {
            //console.log("new dailyPerformanceService");
                    
            this.options = options;
            this.dashboardSalesService = new Services.dailySummaryDataService(options);
            this.bound = false;
        }

        public bindViewModel(): void {
            //console.log("try bind");
            if (this.bound) { return; }
            var element = $(this.options.elementId);

            //console.log("bind");
            kendo.bind(element, this.viewModel);
            this.bound = true;
        }

        private setupResults(): void {
            var dataSource = this.dashboardSalesService.dataSource,
                result = this.dashboardSalesService.result;

            var orderCount = null,
                doorTime = null,
                outDoorTime = null,
                makeTime = null,
                rackTime = null,
                less30Mins = null;

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
                }
            } catch (e) {
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
        }

        public init(): void {
            var internal = this;
            var element = $(this.options.elementId);
            kendo.ui.progress(element, true);
            element.data("Reporting.DailySummaryService", this);

            internal.dashboardSalesService.dataSource.fetch($.proxy(this.setupResults, internal));
        }
    }
}

interface IDashboardSalesOrdersSparkLineServiceOptions
{
    combinedSalesId: string;
    combinedOrderCountId: string;
    combinedDeliveryAndCollectionId: string;
}

interface IDailySummaryServerAggregates {
    
}

interface IDailySummaryModel {
    viewModel: {
        sumSales: string;       //kendo.toString(values.AvgSales / 100, "c"))
        avgSales: string;       //kendo.toString(values.SumSales / 100, "c"))
        sumOrders: number;
        avgOrders: number;
        sumCancelled: number;
        avgCancelled: number;
        sumDelivery: number;
        sumCollection: number;
    }
}

interface IDailySummaryOptions {
    elementId: string;
    autoFetch: boolean;
    url: string;
    data: {};
}

interface IStoreReportSet
{
    AndromediaSiteId: number;
    ClientSiteName: string;
    ExternalSiteId: string;
    AvgSpend: number;
    TotalOrders: number;
    AvgOutTheDoor: number;
    AvgToTheDoor: number;
    AvgRackTime: number;
    AvgMakeTime: number;
    Data: IDailyMetricGroup[]; 

    Less30Mins: number;
}

interface IDailyMetricGroup
{
    AndromediaSiteId: number;
    Date: Date;
    InStore: IOrderMetricGroup;
    Collection: IOrderMetricGroup;
    Delivery: IOrderMetricGroup;
    Combined: IOrderMetricGroup;
    Performance: IPerformanceMetricGroup;
}

interface IOrderMetricGroup
{

}

interface IPerformanceMetricGroup {
    AvgMakeTime: number;
    AvgRackTime: number;
    AvgDoorTime: number;
    AvgOutTheDoor: number;
    NumOrdersLT30Mins: number;
    NumOrdersLT45Mins: number;
}

