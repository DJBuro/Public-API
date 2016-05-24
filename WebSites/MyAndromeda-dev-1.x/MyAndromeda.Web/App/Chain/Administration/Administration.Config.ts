module MyAndromeda.Chain.Administration {
    var app = angular.module("MyAndromeda.Chain.Administration", [
        "MyAndromeda.Chain.Administration.Controllers",
        "MyAndromeda.Chain.Administration.Services",
        "MyAndromeda.Chain.Administration.Directives"
    ]);

    app.config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider) => {

        var chainAdmin: ng.ui.IState = {
            abstract: true,
            url: '/chain-admin',
            template: '<div id="masterUI" ui-view="main"></div>'
        };

        var chainEdit: ng.ui.IState = {
            url: "/:chainId",
            views: {
                "main": {
                    templateUrl: "/App/Views/Chain/Administration/admin-start.page.html",
                    controller: "ChainAdminController"
                },
            },
            onEnter: () => {
                Logger.Notify("Entering chain dashboard.");
            },
            cache: false
        };
        
        // route: /chain-admin
        $stateProvider.state("chain-admin", chainAdmin);
        // /chain-admin/building"
        $stateProvider.state("chain-admin.edit", chainEdit);
    });

}