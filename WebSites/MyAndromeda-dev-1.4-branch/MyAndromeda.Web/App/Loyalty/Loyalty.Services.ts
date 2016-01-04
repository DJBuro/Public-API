module MyAndromeda.Loyalty {
    export var ServicesName = "LoyaltyServices";

    var servicesModule = angular.module(ServicesName, [
        
    ]);

    export module Services 
    {
        export class LoyaltyService 
        {
            private $http: ng.IHttpService

            public AllLoyaltyTypeList: Rx.ISubject<string[]>;
            public StoreLoyalties: Rx.ISubject<Models.IProviderLoyalty[]>;
            public LoyaltyProvider : Rx.ISubject<Models.IProviderLoyalty>;

            public ListLoyaltyTypesBusy: Rx.BehaviorSubject<boolean>;
            public ListBusy: Rx.BehaviorSubject<boolean>;
            public GetBusy: Rx.BehaviorSubject<boolean>;
            public UpdateBusy: Rx.BehaviorSubject<boolean>;


            public constructor($http: ng.IHttpService){
                this.$http = $http;   
               
                /* data messaging */
                // all types that have not been used yet. 
                this.AllLoyaltyTypeList = new Rx.Subject<string[]>();
                // store loyalty types that have been used 
                this.StoreLoyalties = new Rx.Subject<Models.IProviderLoyalty[]>();
                this.LoyaltyProvider = new Rx.Subject<Models.IProviderLoyalty>();

                /* monitor network */
                this.ListLoyaltyTypesBusy = new Rx.BehaviorSubject<boolean>(false);
                this.ListBusy = new Rx.BehaviorSubject<boolean>(false);
                this.GetBusy = new Rx.BehaviorSubject<boolean>(false);
                this.UpdateBusy = new Rx.BehaviorSubject<boolean>(false);

            }

            public ListLoyaltyTypes() : void {
                var partialRoute = "/api/{0}/loyalty/types";
                var route = kendo.format(partialRoute, Settings.AndromedaSiteId);
                var promise = this.$http.get(route);

                this.ListLoyaltyTypesBusy.onNext(true);
                promise.success((data: string[]) => {
                    this.AllLoyaltyTypeList.onNext(data);
                });
                promise.finally(() => {
                    this.ListLoyaltyTypesBusy.onNext(false);
                }); 
            }

            public List(): void  {
                var partialRoute = "/api/{0}/loyalty/list";
                var route = kendo.format(partialRoute, Settings.AndromedaSiteId);
                var promise = this.$http.get(route);

                this.ListBusy.onNext(true);
                promise.success((data: Models.IProviderLoyalty[]) => {
                    this.StoreLoyalties.onNext(data);
                });
                promise.finally(() => {
                    this.ListBusy.onNext(false);
                }); 
            }

            public Get(name: string) : void {
                var partialRoute = "/api/{0}/loyalty/get/{1}"; 
                var route = kendo.format(partialRoute, Settings.AndromedaSiteId, name);
                var promise = this.$http.get(route);

                this.GetBusy.onNext(true);
                
                promise.success((data: Models.IProviderLoyalty) => {
                    this.LoyaltyProvider.onNext(data);
                });
                promise.finally(() => { this.GetBusy.onNext(false); });
            }

            public Update(model: Models.IProviderLoyalty) : void {
                var partialRoute = "/api/{0}/loyalty/update/{1}";
                var route = kendo.format(partialRoute, Settings.AndromedaSiteId, model.ProviderName);
                var promise = this.$http.post(route, model);

                this.UpdateBusy.onNext(true);

                promise.success((data) => { this.UpdateBusy.onNext(false); });
                promise.finally(() => { this.UpdateBusy.onNext(false); });
            }

        }

        var loyaltyService = "loyaltyService";

        servicesModule.factory(loyaltyService, ($http: ng.IHttpService) => {
            return new LoyaltyService($http);
        });
    }
} 