/// <reference path="../../typings/jquery/jquery.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (Service) {
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
        Service.chainService = chainService;
    })(MyAndromeda.Service || (MyAndromeda.Service = {}));
    var Service = MyAndromeda.Service;
})(MyAndromeda || (MyAndromeda = {}));
