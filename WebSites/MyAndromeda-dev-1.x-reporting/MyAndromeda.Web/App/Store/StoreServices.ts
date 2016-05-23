module MyAndromeda.Store.Services {
    var services = angular.module("MyAndromeda.Store.Services", [
        
    ]);

    export class StoreService {
        constructor(private $http: ng.IHttpService) {
            
        }

        public Find(storeName: string): ng.IHttpPromise<Models.IStore[]> {
            let route = "user/stores/find/" + storeName;
            let promise = this.$http.get(route);

            return promise;
        }

        public FindByAndromedaId(andromedaSiteId: number): ng.IHttpPromise<Models.IStore[]> {
            let route = "user/stores/findById/" + andromedaSiteId;
            let promise = this.$http.get(route);

            return promise;
        }
    }

    services.service("storeService", StoreService);
}