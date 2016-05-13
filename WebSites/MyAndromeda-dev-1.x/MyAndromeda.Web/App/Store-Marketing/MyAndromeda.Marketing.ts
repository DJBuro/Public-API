/// <reference path="../general/resizemodule.ts" />
module MyAndromeda.Marketing {

    Logger.Notify("MyAndromeda.MarketingThing");

    export var moduleName = "MyAndromeda.MarketingThing";

    export var m = angular.module(moduleName, [
        "MyAndromeda.Resize",
        "MyAndromeda.Progress",
        "ngAnimate",
        "kendo.directives",
        "ui.bootstrap",
        "oitozero.ngSweetAlert"
    ]);

    m.run(($templateCache: ng.ITemplateCacheService) => {
        Logger.Notify("WebHooks Started");

        angular
            .element('script[type="text/template"]')
            .each((i, element: HTMLElement) => {
                $templateCache.put(element.id, element.innerHTML);
            });
    });

    export var Routes =
        {
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



    export function SetupMaketingEvents(id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, [moduleName]);
    }
}