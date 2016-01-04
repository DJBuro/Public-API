/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(WebOrderingWebApiService.Name,
            ($http, ContextService) => {
                var instnance = new WebOrderingWebApiService($http, ContextService);
                return instnance;
            }
            );
    });

    export class WebOrderingWebApiService {
        public static Name: string = "WebOrderingWebApiService";

        public IsLoading: Rx.BehaviorSubject<boolean>;
        public IsSaving: Rx.BehaviorSubject<boolean>;
        public IsPublishLiveBusy: Rx.BehaviorSubject<boolean>;
        public IsPublishPreviewBusy: Rx.BehaviorSubject<boolean>;

        public Context: Services.ContextService;
        private watcher: Rx.Subject<Models.IWebOrderingSettings>;


        public constructor($http: ng.IHttpService, context: Services.ContextService) {
            this.Context = context;

            this.IsPublishLiveBusy = new Rx.BehaviorSubject<boolean>(false);
            this.IsPublishPreviewBusy = new Rx.BehaviorSubject<boolean>(false);
            this.IsLoading = new Rx.BehaviorSubject<boolean>(false);
            this.IsSaving = new Rx.BehaviorSubject<boolean>(false);

            if (!Settings.AndromedaSiteId) { throw "Settings.AndromedaSiteId is undefined"; }

            var readWebsiteSettings = kendo.format(Settings.ReadRoute, Settings.AndromedaSiteId, Settings.WebSiteId);
            var readStores = kendo.format(Settings.ReadStoreRoute, Settings.AndromedaSiteId, Settings.WebSiteId);

            this.IsLoading.onNext(true);

            this.watcher = new Rx.Subject<Models.IWebOrderingSettings>();
            this.watcher.distinctUntilChanged((x) => x.WebSiteId).subscribe((settings) => {
                this.Context.ModelSubject.onNext(settings);
            });

            this.Context.ModelSubject
                .where((e) => e !== null)
                .subscribe((v) => { console.log(v.WebSiteName); });

            var promise = $http.get<Models.IWebOrderingSettings>(readWebsiteSettings);
            promise.then((result) => {
                //set defaults. 
                var nullOrUndefined = (path) => {
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
                if (nullOrUndefined(result.data.GeneralSettings.EnableDelivery))
                {
                    result.data.GeneralSettings.EnableDelivery = true;
                }
                if (nullOrUndefined(result.data.GeneralSettings.EnableCollection)) {
                    result.data.GeneralSettings.EnableCollection = true;
                }

                return result;
            }).then((result) => {
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
            }).then((result) => {
                //may get rid of this bit some point soon. 
                var observable = <any>kendo.observable(result.data);

                this.Context.Model = observable;
                this.Context.ModelSubject.onNext(observable);

                return result;
            }).then((result) => {
                var storePromise = $http.get<Models.IStore[]>(readStores);

                storePromise.success((stores) => {
                    context.StoreSubject.onNext(stores);
                });

                return result;
            });

            promise.finally(() => {
                this.IsLoading.onNext(false);
            });
        }

        public UpdateThemeSettings(settings: Models.IWebOrderingTheme): void {
            var s: any = settings;
            var m: any = this.Context.Model;

            m.set("ThemeSettings", settings);
            this.Update();
        }

        public Publish(): void {
            var publish = kendo.format(Settings.PublishRoute, Settings.AndromedaSiteId, Settings.WebSiteId);

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

            promise.always(() => {
                this.IsPublishLiveBusy.onNext(false);
            });

        }

        public Preview(): void {
            var preview = kendo.format(Settings.PreviewRoute, Settings.AndromedaSiteId, Settings.WebSiteId);

            var raw = this.GetRawModel();
            var promise = $.ajax({
                url: preview,
                type: "POST",
                contentType: 'application/json',
                dataType: "json",
                data: raw
            });

            this.IsPublishPreviewBusy.onNext(true);

            promise.always(() => {
                this.IsPublishPreviewBusy.onNext(false);
            });
        }

        public Update(): void {
            var update = kendo.format(Settings.UpdateRoute, Settings.AndromedaSiteId, Settings.WebSiteId);
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

            promise.always(() => {
                this.IsSaving.onNext(true);
            });

        }

        private GetRawModel(): string {
            var newObject: Models.IWebOrderingSettings = JSON.parse(JSON.stringify(this.Context.Model))
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
        }



    }
}