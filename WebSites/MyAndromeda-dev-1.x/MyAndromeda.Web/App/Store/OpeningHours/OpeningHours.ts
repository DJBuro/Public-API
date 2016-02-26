module MyAndromeda.Stores.OpeningHours
{
    export var settings = {
        chainId: 0,
        andromedaSiteId : 0
    };

    var app = angular.module("MyAndromeda.Stores.OpeningHours", [
        "MyAndromeda.Store.OpeningHours.Config",
        "MyAndromeda.Core",
        "MyAndromeda.Resize",
        "MyAndromeda.Progress",
        "ngAnimate",
        "ui.bootstrap"
    ]);

    app.run(() => {
        Logger.Notify("HR module is running");
    });

    export function boostrap(element, chainId, andromedaSiteId) {
        settings.chainId = chainId;
        settings.andromedaSiteId = andromedaSiteId;

        var e = $(element);
        angular.bootstrap(e, ["MyAndromeda.Stores.OpeningHours"]);
    }
}