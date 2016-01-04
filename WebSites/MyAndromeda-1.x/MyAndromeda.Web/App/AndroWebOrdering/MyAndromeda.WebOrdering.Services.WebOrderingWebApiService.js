var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="../../Scripts/typings/rx.js/rx.binding-lite.d.ts" />
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.factory(WebOrderingWebApiService.Name, [
                    Services.ContextService.Name,
                    function (contextService) {
                        var instnance = new WebOrderingWebApiService(contextService);
                        return instnance;
                    }
                ]);
            });

            var WebOrderingWebApiService = (function () {
                function WebOrderingWebApiService(context) {
                    var _this = this;
                    this.Context = context;

                    this.IsPreviewReady = new Rx.Subject();
                    this.IsWebOrderingBusy = new Rx.BehaviorSubject(false);
                    this.Search = new Rx.Subject();

                    if (!WebOrdering.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }

                    var read = kendo.format(WebOrdering.Settings.ReadRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);

                    this.IsWebOrderingBusy.onNext(true);
                    var promise = $.ajax({
                        url: read,
                        type: "GET",
                        dataType: "json"
                    });

                    this.watcher = new Rx.Subject();
                    this.watcher.distinctUntilChanged(function (x) {
                        return x.WebSiteId;
                    }).subscribe(function (settings) {
                        _this.Context.ModelSubject.onNext(settings);
                    });

                    this.Context.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (v) {
                        console.log(v.WebSiteName);
                    });

                    promise.done(function (result) {
                        var observable = kendo.observable(result[0]);
                        _this.Context.Model = observable;

                        //this.Context.Model.SocialNetworkSettings.bind("change", (field:kendo.ui.observ) => {
                        //});
                        //this.Context.Model.SocialNetworkSettings.TwitterSettings.set("IsFollow", this.Context.Model.SocialNetworkSettings.TwitterSettings.IsEnable);
                        //$scope.ShowFacebookSettings = websiteSettings.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                        //    $scope.ShowTwitterSettings = websiteSettings.SocialNetworkSettings.TwitterSettings.get("IsEnable");
                        //    $scope.ShowInstagramSettings = websiteSettings.SocialNetworkSettings.InstagramSettings.get("IsEnable");
                        _this.Context.ModelSubject.onNext(observable);

                        _this.IsWebOrderingBusy.onNext(false);
                    });

                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
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

                    var raw = JSON.stringify(this.Context.Model);
                    var promise = $.ajax({
                        url: publish,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsWebOrderingBusy.onNext(true);

                    promise.done(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                };

                WebOrderingWebApiService.prototype.Preview = function () {
                    var _this = this;
                    var preview = kendo.format(WebOrdering.Settings.PreviewRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);

                    var raw = JSON.stringify(this.Context.Model);
                    var promise = $.ajax({
                        url: preview,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsWebOrderingBusy.onNext(true);

                    promise.done(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                        _this.IsPreviewReady.onNext(true);
                    });
                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                };

                WebOrderingWebApiService.prototype.Update = function () {
                    var _this = this;
                    var update = kendo.format(WebOrdering.Settings.UpdateRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    console.log("sync");

                    var raw = JSON.stringify(this.Context.Model);
                    console.log(this.Context.Model);
                    var promise = $.ajax({
                        url: update,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsWebOrderingBusy.onNext(true);

                    promise.done(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                };
                WebOrderingWebApiService.Name = "WebOrderingWebApiService";
                return WebOrderingWebApiService;
            })();
            Services.WebOrderingWebApiService = WebOrderingWebApiService;
        })(WebOrdering.Services || (WebOrdering.Services = {}));
        var Services = WebOrdering.Services;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
