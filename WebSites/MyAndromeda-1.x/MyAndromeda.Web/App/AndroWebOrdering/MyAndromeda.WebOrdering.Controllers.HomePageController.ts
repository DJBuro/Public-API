/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(HomePageController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    HomePageController.OnLoad($scope, $timeout, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    HomePageController.SetupKendoMvvm($scope, contextService);
                }
            ]);
    });

    export class HomePageController {
        public static Name: string = "HomePageController";

        public static OnLoad(
            $scope: Scopes.IHomePageScope,
            $timout: ng.ITimeoutService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            $scope.SaveChanges = () => {
                webOrderingWebApiService.Update();
            };
        }

        public static SetupKendoMvvm($scope: Scopes.IHomePageScope, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {
                    var viewElement = $("#HomePageController");
                    var model = websiteSettings.Home;
                    kendo.bind(viewElement, model);
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}   