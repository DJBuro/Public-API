/// <reference path="../general/resizemodule.ts" />
module MyAndromeda.WebHooks {

    module Models {
        export interface IWebHookType {
            Key: string;
            Name: string;
        }

        export interface IWebHookSettings {
            [key: string]: ISubscription[],
        }

        export interface ISubscription {
            Name?: string;
            CallBackUrl?: string;
            RequestHeaders?: { [key: string]: string };
            Enabled: boolean;
        }
    }

    var moduleName = "MyAndromeda.WebHooks";

    var m = angular.module(moduleName, [
        "MyAndromeda.Resize",
        "MyAndromeda.Progress",
        "ngRoute",
        "ngAnimate",
        "kendo.directives"
    ]);

    m.run(($templateCache: ng.ITemplateCacheService) => {
        Logger.Notify("WebHooks Started");

        angular.element('script[type="text/template"]').each((i, element: HTMLElement) => {
            $templateCache.put(element.id, element.innerHTML);
        });
    });

    m.config(($routeProvider: ng.route.IRouteProvider) => {

        $routeProvider.when('/', {
            templateUrl: "start.html",
            controller: "StartControler"
        });

        
    });

    m.controller("StartControler", (
        $scope,
        $timeout: ng.ITimeoutService,
        resizeService: Services.ResizeService,
        progressService: Services.ProgressService,
        webHookService: WebHookService,
        webHookTypes: Models.IWebHookType[]) => {
        Logger.Notify("start");

        var resizeSubscription = resizeService.ResizeObservable.subscribe((e) => {
            //do i have anything to resize
        });

        var element = document.getElementById("WebHooks");
        progressService.Create(element).Show();

        var globalSettings: Models.IWebHookSettings = null;
        var settingsPromise = webHookService.Read();
        var settingsObservable = Rx.Observable
            .fromPromise(settingsPromise);

        var refresh = (settings: Models.IWebHookSettings) => {
            globalSettings = settings;

            Logger.Notify("settings:");
            Logger.Notify(globalSettings);

            $scope.webHookTypes = webHookTypes;

            var t = Rx.Observable.fromArray(webHookTypes);
            
            t.subscribe((setting) => {
                Logger.Notify(setting);
                //prepare the settings
                
                if (!globalSettings[setting.Key]) {
                    globalSettings[setting.Key] = []
                }
            },
            (ex) => { },
            () => {
                $timeout(() => {
                    Logger.Notify("new settings");
                    Logger.Notify(globalSettings);
                    $scope.settings = globalSettings;
                    progressService.Hide();
                });
            });

        };

        $scope.getGroupNameFromKey = (key: string) => {
            var find = webHookTypes.filter(e=> e.Key === key);
            if (find.length === 0) { return key + " not found" }
            return find[0].Name;
        };


        $scope.add = (key: string) => {
            Logger.Notify("add to: " + key);
            var group: Models.ISubscription[] = $scope.settings[key];

            var model: Models.ISubscription = {
                Name: "Default",
                CallBackUrl: "",
                RequestHeaders: {},
                Enabled: true
            };

            group.push(model);
        };
        $scope.update = () => {
            progressService.Show();

            var promise = webHookService.Update(globalSettings);
            Rx.Observable.fromPromise(promise).subscribe(() => { }, (ex) => {
                progressService.Hide();
            }, () => {
                progressService.Hide();
            });
        };

        $scope.remove = (key: string, subscription: Models.ISubscription) => {
            var group = globalSettings[key];
            globalSettings[key] = group.filter(e=> e !== subscription);
        };

        settingsObservable
            .subscribe((response) => {
                Logger.Notify(response.data);
                //settings = response.data;
                refresh(response.data);
            }, (ex) => {
                Logger.Error(ex);
            }, () => {

            });

    });


    export class WebHookService {
        public static readRoute: string = "";
        public static updateRoute: string = "";
        public static acsApplicationId: string = "";
        public $http: ng.IHttpService;

        public constructor($http: ng.IHttpService) {
            this.$http = $http;
            Logger.Notify("Where am i");
        }

        public Read(): ng.IHttpPromise<Models.IWebHookSettings> {
            var route = WebHookService.readRoute + WebHookService.acsApplicationId;
            var promise = this.$http.get(route);

            return promise;
        }

        public Update(data): ng.IHttpPromise<Models.IWebHookSettings> {
            var route = WebHookService.updateRoute + WebHookService.acsApplicationId;
            var promise = this.$http.post(route, data);

            return promise;
        }
    }

    m.service("webHookService", WebHookService);
    //"Store Online Status", "EDT", "Menu Version", "Order Status"

    var storeStatus: Models.IWebHookType
        = { Key: "StoreOnlineStatus", Name: "Store Online Status" };
    var estimatedDeliveryTime: Models.IWebHookType
        = { Key: "Edt", Name: "EDT" };
    var menuVersion: Models.IWebHookType
        = { Key: "MenuVersion", Name: "Menu Version" };
    var menuItems: Models.IWebHookType
        = { Key: "MenuItems", Name: "Menu Items" };
    var orderStatus: Models.IWebHookType
        = { Key: "OrderStatus", Name: "Order Status" }
    var bringg: Models.IWebHookType
        = { Key: "BringUpdates", Name: "Bringg" };
    var bringgEta: Models.IWebHookType
        = { Key: "BringEtaUpdates", Name: "Bringg ETA" };

    var webHookTypes: Models.IWebHookType[] = [
        storeStatus,
        estimatedDeliveryTime,
        menuVersion,
        menuItems,
        orderStatus,
        bringg,
        bringgEta
    ];

    m.constant("webHookTypes", webHookTypes);

    export function Setup(id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, [moduleName]);
    }
}