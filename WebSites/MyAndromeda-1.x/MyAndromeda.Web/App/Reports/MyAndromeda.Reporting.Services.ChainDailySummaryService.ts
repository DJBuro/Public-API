 module MyAndromeda.Reporting.Services 
 {
    export class chainDailySummaryService {
        public options: Models.IChainDailySummaryOptions; 
        private dashboardSalesService: Services.dailySummaryDataService;
        private graphs: Function[];
        private loaded: boolean;
        private loadDelay = 250;

        constructor(options: Models.IChainDailySummaryOptions ) {
            this.options = options;
            var internal = this;
            this.graphs = [];
        }

        private generateGridChartActions(): void {
            var internal = this,
                grid = $(this.options.gridElement),
                dataSource = this.getDataSource(),
                selectedTabItem = internal.getSelctedTab();

            if (!this.isSelectedTab() || this.loaded)
                return;

            internal.graphs.length = 0;
            $(internal.options.gridElement).find(".chain-sparkline").each(function (index, element) {
                var e = $(element);
                var uid = e.closest("tr").data("uid");
                var dataRow = <any>dataSource.getByUid(uid);
                var dataSet = <IStoreReportSet> dataRow;
                var data = dataRow.Data;

                if (data.length == 0 || data.length == 1) { return; }

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
                    }
                }
                if (showType === "orders") {
                    action = function () {
                        internal.generateGridCharts(e, "Total Orders", "Combined.OrderCount", "column", descriptiveData, "#: value #");
                    }
                }
                if (showType === "avgSpend") {
                    action = function () {
                        internal.generateGridCharts(e, "Avg Spend", "Combined.AvgSale", "area", descriptiveData, "#: kendo.toString(value/100, 'c') #");
                    }
                }
                if (showType === "otd") {
                    action = function () {
                        internal.generateGridCharts(e, "OTD", "Performance.AvgOutTheDoor", "line", descriptiveData, "#: value #");
                    }
                }
                if (showType === "ttd") {
                    action = function () {
                        internal.generateGridCharts(e, "TTD", "Performance.AvgDoorTime", "line", descriptiveData, "#: value #");
                    }
                }

                internal.graphs.push(action);
                        
            });

            if (internal.graphs.length > 0) {
                if (internal.graphs.length > 25) {
                    internal.graphs.forEach(function (act) {
                        setTimeout(function () { act(); }, internal.loadDelay);
                    });
                }
                else {
                    internal.graphs.forEach(function (act) {
                        act()
                    });
                }
                       
            }

            this.loaded = true;
        }

        private generateGridCharts(element, name, field, type, data, format): void {
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
                    
        }

        public getGrid(): kendo.ui.Grid {
            return <kendo.ui.Grid>$(this.options.gridElement).data("kendoGrid");
        }
                
        public getDataSource(): kendo.data.DataSource {
            var grid = this.getGrid(), dataSource = grid.dataSource;
            return dataSource;
        }

        public getTabStrip(): kendo.ui.TabStrip {
            var tabStrip = <kendo.ui.TabStrip>$(this.options.tabStripElement).data("kendoTabStrip");
            return tabStrip;
        }

        public getSelctedTab(): string {
            var selectedElement = $(this.options.tabStripElement).find(".k-tabstrip-items").find(".k-state-active");
            var name = selectedElement.data("tabName");
            return name;
        }

        public isSelectedTab(): boolean {
            return this.getSelctedTab() === this.options.tabStripItemName;
        }

        public runJob(): void {
            this.graphs.forEach(function (value) {
                value();
            });
        }

        public init(): void {
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
                if (internal.loaded) { internal.loadDelay = 0; }
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
        }

        public fetch(): void {

        }
    }
 }