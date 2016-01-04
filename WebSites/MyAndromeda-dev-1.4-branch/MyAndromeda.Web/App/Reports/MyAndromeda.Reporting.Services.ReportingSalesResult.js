var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var Filter = (function () {
                function Filter(from, to, dayRange) {
                    this.from = from;
                    this.to = to;
                    this.dayRange = dayRange;
                }
                return Filter;
            })();
            var ReportingSalesResult = (function () {
                function ReportingSalesResult(name, from, to, dayRange, routeData) {
                    this.name = name;
                    this.filter = new Filter(from, to, dayRange);
                    this.routeData = routeData;
                }
                ReportingSalesResult.prototype.InitDataSource = function () {
                    var internal = this;
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var url = internal.routeData.urlFormat, filter = internal.filter;
                                var destination = kendo.format(url, filter.from.toJSON(), filter.to.toJSON());
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
                };
                ReportingSalesResult.prototype.ReactToDateChange = function () {
                    var internal = this, service = $("#dashboardreport").data("ReportingService");
                    service.Bind("change", function (e) {
                        if (e.field !== "from") {
                            return;
                        }
                        var dateChange = service.Get("from");
                        var from = new Date();
                        var to = dateChange;
                        from.setDate(dateChange.getDate() - internal.filter.dayRange);
                        internal.filter.from = from;
                        internal.filter.to = to;
                    });
                };
                ReportingSalesResult.prototype.InitReactions = function () {
                    this.ReactToDateChange();
                };
                ReportingSalesResult.prototype.Init = function () {
                    this.InitDataSource();
                    this.InitReactions();
                };
                return ReportingSalesResult;
            })();
            Services.ReportingSalesResult = ReportingSalesResult;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
