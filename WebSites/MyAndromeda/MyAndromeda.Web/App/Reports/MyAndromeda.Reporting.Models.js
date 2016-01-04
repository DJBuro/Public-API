var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
        (function (Models) {
            var Result = (function () {
                function Result(key, data) {
                    this.key = key;
                    if (data instanceof kendo.data.DataSource) {
                        this.data = data;
                    } else {
                        this.CreateDataSource(data);
                    }
                }
                Result.prototype.CreateDataSource = function (data) {
                    var model = { data: [] };
                    if (!data) {
                        throw new Error("Data is missing");
                    }
                    if (data.length) {
                        model.data = data;
                    } else if (data.data) {
                        //suggests that data is a data source scaffold instead.
                        model = data;
                    }

                    this.data = new kendo.data.DataSource(model);
                };

                Result.prototype.Bind = function (eventName, handler) {
                    this.data.bind(eventName, handler);
                };
                return Result;
            })();
            Models.Result = Result;
        })(Reporting.Models || (Reporting.Models = {}));
        var Models = Reporting.Models;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
