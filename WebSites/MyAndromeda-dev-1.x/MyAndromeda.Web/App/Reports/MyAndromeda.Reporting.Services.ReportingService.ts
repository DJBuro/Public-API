///// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
module MyAndromeda.Reporting.Services {
    "use strict";

    export class ReportingServiceWatcher {
        private element: any;
        private options: any;
        private results: Models.Result[];
        private viewModel: kendo.Observable;
        private liveUpdate: any;
        private log: boolean;

        constructor(options: any) {
            var internal = this;
            this.options = options;
                
            if (!options.id) { throw new Error("Requires 'id' for the view model to bind to the element"); }
            if (options.id) { this.element = $(options.id); }
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

        private viewModelChagned(e: any): void {
            console.log("change on field :" + e.field);
        }

        public Init(): void {
            var internal = this,
                element = $(this.element);

            if (this.results && this.results.length > 0) {
                this.results.forEach(function (item: Models.Result) {
                    internal.AddResultToViewModel(item.key, item);
                });
            }

            if (!element || element.length == 0) {
                throw new Error("the element is not found:" + this.options.id);
            }

            //kendo.bind(element[0], this.viewModel);
            element.data("ReportingService", this);
                
            this.SetupChangeQueryOptions();
        }

        public AddResult(key: string, result: Models.Result)
        public AddResult(key: string, result: any): Models.Result {
            //already the type we want
            if (result instanceof Models.Result) {
                this.AddResultToViewModel(key, result);
            } else {
                result = new Models.Result(key, result);
                this.AddResultToViewModel(key, result);
            }
            return result;
        }

        private TickAction(): void {
            var value = new Date();
            (<any>this.viewModel).set("today", value);
        }

        public Tick(): void {
            var internal = this;
            this.liveUpdate = setInterval(function () {
                internal.TickAction();
            }, 1000);        
        }

        public StopTick(): void {
            clearInterval(this.liveUpdate);
        }

        public SetupChangeQueryOptions(): void{
            var internal = this;
            $(this.element).on("click", ".k-button-change-date", function () {
                alert("change");
            });
        }

        public Get(key: string): any {
            return (<any>this.viewModel).get(key);
        }
        public Set(key: string, value: any): void {
            (<any>this.viewModel).set(key, value);
        }

        public Bind(eventName: string, handler: Function) {
            this.viewModel.bind(eventName, handler);
        }

        private AddResultToViewModel(key: string, result: Models.Result) {
            var vm = <any>this.viewModel;
            this.results.push(result);

            vm.set(key, result);
            vm.set("resultCount", this.results.length);
        }
	}
} 