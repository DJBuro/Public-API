var MyAndromeda;
(function (MyAndromeda) {
    (function (Chain) {
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
            })();
            Services.chainService = chainService;
        })(Chain.Services || (Chain.Services = {}));
        var Services = Chain.Services;
    })(MyAndromeda.Chain || (MyAndromeda.Chain = {}));
    var Chain = MyAndromeda.Chain;
})(MyAndromeda || (MyAndromeda = {}));
