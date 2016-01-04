/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(CustomEmailTemplateController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    CustomEmailTemplateController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    CustomEmailTemplateController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class CustomEmailTemplateController {
        public static Name: string = "CustomEmailTemplateController";

        public static OnLoad(
            $scope: Scopes.ICustomEmailTemplateScope,
            $timout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.ResetColors = () => {
                console.log("Resetting email template colors..");
                contextService.Model.CustomEmailTemplate.HeaderColour = contextService.Model.CustomEmailTemplate.LiveHeaderColour;
                contextService.Model.CustomEmailTemplate.FooterColour = contextService.Model.CustomEmailTemplate.LiveFooterColour;
                var viewElement = $("#CustomEmailTemplateController");
                kendo.bind(viewElement, contextService.Model.CustomEmailTemplate);
            }

            $scope.SaveChanges = () => {
                console.log("save");
                $scope.SetLiveColors();
                webOrderingWebApiService.Update();
                var viewElement = $("#CustomEmailTemplateController");
                kendo.bind(viewElement, contextService.Model.CustomEmailTemplate);
            };
            
            $scope.SetLiveColors = () => {
                contextService.Model.CustomEmailTemplate.LiveHeaderColour = contextService.Model.CustomEmailTemplate.HeaderColour;
                contextService.Model.CustomEmailTemplate.LiveFooterColour = contextService.Model.CustomEmailTemplate.FooterColour;
            }

        }

        public static SetupKendoMvvm($scope: Scopes.ICustomEmailTemplateScope, $timout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {

                    var customEmailTemplate = websiteSettings.CustomEmailTemplate;
                    var correct = (key: string, value: string) => {
                        if (!customEmailTemplate.get(key)) {
                            customEmailTemplate.set(key, "#EEEEEE");
                        }
                    };
                    correct("HeaderColour", "HeaderColour");
                    correct("FooterColour", "FooterColour");                    
                    $scope.SetLiveColors();
                    var viewElement = $("#CustomEmailTemplateController");
                    kendo.bind(viewElement, websiteSettings.CustomEmailTemplate);
                    $timout(() => {

                    });
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}    