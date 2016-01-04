/// <reference path="../typings/kendo/kendo.all.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
        (function (Services) {
            var ReportingService = (function () {
                function ReportingService(options) {
                    var internal = this;
                    this.options = options;

                    if (!options.id) {
                        throw new Error("Requires 'id' for the view model to bind to the element");
                    }
                    if (options.id) {
                        this.element = $(options.id);
                    }
                    if (options.results) {
                        this.results = options.results;
                    } else {
                        this.results = [];
                    }
                    this.viewModel = kendo.observable({
                        resultCount: 0,
                        max: new Date()
                    });

                    this.viewModel.bind("change", this.viewModelChagned);
                }
                ReportingService.prototype.viewModelChagned = function (e) {
                    console.log("change on field :" + e.field);
                };

                ReportingService.prototype.Init = function () {
                    var internal = this, element = $(this.element);

                    if (this.results && this.results.length > 0) {
                        this.results.forEach(function (item) {
                            internal.AddResultToViewModel(item.key, item);
                        });
                    }

                    if (!element || element.length == 0) {
                        throw new Error("the element is not found:" + this.options.id);
                    }

                    //kendo.bind(element[0], this.viewModel);
                    element.data("ReportingService", this);

                    this.SetupChangeQueryOptions();
                };

                ReportingService.prototype.AddResult = function (key, result) {
                    //already the type we want
                    if (result instanceof Reporting.Models.Result) {
                        this.AddResultToViewModel(key, result);
                    } else {
                        result = new Reporting.Models.Result(key, result);
                        this.AddResultToViewModel(key, result);
                    }
                    return result;
                };

                ReportingService.prototype.TickAction = function () {
                    var value = new Date();
                    this.viewModel.set("today", value);
                };

                ReportingService.prototype.Tick = function () {
                    var internal = this;
                    this.liveUpdate = setInterval(function () {
                        internal.TickAction();
                    }, 1000);
                };

                ReportingService.prototype.StopTick = function () {
                    clearInterval(this.liveUpdate);
                };

                ReportingService.prototype.SetupChangeQueryOptions = function () {
                    var internal = this;
                    $(this.element).on("click", ".k-button-change-date", function () {
                        alert("change");
                    });
                };

                ReportingService.prototype.Get = function (key) {
                    return this.viewModel.get(key);
                };
                ReportingService.prototype.Set = function (key, value) {
                    this.viewModel.set(key, value);
                };

                ReportingService.prototype.Bind = function (eventName, handler) {
                    this.viewModel.bind(eventName, handler);
                };

                ReportingService.prototype.AddResultToViewModel = function (key, result) {
                    var vm = this.viewModel;
                    this.results.push(result);

                    vm.set(key, result);
                    vm.set("resultCount", this.results.length);
                };
                return ReportingService;
            })();
            Services.ReportingService = ReportingService;
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
