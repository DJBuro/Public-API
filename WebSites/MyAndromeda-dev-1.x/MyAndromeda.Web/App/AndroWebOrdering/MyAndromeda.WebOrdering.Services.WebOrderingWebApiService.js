/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Services;
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.service(WebOrderingWebApiService.Name, WebOrderingWebApiService);
            });
            var WebOrderingWebApiService = (function () {
                function WebOrderingWebApiService($http, contextService) {
                    var _this = this;
                    this.Context = contextService;
                    this.IsPublishLiveBusy = new Rx.BehaviorSubject(false);
                    this.IsPublishPreviewBusy = new Rx.BehaviorSubject(false);
                    this.IsLoading = new Rx.BehaviorSubject(false);
                    this.IsSaving = new Rx.BehaviorSubject(false);
                    if (!WebOrdering.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }
                    var readWebsiteSettings = kendo.format(WebOrdering.Settings.ReadRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    var readStores = kendo.format(WebOrdering.Settings.ReadStoreRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    this.IsLoading.onNext(true);
                    this.watcher = new Rx.Subject();
                    this.watcher.distinctUntilChanged(function (x) { return x.WebSiteId; }).subscribe(function (settings) {
                        _this.Context.ModelSubject.onNext(settings);
                    });
                    this.Context.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (v) { console.log(v.WebSiteName); });
                    var promise = $http.get(readWebsiteSettings);
                    promise.then(function (result) {
                        //set defaults. 
                        var nullOrUndefined = function (path) {
                            return typeof (path) === "undefined" || path === null;
                            return true;
                        };
                        if (!result.data.MenuPageSettings) {
                            result.data.MenuPageSettings = {
                                IsSingleToppingsOnlyEnabled: false,
                                IsQuantityDropdownEnabled: true,
                                IsThumbnailsEnabled: true
                            };
                        }
                        if (nullOrUndefined(result.data.GeneralSettings.EnableDelivery)) {
                            result.data.GeneralSettings.EnableDelivery = true;
                        }
                        if (nullOrUndefined(result.data.GeneralSettings.EnableCollection)) {
                            result.data.GeneralSettings.EnableCollection = true;
                        }
                        return result;
                    }).then(function (result) {
                        //transfer pence to decimal so that the editors can work easier. 
                        if (result.data.GeneralSettings.MinimumDeliveryAmount) {
                            result.data.GeneralSettings.MinimumDeliveryAmount /= 100;
                        }
                        if (result.data.GeneralSettings.DeliveryCharge) {
                            result.data.GeneralSettings.DeliveryCharge /= 100;
                        }
                        if (result.data.GeneralSettings.OptionalFreeDeliveryThreshold) {
                            result.data.GeneralSettings.OptionalFreeDeliveryThreshold /= 100;
                        }
                        if (result.data.GeneralSettings.CardCharge) {
                            result.data.GeneralSettings.CardCharge /= 100;
                        }
                        return result;
                    }).then(function (result) {
                        //may get rid of this bit some point soon. 
                        var observable = kendo.observable(result.data);
                        _this.Context.Model = observable;
                        _this.Context.ModelSubject.onNext(observable);
                        return result;
                    }).then(function (result) {
                        var storePromise = $http.get(readStores);
                        storePromise.success(function (stores) {
                            contextService.StoreSubject.onNext(stores);
                        });
                        return result;
                    });
                    promise.finally(function () {
                        _this.IsLoading.onNext(false);
                    });
                }
                WebOrderingWebApiService.prototype.UpdateThemeSettings = function (settings) {
                    var s = settings;
                    var m = this.Context.Model;
                    m.set("ThemeSettings", settings);
                    this.Update();
                };
                WebOrderingWebApiService.prototype.Publish = function () {
                    var _this = this;
                    var publish = kendo.format(WebOrdering.Settings.PublishRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    if (!this.Context.Model.CustomerAccountSettings.get("IsEnable")) {
                        if (!confirm("Are you sure you want to continue with customer account settings disabled? Continue to accept this responsibility, or cancel to apply that setting.")) {
                            this.Context.Model.CustomerAccountSettings.set("IsEnable", true);
                        }
                    }
                    var raw = this.GetRawModel();
                    var promise = $.ajax({
                        url: publish,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });
                    this.IsPublishLiveBusy.onNext(true);
                    promise.always(function () {
                        _this.IsPublishLiveBusy.onNext(false);
                    });
                };
                WebOrderingWebApiService.prototype.Preview = function () {
                    var _this = this;
                    var preview = kendo.format(WebOrdering.Settings.PreviewRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    var raw = this.GetRawModel();
                    var promise = $.ajax({
                        url: preview,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });
                    this.IsPublishPreviewBusy.onNext(true);
                    promise.always(function () {
                        _this.IsPublishPreviewBusy.onNext(false);
                    });
                };
                WebOrderingWebApiService.prototype.Update = function () {
                    var _this = this;
                    var update = kendo.format(WebOrdering.Settings.UpdateRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    console.log("sync");
                    var raw = this.GetRawModel();
                    var promise = $.ajax({
                        url: update,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });
                    this.IsSaving.onNext(true);
                    promise.always(function () {
                        _this.IsSaving.onNext(true);
                    });
                };
                WebOrderingWebApiService.prototype.GetRawModel = function () {
                    var newObject = JSON.parse(JSON.stringify(this.Context.Model));
                    //jQuery.extend(true, {}, this.Context.Model);
                    if (newObject.GeneralSettings.MinimumDeliveryAmount) {
                        newObject.GeneralSettings.MinimumDeliveryAmount *= 100;
                    }
                    if (newObject.GeneralSettings.DeliveryCharge) {
                        newObject.GeneralSettings.DeliveryCharge *= 100;
                    }
                    if (newObject.GeneralSettings.OptionalFreeDeliveryThreshold) {
                        newObject.GeneralSettings.OptionalFreeDeliveryThreshold *= 100;
                    }
                    if (newObject.GeneralSettings.CardCharge) {
                        newObject.GeneralSettings.CardCharge *= 100;
                    }
                    var raw = JSON.stringify(newObject);
                    return raw;
                };
                WebOrderingWebApiService.Name = "webOrderingWebApiService";
                return WebOrderingWebApiService;
            })();
            Services.WebOrderingWebApiService = WebOrderingWebApiService;
        })(Services = WebOrdering.Services || (WebOrdering.Services = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
