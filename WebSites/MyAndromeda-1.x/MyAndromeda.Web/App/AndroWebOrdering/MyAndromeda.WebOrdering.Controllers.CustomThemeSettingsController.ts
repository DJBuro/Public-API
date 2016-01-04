/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(CustomThemeSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    CustomThemeSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    CustomThemeSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class CustomThemeSettingsController {
        public static Name: string = "CustomThemeSettingsController";

        public static OnLoad(
            $scope: Scopes.ICustomThemeSettingsScope,
            $timout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.ResetColors = () => {
                console.log("Resetting colors..");  
                              
                contextService.Model.CustomThemeSettings.ColourRange1 = contextService.Model.CustomThemeSettings.LiveColourRange1;
                contextService.Model.CustomThemeSettings.ColourRange2 = contextService.Model.CustomThemeSettings.LiveColourRange2;
                contextService.Model.CustomThemeSettings.ColourRange3 = contextService.Model.CustomThemeSettings.LiveColourRange3;
                contextService.Model.CustomThemeSettings.ColourRange4 = contextService.Model.CustomThemeSettings.LiveColourRange4;
                contextService.Model.CustomThemeSettings.ColourRange5 = contextService.Model.CustomThemeSettings.LiveColourRange5;
                contextService.Model.CustomThemeSettings.ColourRange6 = contextService.Model.CustomThemeSettings.LiveColourRange6;

                var viewElement = $("#CustomThemeSettingsController");

                kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
            }               

            $scope.SaveChanges = () => {
                console.log("save");
                $scope.SetLiveColors();
                webOrderingWebApiService.Update();
                var viewElement = $("#CustomThemeSettingsController");
                kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
            };

            $scope.SetLiveColors = () => {
                contextService.Model.CustomThemeSettings.LiveColourRange1 = contextService.Model.CustomThemeSettings.ColourRange1;
                contextService.Model.CustomThemeSettings.LiveColourRange2 = contextService.Model.CustomThemeSettings.ColourRange2;
                contextService.Model.CustomThemeSettings.LiveColourRange3 = contextService.Model.CustomThemeSettings.ColourRange3;
                contextService.Model.CustomThemeSettings.LiveColourRange4 = contextService.Model.CustomThemeSettings.ColourRange4;
                contextService.Model.CustomThemeSettings.LiveColourRange5 = contextService.Model.CustomThemeSettings.ColourRange5;
                contextService.Model.CustomThemeSettings.LiveColourRange6 = contextService.Model.CustomThemeSettings.ColourRange6;
            }
           
        }

        public static SetupKendoMvvm($scope: Scopes.ICustomThemeSettingsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {

                    var customThemeSettings = websiteSettings.CustomThemeSettings;
                    var correct = (key: string, value:string) => {
                        if(!customThemeSettings.get(key))
                        {
                            customThemeSettings.set(key, null);
                        }
                    };

                    correct("ColourRange1", "ColourRange1");
                    correct("ColourRange2", "ColourRange2");
                    correct("ColourRange3", "ColourRange3");
                    correct("ColourRange4", "ColourRange4");
                    correct("ColourRange5", "ColourRange5");
                    correct("ColourRange6", "ColourRange6");

                    $scope.SetLiveColors();
                    var viewElement = $("#CustomThemeSettingsController");
                    kendo.bind(viewElement, websiteSettings.CustomThemeSettings);

                    $timout(() => {                        
                        
                    });
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}   