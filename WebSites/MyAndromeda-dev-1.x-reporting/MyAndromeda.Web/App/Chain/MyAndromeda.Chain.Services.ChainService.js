var MyAndromeda;
(function (MyAndromeda) {
    var Chain;
    (function (Chain) {
        var Services;
        (function (Services) {
            var chainService = (function () {
                function chainService(chainServiceRoutes) {
                    this.chainServiceRoutes = chainServiceRoutes;
                }
                chainService.prototype.get = function (id, callback) {
                    var internal = this, route = {
                        type: "POST",
                        dataType: "json",
                        data: { id: id },
                        success: function (data) {
                            callback(data);
                        }
                    };
                    $.ajax($.extend({}, route, {
                        url: internal.chainServiceRoutes.getById
                    }));
                };
                return chainService;
            }());
            Services.chainService = chainService;
        })(Services = Chain.Services || (Chain.Services = {}));
    })(Chain = MyAndromeda.Chain || (MyAndromeda.Chain = {}));
})(MyAndromeda || (MyAndromeda = {}));
