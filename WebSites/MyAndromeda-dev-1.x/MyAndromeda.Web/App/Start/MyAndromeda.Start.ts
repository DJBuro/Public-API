module MyAndromeda.Start
{
    var start = angular.module("MyAndromeda.Start",
        ["MyAndromeda.Start.Controllers", "MyAndromeda.Start.Config"]);


    export function setupStart(id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, ["MyAndromeda.Start"]);
    };
}