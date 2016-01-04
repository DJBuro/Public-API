var MyAndromeda;
(function (MyAndromeda) {
    (function (Service) {
        var storeService = (function () {
            function storeService(routes) {
                this.routes = routes;
            }
            storeService.prototype.get = function (id, callback) {
                var internal = this;
                var route = {
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        callback(data);
                    }
                };
                if (typeof (id) === "string") {
                    $.ajax($.extend({}, route, {
                        url: internal.routes.getByExternalId
                    }));
                }
                if (typeof (id) === "number") {
                    $.ajax($.extend({}, route, {
                        url: internal.routes.getById
                    }));
                }
            };

            storeService.prototype.getStores = function (chainId, callback) {
                var internal = this;
            };
            return storeService;
        })();
        Service.storeService = storeService;
    })(MyAndromeda.Service || (MyAndromeda.Service = {}));
    var Service = MyAndromeda.Service;
})(MyAndromeda || (MyAndromeda = {}));
