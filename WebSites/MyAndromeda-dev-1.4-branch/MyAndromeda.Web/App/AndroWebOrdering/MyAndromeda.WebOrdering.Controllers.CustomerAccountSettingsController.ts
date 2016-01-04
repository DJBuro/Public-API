/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(CustomerAccountSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    CustomerAccountSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    CustomerAccountSettingsController.SetupKendoMvvm($scope, contextService);
                }
            ]);
    });

    export class CustomerAccountSettingsController {
        public static Name: string = "CustomerAccountSettingsController";

        public static OnLoad(
            $scope: Scopes.ICustomerAccountSettingsScope,
            $timout: ng.ITimeoutService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            $scope.SaveChanges = () => {
                webOrderingWebApiService.Update();
            };
        }

        public static SetupKendoMvvm($scope: Scopes.ICustomerAccountSettingsScope, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {

                    var viewElement = $("#CustomerAccountSettingsController");
                    kendo.bind(viewElement, websiteSettings.CustomerAccountSettings);
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}  