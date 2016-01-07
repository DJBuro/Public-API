var MyAndromeda;
(function (MyAndromeda) {
    var Start;
    (function (Start) {
        var Services;
        (function (Services) {
            var services = angular.module("MyAndromeda.Start.Services", []);
            var UserChainDataService = (function () {
                function UserChainDataService($http) {
                    this.$http = $http;
                }
                UserChainDataService.prototype.List = function () {
                    var getChains = this.$http.get("/user/chains");
                    return getChains;
                };
                return UserChainDataService;
            })();
            Services.UserChainDataService = UserChainDataService;
            var UserStoreDataService = (function () {
                function UserStoreDataService($http) {
                    this.$http = $http;
                }
                UserStoreDataService.prototype.ListStores = function () {
                    var route = "/user/stores";
                    var getStores = this.$http.get(route);
                    return getStores;
                };
                UserStoreDataService.prototype.ListStoresByChainId = function (chainId) {
                    var route = kendo.format("/user/chains/{0}", chainId);
                    var getChains = this.$http.get(route);
                    return getChains;
                };
                return UserStoreDataService;
            })();
            Services.UserStoreDataService = UserStoreDataService;
            services.service("userChainDataService", UserChainDataService);
            services.service("userStoreDataService", UserStoreDataService);
        })(Services = Start.Services || (Start.Services = {}));
    })(Start = MyAndromeda.Start || (MyAndromeda.Start = {}));
})(MyAndromeda || (MyAndromeda = {}));
