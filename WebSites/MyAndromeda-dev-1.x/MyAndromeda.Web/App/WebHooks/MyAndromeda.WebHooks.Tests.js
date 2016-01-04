/// <reference path="../general/resizemodule.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebHooks;
    (function (WebHooks) {
        var Tests;
        (function (Tests) {
            var moduleName = "MyAndromeda.WebHook.Tests";
            var m = angular.module(moduleName, [
                "MyAndromeda.Resize",
                "MyAndromeda.Progress",
                "ngRoute",
                "ngAnimate",
                "kendo.directives"
            ]);
            m.run(function ($templateCache) {
                MyAndromeda.Logger.Notify("WebHook tests Started");
                angular.element('script[type="text/template"]').each(function (i, element) {
                    $templateCache.put(element.id, element.innerHTML);
                });
            });
            m.config(function ($routeProvider) {
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
            m.controller("DeliveryTimeController", function ($scope, $routeParams, webHookTestService, progressService) {
                var settings = {
                    Edt: null
                };
                $scope.settings = settings;
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.DeliveryTime({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        Source: "test",
                        Edt: settings.Edt
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            m.controller("MenuVersionController", function ($scope, $routeParams, webHookTestService, progressService) {
                var settings = {
                    Version: null
                };
                $scope.settings = settings;
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.MenuVersion({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        Source: "test",
                        Version: settings.Version
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            m.controller("StartController", function ($scope, $http, $timeout, resizeService, progressService) {
                MyAndromeda.Logger.Notify("start");
                var resizeSubscription = resizeService.ResizeObservable.subscribe(function (e) {
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
            m.controller("StoreStatusController", function ($scope, $routeParams, webHookTestService, progressService) {
                var element = document.getElementById("WebhookTest");
                progressService.Create(element);
                $scope.settings = {};
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.OnlineStateTest({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        Online: value,
                        Source: "test"
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            m.controller("OrderStatusController", function ($scope, $routeParams, progressService, webHookTestService) {
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
                            url: function () { return "/api/acs/list/" + $routeParams.andromedaSiteId; },
                            "type": "GET"
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
                $scope.settings = settings;
                //jQuery(function () { jQuery("#Order_id").kendoComboBox({ 
                //"dataSource": { "transport": { "read": { "url": "/api/GprsOrders", "data": filterOrders }, "prefix": "" }, "serverFiltering": true, "filter": [], "schema": { "errors": "Errors" } }, "dataTextField": "RamesesOrderNum", "dataValueField": "RamesesOrderNum", "cascadeFrom": "andromedaSiteId" }); });
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.OrderStatusTest({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        ExternalOrderId: $scope.settings.Order.ExternalOrderRef,
                        //ExternalAcsApplicationId: $scope.settings.AcsApplication.ExternalApplicationId,
                        AcsApplicationId: $scope.settings.AcsApplication ? $scope.settings.AcsApplication.Id : null,
                        Source: "test",
                        Status: settings.Status,
                        StatusDescription: settings.StatusDescription
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            var WebHookTestService = (function () {
                function WebHookTestService($http) {
                    this.orderStatusUrl = "/web-hooks/store/orders/update-order-status";
                    this.storeStatusUrl = "/web-hooks/store/update-status";
                    this.updateDeliveryTime = "/web-hooks/store/update-estimated-delivery-time";
                    this.updateMenuChange = "/web-hooks/store/update-menu";
                    this.$http = $http;
                    MyAndromeda.Logger.Notify("Where am i");
                }
                WebHookTestService.prototype.OnlineStateTest = function (model) {
                    var promise = this.$http.post(this.storeStatusUrl, model);
                    return promise;
                };
                WebHookTestService.prototype.OrderStatusTest = function (model) {
                    var promise = this.$http.post(this.orderStatusUrl, model);
                    return promise;
                };
                WebHookTestService.prototype.MenuVersion = function (model) {
                    var promise = this.$http.post(this.updateMenuChange, model);
                    return promise;
                };
                WebHookTestService.prototype.DeliveryTime = function (model) {
                    var promise = this.$http.post(this.updateDeliveryTime, model);
                    return promise;
                };
                return WebHookTestService;
            })();
            Tests.WebHookTestService = WebHookTestService;
            m.service("webHookTestService", WebHookTestService);
            //"Store Online Status", "EDT", "Menu Version", "Order Status"
            function Setup(id) {
                var element = document.getElementById(id);
                angular.bootstrap(element, [moduleName]);
            }
            Tests.Setup = Setup;
        })(Tests = WebHooks.Tests || (WebHooks.Tests = {}));
    })(WebHooks = MyAndromeda.WebHooks || (MyAndromeda.WebHooks = {}));
})(MyAndromeda || (MyAndromeda = {}));
