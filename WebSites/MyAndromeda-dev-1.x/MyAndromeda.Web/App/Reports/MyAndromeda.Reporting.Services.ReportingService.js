///// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            "use strict";
            var ReportingServiceWatcher = (function () {
                function ReportingServiceWatcher(options) {
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
                    }
                    else {
                        this.results = [];
                    }
                    this.viewModel = kendo.observable({
                        resultCount: 0,
                        max: new Date()
                    });
                    this.viewModel.bind("change", this.viewModelChagned);
                }
                ReportingServiceWatcher.prototype.viewModelChagned = function (e) {
                    console.log("change on field :" + e.field);
                };
                ReportingServiceWatcher.prototype.Init = function () {
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
                ReportingServiceWatcher.prototype.AddResult = function (key, result) {
                    //already the type we want
                    if (result instanceof Reporting.Models.Result) {
                        this.AddResultToViewModel(key, result);
                    }
                    else {
                        result = new Reporting.Models.Result(key, result);
                        this.AddResultToViewModel(key, result);
                    }
                    return result;
                };
                ReportingServiceWatcher.prototype.TickAction = function () {
                    var value = new Date();
                    this.viewModel.set("today", value);
                };
                ReportingServiceWatcher.prototype.Tick = function () {
                    var internal = this;
                    this.liveUpdate = setInterval(function () {
                        internal.TickAction();
                    }, 1000);
                };
                ReportingServiceWatcher.prototype.StopTick = function () {
                    clearInterval(this.liveUpdate);
                };
                ReportingServiceWatcher.prototype.SetupChangeQueryOptions = function () {
                    var internal = this;
                    $(this.element).on("click", ".k-button-change-date", function () {
                        alert("change");
                    });
                };
                ReportingServiceWatcher.prototype.Get = function (key) {
                    return this.viewModel.get(key);
                };
                ReportingServiceWatcher.prototype.Set = function (key, value) {
                    this.viewModel.set(key, value);
                };
                ReportingServiceWatcher.prototype.Bind = function (eventName, handler) {
                    this.viewModel.bind(eventName, handler);
                };
                ReportingServiceWatcher.prototype.AddResultToViewModel = function (key, result) {
                    var vm = this.viewModel;
                    this.results.push(result);
                    vm.set(key, result);
                    vm.set("resultCount", this.results.length);
                };
                return ReportingServiceWatcher;
            })();
            Services.ReportingServiceWatcher = ReportingServiceWatcher;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
