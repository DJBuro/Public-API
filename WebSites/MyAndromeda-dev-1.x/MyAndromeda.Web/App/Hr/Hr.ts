module MyAndromeda.Hr
{
    var app = angular.module("MyAndromeda.Hr", [
        "MyAndromeda.Hr.Config",
        "MyAndromeda.Resize",
        "MyAndromeda.Progress"
    ]);

    app.run(() => {
        Logger.Notify("HR module is running");
    });
}