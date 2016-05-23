/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(LegalNoticesController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    LegalNoticesController.OnLoad($scope, $timeout, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    LegalNoticesController.SetupKendoMvvm($scope, contextService);
                }
            ]);
    });

    export class LegalNoticesController {
        public static Name: string = "LegalNoticesController";

        public static OnLoad(
            $scope: Scopes.ILegalNoticesScope,
            $timout: ng.ITimeoutService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            $scope.SaveChanges = () => {
                webOrderingWebApiService.Update();
            };
        }

        public static SetupKendoMvvm($scope: Scopes.ILegalNoticesScope, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {

                    var viewElement = $("#LegalNoticesController");
                    kendo.bind(viewElement, websiteSettings.LegalNotices);
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}  