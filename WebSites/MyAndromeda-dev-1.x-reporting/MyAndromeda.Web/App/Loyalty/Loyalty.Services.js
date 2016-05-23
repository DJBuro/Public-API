var MyAndromeda;
(function (MyAndromeda) {
    var Loyalty;
    (function (Loyalty) {
        Loyalty.ServicesName = "LoyaltyServices";
        var servicesModule = angular.module(Loyalty.ServicesName, []);
        var Services;
        (function (Services) {
            var LoyaltyService = (function () {
                function LoyaltyService($http) {
                    this.$http = $http;
                    /* data messaging */
                    // all types that have not been used yet. 
                    this.AllLoyaltyTypeList = new Rx.Subject();
                    // store loyalty types that have been used 
                    this.StoreLoyalties = new Rx.Subject();
                    this.LoyaltyProvider = new Rx.Subject();
                    /* monitor network */
                    this.ListLoyaltyTypesBusy = new Rx.BehaviorSubject(false);
                    this.ListBusy = new Rx.BehaviorSubject(false);
                    this.GetBusy = new Rx.BehaviorSubject(false);
                    this.UpdateBusy = new Rx.BehaviorSubject(false);
                }
                LoyaltyService.prototype.ListLoyaltyTypes = function () {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/types";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId);
                    var promise = this.$http.get(route);
                    this.ListLoyaltyTypesBusy.onNext(true);
                    promise.success(function (data) {
                        _this.AllLoyaltyTypeList.onNext(data);
                    });
                    promise.finally(function () {
                        _this.ListLoyaltyTypesBusy.onNext(false);
                    });
                };
                LoyaltyService.prototype.List = function () {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/list";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId);
                    var promise = this.$http.get(route);
                    this.ListBusy.onNext(true);
                    promise.success(function (data) {
                        _this.StoreLoyalties.onNext(data);
                    });
                    promise.finally(function () {
                        _this.ListBusy.onNext(false);
                    });
                };
                LoyaltyService.prototype.Get = function (name) {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/get/{1}";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId, name);
                    var promise = this.$http.get(route);
                    this.GetBusy.onNext(true);
                    promise.success(function (data) {
                        _this.LoyaltyProvider.onNext(data);
                    });
                    promise.finally(function () { _this.GetBusy.onNext(false); });
                };
                LoyaltyService.prototype.Update = function (model) {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/update/{1}";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId, model.ProviderName);
                    var promise = this.$http.post(route, model);
                    this.UpdateBusy.onNext(true);
                    promise.success(function (data) { _this.UpdateBusy.onNext(false); });
                    promise.finally(function () { _this.UpdateBusy.onNext(false); });
                };
                return LoyaltyService;
            }());
            Services.LoyaltyService = LoyaltyService;
            var loyaltyService = "loyaltyService";
            servicesModule.service(loyaltyService, LoyaltyService);
        })(Services = Loyalty.Services || (Loyalty.Services = {}));
    })(Loyalty = MyAndromeda.Loyalty || (MyAndromeda.Loyalty = {}));
})(MyAndromeda || (MyAndromeda = {}));
