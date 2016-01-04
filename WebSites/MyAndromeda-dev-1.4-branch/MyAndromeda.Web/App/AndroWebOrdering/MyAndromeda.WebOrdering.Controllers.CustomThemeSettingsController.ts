/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {

    Angular.ControllersInitilizations.push((app) => {
        app.controller("ThemeSettingsController",($scope, $timeout,
            ContextService: Services.ContextService,
            WebOrderingWebApiService: Services.WebOrderingWebApiService) => {
            ContextService.ModelSubject.where((e) => e !== null).subscribe((model) => {
                if (typeof (model.CustomThemeSettings.IsPageHeaderVisible) === 'undefined')
                {
                    model.CustomThemeSettings.IsPageHeaderVisible = true;
                }

                $timeout(() => {
                    $scope.CustomThemeSettings = model.CustomThemeSettings;
                });
            });

            $scope.SaveChanges = () => {
                WebOrderingWebApiService.Update();
            };
        });

        app.controller("CustomThemeSettingsController", 
            ($scope: Scopes.ICustomThemeSettingsScope, $timeout, ContextService, WebOrderingWebApiService) => {

                //CustomThemeSettingsController.OnLoad($scope, $timeout, ContextService, WebOrderingWebApiService);

                /* going to leave kendo to manage the observable object */
                //CustomThemeSettingsController.SetupKendoMvvm($scope, $timeout, ContextService);

                var defaultColors = {
                    colour1: "#6ac142",
                    colour2: "#ffffff",
                    colour3: "#6ac142",
                    colour4: "#000000",
                    colour5: "#6ac142",
                    colour6: "#070707"
                };

                var reset = () : void => {
                    Logger.Notify("Resetting colors..");

                    var customThemeSettings = ContextService.Model.CustomThemeSettings;

                    customThemeSettings.ColourRange1 = defaultColors.colour1; //customThemeSettings.LiveColourRange1;
                    customThemeSettings.ColourRange2 = defaultColors.colour2;//customThemeSettings.LiveColourRange2;
                    customThemeSettings.ColourRange3 = defaultColors.colour3;//customThemeSettings.LiveColourRange3;
                    customThemeSettings.ColourRange4 = defaultColors.colour4;//customThemeSettings.LiveColourRange4;
                    customThemeSettings.ColourRange5 = defaultColors.colour5;//customThemeSettings.LiveColourRange5;
                    customThemeSettings.ColourRange6 = defaultColors.colour6;//customThemeSettings.LiveColourRange6;
                };

                var setLive = () : void => {
                    var customThemeSettings = ContextService.Model.CustomThemeSettings;

                    customThemeSettings.LiveColourRange1 = customThemeSettings.ColourRange1;
                    customThemeSettings.LiveColourRange2 = customThemeSettings.ColourRange2;
                    customThemeSettings.LiveColourRange3 = customThemeSettings.ColourRange3;
                    customThemeSettings.LiveColourRange4 = customThemeSettings.ColourRange4;
                    customThemeSettings.LiveColourRange5 = customThemeSettings.ColourRange5;
                    customThemeSettings.LiveColourRange6 = customThemeSettings.ColourRange6;
                };

                var createStyle = (colour: string) => {
                    return {
                        'background-color' : colour
                    };
                };

                $scope.CreateStyle = createStyle;
                $scope.ResetColors = reset;
                $scope.SetLiveColors = setLive;
                $scope.SaveChanges = () => {
                    Logger.Notify("save");
                    setLive();
                    WebOrderingWebApiService.Update();
                };

                var settingsSubscription = ContextService.ModelSubject
                    .where((e) => { return e !== null; })
                    .subscribe((websiteSettings) => {

                    var customThemeSettings = websiteSettings.CustomThemeSettings;
                    var correct = (key: string, value: string) => {
                        if (!customThemeSettings.get(key)) {
                            customThemeSettings.set(key, null);
                        }
                    };

                    correct("ColourRange1", "ColourRange1");
                    correct("ColourRange2", "ColourRange2");
                    correct("ColourRange3", "ColourRange3");
                    correct("ColourRange4", "ColourRange4");
                    correct("ColourRange5", "ColourRange5");
                    correct("ColourRange6", "ColourRange6");

                    setLive();
                    //var viewElement = $("#CustomThemeSettingsController");
                    //kendo.bind(viewElement, websiteSettings.CustomThemeSettings);

                    $timeout(() => {
                        $scope.CustomThemeSettings = websiteSettings.CustomThemeSettings;
                    });
                });

                $scope.$on('$destroy',() => {
                    settingsSubscription.dispose();
                });
               
            }
        );
    });

    export class CustomThemeSettingsController {
        public static Name: string = "CustomThemeSettingsController";

        public static OnLoad(
            $scope: Scopes.ICustomThemeSettingsScope,
            $timout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.ResetColors = () => {
                

                var viewElement = $("#CustomThemeSettingsController");

                kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
            }               

            $scope.SaveChanges = () => {
                Logger.Notify("save");
                $scope.SetLiveColors();
                webOrderingWebApiService.Update();
                var viewElement = $("#CustomThemeSettingsController");
                kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
            };

            $scope.SetLiveColors = () => {
                
            }
           
        }

        public static SetupKendoMvvm($scope: Scopes.ICustomThemeSettingsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService): void {
            
        }
    }
}   