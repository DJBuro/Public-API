/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        var Services;
        (function (Services) {
            DeliveryZonesByRadius.Angular.ServicesInitilizations.push(function (app) {
                app.factory(ContextService.Name, [
                    function () {
                        var instnance = new ContextService();
                        return instnance;
                    }
                ]);
            });
            var ContextService = (function () {
                function ContextService() {
                    this.Model = null;
                    this.ModelSubject = new Rx.Subject();
                    this.PostcodeModels = new Rx.Subject();
                }
                ContextService.Name = "ContextService";
                return ContextService;
            }());
            Services.ContextService = ContextService;
        })(Services = DeliveryZonesByRadius.Services || (DeliveryZonesByRadius.Services = {}));
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
