var MyAndromeda;
(function (MyAndromeda) {
    /// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
    (function (DeliveryZonesByRadius) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;

            Settings.ReadRoute = "/api/{0}/DeliveryZonesByRadius/Read";

            Settings.ReadPostCodesRoute = "/api/{0}/DeliveryZonesByRadius/GeneratePostCodeSectors";

            Settings.UpdateRoute = "/api/{0}/DeliveryZonesByRadius/Update";
            return Settings;
        })();
        DeliveryZonesByRadius.Settings = Settings;
        ;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
