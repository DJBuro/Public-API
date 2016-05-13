/// <reference path="../general/resizemodule.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Marketing;
    (function (Marketing) {
        MyAndromeda.Logger.Notify("MyAndromeda.MarketingThing");
        Marketing.moduleName = "MyAndromeda.MarketingThing";
        Marketing.m = angular.module(Marketing.moduleName, [
            "MyAndromeda.Resize",
            "MyAndromeda.Progress",
            "ngAnimate",
            "kendo.directives",
            "ui.bootstrap",
            "oitozero.ngSweetAlert"
        ]);
        Marketing.m.run(function ($templateCache) {
            MyAndromeda.Logger.Notify("WebHooks Started");
            angular
                .element('script[type="text/template"]')
                .each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        Marketing.Routes = {
            ContactRoute: "/marketing/{0}/marketing/contact",
            RegisteredAndInactiveRoute: "/marketing/{0}/marketing/noorders",
            InactiveForSevenDaysRoute: "/marketing/{0}/marketing/oneweek",
            InactiveForOneMonthRoute: "/marketing/{0}/marketing/onemonth",
            InactiveForThreeMonthsRoute: "/marketing/{0}/marketing/threemonth",
            TestType: "/marketing/{0}/marketing/test",
            Save: "/marketing/{0}/marketing/saveevent",
            Preview: "/marketing/{0}/marketing/preview",
            SendNow: "/marketing/{0}/marketing/sendnow",
            PreviewRecipients: "/marketing/{0}/marketing/previewRecipients"
        };
        function SetupMaketingEvents(id) {
            var element = document.getElementById(id);
            angular.bootstrap(element, [Marketing.moduleName]);
        }
        Marketing.SetupMaketingEvents = SetupMaketingEvents;
    })(Marketing = MyAndromeda.Marketing || (MyAndromeda.Marketing = {}));
})(MyAndromeda || (MyAndromeda = {}));
