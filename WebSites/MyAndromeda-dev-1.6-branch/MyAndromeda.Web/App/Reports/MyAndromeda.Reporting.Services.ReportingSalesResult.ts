module MyAndromeda.Reporting.Services {
    class Filter {
        constructor(public from: Date, public to: Date, public dayRange: number) { }
    }

    export class ReportingSalesResult {
        private filter : Filter
        private dataSource: kendo.data.DataSource;
        public name: string;
        public routeData: IReportingSalesResultRoute;

        constructor(name: string, from: Date, to: Date, dayRange: number, routeData: IReportingSalesResultRoute) {
            this.name = name;
            this.filter = new Filter(from, to, dayRange);
            this.routeData = routeData;
        }

        private InitDataSource(): void {
            var internal = this;
            this.dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        var url = internal.routeData.urlFormat,
                            filter = internal.filter;
                        var destination = kendo.format(url, filter.from.toJSON(), filter.to.toJSON())
                    }
                },
                schema: {
                    model: {
                        fields: {
                            "Day": { "type": "date" },
                            "Total": { "type": "number" },
                            "Count": { "type": "number" },
                            "Average": { "type": "number" },
                            "Min": { "type": "number" },
                            "Max": { "type": "number" }
                        }
                    }
                }
            });
        }

        private ReactToDateChange(): void {
            var internal = this,
                service = <Services.ReportingServiceWatcher>$("#dashboardreport").data("ReportingService");

            service.Bind("change", function (e) {
                if (e.field !== "from") { return; }

                var dateChange = <Date>service.Get("from");
                var from = new Date();
                var to = dateChange;

                from.setDate(dateChange.getDate() - internal.filter.dayRange);

                internal.filter.from = from;
                internal.filter.to = to;
            });
        }

        private InitReactions(): void {
            this.ReactToDateChange();
        }

        public Init(): void {
            this.InitDataSource();
            this.InitReactions();
        }
    }
}


interface IReportingSalesResultRoute
{
    urlFormat: string; //ie. the output of "@Url.Action("ReadOrderSummary", "Orders")" + "?from={0}&to={1}&dayOnly={2}";
}

