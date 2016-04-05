/// <reference path="../general/resizemodule.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebHooks;
    (function (WebHooks) {
        var moduleName = "MyAndromeda.WebHooks";
        var m = angular.module(moduleName, [
            "MyAndromeda.Resize",
            "MyAndromeda.Progress",
            "ngRoute",
            "ngAnimate",
            "kendo.directives"
        ]);
        m.run(function ($templateCache) {
            MyAndromeda.Logger.Notify("WebHooks Started");
            angular.element('script[type="text/template"]').each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        m.config(function ($routeProvider) {
            $routeProvider.when('/', {
                templateUrl: "start.html",
                controller: "StartControler"
            });
        });
        m.controller("StartControler", function ($scope, $timeout, resizeService, progressService, webHookService, webHookTypes) {
            MyAndromeda.Logger.Notify("start");
            var resizeSubscription = resizeService.ResizeObservable.subscribe(function (e) {
                //do i have anything to resize
            });
            var element = document.getElementById("WebHooks");
            progressService.Create(element).Show();
            var globalSettings = null;
            var settingsPromise = webHookService.Read();
            var settingsObservable = Rx.Observable
                .fromPromise(settingsPromise);
            var refresh = function (settings) {
                globalSettings = settings;
                MyAndromeda.Logger.Notify("settings:");
                MyAndromeda.Logger.Notify(globalSettings);
                $scope.webHookTypes = webHookTypes;
                var t = Rx.Observable.fromArray(webHookTypes);
                t.subscribe(function (setting) {
                    MyAndromeda.Logger.Notify(setting);
                    //prepare the settings
                    if (!globalSettings[setting.Key]) {
                        globalSettings[setting.Key] = [];
                    }
                }, function (ex) { }, function () {
                    $timeout(function () {
                        MyAndromeda.Logger.Notify("new settings");
                        MyAndromeda.Logger.Notify(globalSettings);
                        $scope.settings = globalSettings;
                        progressService.Hide();
                    });
                });
            };
            $scope.getGroupNameFromKey = function (key) {
                var find = webHookTypes.filter(function (e) { return e.Key === key; });
                if (find.length === 0) {
                    return key + " not found";
                }
                return find[0].Name;
            };
            $scope.add = function (key) {
                MyAndromeda.Logger.Notify("add to: " + key);
                var group = $scope.settings[key];
                var model = {
                    Name: "Default",
                    CallBackUrl: "",
                    RequestHeaders: {},
                    Enabled: true
                };
                group.push(model);
            };
            $scope.update = function () {
                progressService.Show();
                var promise = webHookService.Update(globalSettings);
                Rx.Observable.fromPromise(promise).subscribe(function () { }, function (ex) {
                    progressService.Hide();
                }, function () {
                    progressService.Hide();
                });
            };
            $scope.remove = function (key, subscription) {
                var group = globalSettings[key];
                globalSettings[key] = group.filter(function (e) { return e !== subscription; });
            };
            settingsObservable
                .subscribe(function (response) {
                MyAndromeda.Logger.Notify(response.data);
                //settings = response.data;
                refresh(response.data);
            }, function (ex) {
                MyAndromeda.Logger.Error(ex);
            }, function () {
            });
        });
        var WebHookService = (function () {
            function WebHookService($http) {
                this.$http = $http;
                MyAndromeda.Logger.Notify("Where am i");
            }
            WebHookService.prototype.Read = function () {
                var route = WebHookService.readRoute + WebHookService.acsApplicationId;
                var promise = this.$http.get(route);
                return promise;
            };
            WebHookService.prototype.Update = function (data) {
                var route = WebHookService.updateRoute + WebHookService.acsApplicationId;
                var promise = this.$http.post(route, data);
                return promise;
            };
            WebHookService.readRoute = "";
            WebHookService.updateRoute = "";
            WebHookService.acsApplicationId = "";
            return WebHookService;
        }());
        WebHooks.WebHookService = WebHookService;
        m.service("webHookService", WebHookService);
        //"Store Online Status", "EDT", "Menu Version", "Order Status"
        var storeStatus = { Key: "StoreOnlineStatus", Name: "Store Online Status" };
        var estimatedDeliveryTime = { Key: "Edt", Name: "EDT" };
        var menuVersion = { Key: "MenuVersion", Name: "Menu Version" };
        var menuItems = { Key: "MenuItems", Name: "Menu Items" };
        var orderStatus = { Key: "OrderStatus", Name: "Order Status" };
        var bringg = { Key: "BringUpdates", Name: "Bringg" };
        var bringgEta = { Key: "BringEtaUpdates", Name: "Bringg ETA" };
        var webHookTypes = [
            storeStatus,
            estimatedDeliveryTime,
            menuVersion,
            menuItems,
            orderStatus,
            bringg,
            bringgEta
        ];
        m.constant("webHookTypes", webHookTypes);
        function Setup(id) {
            var element = document.getElementById(id);
            angular.bootstrap(element, [moduleName]);
        }
        WebHooks.Setup = Setup;
    })(WebHooks = MyAndromeda.WebHooks || (MyAndromeda.WebHooks = {}));
})(MyAndromeda || (MyAndromeda = {}));
