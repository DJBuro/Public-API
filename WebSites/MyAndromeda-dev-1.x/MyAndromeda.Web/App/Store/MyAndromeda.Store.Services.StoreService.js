var MyAndromeda;
(function (MyAndromeda) {
    var Store;
    (function (Store) {
        var Services;
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
        })(Services = Store.Services || (Store.Services = {}));
    })(Store = MyAndromeda.Store || (MyAndromeda.Store = {}));
})(MyAndromeda || (MyAndromeda = {}));
