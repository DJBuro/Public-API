//createDriver(driverUpdate)
var MyAndromeda;
(function (MyAndromeda) {
    var Data;
    (function (Data) {
        var Services;
        (function (Services) {
            var m = angular.module("MyAndromeda.Data.Drivers", []);
            var DriverService = (function () {
                function DriverService($http) {
                    this.$http = $http;
                }
                DriverService.prototype.AddToOrder = function (andromedaSiteId, orderId, driver) {
                    var route = kendo.format("/data/{0}/orders/{1}/addDriver", andromedaSiteId, orderId);
                    var promise = this.$http.post(route, driver);
                    return promise;
                };
                return DriverService;
            })();
            Services.DriverService = DriverService;
            m.service("driverService", DriverService);
        })(Services = Data.Services || (Data.Services = {}));
    })(Data = MyAndromeda.Data || (MyAndromeda.Data = {}));
})(MyAndromeda || (MyAndromeda = {}));
