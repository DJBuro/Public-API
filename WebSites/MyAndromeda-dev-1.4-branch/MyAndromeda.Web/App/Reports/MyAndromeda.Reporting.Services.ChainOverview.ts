module MyAndromeda.Reporting.Services {

    export class chainGridDetailSumarryService {
        public actionsViewModel: kendo.data.ObservableObject;
        public contentViewModel: kendo.data.ObservableObject;
        public data: IStoreReportSet;

        public parent: chainDailySummaryService;
        public contentElement: JQuery;
        public actionsElement: JQuery;

        public url: string; 
        constructor(parent: chainDailySummaryService, contentElement: JQuery, actionsElement: JQuery, data: IStoreReportSet) {
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

        private setupActionsViewModel(): void {
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
        }
        private setupContentViewModel(): void {
            var grid = this.parent.getGrid(),
                dataSource = this.parent.getDataSource();

            if (this.data.Data && this.data.Data.length > 1) {
                this.setupContentViewModelFromExisting();
            } else {
                this.LoadHourly()
            }

            kendo.bind(this.contentElement, this.contentViewModel);
        }

        private setupContentViewModelFromExisting(): void {
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
        }

        private LoadHourly(): void {
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
        }

        private onActionsChange(e: any): void {
            //console.log("ive changed");
            var chart = this.getGridDetailChart(this.contentElement);
            var series = chart.options.series;
            var valueAxis = chart.options.valueAxis;

            series.forEach(function (seriesSet) {
                var a = <any>seriesSet;
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
        }

        private getGridDetailChart(content: JQuery): kendo.dataviz.ui.Chart {
            var contentChart = $(content).find(".k-chart-detail").data("kendoChart");
            return <kendo.dataviz.ui.Chart>contentChart;
        }
    }

}



