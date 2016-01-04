define(["require", "exports"], function (require, exports) {
    var MyAndromeda;
    (function (MyAndromeda) {
        var Start;
        (function (Start) {
            var Service;
            (function (Service) {
                var Models;
                (function (Models) {
                    exportinterface;
                    IStore;
                    {
                        AndromedaSiteId: number;
                        ExternalSiteId: string;
                        Name: string;
                    }
                })(Models = Service.Models || (Service.Models = {}));
                var services = angular.module("MyAndromeda.Start.Services", []);
                var UserChainDataService = (function () {
                    function UserChainDataService($http) {
                        this.$http = $http;
                    }
                    return UserChainDataService;
                })();
                Service.UserChainDataService = UserChainDataService;
                var getChains = this.$http.get("/user/chains");
                return getChains;
            })(Service = Start.Service || (Start.Service = {}));
        })(Start = MyAndromeda.Start || (MyAndromeda.Start = {}));
    })(MyAndromeda || (MyAndromeda = {}));
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
    exports.UserStoreDataService = UserStoreDataService;
    services.service("userChainDataService", UserChainDataService);
    services.service("userStoreDataService", UserStoreDataService);
});
//# sourceMappingURL=MyAndromeda.Start.Services.js.map