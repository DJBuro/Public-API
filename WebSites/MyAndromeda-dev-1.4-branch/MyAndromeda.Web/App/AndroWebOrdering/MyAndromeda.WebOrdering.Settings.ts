/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering {
    export class Settings {
        public static AndromedaSiteId : number =0;
        public static WebSiteId : number = 0;

        //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Read
        public static ReadRoute: string =       "/api/{0}/AndroWebOrdering/{1}/Read";

        //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Update
        public static UpdateRoute: string =     "/api/{0}/AndroWebOrdering/{1}/Update";

        //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Publish
        public static PublishRoute: string =    "/api/{0}/AndroWebOrdering/{1}/Publish";

        //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Preview
        public static PreviewRoute: string = "/api/{0}/AndroWebOrdering/{1}/Preview";

        //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Stores/Read
        public static ReadStoreRoute: string = "/api/{0}/AndroWebOrdering/{1}/Stores/Read";
    };
}