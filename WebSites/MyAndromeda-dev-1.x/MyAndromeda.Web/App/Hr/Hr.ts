module MyAndromeda.Hr
{
    var app = angular.module("MyAndromeda.Hr", [
        "MyAndromeda.Hr.Config",
        "MyAndromeda.Resize",
        "MyAndromeda.Progress",
        "ngAnimate",
        "ui.bootstrap"
    ]);

    app.run(() => {
        Logger.Notify("HR module is running");
    });
}