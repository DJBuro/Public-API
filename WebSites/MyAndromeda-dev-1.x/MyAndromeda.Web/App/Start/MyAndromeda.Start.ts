module MyAndromeda.Start
{
    var start = angular.module("MyAndromeda.Start",
        [
            "ngAnimate",
            "MyAndromeda.Start.Config",
            "MyAndromeda.Hr",
            "MyAndromeda.Chain.Directives",
            "MyAndromeda.Chain.Administration"
        ]);

    export function setupStart(id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, ["MyAndromeda.Start"]);
    };
}