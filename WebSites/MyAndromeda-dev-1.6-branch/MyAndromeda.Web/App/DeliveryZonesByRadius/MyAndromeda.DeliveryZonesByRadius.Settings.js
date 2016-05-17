/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;
            //public static WebSiteId: number = 0;
            //api/{AndromedaSiteId}/DeliveryZonesByRadius/Read
            Settings.ReadRoute = "/api/{0}/DeliveryZonesByRadius/Read";
            //api/{AndromedaSiteId}/DeliveryZonesByRadius/GeneratePostCodeSectors
            Settings.ReadPostCodesRoute = "/api/{0}/DeliveryZonesByRadius/GeneratePostCodeSectors";
            //api/{AndromedaSiteId}/DeliveryZonesByRadius/Update
            Settings.UpdateRoute = "/api/{0}/DeliveryZonesByRadius/Update";
            return Settings;
        }());
        DeliveryZonesByRadius.Settings = Settings;
        ;
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
