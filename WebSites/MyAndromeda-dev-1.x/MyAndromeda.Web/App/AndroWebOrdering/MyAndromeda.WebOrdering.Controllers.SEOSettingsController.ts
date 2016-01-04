/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(SEOSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    SEOSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    SEOSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class SEOSettingsController {
        public static Name: string = "SEOSettingsController";

        public static OnLoad(
            $scope: Scopes.ISEOSettingsScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.SaveChanges = () => {
                if ($scope.SEOSettingsValidator.validate()) {
                    webOrderingWebApiService.Update();
                }
            };
        }

        public static SetupKendoMvvm($scope: Scopes.ISEOSettingsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((webSiteSettings) => {

                    var viewElement = $("#SEOSettingsController");
                    kendo.bind(viewElement, webSiteSettings.SEOSettings);
                    $scope.ShowSEODescription = webSiteSettings.SEOSettings.get("IsEnableDescription");
                    //added 500ms timeout as there are random issues. 
                    $timeout(() => {
                    }, 500, true);
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}  