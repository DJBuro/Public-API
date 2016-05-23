module MyAndromeda.Start
{
    var app = angular.module("MyAndromeda.Start.Config", [
        "MyAndromeda.Components.FindStore",
        "MyAndromeda.Start.Controllers",
        "ui.router",
        "ngAnimate"
    ]);

    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {

        Logger.Notify("set start config");

        var app = {
            abstract: true,
            url: "/chain",
            template: '<div ui-view="main"></div>'
        };

        var appChainList: ng.ui.IState = {
            url: "/list",
            views: {
                "main": {
                    templateUrl: "chain-list.html",
                    controller: "chainListController"
                },
            },
            cache: false
        };

        //var appChainsStoreList: ng.ui.IState = {
        //    url: "/:chainId",
        //    views: {
        //        "main": {
        //            templateUrl: "store-list.html",
        //            controller: "storeListController"
        //        }
        //    },
        //    cache: false
        //};

        $stateProvider.state("chain", app);
        $stateProvider.state("chain.list", appChainList);
        //$stateProvider.state("start-chain-store", appChainsStoreList);

        
        $urlRouterProvider.otherwise("/chain/list");
    });


    app.run(($templateCache: ng.ITemplateCacheService) => {
        Logger.Notify("Started config");

        angular
            .element('script[type="text/ng-template"]')
            .each((i, element: HTMLElement) => {
                $templateCache.put(element.id, element.innerHTML);
            });
    });

}