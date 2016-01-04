var MyAndromeda;
(function (MyAndromeda) {
    (function (Store) {
        (function (Services) {
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
            Services.storeService = storeService;
        })(Store.Services || (Store.Services = {}));
        var Services = Store.Services;
    })(MyAndromeda.Store || (MyAndromeda.Store = {}));
    var Store = MyAndromeda.Store;
})(MyAndromeda || (MyAndromeda = {}));
//# sourceMappingURL=StoreService.js.map
