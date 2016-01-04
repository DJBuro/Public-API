/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
module MyAndromeda.DeliveryZonesByRadius {
    export class Settings {
        public static AndromedaSiteId: number = 0;
        //public static WebSiteId: number = 0;

        //api/{AndromedaSiteId}/DeliveryZonesByRadius/Read
        public static ReadRoute: string = "/api/{0}/DeliveryZonesByRadius/Read";

        //api/{AndromedaSiteId}/DeliveryZonesByRadius/GeneratePostCodeSectors
        public static ReadPostCodesRoute: string = "/api/{0}/DeliveryZonesByRadius/GeneratePostCodeSectors";

        //api/{AndromedaSiteId}/DeliveryZonesByRadius/Update
        public static UpdateRoute: string = "/api/{0}/DeliveryZonesByRadius/Update";
        
    };
} 