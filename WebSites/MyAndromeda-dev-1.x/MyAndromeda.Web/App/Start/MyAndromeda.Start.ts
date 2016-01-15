module MyAndromeda.Start
{
    var start = angular.module("MyAndromeda.Start",
        [
            
            "MyAndromeda.Start.Config",
            "MyAndromeda.Hr"
        ]);


    export function setupStart(id: string) {
        var element = document.getElementById(id);
        angular.bootstrap(element, ["MyAndromeda.Start"]);
    };
}