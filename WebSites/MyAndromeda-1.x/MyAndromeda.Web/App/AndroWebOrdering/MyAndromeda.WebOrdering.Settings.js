var MyAndromeda;
(function (MyAndromeda) {
    /// <reference path="MyAndromeda.WebOrdering.App.ts" />
    (function (WebOrdering) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;
            Settings.WebSiteId = 0;

            Settings.ReadRoute = "/api/{0}/AndroWebOrdering/{1}/Read";

            Settings.UpdateRoute = "/api/{0}/AndroWebOrdering/{1}/Update";

            Settings.PublishRoute = "/api/{0}/AndroWebOrdering/{1}/Publish";

            Settings.PreviewRoute = "/api/{0}/AndroWebOrdering/{1}/Preview";
            return Settings;
        })();
        WebOrdering.Settings = Settings;
        ;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
