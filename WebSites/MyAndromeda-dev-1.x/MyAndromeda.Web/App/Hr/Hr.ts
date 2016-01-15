module MyAndromeda.Hr
{
    var app = angular.module("MyAndromeda.Hr", [
        "MyAndromeda.Hr.Config",
    ]);

    app.run(() => {
        Logger.Notify("HR module is running");
    });
}