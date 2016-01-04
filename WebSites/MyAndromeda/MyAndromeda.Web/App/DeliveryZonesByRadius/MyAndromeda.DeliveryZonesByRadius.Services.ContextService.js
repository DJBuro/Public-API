var MyAndromeda;
(function (MyAndromeda) {
    (function (DeliveryZonesByRadius) {
        /// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
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
            })();
            Services.ContextService = ContextService;
        })(DeliveryZonesByRadius.Services || (DeliveryZonesByRadius.Services = {}));
        var Services = DeliveryZonesByRadius.Services;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
