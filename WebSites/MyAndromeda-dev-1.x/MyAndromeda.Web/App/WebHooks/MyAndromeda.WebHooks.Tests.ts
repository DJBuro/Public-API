/// <reference path="../general/resizemodule.ts" />
module MyAndromeda.WebHooks.Tests {

    module Models {
        export interface IStoreOnlineStatus {
            AndromedaSiteId: number;
            Online: boolean;
            Source: string;
            //ReportedBy: string;
        }
        export interface IOrderStatusChange {
            AndromedaSiteId: number;
            ExternalOrderId: string;
            //RamesesOrderNum: number;
            Status: string;
            StatusDescription: string;
            Source: string;
        }

        export interface IUpdateDeliveryTime {
            AndromedaSiteId: number;
            Edt: number;
            Source: string;
        }

        export interface IMenuChange {
            AndromedaSiteId: number;
            Source: string;
            Version: string;
        }
    }

    var moduleName = "MyAndromeda.WebHook.Tests";

    var m = angular.module(moduleName, [
        "MyAndromeda.Resize",
        "MyAndromeda.Progress",
        "ngRoute",
        "ngAnimate",
        "kendo.directives"
    ]);

    m.run(($templateCache: ng.ITemplateCacheService) => {
        Logger.Notify("WebHook tests Started");

        angular.element('script[type="text/template"]').each((i, element: HTMLElement) => {
            $templateCache.put(element.id, element.innerHTML);
        });
    });

    m.config(($routeProvider: ng.route.IRouteProvider) => {

        $routeProvider.when('/', {
            templateUrl: "start.html",
            controller: "StartController"
        });

        $routeProvider.when('/store-status/:andromedaSiteId', {
            templateUrl: "store-status.html",
            controller: "StoreStatusController"
        });

        $routeProvider.when('/order-status/:andromedaSiteId', {
            templateUrl: "order-status.html",
            controller: "OrderStatusController"
        });

        $routeProvider.when('/menu-version/:andromedaSiteId', {
            templateUrl: "menu-version.html",
            controller: "MenuVersionController"
        });

        $routeProvider.when('/delivery-time/:andromedaSiteId', {
            templateUrl: "delivery-time.html",
            controller: "DeliveryTimeController"
        });
    });

    m.controller("DeliveryTimeController", ($scope, $routeParams,
        webHookTestService: WebHookTestService,
        progressService: Services.ProgressService) => {

        var settings = {
            Edt: null
        };
        $scope.settings = settings;

        $scope.send = (value: boolean) => {
            progressService.Show();

            var request = webHookTestService.DeliveryTime({
                AndromedaSiteId: $routeParams.andromedaSiteId,
                Source: "test",
                Edt: settings.Edt
            });

            Rx.Observable.fromPromise(request).subscribe(() => {
                $scope.settings.messages = "Sent! No problems.";
                progressService.Hide();
            }, (ex) => {
                $scope.settings.messages = "Error";
                progressService.Hide();
            }, () => {
                progressService.Hide();
            });
        }
    });
    m.controller("MenuVersionController", ($scope, $routeParams,
        webHookTestService: WebHookTestService,
        progressService: Services.ProgressService) => {

        var settings = {
            Version: null
        };
        $scope.settings = settings;

        $scope.send = (value: boolean) => {
            progressService.Show();

            var request = webHookTestService.MenuVersion({
                AndromedaSiteId: $routeParams.andromedaSiteId,
                Source: "test",
                Version: settings.Version
            });

            Rx.Observable.fromPromise(request).subscribe(() => {
                $scope.settings.messages = "Sent! No problems.";
                progressService.Hide();
            }, (ex) => {
                $scope.settings.messages = "Error";
                progressService.Hide();
            }, () => {
                progressService.Hide();
            });
        }
    });

    m.controller("StartController", (
        $scope,
        $http,
        $timeout: ng.ITimeoutService,
        resizeService: Services.ResizeService,
        progressService: Services.ProgressService) => {
        Logger.Notify("start");

        var resizeSubscription = resizeService.ResizeObservable.subscribe((e) => {
            //do i have anything to resize
        });

        var dataSource = new kendo.data.DataSource({
            "transport": { "read": { "url": "/api/Store" }, "prefix": "" },
            "schema": { "errors": "Errors" }
        });

        $scope.storeDataSource = dataSource;
        $scope.settings = {
            AndromedaSiteId: null
        };
    });

