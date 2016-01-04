var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="../../Scripts/typings/rx.js/rx.time-lite.d.ts" />
        /// <reference path="../../Scripts/typings/rx.js/rx.binding-lite.d.ts" />
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.factory(WebOrderingThemeWebApiService.Name, [
                    function () {
                        WebOrdering.Logger.Notify("new WebOrderingThemeWebApiService");
                        var instnance = new WebOrderingThemeWebApiService();
                        return instnance;
                    }
                ]);
            });

            var WebOrderingThemeWebApiService = (function () {
                function WebOrderingThemeWebApiService() {
                    var _this = this;
                    this.IsBusy = new Rx.BehaviorSubject(false);
                    this.Search = new Rx.Subject();

                    if (!WebOrdering.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }

                    //throttle input for 1 second. ie search will resume after the user stops typing.
                    this.Search.throttle(1000).subscribe(function (value) {
                        return _this.SearcInternal(value);
                    });
                }
                WebOrderingThemeWebApiService.prototype.GetThemeDataSource = function () {
                    var _this = this;
                    var route = kendo.format('/api/AndroWebOrderingTheme/{0}/List', WebOrdering.Settings.AndromedaSiteId);
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: route
                        }
                    });

                    this.dataSource.bind("requestStart", function () {
                        _this.IsBusy.onNext(true);
                    });
                    this.dataSource.bind("requestEnd", function () {
                        _this.IsBusy.onNext(false);
                    });

                    return this.dataSource;
                };

                WebOrderingThemeWebApiService.prototype.SearchText = function (value) {
                    this.Search.onNext(value);
                };

                WebOrderingThemeWebApiService.prototype.SearcInternal = function (value) {
                    value || (value = "");
                    value = value.trim();

                    if (value.length === 0) {
                        this.dataSource.filter([]);
                        return;
                    }

                    var op = "contains";
                    var filterFileName = {
                        field: "FileName",
                        operator: op,
                        value: value
                    };
                    var filterInterName = {
                        field: "InternalName",
                        operator: op,
                        value: value
                    };

                    var filterGroup = {
                        filters: [filterFileName, filterInterName],
                        logic: "or"
                    };

                    this.dataSource.filter([
                        filterGroup
                    ]);
                };
                WebOrderingThemeWebApiService.Name = "WebOrderingThemeWebApiService";
                return WebOrderingThemeWebApiService;
            })();
            Services.WebOrderingThemeWebApiService = WebOrderingThemeWebApiService;
        })(WebOrdering.Services || (WebOrdering.Services = {}));
        var Services = WebOrdering.Services;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
