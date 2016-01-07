module MyAndromeda.Start
{
    var app = angular.module("MyAndromeda.Start.Config", [
        "ui.router",
        "ngAnimate"
    ]);

    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {

        //var app = {
        //    abstract: true,
        //    url: "/",
        //    templateUrl: "start-app-template.html"
        //};

        var appChainList: ng.ui.IState = {

            url: "/chains",
            views: {
                "main": {
                    templateUrl: "chain-list.html",
                    controller: "chainListController"
                },
            },
            cache: true
        };

        var appChainsStoreList: ng.ui.IState = {
            url: "/:chainId",
            views: {
                "main": {
                    templateUrl: "store-list.html",
                    controller: "storeListController"
                }
            },
            cache: false
        };

        //$stateProvider.state("app", app);
        $stateProvider.state("start-chains", appChainList);
        $stateProvider.state("start-chain-store", appChainsStoreList);

        $urlRouterProvider.otherwise("/chains");
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