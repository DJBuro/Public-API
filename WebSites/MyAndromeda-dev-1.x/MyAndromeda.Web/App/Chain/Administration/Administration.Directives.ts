module MyAndromeda.Chain.Administration.Directives {
    var app = angular.module("MyAndromeda.Chain.Administration.Directives", []);

    app.directive("chainAdminTreeview", () => {


        return {
            name: "chainAdminTreeview",
            scope: {
                chainId : "=chainId"
            },
            controller: ($scope) => { }
        };

    });

    app.directive("storeTickList", () => {

        return {
            name: "storeTickList",
            scope: {
                selectedChainId: "=chainId",
            },
            controller: ($scope) => {
                
            }
        };

    });
}