    m.controller("StoreStatusController", ($scope, $routeParams,
        webHookTestService: WebHookTestService,
        progressService: Services.ProgressService) => {

        var element = document.getElementById("WebhookTest");
        progressService.Create(element);

        $scope.settings = {};
        $scope.send = (value: boolean) => {
            progressService.Show();

            var request = webHookTestService.OnlineStateTest({
                AndromedaSiteId: $routeParams.andromedaSiteId,
                Online: value,
                Source: "test"
            });

            Rx.Observable.fromPromise(request).subscribe(() => {
                $scope.settings.messages = "Sent! No problems.";
                progressService.Hide();
            }, (ex) => {
                $scope.settings.messages = "Error";
                progressService.Hide();
            }, () => {
                progressService.Hide();
            });
        }

    });
    m.controller("OrderStatusController", ($scope, $routeParams,
        progressService: Services.ProgressService, webHookTestService: WebHookTestService) => {
        var orderDataSource = new kendo.data.DataSource({
            "transport": {
                "read": {
                    "url": "/api/GprsOrders",
                    "data": {
                        andromedaSiteId: $routeParams.andromedaSiteId
                    }
                },
                "prefix": ""
            }
        });
        var acsDataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: function () { return "/api/acs/list/" + $routeParams.andromedaSiteId },
                    "type" : "GET"
                }
                
            }
        });

        var element = document.getElementById("WebhookTest");
        progressService.Create(element);

        var settings = {
            Status: null,
            StatusDescription: null
        };

        $scope.orderDataSource = orderDataSource;
        $scope.acsDataSource = acsDataSource;

        $scope.settings = settings
        //jQuery(function () { jQuery("#Order_id").kendoComboBox({ 
        //"dataSource": { "transport": { "read": { "url": "/api/GprsOrders", "data": filterOrders }, "prefix": "" }, "serverFiltering": true, "filter": [], "schema": { "errors": "Errors" } }, "dataTextField": "RamesesOrderNum", "dataValueField": "RamesesOrderNum", "cascadeFrom": "andromedaSiteId" }); });
        $scope.send = (value: boolean) => {
            progressService.Show();

            var request = webHookTestService.OrderStatusTest({
                AndromedaSiteId: $routeParams.andromedaSiteId,
                ExternalOrderId: $scope.settings.Order.ExternalOrderRef,
                //ExternalAcsApplicationId: $scope.settings.AcsApplication.ExternalApplicationId,
                AcsApplicationId: $scope.settings.AcsApplication ? $scope.settings.AcsApplication.Id: null,
                Source: "test",
                Status: settings.Status,
                StatusDescription: settings.StatusDescription
            });

            Rx.Observable.fromPromise(request).subscribe(() => {
                $scope.settings.messages = "Sent! No problems.";
                progressService.Hide();
            }, (ex) => {
                $scope.settings.messages = "Error";
                progressService.Hide();
            }, () => {
                progressService.Hide();
            });
        }

    });

    export class WebHookTestService {
        public $http: ng.IHttpService;

        public orderStatusUrl: string = "/web-hooks/store/orders/update-order-status";
        public storeStatusUrl: string = "/web-hooks/store/update-status";
        public updateDeliveryTime: string = "/web-hooks/store/update-estimated-delivery-time";
        public updateMenuChange: string = "/web-hooks/store/update-menu";

        public constructor($http: ng.IHttpService) {
            this.$http = $http;
            Logger.Notify("Where am i");
        }

        public OnlineStateTest(model: Models.IStoreOnlineStatus): ng.IHttpPromise<any> {
            var promise = this.$http.post(this.storeStatusUrl, model);

            return promise;
        }

        public OrderStatusTest(model: Models.IOrderStatusChange): ng.IHttpPromise<any> {
            var promise = this.$http.post(this.orderStatusUrl, model);

            return promise;
        }

        public MenuVersion(model: Models.IMenuChange): ng.IHttpPromise<any> {
            var promise = this.$http.post(this.updateMenuChange, model);

            return promise;
        }

        public DeliveryTime(model: Models.IUpdateDeliveryTime): ng.IHttpPromise<any>
        {
            var promise = this.$http.post(this.updateDeliveryTime, model);

            return promise;
        }
    }

    m.service("webHookTestService", WebHookTestService);
    //"Store Online Status", "EDT", "Menu Version", "Order Status"

    export function Setup(id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, [moduleName]);
    }
}