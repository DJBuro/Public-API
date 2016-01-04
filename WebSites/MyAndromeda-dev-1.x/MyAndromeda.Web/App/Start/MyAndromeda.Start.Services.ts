module MyAndromeda.Start.Services {

    export module Models {
        export interface IChain {
            Id: number;
            Name: string;
            Stores: number;
            ParentId: number;
        }

        export interface IStore
        {
            AndromedaSiteId: number;
            ExternalSiteId: string;
            Name: string;
        }
    } 


    var services = angular.module("MyAndromeda.Start.Services", []);

    export class UserChainDataService
    {
        constructor(private $http: angular.IHttpService) {
        }

        public List() : ng.IHttpPromise<Models.IChain>
        {
            var getChains = this.$http.get("/user/chains");
            return getChains;
        }
    }

    export class UserStoreDataService
    {
        constructor(private $http: angular.IHttpService) {
        }

        public ListStores() : ng.IHttpPromise<Models.IStore>
        {
            var route = "/user/stores";
            var getStores = this.$http.get(route);

            return getStores;
        }

        public ListStoresByChainId(chainId: number): ng.IHttpPromise<Models.IStore>
        {
            var route = kendo.format("/user/chains/{0}", chainId);
            var getChains = this.$http.get(route);

            return getChains;
        }
    }

    services.service("userChainDataService", UserChainDataService);
    services.service("userStoreDataService", UserStoreDataService);
}
