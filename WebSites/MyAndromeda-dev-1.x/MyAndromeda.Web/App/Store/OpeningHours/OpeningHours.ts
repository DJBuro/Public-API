module MyAndromeda.Stores.OpeningHours
{
    var app = angular.module("MyAndromeda.Stores.OpeningHours", [
        "MyAndromeda.Core",
        "MyAndromeda.Resize",
        "MyAndromeda.Progress",
        "ngAnimate",
        "ui.bootstrap"
    ]);

    app.run(() => {
        Logger.Notify("HR module is running");
    });
}