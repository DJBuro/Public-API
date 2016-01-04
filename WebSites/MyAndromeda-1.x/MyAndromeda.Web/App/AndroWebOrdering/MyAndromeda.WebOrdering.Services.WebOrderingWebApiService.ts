/// <reference path="../../Scripts/typings/rx.js/rx.binding-lite.d.ts" />
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Services
{
    Angular.ServicesInitilizations.push((app) => {
        app.factory(WebOrderingWebApiService.Name, [
            Services.ContextService.Name,
            (contextService) => {
                var instnance = new WebOrderingWebApiService(contextService);
                return instnance;
            }
        ]);
    });

    export class WebOrderingWebApiService {
        public static Name:string  = "WebOrderingWebApiService";

        public IsWebOrderingBusy : Rx.BehaviorSubject<boolean>;
        public IsPreviewReady: Rx.BehaviorSubject<boolean>;

        public Search : Rx.Subject<string>;

        //public DataSource: kendo.data.DataSource;
        public Context: Services.ContextService;
        private watcher: Rx.Subject<Models.IWebOrderingSettings>;


        public constructor(context: Services.ContextService) {
            this.Context = context;

            this.IsPreviewReady = new Rx.Subject<boolean>();
            this.IsWebOrderingBusy = new Rx.BehaviorSubject<boolean>(false);
            this.Search  = new Rx.Subject<string>();

            if(!Settings.AndromedaSiteId){ throw "Settings.AndromedaSiteId is undefined"; }
            
            var read = kendo.format(Settings.ReadRoute, Settings.AndromedaSiteId, Settings.WebSiteId);
           
            this.IsWebOrderingBusy.onNext(true);
            var promise = $.ajax({
                url : read,
                type: "GET",
                dataType: "json"
            });
            

            this.watcher = new Rx.Subject<Models.IWebOrderingSettings>();
            this.watcher.distinctUntilChanged((x) => x.WebSiteId).subscribe((settings)=> {
                this.Context.ModelSubject.onNext(settings);
            });

            this.Context.ModelSubject
                .where((e) => e !== null)
                .subscribe((v) => { console.log(v.WebSiteName); });

            promise.done((result) => {

                var observable = <any>kendo.observable(result[0]);
                this.Context.Model = observable;
                
                //this.Context.Model.SocialNetworkSettings.bind("change", (field:kendo.ui.observ) => {
                    
                //});
                //this.Context.Model.SocialNetworkSettings.TwitterSettings.set("IsFollow", this.Context.Model.SocialNetworkSettings.TwitterSettings.IsEnable);

                //$scope.ShowFacebookSettings = websiteSettings.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                //    $scope.ShowTwitterSettings = websiteSettings.SocialNetworkSettings.TwitterSettings.get("IsEnable");
                //    $scope.ShowInstagramSettings = websiteSettings.SocialNetworkSettings.InstagramSettings.get("IsEnable");

                this.Context.ModelSubject.onNext(observable);

                this.IsWebOrderingBusy.onNext(false);
            });

            promise.fail(() => {
                this.IsWebOrderingBusy.onNext(false);
            });

        }

        public UpdateThemeSettings(settings: Models.IWebOrderingTheme): void {
            var s :any = settings;
            var m: any = this.Context.Model;
 
            m.set("ThemeSettings", settings);
            this.Update();
        }

        public Publish(): void {
            var publish = kendo.format(Settings.PublishRoute, Settings.AndromedaSiteId, Settings.WebSiteId);

            var raw = JSON.stringify(this.Context.Model);
            var promise = $.ajax({
                url: publish,
                type: "POST",
                contentType: 'application/json',
                dataType: "json",              
                data: raw
            });

            this.IsWebOrderingBusy.onNext(true);

            promise.done(() => {
                this.IsWebOrderingBusy.onNext(false); 
            });
            promise.fail(() => {
                this.IsWebOrderingBusy.onNext(false);
            });
        }

        public Preview(): void {
            var preview = kendo.format(Settings.PreviewRoute, Settings.AndromedaSiteId, Settings.WebSiteId);

            var raw = JSON.stringify(this.Context.Model);
            var promise = $.ajax({
                url: preview,
                type: "POST",
                contentType: 'application/json',
                dataType: "json",              
                data: raw
            });

            this.IsWebOrderingBusy.onNext(true);

            promise.done(() => {
                this.IsWebOrderingBusy.onNext(false); 
                this.IsPreviewReady.onNext(true);
            });
            promise.fail(() => {
                this.IsWebOrderingBusy.onNext(false);
            });
        }

        public Update(): void {
            var update = kendo.format(Settings.UpdateRoute, Settings.AndromedaSiteId, Settings.WebSiteId);
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


            promise.done(() => {
                this.IsWebOrderingBusy.onNext(false);
            });
            promise.fail(() => {
                this.IsWebOrderingBusy.onNext(false);
            });
        }

        //private ChallengeExpectations(): void {
        //    var d = this.DataSource, 
        //        data= d.view();
               

        //    if(data.length === 0){ return; } 
        //    if(data.length > 1){ throw "There should not be this many settings!"; }

        //    var model = data[0];

        //    this.webOrderingSettings = model;
        //    this.watcher.onNext(model);
        //}
    }
 }