/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("ThemeSettingsController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    contextService.ModelSubject.where(function (e) { return e !== null; }).subscribe(function (model) {
                        if (typeof (model.CustomThemeSettings.IsPageHeaderVisible) === 'undefined') {
                            model.CustomThemeSettings.IsPageHeaderVisible = true;
                        }
                        $timeout(function () {
                            $scope.CustomThemeSettings = model.CustomThemeSettings;
                        });
                    });
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                });
                app.controller("CustomThemeSettingsController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
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
                    var reset = function () {
                        WebOrdering.Logger.Notify("Resetting colors..");
                        var customThemeSettings = contextService.Model.CustomThemeSettings;
                        customThemeSettings.ColourRange1 = defaultColors.colour1; //customThemeSettings.LiveColourRange1;
                        customThemeSettings.ColourRange2 = defaultColors.colour2; //customThemeSettings.LiveColourRange2;
                        customThemeSettings.ColourRange3 = defaultColors.colour3; //customThemeSettings.LiveColourRange3;
                        customThemeSettings.ColourRange4 = defaultColors.colour4; //customThemeSettings.LiveColourRange4;
                        customThemeSettings.ColourRange5 = defaultColors.colour5; //customThemeSettings.LiveColourRange5;
                        customThemeSettings.ColourRange6 = defaultColors.colour6; //customThemeSettings.LiveColourRange6;
                    };
                    var setLive = function () {
                        var customThemeSettings = contextService.Model.CustomThemeSettings;
                        customThemeSettings.LiveColourRange1 = customThemeSettings.ColourRange1;
                        customThemeSettings.LiveColourRange2 = customThemeSettings.ColourRange2;
                        customThemeSettings.LiveColourRange3 = customThemeSettings.ColourRange3;
                        customThemeSettings.LiveColourRange4 = customThemeSettings.ColourRange4;
                        customThemeSettings.LiveColourRange5 = customThemeSettings.ColourRange5;
                        customThemeSettings.LiveColourRange6 = customThemeSettings.ColourRange6;
                    };
                    var createStyle = function (colour) {
                        return {
                            'background-color': colour
                        };
                    };
                    $scope.CreateStyle = createStyle;
                    $scope.ResetColors = reset;
                    $scope.SetLiveColors = setLive;
                    $scope.SaveChanges = function () {
                        WebOrdering.Logger.Notify("save");
                        setLive();
                        webOrderingWebApiService.Update();
                    };
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var customThemeSettings = websiteSettings.CustomThemeSettings;
                        var correct = function (key, value) {
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
                        $timeout(function () {
                            $scope.CustomThemeSettings = websiteSettings.CustomThemeSettings;
                        });
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                });
            });
            var CustomThemeSettingsController = (function () {
                function CustomThemeSettingsController() {
                }
                CustomThemeSettingsController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.ResetColors = function () {
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };
                    $scope.SaveChanges = function () {
                        WebOrdering.Logger.Notify("save");
                        $scope.SetLiveColors();
                        webOrderingWebApiService.Update();
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };
                    $scope.SetLiveColors = function () {
                    };
                };
                CustomThemeSettingsController.Name = "CustomThemeSettingsController";
                return CustomThemeSettingsController;
            })();
            Controllers.CustomThemeSettingsController = CustomThemeSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
