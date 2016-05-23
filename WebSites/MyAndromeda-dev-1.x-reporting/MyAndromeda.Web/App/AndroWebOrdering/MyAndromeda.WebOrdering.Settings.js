/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;
            Settings.WebSiteId = 0;
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Read
            Settings.ReadRoute = "/api/{0}/AndroWebOrdering/{1}/Read";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Update
            Settings.UpdateRoute = "/api/{0}/AndroWebOrdering/{1}/Update";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Publish
            Settings.PublishRoute = "/api/{0}/AndroWebOrdering/{1}/Publish";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Preview
            Settings.PreviewRoute = "/api/{0}/AndroWebOrdering/{1}/Preview";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Stores/Read
            Settings.ReadStoreRoute = "/api/{0}/AndroWebOrdering/{1}/Stores/Read";
            return Settings;
        }());
        WebOrdering.Settings = Settings;
        ;
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
